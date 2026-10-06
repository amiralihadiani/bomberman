using UnityEngine;
using UnityEngine.InputSystem;

public class movement_player : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 8f;
    [SerializeField] private float sideSpeed = 6f;
    private Rigidbody playmovementrigidbody;
    private void Start()
    {
        playmovementrigidbody = GetComponent<Rigidbody>();
        if (playmovementrigidbody != null)
        {
            playmovementrigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            playmovementrigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            playmovementrigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
    private void FixedUpdate()
    {
        playerMovement();
    }
    private void playerMovement()
    {
        if (playmovementrigidbody == null)
            return;
        float horizontalInput = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                horizontalInput = -1f;
            }
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                horizontalInput = 1f;
            }
        }
        Vector3 currentVelocity = playmovementrigidbody.linearVelocity;
        float yVelocity = currentVelocity.y > 0.1f ? 0f : currentVelocity.y;
        Vector3 targetVelocity = new Vector3(horizontalInput * sideSpeed, yVelocity, forwardSpeed);
        playmovementrigidbody.linearVelocity = targetVelocity;
    } 
}
