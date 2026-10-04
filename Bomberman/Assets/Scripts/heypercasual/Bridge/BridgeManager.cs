using UnityEngine;

public class BridgeManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject bridgeTilePrefab; 
    [SerializeField] private float woodLength = 0.5f;       
    [SerializeField] private float bridgeYPosition = -0.5f;  
    [SerializeField] private Vector3 woodRotation = new Vector3(0f, 0f, 0f);
    [SerializeField] private int collectedWoodCount = 0;
    private float nextSpawnZ = 0f;
    private bool isBuildingBridge = false;

    public void AddWood()
    {
        collectedWoodCount++;
        Debug.Log("Wood Count: " + collectedWoodCount);
    }
    private void Update()
    {
        CheckAndBuildBridge();
    }
    private void CheckAndBuildBridge()
    {
        if (player == null || bridgeTilePrefab == null) return;
        Vector3 rayOrigin = player.position + Vector3.forward * 0.4f + Vector3.up * 0.5f;
        int groundLayer = LayerMask.GetMask("Ground");
        bool isGrounded = Physics.Raycast(rayOrigin, Vector3.down, 2f, groundLayer);
        if (!isGrounded && collectedWoodCount > 0)
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
        else if (isGrounded)
        {
            isBuildingBridge = false;
        }
    }
    private void BuildPlank()
    {
        collectedWoodCount--;
        Vector3 spawnPos = new Vector3(0f, bridgeYPosition, nextSpawnZ);
        GameObject bridgeTile = Instantiate(bridgeTilePrefab, spawnPos, Quaternion.Euler(woodRotation));
        int groundLayerIndex = LayerMask.NameToLayer("Ground");
        if (groundLayerIndex != -1)
        {
            bridgeTile.layer = groundLayerIndex;
        }
        nextSpawnZ += woodLength;
    } 
}
