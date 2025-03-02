using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class GameLogsController : MonoBehaviour
{
    [Header("References to my objects")]
    [SerializeField] private GameObject _holderForLogs;

    [Header("References to other objects")]
    [SerializeField] private ResourcesManager _resourcesManager;

    [Header("References to assets")]
    [SerializeField] private GameLog _prefabGameLog;
    [SerializeField] private GameplayConfig _gameplayConfig;

    [Header("Parameters")]
    [SerializeField] private float _heightFor1RowOfText = 21f;

    private void Awake()
    {
        _resourcesManager.PropertiesWood.DiscoveredDeposits.Subscribe(newValue =>
        {
            if (newValue == 1) 
            {
                GameLog gameLog = Instantiate(_prefabGameLog, _holderForLogs.transform);
                gameLog.RectTransform.sizeDelta = new Vector2(
                    gameLog.RectTransform.sizeDelta.x, 
                    _heightFor1RowOfText * 3);
                gameLog.TextMesh.text = "The robot has discovered a forest. I can cut down trees " +
                    "and burn the wood to generate energy.";
            }
        }).AddTo(this);

        _resourcesManager.PropertiesWood.ExtractedResources.Subscribe(newValue =>
        {
            if (newValue == 1) 
            {
                GameLog gameLog = Instantiate(_prefabGameLog, _holderForLogs.transform);
                gameLog.RectTransform.sizeDelta = new Vector2(
                    gameLog.RectTransform.sizeDelta.x, 
                    _heightFor1RowOfText * 2);
                gameLog.TextMesh.text = $"I collected wood. By burning every " +
                    $"{_gameplayConfig.AmountOfWoodToBurn} units of wood, I will gain " +
                    $"{_gameplayConfig.AmountOfEnergyAfterBurningWood} units of energy.";
            }
        }).AddTo(this);
        
        _resourcesManager.PropertiesCoal.DiscoveredDeposits.Subscribe(newValue =>
        {
            if (newValue == 1) 
            {
                GameLog gameLog = Instantiate(_prefabGameLog, _holderForLogs.transform);
                gameLog.RectTransform.sizeDelta = new Vector2(
                    gameLog.RectTransform.sizeDelta.x, 
                    _heightFor1RowOfText * 2);
                gameLog.TextMesh.text = "The robot has discovered a coal deposit. Coal will " +
                    "provide more energy than wood.";
            }
        }).AddTo(this);

        _resourcesManager.PropertiesCoal.ExtractedResources.Subscribe(newValue =>
        {
            if (newValue == 1) 
            {
                GameLog gameLog = Instantiate(_prefabGameLog, _holderForLogs.transform);
                gameLog.RectTransform.sizeDelta = new Vector2(
                    gameLog.RectTransform.sizeDelta.x, 
                    _heightFor1RowOfText * 5);
                gameLog.TextMesh.text = $"I collected coal. By burning every " +
                    $"{_gameplayConfig.AmountOfCoalToBurn} units of wood, I will gain " +
                    $"{_gameplayConfig.AmountOfEnergyAfterBurningCoal} units of energy. " +
                    $"Burning coal is more efficient than burning wood, but on most planets, " +
                    $"coal is rarer than trees.";
            }
        }).AddTo(this);
        
        _resourcesManager.PropertiesSilicon.DiscoveredDeposits.Subscribe(newValue =>
        {
            if (newValue == 1) 
            {
                GameLog gameLog = Instantiate(_prefabGameLog, _holderForLogs.transform);
                gameLog.RectTransform.sizeDelta = new Vector2(
                    gameLog.RectTransform.sizeDelta.x, 
                    _heightFor1RowOfText * 4);
                gameLog.TextMesh.text = "The robot has discovered a silicon deposit. If I have " +
                    "enough silicon and metals, I will be able to create more robots and build " +
                    "solar panels.";
            }
        }).AddTo(this);
    }
}
