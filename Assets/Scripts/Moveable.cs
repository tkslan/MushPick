using UnityEngine;

public class Moveable : MonoBehaviour
{
    [SerializeField] ValueReference SpeedValue;

    private SpriteRenderer sprite;
    [HideInInspector]public Vector2 TargetPosition = Vector2.zero;

    public Vector2 GetRandomWorldPosition()
    {
        var newDir = new Vector2(Random.Range(-8,8),
            Random.Range(-8, 8));
        return newDir != Vector2.zero ? newDir : GetRandomWorldPosition();
    }

    public void SetColor(Color color)
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = color;
    }

    public void SetNewTarget(Vector2 position)
    {
        TargetPosition = position;
    }

    public void SetNewRandomTarget()
    {
        SetNewTarget(GetRandomWorldPosition());
    }

    public void Stop()
    {
        TargetPosition = Vector2.zero;
    }

    public void MoveTick()
    {
        var dir = TargetPosition - (Vector2)transform.position;
        var velocity = dir.normalized.magnitude * SpeedValue.FloatVariable;
        transform.position += (Vector3)dir * velocity;
        if (dir.magnitude < 0.1f) SetNewRandomTarget();
    }

    private void Update()
    {
        if (TargetPosition!= Vector2.zero) MoveTick();
    }
}
