using UnityEngine;

public class PlayerBridgeCollector : MonoBehaviour
{ 
    [SerializeField] private GameObject bridgeWoodPrefab;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayDistance = 2f;
    [SerializeField] private float playerMoveSpeed = 8f;
    [SerializeField] private int collectedWoodCount = 0;
    private bool isBuildingBridge = false;
    private float stepZDistance = 1f;
    private float timer = 0f;
    private float fixedBridgeYPosition;
    private float nextPlankZ;
    public int CollectedWoodCount => collectedWoodCount;
    private void Start()
    {
        if (bridgeWoodPrefab != null)
        {
            stepZDistance = bridgeWoodPrefab.transform.localScale.z;
            if (stepZDistance <= 0.05f)
                stepZDistance = 1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WoodPickup"))
        {
            collectedWoodCount++;
            Destroy(other.gameObject);
        }
    }
    private void Update()
    {
        CheckAndBuildBridge();
    }
    private void CheckAndBuildBridge()
    {
        Vector3 checkOrigin = transform.position + Vector3.forward * 0.6f;
        bool isGrounded = Physics.Raycast(checkOrigin, Vector3.down, rayDistance, groundLayer);
        if (!isGrounded)
        {
            if (collectedWoodCount > 0)
            {
                if (!isBuildingBridge)
                {
                    fixedBridgeYPosition = transform.position.y - 0.4f;
                    nextPlankZ = transform.position.z + stepZDistance;
                    isBuildingBridge = true;
                    timer = 0f;
                }
                float timeBetweenPlanks = stepZDistance / playerMoveSpeed;
                timer += Time.deltaTime;
                if (timer >= timeBetweenPlanks)
                {
                    BuildPlank();
                    timer = 0f;
                }
            }
        }
        else
        {
            isBuildingBridge = false;
            timer = 0f;
        }
    }
    private void BuildPlank()
    {
        if (collectedWoodCount <= 0)
            return;
        collectedWoodCount--;
        Vector3 spawnPosition = new Vector3(transform.position.x, fixedBridgeYPosition, nextPlankZ);
        GameObject plank = Instantiate(bridgeWoodPrefab, spawnPosition, Quaternion.identity);
        nextPlankZ += stepZDistance;
        int groundLayerIndex = LayerMask.NameToLayer("Ground");
        if (groundLayerIndex != -1)
        {
            plank.layer = groundLayerIndex;
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 checkOrigin = transform.position + Vector3.forward * 0.6f;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(checkOrigin, Vector3.down * rayDistance);
    } 
}
