using UnityEngine;

public class GroundPool : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private int poolSize = 7;
    [SerializeField] private float groundLength = 10f;
    [SerializeField] private int safeStartGrounds = 3;
    [SerializeField] private float minGap = 3f;
    [SerializeField] private float maxGap = 7f;
    [SerializeField] private float gapChance = 0.6f;
    [SerializeField] private WoodPool woodPool;
    [SerializeField] private float woodSpacing = 1f;
    [SerializeField] private GameObject finishLinePrefab;
    [SerializeField] private int totalGroundsToFinish = 15; 
    private GameObject[] groundPool;
    private int firstGroundIndex = 0;
    private float nextGroundZ = 0f;
    private int spawnedGroundCount = 0; 
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
            groundPool[i].transform.position = new Vector3(0f, -0.5f, currentZ + (groundLength / 2f));
            currentZ += groundLength;
            spawnedGroundCount++;
            CheckAndSpawnFinishLine(currentZ - groundLength);
            if (i >= safeStartGrounds && Random.value < gapChance && !isFinishLineSpawned)
            {
                float gapLength = Random.Range(minGap, maxGap);
                SpawnWoodForGap(currentZ - groundLength, gapLength);
                currentZ += gapLength;
            }
        }
        nextGroundZ = currentZ;
    }
    private void RecycleGrounds()
    {
        if (player == null) return;
        if (isFinishLineSpawned) return;
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
        ground.transform.position = new Vector3(0f, -0.5f, nextGroundZ + (groundLength / 2f));
        nextGroundZ += groundLength;
        spawnedGroundCount++;
        CheckAndSpawnFinishLine(nextGroundZ - groundLength);
        if (Random.value < gapChance && !isFinishLineSpawned)
        {
            float gapLength = Random.Range(minGap, maxGap);
            SpawnWoodForGap(nextGroundZ - groundLength, gapLength);
            nextGroundZ += gapLength;
        }
    }
    private void CheckAndSpawnFinishLine(float groundStartZ)
    {
        if (spawnedGroundCount >= totalGroundsToFinish && !isFinishLineSpawned)
        {
            isFinishLineSpawned = true;

            if (finishLinePrefab != null)
            {
                Vector3 finishPos = new Vector3(0f, 0.5f, groundStartZ + groundLength - 1f);
                GameObject finishObj = Instantiate(finishLinePrefab, finishPos, Quaternion.identity);
                finishObj.tag = "FinishLine";
            }
            else
            {
                Debug.LogWarning("Finish Line Prefab is not assigned in GroundPool!");
            }
        }
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
