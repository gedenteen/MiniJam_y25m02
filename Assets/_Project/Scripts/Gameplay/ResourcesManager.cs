using System;
using System.Threading;
using UnityEngine;
using UniRx;
using NaughtyAttributes;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class ResourcesManager : MonoBehaviour
{
    [Header("Resources: main")]
    public ReactiveProperty<int> Energy = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> TotalCountOfRobots = new ReactiveProperty<int>(5);
    public ReactiveProperty<int> CountOfAvailableRobots = new ReactiveProperty<int>(5);
    public ReactiveProperty<int> CountOfDays = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> HarmToNature = new ReactiveProperty<int>(0);

    [Header("Resources: extractable")]
    public PropertiesOfExtractableResource PropertiesWood = new PropertiesOfExtractableResource();
    public PropertiesOfExtractableResource PropertiesCoal = new PropertiesOfExtractableResource();
    public PropertiesOfExtractableResource PropertiesSilicon = new PropertiesOfExtractableResource();

    [Header("References to other objects")]
    [SerializeField] private RobotsManager _robotsManager;
    [SerializeField] private ConfigInvestigations _configOfInvestigations;

    [SerializeField] private float _minTimeForInvestigate = 1f;
    [SerializeField] private float _maxTimeForInvestigate = 3f;

    private Dictionary<ExtractableResourceId, Vector2> _dictChancesOfInvestigations;
    private Dictionary<ExtractableResourceId, PropertiesOfExtractableResource> 
        _dictExtractableResources;

    private readonly object _lock = new object();

    private void Awake()
    {
        FillDictChancesOfInvestigations();
        FillDictExtractableResources();
    }

    private void FillDictChancesOfInvestigations()
    {
        // Fill _dictChancesOfInvestigations with values from SO ConfigInvestigations
        _dictChancesOfInvestigations = new Dictionary<ExtractableResourceId, Vector2>();
        _dictChancesOfInvestigations[ExtractableResourceId.Wood] = 
            _configOfInvestigations.ChanceForForest;
        _dictChancesOfInvestigations[ExtractableResourceId.Coal] = 
            _configOfInvestigations.ChanceForCoal;
        _dictChancesOfInvestigations[ExtractableResourceId.Silicon] = 
            _configOfInvestigations.ChanceForSilicon;

        float sumOfChances = 0f;
        foreach (var pair in _dictChancesOfInvestigations)
        {
            Vector2 chanceRange = pair.Value;
            sumOfChances += chanceRange.y - chanceRange.x;
        }

        //Debug.Log($"ResourcesManager: FillDictChancesOfInvestigations: sumOfChances={sumOfChances}");
        if (sumOfChances < 0.999f || sumOfChances > 1.001f)
        {
            Debug.LogError($"ResourcesManager: FillDictChancesOfInvestigations: wrong sum of " +
                $"chances = {sumOfChances} (should be 1.0)");
        }
    }

    private void FillDictExtractableResources()
    {
        _dictExtractableResources =
            new Dictionary<ExtractableResourceId, PropertiesOfExtractableResource>();
        _dictExtractableResources[ExtractableResourceId.Wood] = PropertiesWood;
        _dictExtractableResources[ExtractableResourceId.Coal] = PropertiesCoal;
        _dictExtractableResources[ExtractableResourceId.Silicon] = PropertiesSilicon;
    }

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
        ExtractableResourceId resourceId = await _robotsManager.SendRobotToResourcesDeposit();

        if (resourceId == ExtractableResourceId.Undefined)
        {
            Debug.Log("ResourcesManager: SendRobotToInvestigate: got " +
                "ExtractableResourceId.Undefined");
            return;
        }

        _dictExtractableResources[resourceId].DiscoveredDeposits.Value++;
        _dictExtractableResources[resourceId].AvailableDeposits.Value++;

        lock (_lock)
        {
            CountOfAvailableRobots.Value++;
        }
        Debug.Log("ResourcesManager: SendRobotToInvestigate: Robot returned from investigation");
    }

    public async UniTask ExtractResource(ExtractableResourceId resourceId)
    {
        if (_dictExtractableResources[resourceId].AvailableDeposits.Value <= 0)
        {
            Debug.LogError($"ResourcesManager: ExtractResource: there is no available " +
                $"deposits for {resourceId}");
            return;
        }

        _dictExtractableResources[resourceId].ExtractedResources.Value += 123;
        _dictExtractableResources[resourceId].AvailableResources.Value += 123;
    }

    // public void ChangeCountOfEnergy(int summand)
    // {
    //     Energy.Value += summand;
    // }

    // public void ChangeCountOfCoal(int summand)
    // {
    //     if (summand > 0)
    //     {
    //         PropertiesCoal.ExtractedResources.Value += summand;
    //     }

    //     PropertiesCoal.AvailableResources.Value += summand;
    // }
}
