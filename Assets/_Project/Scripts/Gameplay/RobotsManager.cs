using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

public class RobotsManager : MonoBehaviour
{
    [Header("References to other objects")]
    [SerializeField] private List<Robot> _listRobots;
    [SerializeField] private List<ResourcesDeposit> _initialListDeposits;
    [SerializeField] private ResourcesManager _resourcesManager;

    [Header("These lists should be empty at game start")]
    [SerializeField] private List<ResourcesDeposit> _listUndiscoveredDeposits;
    private Dictionary<ExtractableResourceId, List<ResourcesDeposit>> _dictAvailableDeposits;

    [Header("Parameters")]
    [SerializeField] private float _robotSpeed = 2f;

    #region MonoBehaviour's methods

    private void Start()
    {
        CheckLists();

        _listUndiscoveredDeposits = _initialListDeposits;

        InitializelDictAvailableDeposits();
    }

    private void CheckLists()
    {
        if (_listRobots == null || _listRobots.Count == 0)
        {
            Debug.LogError("RobotsManager: CheckLists: No robots assigned");
        }

        if (_initialListDeposits == null || _initialListDeposits.Count == 0)
        {
            Debug.LogError("RobotsManager: CheckLists: No resource deposits assigned");
        }

        // if (_listAvailableDeposits != null && _listAvailableDeposits.Count != 0)
        // {
        //     Debug.LogError("RobotsManager: CheckLists: _listAvailableDeposits should be empty");
        // }
    }

    private void InitializelDictAvailableDeposits()
    {
        _dictAvailableDeposits = new Dictionary<ExtractableResourceId, List<ResourcesDeposit>>();

        foreach (ExtractableResourceId resourceId in Enum.GetValues(typeof(ExtractableResourceId)))
        {
            if (resourceId == ExtractableResourceId.Undefined)
                continue;

            _dictAvailableDeposits[resourceId] = new List<ResourcesDeposit>();
        }
    }

    #endregion

    [Button] // Call it in Editor
    private void FindAllResourcesDeposits()
    {
        ResourcesDeposit[] arrayDeposits = FindObjectsOfType<ResourcesDeposit>();
        _initialListDeposits = new List<ResourcesDeposit>(arrayDeposits);
    }

    private ResourcesDeposit FindNearestDeposit()
    {
        return _listUndiscoveredDeposits
            .OrderBy(d => Vector2.Distance(transform.position, d.transform.position))
            .FirstOrDefault();
    }

    private async UniTask MoveRobotAsync(Robot robot, ResourcesDeposit resourcesDeposit, 
        bool isExtraction)
    {
        await MoveToPosition(robot, resourcesDeposit.transform.position);
        await UniTask.Delay(1000);
        
        if (isExtraction)
        {
            resourcesDeposit.SetSpriteAfterExtraction();
        }
        
        await MoveToPosition(robot, transform.position);
        robot.gameObject.SetActive(false);
    }

    private async UniTask MoveToPosition(Robot robot, Vector2 targetPosition)
    {
        while ((Vector2)robot.transform.position != targetPosition)
        {
            robot.transform.position = Vector2.MoveTowards(
                robot.transform.position, targetPosition, _robotSpeed * Time.deltaTime
            );
            await UniTask.Yield();
        }
    }

    private Robot GetInactiveRobot()
    {
        return _listRobots.FirstOrDefault(r => !r.gameObject.activeSelf);
    } 

    #region Public methods

    public async UniTask<ExtractableResourceId> SendRobotToResourcesDeposit()
    {
        Robot inactiveRobot = GetInactiveRobot();

        if (inactiveRobot == null)
        {
            Debug.LogError("RobotsManager: SendRobotToResourcesDeposit: No inactive robots " +
                "available!");
            return ExtractableResourceId.Undefined;
        }

        if (_listUndiscoveredDeposits.Count == 0)
        {
            Debug.LogError("RobotsManager: SendRobotToResourcesDeposit: there is no " +
                $"undiscovered deposits");
            return ExtractableResourceId.Undefined;

        }

        ResourcesDeposit nearestDeposit = FindNearestDeposit();
        _listUndiscoveredDeposits.Remove(nearestDeposit);
        _dictAvailableDeposits[nearestDeposit.ResourceId].Add(nearestDeposit);

        if (nearestDeposit == null)
        {
            Debug.LogError("RobotsManager: SendRobotToResourcesDeposit: No valid resource " +
                $"deposits found!");
            return ExtractableResourceId.Undefined;
        }

        inactiveRobot.gameObject.SetActive(true);
        await MoveRobotAsync(inactiveRobot, nearestDeposit, false);
        return nearestDeposit.ResourceId;
    }

    public async UniTask<int> ExtractResource(ExtractableResourceId resourceId)
    {
        if (_dictAvailableDeposits[resourceId].Count == 0)
        {
            Debug.LogError($"RobotsManager: ExtractResource: i have no available deposits " +
                $"of {resourceId}");
            return 0;
        }

        ResourcesDeposit resourcesDeposit = _dictAvailableDeposits[resourceId][0];

        Robot inactiveRobot = GetInactiveRobot();

        if (inactiveRobot == null)
        {
            Debug.LogError("RobotsManager: ExtractResource: No inactive robots " +
                "available!");
            return 0;
        }

        int countOfResources = resourcesDeposit.ExtractResources();
        if (resourcesDeposit.CountOfAvailableExtractions == 0)
        {
            _dictAvailableDeposits[resourceId].Remove(resourcesDeposit);
            _resourcesManager.DecreaseCountOfAvailableDeposits(resourceId);
        }

        inactiveRobot.gameObject.SetActive(true);
        await MoveRobotAsync(inactiveRobot, resourcesDeposit, true);
        return countOfResources;
    }

    #endregion
}
