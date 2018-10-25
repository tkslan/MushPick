
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Transforms;
using UnityEngine.Jobs;

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
    JobHandle handle;
    TransformAccessArray accessArray;
    NativeArray<MoveableEntity>  _entities;
    UpdateMove updateMove;
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
            entities[i].ID = i;
        }
        accessArray = new TransformAccessArray(transforms);
        _entities = new NativeArray<MoveableEntity>(entities, Allocator.Persistent);
        Debug.Log(accessArray.length + "-" + _entities.Length);
        systemReady = true;

    }
    private void OnDestroy()
    {
        accessArray.Dispose();
        _entities.Dispose();
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
        handle.Complete();
        updateMove = new UpdateMove()
        {
            Entities = _entities
        };
     
        handle = updateMove.Schedule(accessArray);
        count++;
        avg += Time.deltaTime;
        if (count == 30) { ms = avg /= 30; count = 0; avg = 0; }

        //for (int i = 0; i < entities.Length - 1; i++) MoveEntity(i);
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

    struct UpdateMove : IJobParallelForTransform
    {
        public NativeArray<MoveableEntity> Entities;

        void IJobParallelForTransform.Execute(int index, TransformAccess transform)
        {
            var entity = Entities[index];
            Vector3 dir = (Vector3)entity.TargetPosition - transform.position;
            transform.position += dir * 0.1f;
        }

        Vector2 GetRandomWorldPosition()
        {
            var size = 8;
            var newDir = new Vector2(Random.Range(-size, size), Random.Range(-size, size));
            return newDir != Vector2.zero ? newDir : GetRandomWorldPosition();
        }
    }

}
