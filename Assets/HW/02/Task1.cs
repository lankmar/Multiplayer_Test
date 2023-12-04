using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Collections;
//Часть 1: Создайте задачу типа IJob, которая принимает данные в формате
//NativeArray<int> и в результате выполнения все значения более десяти делает
//равными нулю.
//Вызовите выполнение этой задачи из внешнего метода и выведите в консоль
//результат.
public class Task1 : MonoBehaviour
{
    NativeArray<int> _array;

    void Start()
    {
        _array = new NativeArray<int>(20, Allocator.Persistent);
        SetArrayValue(_array);

        MyJob myJob = new MyJob()
        {
            jobArray = _array
        };

        JobHandle jobHandle = myJob.Schedule();
        jobHandle.Complete();//awaits

        for (int i = 0; i < _array.Length; i++)
        {
            Debug.Log($"{++i} - {_array[i]}");
        }

        Dispose();
    }


    private void Dispose()
    { 
        _array.Dispose();
    
    }

    private void SetArrayValue(NativeArray<int> array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = Random.Range(1, 30);
        }
    }

}
public struct MyJob : IJob
{
    public NativeArray<int> jobArray;

    public void Execute()
    {
        for (int i = 0; i < jobArray.Length; i++)
        {
            if (jobArray[i] > 10)
                jobArray[i] = 0;

        }

    }
}
