using UnityEngine;

public class PlankMovement : MonoBehaviour
{
    public float moveSpeed ; 
    private void Update()
    {
        transform.Translate(Vector3.back * (moveSpeed * Time.deltaTime), Space.World);
    } 
}
