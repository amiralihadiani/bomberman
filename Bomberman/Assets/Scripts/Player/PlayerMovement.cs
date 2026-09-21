using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidbodyPlayer;
    private Vector2 direction = Vector2.down;

    [SerializeField] private float speed = 5f;
    
    [SerializeField] AnimationPlayer spriteRendererUp;
    [SerializeField] AnimationPlayer spriteRendererDown;
    [SerializeField] AnimationPlayer spriteRendererLeft;
    [SerializeField] AnimationPlayer spriteRendererRight;
    private AnimationPlayer _activeSpriteRenderer;

    private void Awake()
    {
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        _activeSpriteRenderer = spriteRendererDown;

    }

    private void Update()
    {
        Movement();
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbodyPlayer.position;
        Vector2 translation = direction * (speed * Time.fixedDeltaTime);

        rigidbodyPlayer.MovePosition(position + translation);
    }

    private void Movement()
    {
        if (Keyboard.current.aKey.isPressed)
        {
            SetDirection(Vector2.left,spriteRendererLeft);
        }
        else if (Keyboard.current.dKey.isPressed)
        {
            SetDirection(Vector2.right,spriteRendererRight);
        }
        else if (Keyboard.current.wKey.isPressed)
        {
            SetDirection(Vector2.up,spriteRendererUp);
        }
        else if (Keyboard.current.sKey.isPressed)
        {
            SetDirection(Vector2.down,spriteRendererDown);
        }
        else
        {
            SetDirection(Vector2.zero,_activeSpriteRenderer);
        }
    } 

    private void SetDirection(Vector2 newDirection,AnimationPlayer spriteRenderer)
    {
        direction = newDirection;
        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        _activeSpriteRenderer = spriteRenderer;
        _activeSpriteRenderer.idle = direction == Vector2.zero;
    }
}