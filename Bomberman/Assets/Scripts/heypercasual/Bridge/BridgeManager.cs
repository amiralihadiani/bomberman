using UnityEngine;
using System.Collections.Generic;

public class BridgeManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private BridgeTilePool bridgeTilePool;
    [SerializeField] private float woodLength = 0.4f;      
    [SerializeField] private float bridgeYPosition = -0.5f;   
    [SerializeField] private Vector3 woodRotation = new Vector3(0f, 0f, 0f);
    [SerializeField] private int maxPlanksOnBridge = 10;    
    [SerializeField] private int collectedWoodCount = 0;    
    private Queue<GameObject> activeBridgePlanks = new Queue<GameObject>();
    private float nextSpawnZ = 0f;
    private bool isBuildingBridge = false;
    public void AddWood()
    {
        collectedWoodCount++;
        Debug.Log("Wood Collected! Total: " + collectedWoodCount);
    }
    private void Update()
    {
        CheckAndBuildBridge();
    }
    private void CheckAndBuildBridge()
    {
        if (player == null || bridgeTilePool == null) return;
        Vector3 rayOrigin = player.position + Vector3.forward * 0.4f + Vector3.up * 0.5f;
        int groundLayer = LayerMask.GetMask("Ground");
        bool isGrounded = Physics.Raycast(rayOrigin, Vector3.down, 2f, groundLayer);
        if (!isGrounded)
        {
            if (!isBuildingBridge)
            {
                isBuildingBridge = true;
                nextSpawnZ = player.position.z + 0.1f;
            }
            while (player.position.z >= nextSpawnZ - 0.1f && collectedWoodCount > 0)
            {
                BuildPlank();
            }
        }
        else
        {
            if (isBuildingBridge)
            {
                ClearActiveBridgePlanks();
                isBuildingBridge = false;
            }
        }
    }
    private void BuildPlank()
    {
        collectedWoodCount--;
        if (activeBridgePlanks.Count >= maxPlanksOnBridge)
        {
            GameObject oldestPlank = activeBridgePlanks.Dequeue();
            if (oldestPlank != null)
            {
                oldestPlank.SetActive(false);
            }
        }
        Vector3 spawnPos = new Vector3(0f, bridgeYPosition, nextSpawnZ);
        GameObject newPlank = bridgeTilePool.GetTileAtPosition(spawnPos, woodRotation);
        if (newPlank != null)
        {
            activeBridgePlanks.Enqueue(newPlank);
        }
        nextSpawnZ += woodLength;
    }
    private void ClearActiveBridgePlanks()
    {
        while (activeBridgePlanks.Count > 0)
        {
            GameObject plank = activeBridgePlanks.Dequeue();
            if (plank != null)
            {
                plank.SetActive(false);
            }
        }
    }
}
