using System;
using System.Threading;
using UnityEngine;
using UniRx;
using NaughtyAttributes;
using Cysharp.Threading.Tasks;

public class ResourcesManager : MonoBehaviour
{
    public ReactiveProperty<int> Energy = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> TotalCountOfRobots = new ReactiveProperty<int>(5);
    public ReactiveProperty<int> CountOfAvailableRobots = new ReactiveProperty<int>(5);
    public ReactiveProperty<int> CountOfDays = new ReactiveProperty<int>(1);
    public ReactiveProperty<int> Wood = new ReactiveProperty<int>(50);
    public ReactiveProperty<int> Coal = new ReactiveProperty<int>(30);

    [SerializeField] private float _minTimeForInvestigate = 1f;
    [SerializeField] private float _maxTimeForInvestigate = 3f;

    private readonly object _lock = new object();

    [Button]
    public async UniTask SendRobotToInvestigate()
    {
        lock (_lock)
        {
            if (CountOfAvailableRobots.Value <= 0)
            {
                Debug.Log("ResourcesManager: SendRobotToInvestigate: No available robots to send");
                return;
            }
            CountOfAvailableRobots.Value--;
        }
        Debug.Log("ResourcesManager: SendRobotToInvestigate: Robot sent to investigate");
        
        // Дальше идёт асинхронная логика
        float waitTime = UnityEngine.Random.Range(_minTimeForInvestigate, _maxTimeForInvestigate);
        await UniTask.Delay(TimeSpan.FromSeconds(waitTime));

        lock (_lock)
        {
            CountOfAvailableRobots.Value++;
        }
        Debug.Log("ResourcesManager: SendRobotToInvestigate: Robot returned from investigation");
    }

    public void ChangeCountOfEnergy(int summand)
    {
        Energy.Value += summand;
    }

    public void ChangeCountOfCoal(int summand)
    {
        Coal.Value += summand;
    }
}
