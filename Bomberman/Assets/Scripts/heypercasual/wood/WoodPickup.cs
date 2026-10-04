using UnityEngine;

public class WoodPickup : MonoBehaviour
{
    private BridgeManager bridgeManager;
    private void Start()
    {
        bridgeManager = FindFirstObjectByType<BridgeManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (bridgeManager == null) 
                bridgeManager = FindFirstObjectByType<BridgeManager>();
            if (bridgeManager != null)
            {
                bridgeManager.AddWood();
            }
            gameObject.SetActive(false);
        }
    } 
}
