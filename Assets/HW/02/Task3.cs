using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

//создайте задачу типа IJobForTransform, которая будет вращать указанные
//Transform вокруг своей оси с заданной скоростью.

public class Task3 : MonoBehaviour
{
    [SerializeField] Transform _transform;
    [SerializeField] GameObject _prefab;

    Quaternion _rotation;
    [SerializeField] private int _speed = 5;
   //TransformAccess _transformAccess;
    TransformAccessArray _accessArray;
    NativeArray<int> _angle;


    void Start()
    {
        Transform[] transforms = new Transform [] { _transform };
        _accessArray = new TransformAccessArray(transforms, 1);
        // _rotation = _transform.rotation;
        _angle = new NativeArray<int>(1, Allocator.TempJob);
    }

    private void Update()
    {
        JobTask3 rotationJob = new JobTask3()
        {
            Rotation = _rotation,
            Speed = _speed,
            angle = _angle
        };

        Debug.Log("_angle - " + _angle);
        JobHandle moveHandle = rotationJob.Schedule(_accessArray);
        moveHandle.Complete();

    }

    private void OnDestroy()
    {
        if (_accessArray.isCreated)
        {
            _accessArray.Dispose();
        }    
    }

}

public struct JobTask3 : IJobParallelForTransform
{
    public Quaternion Rotation;
    public int Speed;
    public NativeArray<int> angle;

    [ReadOnly]
    public float DeltaTime;

    public void Execute(int index, TransformAccess transform)
    {
        //angle += Speed;
        transform.localRotation = Quaternion.AngleAxis(angle[0], Vector3.up);
        angle[0] = angle[0] == 180 ? 0 : angle[0] + Speed;

        Debug.Log("Execute");

    }
}
