using UnityEngine;

public class WoodSpawner : MonoBehaviour
{
    [SerializeField] private GameObject woodPickupPrefab;
    [SerializeField] private float gapLength = 4f; 
    [SerializeField] private float plankZSize = 0.4f; 
    private void Start()
    {
        SpawnRequiredWood();
    }
    private void SpawnRequiredWood()
    {
        int requiredWood = Mathf.CeilToInt(gapLength / plankZSize);

        for (int i = 0; i < requiredWood; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(0, 0.5f, (i * 0.8f) - (requiredWood * 0.4f));
            GameObject wood = Instantiate(woodPickupPrefab, spawnPos, Quaternion.identity);
            wood.transform.SetParent(transform, true);
            wood.transform.localScale = woodPickupPrefab.transform.localScale;
        }
    } 
}
