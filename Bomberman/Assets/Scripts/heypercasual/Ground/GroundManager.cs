using UnityEngine;
using System.Collections.Generic;
public class GroundManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject woodPickupPrefab;
    [SerializeField] private float bridgeWoodLength = 0.4f;
    [SerializeField] private int woodPoolSize = 50;
    [SerializeField] private Material groundMaterial;
    [SerializeField] private float groundWidth = 6f;
    [SerializeField] private float groundLength = 10f;
    [SerializeField] private int initialGroundsCount = 3;
    [SerializeField] private float chanceOfGap = 0.5f;
    [SerializeField] private float minGapLength = 4f;
    [SerializeField] private float maxGapLength = 8f;
    private float spawnZPosition = 0f;
    private List<GameObject> woodPool = new List<GameObject>();
    private void Start()
    {
        CreateWoodPool();
        for (int i = 0; i < initialGroundsCount; i++)
        {
            SpawnGroundPiece(false);
        }
    }
    private void Update()
    {
        if (player != null &&
            player.position.z + 40f > spawnZPosition)
        {
            bool shouldHaveGap = Random.value < chanceOfGap;
            SpawnGroundPiece(shouldHaveGap);
        }
    }
    private void CreateWoodPool()
    {
        if (woodPickupPrefab == null)
        {
            Debug.LogWarning("Wood Pickup Prefab is not assigned!");
            return;
        }
        for (int i = 0; i < woodPoolSize; i++)
        {
            GameObject wood = Instantiate(woodPickupPrefab, transform);
            wood.SetActive(false);
            woodPool.Add(wood);
        }
    }
    private void SpawnGroundPiece(bool createGap)
    {
        GameObject newGround = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newGround.name = "GeneratedGround";
        Vector3 groundPos = new Vector3(0f, -0.5f, spawnZPosition + (groundLength / 2f));
        newGround.transform.position = groundPos;
        newGround.transform.localScale = new Vector3(groundWidth, 1f, groundLength);
        int groundLayerIndex = LayerMask.NameToLayer("Ground");
        if (groundLayerIndex != -1)
        {
            newGround.layer = groundLayerIndex;
        }
        if (groundMaterial != null)
        {
            newGround.GetComponent<Renderer>().material = groundMaterial;
        }
        float gapSize = 0f;
        if (createGap)
        {
            gapSize = Random.Range(minGapLength, maxGapLength);
        }
        int requiredWoodCount = 0;
        if (gapSize > 0f)
        {
            requiredWoodCount = Mathf.CeilToInt(gapSize / bridgeWoodLength);
        }
        SpawnWoodPickups(groundPos.z, requiredWoodCount);
        spawnZPosition += groundLength;
        spawnZPosition += gapSize;
    }
    private void SpawnWoodPickups(float centerZ, int woodCount)
    {
        if (woodPickupPrefab == null)
            return;
        if (woodCount <= 0)
            return;
        float startZ = centerZ - (groundLength / 2f) + 1f;
        float endZ = centerZ + (groundLength / 2f) - 1f;
        float minX = -(groundWidth / 2f) + 0.5f;
        float maxX = (groundWidth / 2f) - 0.5f;
        int spawnedCount = 0;
        for (int i = 0; i < woodPool.Count; i++)
        {
            if (spawnedCount >= woodCount)
                break;
            GameObject wood = woodPool[i];
            if (wood.activeSelf)
                continue;
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(startZ, endZ);
            Vector3 woodPosition = new Vector3(randomX, 0.5f, randomZ);
            wood.transform.position =
                woodPosition;
            wood.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            wood.SetActive(true);
            spawnedCount++;
        }
        if (spawnedCount < woodCount)
        {
            Debug.LogWarning("Wood Pool is not large enough! " + "Required: " + woodCount + " | Spawned: " + spawnedCount);
        }
    }
}
