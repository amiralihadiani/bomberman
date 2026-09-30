using UnityEngine;

public class PlayerBridgeCollector : MonoBehaviour
{
    [SerializeField] private GameObject bridgeWoodPrefab; 
    [SerializeField] private Transform groundCheckPoint;  
    [SerializeField] private LayerMask groundLayer;      
    [SerializeField] private float rayDistance = 1.0f;
    private int collectedWoodCount = 0;
    private float lastPlankZ = -999f;
    private bool isBuildingBridge = false;
    private float stepZDistance = 0.4f; 
    public float StepDistance => stepZDistance;
    public int CollectedWoodCount => collectedWoodCount;
    private void Start()
    {
        if (bridgeWoodPrefab != null)
        {
            stepZDistance = bridgeWoodPrefab.transform.localScale.z;
            if (stepZDistance <= 0.05f) stepZDistance = 0.4f;
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
        Vector3 origin = groundCheckPoint != null ? groundCheckPoint.position : transform.position;
        bool isGrounded = Physics.Raycast(origin, Vector3.down, rayDistance, groundLayer);
        if (!isGrounded)
        {
            if (collectedWoodCount > 0)
            {
                if (!isBuildingBridge || (transform.position.z - lastPlankZ) >= stepZDistance)
                {
                    BuildPlank();
                }
            }
            else
            {
                isBuildingBridge = false;
            }
        }
        else
        {
            isBuildingBridge = false;
            lastPlankZ = -999f;
        }
    }
    private void BuildPlank()
    {
        collectedWoodCount--;
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y - 0.45f, transform.position.z);
        Quaternion spawnRotation = Quaternion.identity;
        Instantiate(bridgeWoodPrefab, spawnPosition, spawnRotation);
        lastPlankZ = transform.position.z;
        isBuildingBridge = true;
    } 
}
