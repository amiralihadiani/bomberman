using UnityEngine;

public class WoodPool : MonoBehaviour
{
    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private int poolSize = 60;
    private GameObject[] woodPool;
    private void Awake()
    {
        CreatePool();
    }
    private void CreatePool()
    {
        woodPool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            GameObject wood = Instantiate(woodPrefab, transform);
            wood.name = "Wood_" + i;
            wood.SetActive(false);
            woodPool[i] = wood;
        }
    } 
    public GameObject GetWood()
    {
        for (int i = 0; i < woodPool.Length; i++)
        {
            if (!woodPool[i].activeInHierarchy)
            {
                woodPool[i].SetActive(true);
                return woodPool[i];
            }
        }
        Debug.LogWarning("WOOD POOL IS FULL!");
        return null;
    }
    public void ReturnWood(GameObject wood)
    {
        if (wood == null) return;
        wood.SetActive(false);
    }
    public GameObject GetWoodAtPosition(Vector3 position)
    {
        GameObject wood = GetWood();
        if (wood == null) return null;
        wood.transform.position = position;
        wood.transform.rotation = Quaternion.identity;
        return wood;
    } 
}
