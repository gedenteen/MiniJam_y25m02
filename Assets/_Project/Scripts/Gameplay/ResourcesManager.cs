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
    public ReactiveProperty<int> CountOfCreatedRobots = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> CountOfSolarPanels = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> HarmToNature = new ReactiveProperty<int>(0);

    [Header("Resources: extractable")]
    public PropertiesOfExtractableResource PropertiesWood = new PropertiesOfExtractableResource();
    public PropertiesOfExtractableResource PropertiesCoal = new PropertiesOfExtractableResource();
    public PropertiesOfExtractableResource PropertiesSilicon = new PropertiesOfExtractableResource();
    public PropertiesOfExtractableResource PropertiesMetals = new PropertiesOfExtractableResource();

    [Header("References to other objects")]
    [SerializeField] private RobotsManager _robotsManager;

    [Header("References to assets")]
    [SerializeField] private ConfigInvestigations _configOfInvestigations;
    [SerializeField] private GameplayConfig _gameplayConfig;

    // Private fields
    private Dictionary<ExtractableResourceId, PropertiesOfExtractableResource> 
        _dictExtractableResources;

    private void Awake()
    {
        FillDictExtractableResources();
    }

    private void FillDictExtractableResources()
    {
        _dictExtractableResources =
            new Dictionary<ExtractableResourceId, PropertiesOfExtractableResource>();
        _dictExtractableResources[ExtractableResourceId.Wood] = PropertiesWood;
        _dictExtractableResources[ExtractableResourceId.Coal] = PropertiesCoal;
        _dictExtractableResources[ExtractableResourceId.Silicon] = PropertiesSilicon;
        _dictExtractableResources[ExtractableResourceId.Metals] = PropertiesMetals;
    }

    [Button]
    public async UniTask SendRobotToInvestigate()
    {
        if (CountOfAvailableRobots.Value <= 0)
        {
            Debug.Log("ResourcesManager: SendRobotToInvestigate: No available robots to send");
            return;
        }
        CountOfAvailableRobots.Value--;
        Debug.Log("ResourcesManager: SendRobotToInvestigate: Robot sent to investigate");
        
        // Дальше идёт асинхронная логика
        ExtractableResourceId resourceId = await _robotsManager.SendRobotToResourcesDeposit();

        if (resourceId == ExtractableResourceId.Undefined)
        {
            Debug.LogError("ResourcesManager: SendRobotToInvestigate: got " +
                "ExtractableResourceId.Undefined");
        }
        else
        {
            _dictExtractableResources[resourceId].DiscoveredDeposits.Value++;
            _dictExtractableResources[resourceId].AvailableDeposits.Value++;
        }

        CountOfAvailableRobots.Value++;
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

        if (CountOfAvailableRobots.Value <= 0)
        {
            Debug.Log("ResourcesManager: ExtractResource: No available robots to send");
            return;
        }
        CountOfAvailableRobots.Value--;

        int countOfResources = await _robotsManager.ExtractResource(resourceId);

        _dictExtractableResources[resourceId].ExtractedResources.Value += countOfResources;
        _dictExtractableResources[resourceId].AvailableResources.Value += countOfResources;

        CountOfAvailableRobots.Value++;
    }

    public void DecreaseCountOfAvailableDeposits(ExtractableResourceId resourceId)
    {
        _dictExtractableResources[resourceId].AvailableDeposits.Value--;
    }

    public async UniTask ConvertResourceToEnergy(ExtractableResourceId resourceId)
    {
        int amountOfResourceToBurn = 0;
        int amountOfEnergyAfterBurn = 0;

        switch (resourceId)
        {
            case ExtractableResourceId.Undefined:
            case ExtractableResourceId.Silicon:
            case ExtractableResourceId.Metals:
                Debug.LogError($"ResourcesManager: ConvertResourceToEnergy: can't convert " +
                    $"{resourceId} into energy");
                return;
            case ExtractableResourceId.Wood:
                amountOfResourceToBurn = _gameplayConfig.AmountOfWoodToBurn;
                amountOfEnergyAfterBurn = _gameplayConfig.AmountOfEnergyAfterBurningWood;
                break;
            case ExtractableResourceId.Coal:
                amountOfResourceToBurn = _gameplayConfig.AmountOfCoalToBurn;
                amountOfEnergyAfterBurn = _gameplayConfig.AmountOfEnergyAfterBurningCoal;
                break;
            default:
                Debug.LogError($"ResourcesManager: ConvertResourceToEnergy: unexpected " +
                    $"resourceId={resourceId}");
                return;
        }

        if (_dictExtractableResources[resourceId].AvailableResources.Value < amountOfResourceToBurn)
        {
            Debug.LogError($"ResourcesManager: ExtractResource: there is no available " +
                $"resources of {resourceId}");
            return;
        }

        _dictExtractableResources[resourceId].AvailableResources.Value -= amountOfResourceToBurn;
        await UniTask.WaitForSeconds(3f);
        Energy.Value += amountOfEnergyAfterBurn;
    }

    public async UniTask CreateRobot()
    {
        if (PropertiesSilicon.AvailableResources.Value < _gameplayConfig.AmountOfSiliconForCreateRobot
            ||
            PropertiesMetals.AvailableResources.Value < _gameplayConfig.AmountOfMetalsForCreateRobot)
        {
            Debug.LogError($"ResourcesManager: CreateRobot: don't have enough resources for it");
            return;
        }

        PropertiesSilicon.AvailableResources.Value -= _gameplayConfig.AmountOfSiliconForCreateRobot;
        PropertiesMetals.AvailableResources.Value -= _gameplayConfig.AmountOfMetalsForCreateRobot;

        await UniTask.WaitForSeconds(1f);
        
        TotalCountOfRobots.Value++;
        CountOfAvailableRobots.Value++;
        CountOfCreatedRobots.Value++;
    }

    public async UniTask CreateSolarPanel()
    {
        if (PropertiesSilicon.AvailableResources.Value < _gameplayConfig.AmountOfSiliconForCreateSolarPanel
            ||
            PropertiesMetals.AvailableResources.Value < _gameplayConfig.AmountOfMetalsForCreateSolarPanel)
        {
            Debug.LogError($"ResourcesManager: CreateSolarPanel: don't have enough resources for it");
            return;
        }

        PropertiesSilicon.AvailableResources.Value -= _gameplayConfig.AmountOfSiliconForCreateSolarPanel;
        PropertiesMetals.AvailableResources.Value -= _gameplayConfig.AmountOfMetalsForCreateSolarPanel;

        await UniTask.WaitForSeconds(1f);
        
        CountOfSolarPanels.Value++;
    }
}
