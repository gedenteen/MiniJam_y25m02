using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ActionsManager : MonoBehaviour
{
    [Header("References to my buttons")]
    [SerializeField] private Button _buttonSendRobotToInvestigate;

    [Header("References to other objects")]
    [SerializeField] private ResourcesManager _resourcesManager;

    private void Awake()
    {
        _buttonSendRobotToInvestigate.onClick.AddListener(() => 
            _resourcesManager.SendRobotToInvestigate().Forget());
    }
}
