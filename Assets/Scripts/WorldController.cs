using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public bool UseSystem = false;
    public struct WorldGrid { public int X; public int Y; };
    public static WorldGrid Grid = new WorldGrid() { X = 8, Y = 8 };
    public List<Moveable> Spawns;
    private Moveable[] moveables;
    public ValueReference SpawnsCount;
    float avg = 0f;
    float ms = 0f;
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        var spawnsCount = (int)SpawnsCount.FloatVariable;

        moveables = new Moveable[spawnsCount + 1];
        for (int i = 0; i < spawnsCount; i++)
        {
            var spawn = Instantiate(Spawns[0], transform);
            spawn.SetColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
            spawn.SetNewRandomTarget();
            moveables[i] = spawn;
        }
    }

    private void Update()
    {
        count++;
        avg += Time.deltaTime;
        if (count == 30) { ms = avg /= 30; count = 0; avg = 0; }
        
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), ms.ToString("F3") + "ms");
    }
}
