using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SolarPanelController : MonoBehaviour
{
    [Header("References to other objects")]
    [SerializeField] private ResourcesManager _resourcesManager;
    [SerializeField] private GameStopwatch _gameStopwatch;

    [Header("References to assets")]
    [SerializeField] private GameplayConfig _gameplayConfig;

    private void Awake()
    {
        _gameStopwatch.CountOfDays.Subscribe(newValue =>
        {
            if (_resourcesManager.CountOfSolarPanels.Value > 0)
            {
                int countOfEnergy = _gameplayConfig.CountOfEnergyFrom1SolarPanel *
                    _resourcesManager.CountOfSolarPanels.Value;
                    
                _resourcesManager.Energy.Value += countOfEnergy;

                Debug.Log($"SolarPanelController: got {countOfEnergy} energy");
            }
        }).AddTo(this);
    }
}
