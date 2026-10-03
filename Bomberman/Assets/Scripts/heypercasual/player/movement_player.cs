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
    }
    private void FixedUpdate()
    {
        if (playmovementrigidbody == null) return;
        float horizontalInput = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                horizontalInput = -1f;
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                horizontalInput = 1f;
        }
        Vector3 currentVel = playmovementrigidbody.linearVelocity;
        playmovementrigidbody.linearVelocity = new Vector3(horizontalInput * sideSpeed, currentVel.y, forwardSpeed);
    } 
}
