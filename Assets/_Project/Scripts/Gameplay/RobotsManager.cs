using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

public class RobotsManager : MonoBehaviour
{
    [SerializeField] private List<Robot> _listRobots;
    [SerializeField] private List<ResourcesDeposit> _listResorcesDeposits;
    [SerializeField] private float _robotSpeed = 2f;

    private void Start()
    {
        if (_listRobots == null || _listRobots.Count == 0)
        {
            Debug.LogError("RobotsManager: Start: No robots assigned");
        }

        if (_listResorcesDeposits == null || _listResorcesDeposits.Count == 0)
        {
            Debug.LogError("RobotsManager: Start: No resource deposits assigned");
        }
    }

    public async UniTask<ExtractableResourceId> SendRobotToResourcesDeposit()
    {
        Robot inactiveRobot = _listRobots.FirstOrDefault(r => !r.gameObject.activeSelf);

        if (inactiveRobot == null)
        {
            Debug.LogError("RobotsManager: Start: No inactive robots available!");
            return ExtractableResourceId.Undefined;
        }

        ResourcesDeposit nearestDeposit = FindNearestDeposit();

        if (nearestDeposit == null)
        {
            Debug.LogError("RobotsManager: Start: No valid resource deposits found!");
            return ExtractableResourceId.Undefined;
        }

        inactiveRobot.gameObject.SetActive(true);
        await MoveRobotAsync(inactiveRobot, nearestDeposit);
        return nearestDeposit.ResourceId;
    }

    private ResourcesDeposit FindNearestDeposit()
    {
        return _listResorcesDeposits
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
