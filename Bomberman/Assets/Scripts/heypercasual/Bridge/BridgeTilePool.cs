using UnityEngine;

public class BridgeTilePool : MonoBehaviour
{
    [SerializeField] private GameObject bridgeTilePrefab;
    [SerializeField] private int poolSize = 50;
    private GameObject[] tilePool;
    private void Awake()
    {
        CreatePool();
    }
    private void CreatePool()
    {
        tilePool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            tilePool[i] = InstantiateTile(i);
        }
    }
    private GameObject InstantiateTile(int index)
    {
        GameObject tile = Instantiate(bridgeTilePrefab, transform);
        tile.name = "BridgeTile_" + index;
        int groundLayer = LayerMask.NameToLayer("Ground");
        if (groundLayer != -1) tile.layer = groundLayer;
        tile.SetActive(false);
        return tile;
    }
    public GameObject GetTileAtPosition(Vector3 position, Vector3 rotationEuler)
    {
        for (int i = 0; i < tilePool.Length; i++)
        {
            if (tilePool[i] == null)
            {
                tilePool[i] = InstantiateTile(i);
            }
            if (!tilePool[i].activeInHierarchy)
            {
                tilePool[i].transform.position = position;
                tilePool[i].transform.rotation = Quaternion.Euler(rotationEuler);
                tilePool[i].SetActive(true);
                return tilePool[i];
            }
        }
        Debug.LogWarning("BRIDGE TILE POOL IS FULL!");
        return null;
    } 
}
