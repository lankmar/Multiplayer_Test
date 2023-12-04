using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

//Часть 2.Cоздайте задачу типа IJobParallelFor, которая будет принимать данные в
//виде двух контейнеров: Positions и Velocities — типа NativeArray<Vector3>. Также
//создайте массив FinalPositions типа NativeArray<Vector3>.
//Сделайте так, чтобы в результате выполнения задачи в элементы массива
//FinalPositions были записаны суммы соответствующих элементов массивов Positions
//и Velocities.
//Вызовите выполнение созданной задачи из внешнего метода и выведите в консоль
//результат.

public class Task2 : MonoBehaviour
{
    NativeArray<Vector3> _positions;
    NativeArray<Vector3> _velocities;
    NativeArray<Vector3> _finalPositions;

    int _arrayLength = 30;

    private JobHandle handle;


    // Start is called before the first frame update
    void Start()
    {
        _positions = new NativeArray<Vector3>(_arrayLength, Allocator.TempJob);
        _velocities = new NativeArray<Vector3>(_arrayLength, Allocator.TempJob);
        _finalPositions = new NativeArray<Vector3>(_arrayLength, Allocator.TempJob);
        SetArrayValue(_positions);
        SetArrayValue(_velocities);

        JobTask2 job = new JobTask2()
        {
            Positions = _positions,
            Velocities = _velocities,
            FinalPositions = _finalPositions
        };

        handle = job.Schedule(_arrayLength, 0);
        handle.Complete();


            _finalPositions.Dispose();
            _velocities.Dispose();
    }



    private void SetArrayValue(NativeArray<Vector3> vectors)
    {     
        for (int i = 0; i < vectors.Length; i++)
        {
            vectors[i] = new Vector3(Random.Range(1,100), Random.Range(1, 100), Random.Range(1, 100));
        }
    }

}

public struct JobTask2 : IJobParallelFor
{

    public NativeArray<Vector3> Positions;
    public NativeArray<Vector3> Velocities;

    public NativeArray<Vector3> FinalPositions;

    void IJobParallelFor.Execute(int index)
    {
        FinalPositions[index] = Positions[index] + Velocities[index];
        Debug.Log(FinalPositions[index]);
    }
}

