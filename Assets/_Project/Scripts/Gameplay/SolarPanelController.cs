using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SolarPanelController : MonoBehaviour
{
    [Header("References to other objects")]
    [SerializeField] private ResourcesManager _resourcesManager;
    [SerializeField] private GameStopwatch _gameStopwatch;

    private void Awake()
    {
        _gameStopwatch.CountOfDays.Subscribe(newValue =>
        {
            if (_resourcesManager.CountOfSolarPanels.Value > 0)
            {
                _resourcesManager.Energy.Value += _resourcesManager.CountOfSolarPanels.Value;

                Debug.Log($"SolarPanelController: got " +
                    $"{_resourcesManager.CountOfSolarPanels.Value} energy");
            }
        }).AddTo(this);
    }
}
