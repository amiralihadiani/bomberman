using UnityEngine;
using UnityEngine.InputSystem;

public class movement_player : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 8f;
    [SerializeField] private float sideSpeed = 6f;
    private Rigidbody playmovementrigidbody;
    private bool isFinished = false;
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
        if (!isFinished)
        {
            playerMovement();
        }
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine") && !isFinished)
        {
            isFinished = true;
            if (playmovementrigidbody != null)
            {
                playmovementrigidbody.linearVelocity = Vector3.zero;
                playmovementrigidbody.isKinematic = true; 
            }
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LevelCompleted(); 
            }
            else
            {
                Debug.Log("مرحله با موفقیت تمام شد!");
            }
            this.enabled = false;
        }
    } 
}
