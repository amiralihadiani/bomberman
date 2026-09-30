using UnityEngine;

public class PlayerBridgeCollector : MonoBehaviour
{ 
    [SerializeField] private GameObject bridgeWoodPrefab; 
    [SerializeField] private Transform groundCheckPoint;  
    [SerializeField] private LayerMask groundLayer;      
    [SerializeField] private float rayDistance = 0.2f; 
    [SerializeField] private float groundMoveSpeed = 8f; 
    private int collectedWoodCount = 0;
    private bool isBuildingBridge = false;
    private float stepZDistance = 0.4f;
    private float timer = 0f;
    private float fixedZPosition; 
    private float fixedBridgeYPosition; 
    public float StepDistance => stepZDistance;
    public int CollectedWoodCount => collectedWoodCount;
    private void Start()
    {
        fixedZPosition = transform.position.z;
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
        transform.position = new Vector3(transform.position.x, transform.position.y, fixedZPosition);
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
                if (!isBuildingBridge)
                {
                    fixedBridgeYPosition = transform.position.y - 0.45f;
                }
                float timeBetweenPlanks = stepZDistance / groundMoveSpeed;
                timer += Time.deltaTime;

                if (!isBuildingBridge || timer >= timeBetweenPlanks)
                {
                    BuildPlank();
                    timer = 0f;
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
            timer = 0f; 
        }
    }
    private void BuildPlank()
    {
        collectedWoodCount--;
        Vector3 spawnPosition = new Vector3(transform.position.x, fixedBridgeYPosition, transform.position.z + stepZDistance);
        GameObject plank = Instantiate(bridgeWoodPrefab, spawnPosition, Quaternion.identity);
        PlankMovement moveScript = plank.GetComponent<PlankMovement>();
        if (moveScript != null)
        {
            moveScript.moveSpeed = groundMoveSpeed;
        }
        plank.layer = LayerMask.NameToLayer("Ignore Raycast");
        isBuildingBridge = true;
    }
    public int CalculateRequiredWood(float gapLength)
    {
        return Mathf.CeilToInt(gapLength / stepZDistance);
    }
    private void OnDrawGizmos()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(groundCheckPoint.position, Vector3.down * rayDistance);
        }
    } 
}
