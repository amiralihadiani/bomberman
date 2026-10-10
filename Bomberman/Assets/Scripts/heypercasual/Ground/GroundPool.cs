using UnityEngine;
using System.Collections.Generic;
public class GroundPool : MonoBehaviour
{ 
    public enum Difficulty { VeryEasy, Easy, Medium, Hard, VeryHard }
    [SerializeField] private Difficulty gameDifficulty = Difficulty.Medium;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private int poolSize = 7;
    [SerializeField] private float groundLength = 10f;
    [SerializeField] private int safeStartGrounds = 2;
    [SerializeField] private WoodPool woodPool;
    [SerializeField] private float woodSpacing = 1f;
    [SerializeField] private GameObject gatePrefab; 
    [SerializeField] private int totalGatesToSpawn = 6; 
    [SerializeField] private GameObject gemPrefab; 
    [SerializeField] private int totalGemsToSpawn = 10;
    [SerializeField] private float gemXRange = 2.0f; 
    [SerializeField] private float woodXRange = 2.0f;
    [SerializeField] private GameObject finishLinePrefab;
    [SerializeField] private int totalGroundsToFinish = 15;
    private GameObject[] groundPool;
    private int firstGroundIndex = 0;
    private float nextGroundZ = 0f;
    private int spawnedGroundCount = 0;
    private int spawnedGateCount = 0;
    private int lastGateGroundIndex = -2; 
    private bool isFinishLineSpawned = false;
    private float totalTrackLength;
    private float gemInterval;
    private float currentMinGap;
    private float currentMaxGap;
    private float currentGapChance;
    private int currentMinWoods;
    private int currentMaxWoods;
    private void Start()
    {
        ApplyDifficultySettings();
        totalTrackLength = totalGroundsToFinish * groundLength;
        gemInterval = totalTrackLength / (totalGemsToSpawn + 1);
        CreateGroundPool();
        SpawnInitialGrounds();
    }
    private void ApplyDifficultySettings()
    {
        switch (gameDifficulty)
        {
            case Difficulty.VeryEasy:
                currentMinGap = 1.5f;
                currentMaxGap = 3f;
                currentGapChance = 0.2f; 
                currentMinWoods = 6;  
                currentMaxWoods = 8;
                break;

            case Difficulty.Easy:
                currentMinGap = 2f;
                currentMaxGap = 4f;
                currentGapChance = 0.35f;
                currentMinWoods = 4;     
                currentMaxWoods = 7;
                break;

            case Difficulty.Medium:
                currentMinGap = 4f;
                currentMaxGap = 6.5f;
                currentGapChance = 0.55f; 
                currentMinWoods = 4;      
                currentMaxWoods = 5;
                break;

            case Difficulty.Hard:
                currentMinGap = 5.5f;
                currentMaxGap = 7.5f;
                currentGapChance = 0.7f; 
                currentMinWoods = 4;     
                currentMaxWoods = 8;
                break;

            case Difficulty.VeryHard:
                currentMinGap = 7f;
                currentMaxGap = 9.5f;
                currentGapChance = 0.85f;
                currentMinWoods = 4;     
                currentMaxWoods = 8;
                break;
        } 
    } 
    private void Update()
    {
        RecycleGrounds();
    }
    private void CreateGroundPool()
    {
        groundPool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ground = Instantiate(groundPrefab, transform);
            ground.name = "Ground_" + i;
            int groundLayer = LayerMask.NameToLayer("Ground");
            if (groundLayer != -1) ground.layer = groundLayer;
            groundPool[i] = ground;
        }
    }
    private void SpawnInitialGrounds()
    {
        float currentZ = 0f;
        for (int i = 0; i < poolSize; i++)
        {
            groundPool[i].SetActive(true);
            spawnedGroundCount++; 
            float groundStartZ = currentZ;
            float groundCenterZ = currentZ + (groundLength / 2f);
            groundPool[i].transform.position = new Vector3(0f, -0.5f, groundCenterZ);
            currentZ += groundLength;
            bool isGap = false;
            if (i >= safeStartGrounds && Random.value < currentGapChance && spawnedGroundCount < totalGroundsToFinish - 1)
            {
                isGap = true;
                float gapLength = Random.Range(currentMinGap, currentMaxGap);
                float gapStart = currentZ - groundLength;
                SpawnWoodForGap(gapStart, gapLength);
                currentZ += gapLength;
            }
            if (!isGap && CanSpawnGate())
            {
                SpawnGateAtCenter(groundCenterZ);
            }
            TrySpawnUniformGems(groundStartZ, groundStartZ + groundLength);
        }
        nextGroundZ = currentZ;
    }
    private void RecycleGrounds()
    {
        if (player == null || isFinishLineSpawned) return;
        GameObject firstGround = groundPool[firstGroundIndex];
        float groundZ = firstGround.transform.position.z;
        if (player.position.z > groundZ + groundLength)
        {
            MoveGroundToFront(firstGround);
            firstGroundIndex = (firstGroundIndex + 1) % poolSize;
        }
    }
    private void MoveGroundToFront(GameObject ground)
    {
        if (isFinishLineSpawned) return;
        spawnedGroundCount++;
        float groundStartZ = nextGroundZ;
        float groundCenterZ = nextGroundZ + (groundLength / 2f);
        ground.transform.position = new Vector3(0f, -0.5f, groundCenterZ);
        if (spawnedGroundCount >= totalGroundsToFinish)
        {
            isFinishLineSpawned = true;
            if (finishLinePrefab != null)
            {
                Vector3 finishPos = new Vector3(0f, 0.5f, groundCenterZ);
                GameObject finishObj = Instantiate(finishLinePrefab, finishPos, Quaternion.identity);
                finishObj.tag = "FinishLine";
            }
            return;
        }
        bool isGap = false;
        nextGroundZ += groundLength;
        if (Random.value < currentGapChance && spawnedGroundCount < totalGroundsToFinish - 1)
        {
            isGap = true;
            float gapLength = Random.Range(currentMinGap, currentMaxGap);
            float gapStart = nextGroundZ - groundLength;
            SpawnWoodForGap(gapStart, gapLength);
            nextGroundZ += gapLength;
        }
        if (!isGap && CanSpawnGate())
        {
            SpawnGateAtCenter(groundCenterZ);
        }
        TrySpawnUniformGems(groundStartZ, groundStartZ + groundLength);
    }
    private bool CanSpawnGate()
    {
        if (spawnedGateCount >= totalGatesToSpawn) return false;
        if (isFinishLineSpawned || spawnedGroundCount >= totalGroundsToFinish) return false;
        if (spawnedGroundCount - lastGateGroundIndex < 2) return false;
        return true;
    }
    private void SpawnGateAtCenter(float groundCenterZ)
    {
        if (gatePrefab == null) return;
        Vector3 gatePosition = new Vector3(0f, 0.5f, groundCenterZ);
        Instantiate(gatePrefab, gatePosition, Quaternion.identity);
        spawnedGateCount++;
        lastGateGroundIndex = spawnedGroundCount;
    }
    private void TrySpawnUniformGems(float startZ, float endZ)
    {
        if (gemPrefab == null) return;
        for (int g = 0; g < totalGemsToSpawn; g++)
        {
            float targetGemZ = (g + 1) * gemInterval;
            if (targetGemZ >= startZ && targetGemZ < endZ)
            {
                float finalZ = targetGemZ + Random.Range(-1f, 1f);
                float finalX = Random.Range(-gemXRange, gemXRange);
                Vector3 gemPos = new Vector3(finalX, 0.5f, finalZ);
                Instantiate(gemPrefab, gemPos, Quaternion.identity);
            }
        }
    }
    private void SpawnWoodForGap(float gapStartZ, float gapLength)
    {
        if (woodPool == null) return;
        int woodCount = Random.Range(currentMinWoods, currentMaxWoods + 1); 
        float margin = 0.5f;
        float availableSpan = Mathf.Max(1f, gapLength - (margin * 2f));
        float zStep = availableSpan / (woodCount + 1);
        float safeXRange = Mathf.Min(woodXRange, 1.4f);
        float lastX = -99f; 
        for (int i = 0; i < woodCount; i++)
        {
            GameObject wood = woodPool.GetWood();
            if (wood == null) return;
            float woodZ = gapStartZ + margin + ((i + 1) * zStep) + Random.Range(-0.1f, 0.1f);
            float randomX = 0f;
            int safetyCheck = 0;
            do
            {
                randomX = Random.Range(-safeXRange, safeXRange);
                safetyCheck++;
            } 
            while (Mathf.Abs(randomX - lastX) < 0.8f && safetyCheck < 5);
            lastX = randomX;
            wood.transform.position = new Vector3(randomX, 0.5f, woodZ);
            wood.tag = "WoodPickup";
        }
    }
}
