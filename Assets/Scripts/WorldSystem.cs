using UnityEngine;
using Unity.Entities;

class WorldSystem : ComponentSystem
{
    public struct WorldGrid { public int X; public int Y; };
    public static WorldGrid Grid = new WorldGrid() { X = 8, Y = 8 };

    struct Components
    {
        public Moveable Moveable;
    }

    protected override void OnUpdate()
    {
        foreach (var a in GetEntities<Components>())
        {
            var dir = a.Moveable.TargetPosition - (Vector2)a.Moveable.transform.position;
            var velocity = dir.normalized.magnitude * 0.01f;
            a.Moveable.transform.position += (Vector3)dir * velocity;
            if (dir.magnitude < 0.1f)
                a.Moveable.TargetPosition = GetRandomWorldPosition();
        }
    }

    static Vector2 GetRandomWorldPosition()
    {
        var newDir = new Vector2(Random.Range(-Grid.X, Grid.X),
            Random.Range(-Grid.Y, Grid.Y));
        return newDir != Vector2.zero ? newDir : GetRandomWorldPosition();
    }
}