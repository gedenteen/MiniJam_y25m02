using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ExtendedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References to my objects")]
    [SerializeField] private Button _myButton;
    [SerializeField] private Image _myImage;
    [SerializeField] private TextMeshProUGUI _myText;
    [SerializeField] private List<Image> _verticalImages;

    [Header("Parameters")]
    [SerializeField] private int _timeForChangeColorsInMs = 50;

    private Color _startColorOfImage;
    private Color _startColorOfText;

    private void Awake()
    {
        if (_myButton == null)
        {
            Debug.LogError($"ExtendedButton: Awake: i have no reference to my button");
            return;
        }

        _startColorOfImage = _myImage.color;
        _startColorOfText = _myText.color;

        _myButton.onClick.AddListener(OnClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < _verticalImages.Count; i++)
        {
            _verticalImages[i].gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        for (int i = 0; i < _verticalImages.Count; i++)
        {
            _verticalImages[i].gameObject.SetActive(false);
        }
    }

    private void OnClick()
    {
        ExtendedButtonManager.eventPlaySound.Invoke();
        ChangeColors().Forget();
    }

    private async UniTask ChangeColors()
    {
        _myImage.color = _startColorOfText;
        _myText.color = _startColorOfImage;
        await UniTask.Delay(TimeSpan.FromMilliseconds(_timeForChangeColorsInMs));

        if (_myImage != null)
            _myImage.color = _startColorOfImage;
        if (_myText != null)
            _myText.color = _startColorOfText;
    }
}
