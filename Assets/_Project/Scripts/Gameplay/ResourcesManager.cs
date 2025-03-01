using System;
using System.Threading;
using UnityEngine;
using UniRx;
using NaughtyAttributes;
using Cysharp.Threading.Tasks;

public class ResourcesManager : MonoBehaviour
{
    [Header("Resources: main")]
    public ReactiveProperty<int> Energy = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> TotalCountOfRobots = new ReactiveProperty<int>(5);
    public ReactiveProperty<int> CountOfAvailableRobots = new ReactiveProperty<int>(5);
    public ReactiveProperty<int> CountOfDays = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> HarmToNature = new ReactiveProperty<int>(0);

    [Header("Resources: wood")]
    public ReactiveProperty<int> DiscoveredForests = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> AvailableForests = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> Wood = new ReactiveProperty<int>(0);

    [Header("Resources: coal")]
    public ReactiveProperty<int> DiscoveredCoalDeposits = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> AvailableCoalDeposits = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> Coal = new ReactiveProperty<int>(0);

    [Header("Resources: coal")]
    public ReactiveProperty<int> DiscoveredSiliconDeposits = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> AvailableSiliconDeposits = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> Silicon = new ReactiveProperty<int>(0);

    [Header("References to other objects")]
    [SerializeField] private ConfigInvestigations _configOfInvestigations;

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

        float chanceValue = UnityEngine.Random.Range(0f, 1f);
        Debug.Log($"ResourcesManager: SendRobotToInvestigate: chanceValue={chanceValue}");

        if (chanceValue >= _configOfInvestigations.ChanceForForest.x 
            &&
            chanceValue <= _configOfInvestigations.ChanceForForest.y)
        {
            DiscoveredForests.Value++;
            AvailableForests.Value++;
        }
        else if (chanceValue >= _configOfInvestigations.ChanceForCoal.x 
                 &&
                 chanceValue <= _configOfInvestigations.ChanceForCoal.y)
        {
            DiscoveredCoalDeposits.Value++;
            AvailableCoalDeposits.Value++;
        }
        else if (chanceValue >= _configOfInvestigations.ChanceForSilicon.x 
                 &&
                 chanceValue <= _configOfInvestigations.ChanceForSilicon.y)
        {
            DiscoveredSiliconDeposits.Value++;
            AvailableSiliconDeposits.Value++;
        }

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
