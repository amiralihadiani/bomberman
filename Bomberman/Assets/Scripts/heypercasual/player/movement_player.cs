using UnityEngine;
using UnityEngine.InputSystem;

public class movement_player : MonoBehaviour
{
    [SerializeField] private float moveSpeed ;

    private void Update()
    {
        playerMovement();
    }
    private void playerMovement()
    {
        float horizontal = 0f;
        switch (true)
        {
            case var _ when Keyboard.current.aKey.isPressed:
            case var _ when Keyboard.current.leftArrowKey.isPressed:
                horizontal = -1f;
                break;

            case var _ when Keyboard.current.dKey.isPressed:
            case var _ when Keyboard.current.rightArrowKey.isPressed:
                horizontal = 1f;
                break;

            default:
                horizontal = 0f;
                break;
        }
        Vector3 movement = Vector3.right * (horizontal * moveSpeed * Time.deltaTime);
        transform.Translate(movement, Space.World);
    } 
}
