using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0f, 6f, -10f);
    [SerializeField] private float smoothSpeed = 5f;
    private void LateUpdate()
    {
        if (player == null) return;
        Vector3 targetPosition = new Vector3(offset.x, offset.y, player.position.z + offset.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    } 
}
