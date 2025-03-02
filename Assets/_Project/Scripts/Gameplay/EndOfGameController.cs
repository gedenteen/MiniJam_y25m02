using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EndOfGameController : MonoBehaviour
{
    [Header("References to other objects")]
    [SerializeField] private List<GameObject> _objectsToDeactivate;
    [SerializeField] private GameObject _panelEndOfGame;

    [Header("References to other objects")]
    [SerializeField] private ResourcesManager _resourcesManager;
    [SerializeField] private GameStopwatch _gameStopwatch;

    [Header("References to assets")]
    [SerializeField] private GameplayConfig _gameplayConfig;

    private void Awake()
    {
        int countOfEnergyForEnd = 
            _gameplayConfig.EnergyFor1Battery * _gameplayConfig.CountOfBatteries;

        _resourcesManager.Energy.Subscribe(newValue =>
        {
            if (newValue >= countOfEnergyForEnd) 
            {
                for (int i = 0; i < _objectsToDeactivate.Count; i++)
                {
                    _objectsToDeactivate[i].SetActive(false);
                }
                
                _panelEndOfGame.SetActive(true);
            }
        }).AddTo(this);
    }
}
