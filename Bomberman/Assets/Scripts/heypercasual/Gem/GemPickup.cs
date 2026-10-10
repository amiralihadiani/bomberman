using UnityEngine;

public class GemPickup : MonoBehaviour
{
    [SerializeField] private int gemValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddGems(gemValue);
            }
            gameObject.SetActive(false);
        } 
    } 
}
