using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;

public partial class WorldController : MonoBehaviour
{
    public static WorldGrid Grid = new WorldGrid() { X = 8, Y = 8 };
    public Moveable Spawn;
    public Transform SystemSpawn;
    public ValueReference SpawnsCount;
    public ValueReference SystemSpeed;
    float avg = 0f;
    float ms = 0f;
    int count = 0;
    MoveableEntity[] entities;
    Transform[] transforms;
    bool SystemReady = false;
    // Start is called before the first frame update
    void Start()
    {
        var spawnsCount = (int)SpawnsCount.FloatVariable;
        entities = new MoveableEntity[spawnsCount + 1];
        transforms=new Transform[spawnsCount + 1];
        for (int i = 0; i < spawnsCount; i++)
        {
            Transform spawn = Instantiate(SystemSpawn, transform);
            transforms[i] = spawn;
            entities[i].TargetPosition = GetRandomWorldPosition();
            entities[i].CurrentPosition = transforms[i].position;
            spawn.gameObject.GetComponent<SpriteRenderer>().color = RandomColor();
        }
        SystemReady = true;
    }

    Color RandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    static Vector2 GetRandomWorldPosition()
    {
        var newDir = new Vector2(Random.Range(-Grid.X, Grid.X),
            Random.Range(-Grid.Y, Grid.Y));
        return newDir != Vector2.zero ? newDir : GetRandomWorldPosition();
    }

    private void Update()
    {
        if (!SystemReady) return;
        count++;
        avg += Time.deltaTime;
        if (count == 30) { ms = avg /= 30; count = 0; avg = 0; }
        SystemTick();
    }

    void SystemTick()
    {
        for (int i = 0; i < entities.Length - 1; i++)
            MoveEntity(i);
    }

    void MoveEntity(int i)
    {
        Vector2 dir = entities[i].TargetPosition - entities[i].CurrentPosition;
        entities[i].CurrentPosition += dir * SystemSpeed.FloatVariable;
        transforms[i].position = entities[i].CurrentPosition;
        if (dir.magnitude < 0.1f)
            entities[i].TargetPosition = GetRandomWorldPosition();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), ms.ToString("F3") + "ms");
    }

}
