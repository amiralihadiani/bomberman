using UnityEngine;

public class WoodSpawner : MonoBehaviour
{
    [SerializeField] private GameObject woodPickupPrefab;
    [SerializeField] private float plankZSize = 0.4f; 
    [SerializeField] private float groundLength = 20f; 
    [SerializeField] private float roadWidth = 2.5f;   
    [SerializeField] private float marginZ = 1.5f;     
    private void Start()
    {
        SpawnRequiredWood();
    }
    private void SpawnRequiredWood()
    {
        float currentGapLength = GapManager.Instance != null ? GapManager.Instance.CurrentGapLength : 4f;
        int requiredWood = Mathf.CeilToInt(currentGapLength / plankZSize);
        float usableLength = groundLength - (marginZ * 2f);
        float stepZ = requiredWood > 1 ? usableLength / (requiredWood - 1) : 0f;
        float startZ = -usableLength / 2f;
        for (int i = 0; i < requiredWood; i++)
        {
            float randomX = Random.Range(-roadWidth / 2f, roadWidth / 2f);
            float currentZ = startZ + (i * stepZ);
            Vector3 spawnPos = transform.position + new Vector3(randomX, 0.5f, currentZ);
            GameObject wood = Instantiate(woodPickupPrefab, spawnPos, Quaternion.identity);
            wood.transform.SetParent(transform, true);
            wood.transform.localScale = woodPickupPrefab.transform.localScale;
        }
    } 
}
