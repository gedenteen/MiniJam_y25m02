using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ActionsManager : MonoBehaviour
{
    [Header("References to my buttons")]
    [SerializeField] private Button _buttonSendRobotToInvestigate;
    [SerializeField] private Button _buttonGatherWood;
    [SerializeField] private Button _buttonGatherCoal;

    [Header("References to other objects")]
    [SerializeField] private ResourcesManager _resourcesManager;

    private void Awake()
    {
        _buttonSendRobotToInvestigate.onClick.AddListener(() => 
            _resourcesManager.SendRobotToInvestigate().Forget());
        SubscribeToChanges(_resourcesManager.CountOfAvailableRobots, _buttonSendRobotToInvestigate);
        
        _buttonGatherWood.onClick.AddListener(() => 
            _resourcesManager.ExtractResource(ExtractableResourceId.Wood).Forget());
        SubscribeToChanges(_resourcesManager.PropertiesWood.AvailableDeposits, _buttonGatherWood);

        _buttonGatherCoal.onClick.AddListener(() => 
            _resourcesManager.ExtractResource(ExtractableResourceId.Coal).Forget());
        SubscribeToChanges(_resourcesManager.PropertiesCoal.AvailableDeposits, _buttonGatherCoal);
    }

    private void SubscribeToChanges(ReactiveProperty<int> property, Button button)
    {
        int currentValue = property.Value;

        if (currentValue == 0)
        {
            button.gameObject.SetActive(false);
        }

        property.Subscribe(newValue =>
        {
            // Объект сейчас деактивирован и новое значение больше 0?
            if (!button.gameObject.activeSelf && newValue > 0) 
            {
                button.gameObject.SetActive(true);
            }
        }).AddTo(this);
    }
}
