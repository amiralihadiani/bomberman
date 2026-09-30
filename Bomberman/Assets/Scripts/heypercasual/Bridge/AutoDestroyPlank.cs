using UnityEngine;

public class AutoDestroyPlank : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f; 

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    } 
}
