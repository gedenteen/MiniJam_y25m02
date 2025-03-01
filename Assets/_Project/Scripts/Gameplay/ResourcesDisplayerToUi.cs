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
    [SerializeField] private ResourceCellOnUi _cellAvailableForests;
    [SerializeField] private ResourceCellOnUi _cellWood;
    [SerializeField] private ResourceCellOnUi _cellAvailableCoalDeposits;
    [SerializeField] private ResourceCellOnUi _cellCoal;

    [Header("")]
    [SerializeField] private float _animationDuration = 1f;

    private void Start()
    {
        AnimateValue(_resourceManager.TotalCountOfRobots, _cellTotalCountOfRobots);
        AnimateValue(_resourceManager.CountOfAvailableRobots, _cellCountOfAvailableRobots);
        AnimateValue(_resourceManager.PropertiesWood.AvailableDeposits, _cellAvailableForests);
        AnimateValue(_resourceManager.PropertiesWood.AvailableResources, _cellWood);
        AnimateValue(_resourceManager.PropertiesCoal.AvailableDeposits, _cellAvailableCoalDeposits);
        AnimateValue(_resourceManager.PropertiesCoal.AvailableResources, _cellCoal);
    }

    private void AnimateValue(ReactiveProperty<int> property, ResourceCellOnUi cell)
    {
        int currentValue = property.Value;

        if (currentValue == 0)
        {
            cell.gameObject.SetActive(false);
        }

        property.Subscribe(newValue =>
        {
            // Объект сейчас деактивирован?
            if (!cell.gameObject.activeSelf) 
            {
                if (newValue == 0)
                {
                    // Ничего не делаем (сейчас объект деактивирован И значение = 0)
                    return;
                }
                
                // Активируем его и сразу показываем значение
                cell.TextMeshCount.text = newValue.ToString();
                currentValue = newValue;
                cell.gameObject.SetActive(true);
            }
            else
            {
                // Нет, делаем анимацию для увеличения числа
                DOTween.To(() => currentValue, x => 
                {
                    currentValue = x;
                    cell.TextMeshCount.text = currentValue.ToString();
                }, newValue, _animationDuration);
            }
        }).AddTo(this);
    }
}
