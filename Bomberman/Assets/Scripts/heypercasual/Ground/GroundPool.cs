using UnityEngine;
using System.Collections.Generic;
public class GroundPool : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private int poolSize = 7;
    [SerializeField] private float groundLength = 10f;
    [SerializeField] private int safeStartGrounds = 2;
    [SerializeField] private float minGap = 3f;
    [SerializeField] private float maxGap = 7f;
    [SerializeField] private float gapChance = 0.5f;
    [SerializeField] private WoodPool woodPool;
    [SerializeField] private float woodSpacing = 1f;
    [SerializeField] private GameObject gatePrefab; 
    [SerializeField] private int totalGatesToSpawn = 6; 
    [SerializeField] private GameObject finishLinePrefab;
    [SerializeField] private int totalGroundsToFinish = 15;
    private GameObject[] groundPool;
    private int firstGroundIndex = 0;
    private float nextGroundZ = 0f;
    private int spawnedGroundCount = 0;
    private int spawnedGateCount = 0;
    private int lastGateGroundIndex = -2; 
    private bool isFinishLineSpawned = false;
    private void Start()
    {
        CreateGroundPool();
        SpawnInitialGrounds();
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
            float groundCenterZ = currentZ + (groundLength / 2f);
            groundPool[i].transform.position = new Vector3(0f, -0.5f, groundCenterZ);
            currentZ += groundLength;
            bool isGap = false;
            if (i >= safeStartGrounds && Random.value < gapChance && spawnedGroundCount < totalGroundsToFinish - 1)
            {
                isGap = true;
                float gapLength = Random.Range(minGap, maxGap);
                float gapStart = currentZ - groundLength;
                SpawnWoodForGap(gapStart, gapLength);
                currentZ += gapLength;
            }
            if (!isGap && CanSpawnGate())
            {
                SpawnGateAtCenter(groundCenterZ);
            }
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
        if (Random.value < gapChance && spawnedGroundCount < totalGroundsToFinish - 1)
        {
            isGap = true;
            float gapLength = Random.Range(minGap, maxGap);
            float gapStart = nextGroundZ - groundLength;
            SpawnWoodForGap(gapStart, gapLength);
            nextGroundZ += gapLength;
        }
        if (!isGap && CanSpawnGate())
        {
            SpawnGateAtCenter(groundCenterZ);
        }
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
    private void SpawnWoodForGap(float groundStartZ, float gapLength)
    {
        if (woodPool == null) return;
        int woodCount = Random.Range(5, 10); 
        float startZ = groundStartZ + 1f;
        for (int i = 0; i < woodCount; i++)
        {
            GameObject wood = woodPool.GetWood();
            if (wood == null) return;
            wood.transform.position = new Vector3(0f, 0.5f, startZ + (i * woodSpacing));
            wood.tag = "WoodPickup";
        }
    } 
}
