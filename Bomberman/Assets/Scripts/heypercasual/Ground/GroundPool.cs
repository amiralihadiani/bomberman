using UnityEngine;

public class GroundPool : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject woodPickupPrefab;
    [SerializeField] private PlayerBridgeCollector playerCollector;
    [SerializeField] private int groundCount = 3;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float minGap = 2f;
    [SerializeField] private float maxGap = 6f;
    [SerializeField, Range(0f, 1f)] private float gapChance = 0.6f;
    [SerializeField] private float woodYOffset = 4f; 
    [SerializeField] private float roadWidth = 4f;  
    private Transform[] grounds;
    private float groundLength;
    private void Start()
    {
        grounds = new Transform[groundCount];
        GameObject firstGround = Instantiate(groundPrefab, transform);
        grounds[0] = firstGround.transform;
        firstGround.transform.position = transform.position;
        Renderer renderer = firstGround.GetComponentInChildren<Renderer>();
        groundLength = (renderer != null) ? renderer.bounds.size.z : 10f;
        for (int i = 1; i < groundCount; i++)
        {
            GameObject ground = Instantiate(groundPrefab, transform);
            grounds[i] = ground.transform;
            Transform previousGround = grounds[i - 1];
            float gap = GetRandomGap();
            ground.transform.position = new Vector3(previousGround.position.x, previousGround.position.y, previousGround.position.z + groundLength + gap);
            SpawnExactWoodForGap(previousGround, gap);
        }
    }
    private void Update()
    {
        if (grounds == null) return;
        MoveGrounds();
        RecycleGrounds();
    }
    private void MoveGrounds()
    {
        Vector3 movement = Vector3.back * (moveSpeed * Time.deltaTime);
        foreach (Transform ground in grounds)
        {
            ground.position += movement;
        }
    }
    private void RecycleGrounds()
    {
        foreach (Transform ground in grounds)
        {
            if (ground.position.z < player.position.z - groundLength)
            {
                for (int i = ground.childCount - 1; i >= 0; i--)
                {
                    Destroy(ground.GetChild(i).gameObject);
                }
                Transform lastGround = GetLastGround();
                float gap = GetRandomGap();
                ground.position = new Vector3(lastGround.position.x, lastGround.position.y, lastGround.position.z + groundLength + gap);
                SpawnExactWoodForGap(ground, gap);
            }
        }
    }
    private void SpawnExactWoodForGap(Transform ground, float gap)
    {
        if (woodPickupPrefab == null) return;
        float stepDistance = (playerCollector != null) ? playerCollector.StepDistance : 0.5f;
        int neededWood = (gap > 0f) ? Mathf.CeilToInt(gap / stepDistance) + Random.Range(1, 3) : Random.Range(1, 3);
        float spacing = (groundLength - 2f) / Mathf.Max(neededWood, 1);
        for (int i = 0; i < neededWood; i++)
        {
            float zPos = (-groundLength / 2f + 1f) + (i * spacing);
            float xOffset = Random.Range(-roadWidth / 2f, roadWidth / 2f);
            Vector3 pickupPos = new Vector3(ground.position.x + xOffset, ground.position.y + woodYOffset, ground.position.z + zPos);
            GameObject pickup = Instantiate(woodPickupPrefab, pickupPos, Quaternion.identity, ground);
            pickup.tag = "WoodPickup";
        }
    }
    private Transform GetLastGround()
    {
        Transform lastGround = grounds[0];
        foreach (Transform ground in grounds)
        {
            if (ground.position.z > lastGround.position.z)
            {
                lastGround = ground;
            }
        }
        return lastGround;
    }
    private float GetRandomGap()
    {
        if (Random.value > gapChance) return 0f;
        return Random.Range(minGap, maxGap);
    } 
}
