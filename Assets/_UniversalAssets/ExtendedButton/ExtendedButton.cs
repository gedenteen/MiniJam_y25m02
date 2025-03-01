using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.EventSystems;

public class ExtendedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References to my objects")]
    [SerializeField] public Button Button;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _myText;
    [SerializeField] private List<Image> _verticalImages;
    [SerializeField] private Image _imageLock;

    [Header("Parameters")]
    [SerializeField] private int _timeForChangeColorsInMs = 50;

    private Color _startColorOfImage;
    private Color _startColorOfText;

    private void Awake()
    {
        if (Button == null)
        {
            Debug.LogError($"ExtendedButton: Awake: i have no reference to my button");
            return;
        }

        _startColorOfImage = _backgroundImage.color;
        _startColorOfText = _myText.color;

        Button.onClick.AddListener(OnClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Button.interactable)
        {
            return;
        }

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

    public void SetInteractable(bool isInteractable)
    {
        Button.interactable = isInteractable;
        if (_imageLock.gameObject.activeSelf == isInteractable)
        {
            _imageLock.gameObject.SetActive(!isInteractable);
        }
    }

    private void OnClick()
    {
        ExtendedButtonManager.eventPlaySound.Invoke();
        ChangeColors().Forget();
    }

    private async UniTask ChangeColors()
    {
        _backgroundImage.color = _startColorOfText;
        _myText.color = _startColorOfImage;
        await UniTask.Delay(TimeSpan.FromMilliseconds(_timeForChangeColorsInMs));

        if (_backgroundImage != null)
            _backgroundImage.color = _startColorOfImage;
        if (_myText != null)
            _myText.color = _startColorOfText;
    }
}
