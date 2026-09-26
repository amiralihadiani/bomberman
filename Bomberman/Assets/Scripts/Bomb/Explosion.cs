using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AnimationPlayer start;
    public AnimationPlayer middle;
    public AnimationPlayer end;

    public void SetActiveRenderer(AnimationPlayer renderer)
    {
        start.enabled = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled = renderer == end;
    }

    public void SetDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void DestroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    } 
}
