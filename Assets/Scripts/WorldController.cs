
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public Moveable Spawn;
    public Transform SystemSpawn;
    public ValueReference SpawnsCount;
    public ValueReference SystemSpeed;

    WorldGrid grid = new WorldGrid() { X = 8, Y = 8 };
    float avg = 0f;
    float ms = 0f;
    int count = 0;
    MoveableEntity[] entities;
    Transform[] transforms;
    bool systemReady = false;

    void Start()
    {
        var spawnsCount = (int)SpawnsCount.FloatVariable;
        entities = new MoveableEntity[spawnsCount + 1];
        transforms = new Transform[spawnsCount + 1];
        for (int i = 0; i < spawnsCount; i++)
        {
            transforms[i] = Instantiate(SystemSpawn, transform);
            transforms[i].GetComponent<SpriteRenderer>().color = RandomColor();
            entities[i].TargetPosition = GetRandomWorldPosition();
            entities[i].CurrentPosition = transforms[i].position;
        }
        systemReady = true;
    }

    Color RandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    Vector2 GetRandomWorldPosition()
    {
        var newDir = new Vector2(Random.Range(-grid.X, grid.X), Random.Range(-grid.Y, grid.Y));
        return newDir != Vector2.zero ? newDir : GetRandomWorldPosition();
    }

    void Update()
    {
        if (!systemReady) return;
        count++;
        avg += Time.deltaTime;
        if (count == 30) { ms = avg /= 30; count = 0; avg = 0; }

        for (int i = 0; i < entities.Length - 1; i++) MoveEntity(i);
    }

    void MoveEntity(int index)
    {
        Vector2 dir = entities[index].TargetPosition - entities[index].CurrentPosition;
        entities[index].CurrentPosition += dir * SystemSpeed.FloatVariable;
        transforms[index].position = entities[index].CurrentPosition;
        if (dir.magnitude < 0.1f)
            entities[index].TargetPosition = GetRandomWorldPosition();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), ms.ToString("F3") + "ms");
    }
}
