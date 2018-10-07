using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public bool UseSystem = false;
    public struct WorldGrid { public int X; public int Y; };
    public static WorldGrid Grid = new WorldGrid() { X = 8, Y = 8 };
    public Moveable Spawn;
    public Transform SystemSpawn;
    private Moveable[] moveables;
    public ValueReference SpawnsCount;
    float avg = 0f;
    float ms = 0f;
    int count = 0;

    public struct MoveableEntity
    {
        public int ID;
        public Transform Transform;
        public Vector2 TargetPosition;
    }
    MoveableEntity[] entities;
    bool SystemReady = false;
    // Start is called before the first frame update
    void Start()
    {
        var spawnsCount = (int)SpawnsCount.FloatVariable;

        moveables = new Moveable[spawnsCount + 1];
        entities = new MoveableEntity[spawnsCount + 1];
        for (int i = 0; i < spawnsCount; i++)
        {
            if (UseSystem)
            {

                Transform spawn = Instantiate(SystemSpawn, transform);
                entities[i] = new MoveableEntity()
                {
                    ID = i,
                    Transform = spawn,
                    TargetPosition = GetRandomWorldPosition()
                };
                spawn.gameObject.GetComponent<SpriteRenderer>().color = RandomColor();
            }
            else
            {
                var spawn = Instantiate(Spawn, transform);
                spawn.SetColor(RandomColor());
                moveables[i] = spawn;
                spawn.SetNewRandomTarget();
            }
        }
        SystemReady = UseSystem;
    }
    Color RandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    Vector2 GetRandomWorldPosition()
    {
        var newDir = new Vector2(Random.Range(-Grid.X, Grid.X),
            Random.Range(-Grid.Y, Grid.Y));
        return newDir != Vector2.zero ? newDir : GetRandomWorldPosition();
    }

    private void Update()
    {
        count++;
        avg += Time.deltaTime;
        if (count == 30) { ms = avg /= 30; count = 0; avg = 0; }
        if (SystemReady) SystemTick();
    }

    void SystemTick()
    {
        for (int i = 0; i < entities.Length-1; i++)
            MoveEntity(ref entities[i]);
    }

    void MoveEntity(ref MoveableEntity entity)
    {
        var dir = entity.TargetPosition - (Vector2)entity.Transform.position;
        if (dir.magnitude < 0.1f) 
            entity.TargetPosition = GetRandomWorldPosition();
        var velocity = dir.normalized.magnitude * 0.05f;
        entity.Transform.Translate(dir * velocity);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), ms.ToString("F3") + "ms");
    }
}
