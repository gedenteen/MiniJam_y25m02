using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class EnergyDisplayer : MonoBehaviour
{
    [Header("References to my objects: left part")]
    [SerializeField] private TextMeshProUGUI _textMeshEnergy;
    [SerializeField] private Image _imageEnergySticks;
    [SerializeField] private RectTransform _rectTransformEnergySticks;
    [SerializeField] private GameObject _partWithBatteryInfo;
    [SerializeField] private Transform _leftTransofrmForPartWithBatteryInfo;

    [Header("References to my objects: right part")]
    [SerializeField] private GameObject _partWithBatteries;
    [SerializeField] private TextMeshProUGUI _textMeshBatteriesCharged;

    [Header("References to other objects")]
    [SerializeField] private ResourcesManager _resourcesManager;

    [Header("References to assets")]
    [SerializeField] private GameplayConfig _gameplayConfig;

    private float _maxWidthOfImageEnergySticks = 0f;

    private void Awake()
    {
        _maxWidthOfImageEnergySticks = _rectTransformEnergySticks.sizeDelta.x;
        _rectTransformEnergySticks.sizeDelta =
            new Vector2(0f, _rectTransformEnergySticks.sizeDelta.y);

        _resourcesManager.Energy.Subscribe(newValue =>
        {
            int energyForCurrentBattery = newValue % _gameplayConfig.EnergyFor1Battery;
            int countOfChargedBatteries = newValue / _gameplayConfig.EnergyFor1Battery;

            _textMeshEnergy.text =
                $"Energy {energyForCurrentBattery}/{_gameplayConfig.EnergyFor1Battery}";

            float newWidthForImage = Mathf.Lerp(0f, _maxWidthOfImageEnergySticks, 
                (float)energyForCurrentBattery / _gameplayConfig.EnergyFor1Battery);

            _rectTransformEnergySticks.sizeDelta = new Vector2(
                newWidthForImage,
                _rectTransformEnergySticks.sizeDelta.y);

            if (countOfChargedBatteries > 0)
            {
                if (!_partWithBatteries.activeSelf)
                {
                    _partWithBatteries.SetActive(true);
                    _partWithBatteryInfo.transform.position = _leftTransofrmForPartWithBatteryInfo.position;
                }

                _textMeshBatteriesCharged.text = 
                    $"{countOfChargedBatteries}/{_gameplayConfig.CountOfBatteries}";
            }
        }).AddTo(this);
    }
}
