using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ActionsManager : MonoBehaviour
{
    [Header("References to my buttons")]
    [SerializeField] private ActionButton _buttonSendRobotToInvestigate;
    [SerializeField] private ActionButton _buttonGatherWood;
    [SerializeField] private ActionButton _buttonBurnWood;
    [SerializeField] private ActionButton _buttonGatherCoal;
    [SerializeField] private ActionButton _buttonBurnCoal;
    [SerializeField] private ActionButton _buttonGatherSilicon;
    [SerializeField] private ActionButton _buttonGatherMetals;

    [Header("References to other objects")]
    [SerializeField] private ResourcesManager _resourcesManager;

    [Header("References to assets")]
    [SerializeField] private GameplayConfig _gameplayConfig;

    private void Awake()
    {
        // Send Robot To Investigate
        _buttonSendRobotToInvestigate.Button.onClick.AddListener(() => 
            _resourcesManager.SendRobotToInvestigate().Forget());
        SubscribeToAvailableRobots(_buttonSendRobotToInvestigate);
        
        // Extract wood
        _buttonGatherWood.Button.onClick.AddListener(() => 
            _resourcesManager.ExtractResource(ExtractableResourceId.Wood).Forget());
        SubscribeToAvailableDeposits(
            _resourcesManager.PropertiesWood.AvailableDeposits, 
            _buttonGatherWood);

        // Burn wood
        _buttonBurnWood.Button.onClick.AddListener(() =>
            _resourcesManager.ConvertResourceToEnergy(ExtractableResourceId.Wood).Forget());
        SubscribeToAvailableResource(
            _resourcesManager.PropertiesWood.AvailableResources, 
            _buttonBurnWood,
            _gameplayConfig.AmountOfWoodToBurn);
        Debug.Log($"ActionsManager: Manager: avaiable wood = " +
            $"{_resourcesManager.PropertiesWood.AvailableResources}");

        // Extract coal
        _buttonGatherCoal.Button.onClick.AddListener(() => 
            _resourcesManager.ExtractResource(ExtractableResourceId.Coal).Forget());
        SubscribeToAvailableDeposits(
            _resourcesManager.PropertiesCoal.AvailableDeposits,
            _buttonGatherCoal);

        // Burn coal
        _buttonBurnCoal.Button.onClick.AddListener(() =>
            _resourcesManager.ConvertResourceToEnergy(ExtractableResourceId.Coal).Forget());
        SubscribeToAvailableResource(
            _resourcesManager.PropertiesCoal.AvailableResources, 
            _buttonBurnCoal,
            _gameplayConfig.AmountOfCoalToBurn);

        // Extract silicon
        _buttonGatherSilicon.Button.onClick.AddListener(() => 
            _resourcesManager.ExtractResource(ExtractableResourceId.Silicon).Forget());
        SubscribeToAvailableDeposits(
            _resourcesManager.PropertiesSilicon.AvailableDeposits,
            _buttonGatherSilicon);

        // Extract silicon
        _buttonGatherMetals.Button.onClick.AddListener(() => 
            _resourcesManager.ExtractResource(ExtractableResourceId.Metals).Forget());
        SubscribeToAvailableDeposits(
            _resourcesManager.PropertiesMetals.AvailableDeposits,
            _buttonGatherMetals);

    }

    private void SubscribeToAvailableRobots(ActionButton button)
    {
        _resourcesManager.CountOfAvailableRobots.Subscribe(newValue =>
        {
            if (newValue <= 0) 
            {
                button.SetInteractable(false);
            }
            else
            {
                button.SetInteractable(true);
            }
        }).AddTo(this);
    }

    private void SubscribeToAvailableResource(ReactiveProperty<int> resourceProperty,
        ActionButton button, int minAmountOfResource)
    {
        int currentValue = resourceProperty.Value;

        if (currentValue == 0)
        {
            button.ActivateCanvasGroup(false);
        }

        resourceProperty.Subscribe(newValue =>
        {
            // Кнопка сейчас скрыта и новое значение больше 0?
            if (!button.CanvasGroup.interactable && newValue > 0) 
            {
                // Показываем кнопку
                button.ActivateCanvasGroup(true);
            }
            if (newValue < minAmountOfResource) 
            {
                button.SetInteractable(false);
            }
            else
            {
                button.SetInteractable(true);
            }
        }).AddTo(this);
    }

    private void SubscribeToAvailableDeposits(ReactiveProperty<int> depositProperty, 
        ActionButton button)
    {
        int currentValue = depositProperty.Value;

        if (currentValue == 0)
        {
            button.ActivateCanvasGroup(false);
        }

        _resourcesManager.CountOfAvailableRobots.Subscribe(newValue =>
        {
            if (newValue <= 0) 
            {
                button.SetInteractable(false);
            }
            else if (depositProperty.Value > 0)
            {
                // Делаем кнопку интерактивной, только если кол-во доступных месторождений > 0
                // И кол-во доступных роботов > 0
                button.SetInteractable(true);
            }
        }).AddTo(this);

        depositProperty.Subscribe(newValue =>
        {
            // Кнопка сейчас скрыта и новое значение больше 0?
            if (!button.CanvasGroup.interactable && newValue > 0) 
            {
                // Показываем кнопку
                button.ActivateCanvasGroup(true);
            }
            if (button.gameObject.activeSelf)
            {
                if (newValue <= 0)
                {
                    button.SetInteractable(false);
                }
                else if (_resourcesManager.CountOfAvailableRobots.Value != 0)
                {
                    // Делаем кнопку интерактивной, только если кол-во доступных месторождений > 0
                    // И кол-во доступных роботов > 0
                    button.SetInteractable(true);
                }
            }
        }).AddTo(this);
    }
}
