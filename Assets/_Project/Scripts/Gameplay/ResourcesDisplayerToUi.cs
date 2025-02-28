using UnityEngine;
using UniRx;
using TMPro;
using DG.Tweening;

public class ResourcesDisplayerToUi : MonoBehaviour
{
    [Header("")]
    [SerializeField] private ResourcesManager _resourceManager;

    [Header("")]
    [SerializeField] private ResourceCellOnUi _cellTotalCountOfRobots;
    [SerializeField] private ResourceCellOnUi _cellCountOfAvailableRobots;
    [SerializeField] private ResourceCellOnUi _cellWood;
    [SerializeField] private ResourceCellOnUi _cellCoal;

    [Header("")]
    [SerializeField] private float _animationDuration = 1f;

    private void Start()
    {
        AnimateValue(_resourceManager.TotalCountOfRobots, _cellTotalCountOfRobots);
        AnimateValue(_resourceManager.CountOfAvailableRobots, _cellCountOfAvailableRobots);
        AnimateValue(_resourceManager.Wood, _cellWood);
        AnimateValue(_resourceManager.Coal, _cellCoal);
    }

    private void AnimateValue(ReactiveProperty<int> property, ResourceCellOnUi cell)
    {
        int currentValue = property.Value;
        property.Subscribe(value =>
        {
            DOTween.To(() => currentValue, x => 
            {
                currentValue = x;
                cell.TextMeshCount.text = currentValue.ToString();
            }, value, _animationDuration);
        }).AddTo(this);
    }
}
