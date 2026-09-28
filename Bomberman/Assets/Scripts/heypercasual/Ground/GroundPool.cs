using UnityEngine;

public class GroundPool : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject groundPrefab;

    [SerializeField] private int groundCount = 7;
    [SerializeField] private float moveSpeed = 8f;

    [SerializeField] private float minGap = 2f;
    [SerializeField] private float maxGap = 8f;

    [SerializeField, Range(0f, 1f)]
    private float gapChance = 1f;

    private Transform[] grounds;

    private float groundLength;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player is not assigned!");
            return;
        }

        if (groundPrefab == null)
        {
            Debug.LogError("Ground Prefab is not assigned!");
            return;
        }
        grounds = new Transform[groundCount];
        GameObject firstGround = Instantiate(groundPrefab, transform);
        grounds[0] = firstGround.transform;

        firstGround.transform.position = transform.position;

        Renderer renderer = firstGround.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            groundLength = renderer.bounds.size.z;
        }
        else
        {
            groundLength = 10f;
            Debug.LogWarning("Renderer پیدا نشد. Ground Length = 10 استفاده شد.");
        }

        for (int i = 1; i < groundCount; i++)
        {
            GameObject ground =
                Instantiate(groundPrefab, transform);

            grounds[i] = ground.transform;

            Transform previousGround = grounds[i - 1];

            float gap = GetRandomGap();

            ground.transform.position = new Vector3(previousGround.position.x, previousGround.position.y, previousGround.position.z + groundLength + gap);
        }
    }

    private void Update()
    {
        if (grounds == null)
            return;

        MoveGrounds();
        RecycleGrounds();
    }

    private void MoveGrounds()
    {
        Vector3 movement = Vector3.back * moveSpeed * Time.deltaTime;

        foreach (Transform ground in grounds)
        {
            ground.position += movement;
        }
    }

    private void RecycleGrounds()
    {
        foreach (Transform ground in grounds)
        {
            if (ground.position.z <
                player.position.z - groundLength)
            {
                Transform lastGround =
                    GetLastGround();
                float gap = GetRandomGap();

                ground.position = new Vector3(lastGround.position.x, lastGround.position.y, lastGround.position.z + groundLength + gap);
            }
        }
    }

    private Transform GetLastGround()
    {
        Transform lastGround = grounds[0];

        foreach (Transform ground in grounds)
        {
            if (ground.position.z >
                lastGround.position.z)
            {
                lastGround = ground;
            }
        }

        return lastGround;
    }

    private float GetRandomGap()
    {
        if (Random.value > gapChance)
            return 0f;
        return Random.Range(minGap, maxGap);
    } 
}
