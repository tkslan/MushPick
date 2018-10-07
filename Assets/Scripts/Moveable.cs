using UnityEngine;

public class Moveable : MonoBehaviour
{
    [SerializeField] ValueReference SpeedValue;
    public Vector2 TargetPosition = Vector2.zero;

    public bool DontUpdate = false;

    public Vector2 GetRandomWorldPosition()
    {
        var newDir = new Vector2(Random.Range(-WorldController.Grid.X, WorldController.Grid.X),
            Random.Range(-WorldController.Grid.Y, WorldController.Grid.Y));
        if (newDir != Vector2.zero)
            return newDir;
        else
            return GetRandomWorldPosition();
    }
    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    public void SetNewRandomTarget()
    {
        TargetPosition = GetRandomWorldPosition();
    }

    public void SetNewTarget(Vector2 position)
    {
        TargetPosition = position;
    }

    public void Stop()
    {
        TargetPosition = Vector2.zero;
    }

    public void MoveTick()
    {
        var dir = TargetPosition - (Vector2)transform.position;
        transform.Translate(dir * (SpeedValue.FloatVariable / dir.normalized.sqrMagnitude));

        if (dir.magnitude < 0.1f) SetNewRandomTarget();
    }

    private void Update()
    {
       // if (TargetPosition == Vector2.zero || DontUpdate) return;

        MoveTick();
    }
}
