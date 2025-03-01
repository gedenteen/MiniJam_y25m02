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

    [Header("These lists should be empty at game start")]
    [SerializeField] private List<ResourcesDeposit> _listUndiscoveredDeposits;
    [SerializeField] private List<ResourcesDeposit> _listAvailableDeposits;

    [Header("Parameters")]
    [SerializeField] private float _robotSpeed = 2f;

    private void Start()
    {
        if (_listRobots == null || _listRobots.Count == 0)
        {
            Debug.LogError("RobotsManager: Start: No robots assigned");
        }

        if (_initialListDeposits == null || _initialListDeposits.Count == 0)
        {
            Debug.LogError("RobotsManager: Start: No resource deposits assigned");
        }

        _listUndiscoveredDeposits = _initialListDeposits;

        if (_listAvailableDeposits != null && _listAvailableDeposits.Count != 0)
        {
            Debug.LogError("RobotsManager: Start: _listAvailableDeposits should be empty");
        }
    }

    public async UniTask<ExtractableResourceId> SendRobotToResourcesDeposit()
    {
        Robot inactiveRobot = _listRobots.FirstOrDefault(r => !r.gameObject.activeSelf);

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
        _listAvailableDeposits.Add(nearestDeposit);

        if (nearestDeposit == null)
        {
            Debug.LogError("RobotsManager: SendRobotToResourcesDeposit: No valid resource " +
                $"deposits found!");
            return ExtractableResourceId.Undefined;
        }

        inactiveRobot.gameObject.SetActive(true);
        await MoveRobotAsync(inactiveRobot, nearestDeposit);
        return nearestDeposit.ResourceId;
    }

    private ResourcesDeposit FindNearestDeposit()
    {
        return _listUndiscoveredDeposits
            .OrderBy(d => Vector2.Distance(transform.position, d.transform.position))
            .FirstOrDefault();
    }

    private async UniTask MoveRobotAsync(Robot robot, ResourcesDeposit resourcesDeposit)
    {
        await MoveToPosition(robot, resourcesDeposit.transform.position);
        await UniTask.Delay(1000);
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
}
