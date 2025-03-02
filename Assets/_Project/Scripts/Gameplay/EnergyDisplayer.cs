using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class EnergyDisplayer : MonoBehaviour
{
    [Header("References to my objects")]
    [SerializeField] private TextMeshProUGUI _textMeshEnergy;
    [SerializeField] private Image _imageEnergySticks;
    [SerializeField] private RectTransform _rectTransformEnergySticks;

    [Header("References to other objects")]
    [SerializeField] private ResourcesManager _resourcesManager;

    // [Header("References to assets")]

    [Header("Parameters")]
    [SerializeField] private int _maxEnergyOfBattery = 100;

    private float _maxWidthOfImageEnergySticks = 0f;

    private void Awake()
    {
        _maxWidthOfImageEnergySticks = _rectTransformEnergySticks.sizeDelta.x;
        _rectTransformEnergySticks.sizeDelta =
            new Vector2(0f, _rectTransformEnergySticks.sizeDelta.y);

        _resourcesManager.Energy.Subscribe(newValue =>
        {
            _textMeshEnergy.text = $"Energy {newValue}/{_maxEnergyOfBattery}";

            float newWidthForImage = Mathf.Lerp(0f, _maxWidthOfImageEnergySticks, 
                (float)newValue / _maxEnergyOfBattery);

            _rectTransformEnergySticks.sizeDelta = new Vector2(
                newWidthForImage,
                _rectTransformEnergySticks.sizeDelta.y);
        }).AddTo(this);
    }
}
