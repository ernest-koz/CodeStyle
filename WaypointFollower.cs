using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    private const float ArrivalThreshold = 0.001f;
    private const float ArrivalThresholdSquared = ArrivalThreshold * ArrivalThreshold;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Transform _waypointsContainer;
    [SerializeField] private Transform[] _waypoints = System.Array.Empty<Transform>();

    private int _currentWaypointIndex;

    private void Start()
    {
        if (_waypoints.Length == 0)
        {
            Debug.LogError($"No waypoints assigned on '{name}'.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        if (_waypoints.Length == 0)
            return;

        Transform target = _waypoints[_currentWaypointIndex];

        transform.position = Vector3.MoveTowards(
            transform.position, target.position, _moveSpeed * Time.deltaTime);

        Vector3 toTarget = target.position - transform.position;

        if (toTarget.sqrMagnitude <= ArrivalThresholdSquared)
            AdvanceWaypoint();
    }

    private void AdvanceWaypoint()
    {
        _currentWaypointIndex = ++_currentWaypointIndex % _waypoints.Length;
        FaceCurrentWaypoint();
    }

    private void FaceCurrentWaypoint()
    {
        Vector3 direction = _waypoints[_currentWaypointIndex].position - transform.position;

        if (direction.sqrMagnitude > 0f)
            transform.forward = direction;
    }

#if UNITY_EDITOR
    [ContextMenu("Build Waypoints")]
    private void BuildWaypoints()
    {
        if (_waypointsContainer == null)
            return;

        int count = _waypointsContainer.childCount;
        _waypoints = new Transform[count];

        for (int i = 0; i < count; i++)
            _waypoints[i] = _waypointsContainer.GetChild(i);
    }
#endif
}
