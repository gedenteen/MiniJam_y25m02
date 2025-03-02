using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using DG.Tweening;

public class EndOfGameController : MonoBehaviour
{
    [Header("References to other objects")]
    [SerializeField] private List<GameObject> _objectsToDeactivate;
    [SerializeField] private GameObject _panelEndOfGame;
    [SerializeField] private GameObject _spaceship;

    [Header("References to other objects")]
    [SerializeField] private ResourcesManager _resourcesManager;
    [SerializeField] private GameStopwatch _gameStopwatch;

    [Header("References to assets")]
    [SerializeField] private GameplayConfig _gameplayConfig;
    
    [Header("Parameters")]
    [SerializeField] private float _targetHeight = 10f;
    [SerializeField] private float _duration = 3f;
    [SerializeField] private Ease _easeType = Ease.InCirc;

    private void Awake()
    {
        int countOfEnergyForEnd = 
            _gameplayConfig.EnergyFor1Battery * _gameplayConfig.CountOfBatteries;

        _resourcesManager.Energy.Subscribe(newValue =>
        {
            if (newValue >= countOfEnergyForEnd) 
            {
                Finish().Forget();
            }
        }).AddTo(this);
    }

    [Button]
    public async UniTask Finish()
    {
        for (int i = 0; i < _objectsToDeactivate.Count; i++)
        {
            _objectsToDeactivate[i].SetActive(false);
        }

        _spaceship.transform.DOMoveY(transform.position.y + _targetHeight, _duration)
        .SetEase(_easeType);

        await UniTask.WaitForSeconds(3f);

        _panelEndOfGame.SetActive(true);
    }
}
