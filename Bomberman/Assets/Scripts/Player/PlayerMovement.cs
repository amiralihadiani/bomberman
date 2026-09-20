using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidbodyPlayer;
    private Vector2 direction = Vector2.down;

    [SerializeField] private float speed = 5f;

    private void Awake()
    {
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Movement();
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbodyPlayer.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;

        rigidbodyPlayer.MovePosition(position + translation);
    }

    private void Movement()
    {
        if (Keyboard.current.aKey.isPressed)
        {
            SetDirection(Vector2.left);
        }
        else if (Keyboard.current.dKey.isPressed)
        {
            SetDirection(Vector2.right);
        }
        else if (Keyboard.current.wKey.isPressed)
        {
            SetDirection(Vector2.up);
        }
        else if (Keyboard.current.sKey.isPressed)
        {
            SetDirection(Vector2.down);
        }
        else
        {
            SetDirection(Vector2.zero);
        }
    } 

    private void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }
}