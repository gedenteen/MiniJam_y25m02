using UnityEngine;
using UniRx;
using TMPro;
using DG.Tweening;

public class ResourcesDisplayerToUi : MonoBehaviour
{
    [Header("")]
    [SerializeField] private ResourcesManager _resourceManager;

    [Header("")]
    [SerializeField] private TextMeshProUGUI _energyText;
    [SerializeField] private TextMeshProUGUI _robotsText;
    [SerializeField] private TextMeshProUGUI _daysText;
    [SerializeField] private TextMeshProUGUI _woodText;
    [SerializeField] private TextMeshProUGUI _coalText;

    [Header("")]
    [SerializeField] private float _animationDuration = 1f;

    private void Start()
    {
        AnimateValue(_resourceManager.Energy, _energyText, "Energy: ");
        AnimateValue(_resourceManager.CountOfRobots, _robotsText, "Robots: ");
        AnimateValue(_resourceManager.CountOfDays, _daysText, "Days: ");
        AnimateValue(_resourceManager.Wood, _woodText, "Wood: ");
        AnimateValue(_resourceManager.Coal, _coalText, "Coal: ");
    }

    private void AnimateValue(ReactiveProperty<int> property, TMP_Text text, string prefix)
    {
        int currentValue = property.Value;
        property.Subscribe(value =>
        {
            DOTween.To(() => currentValue, x => 
            {
                currentValue = x;
                text.text = $"{prefix}{currentValue}";
            }, value, _animationDuration);
        }).AddTo(this);
    }
}
