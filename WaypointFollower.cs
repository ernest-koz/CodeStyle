using UnityEngine;

/// <summary>
/// Moves the object along a chain of child transforms placed under
/// <see cref="_waypointsContainer"/> and loops infinitely.
/// </summary>
public class WaypointFollower : MonoBehaviour
{
    private const float ArrivalThreshold = 0.001f;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Transform _waypointsContainer;

    private Transform[] _waypoints = System.Array.Empty<Transform>();
    private int _currentWaypointIndex;

    private void Start()
    {
        if (_waypointsContainer == null)
        {
            Debug.LogError($"{nameof(_waypointsContainer)} is not assigned on '{name}'.", this);
            enabled = false;
            return;
        }

        BuildWaypoints();
    }

    private void BuildWaypoints()
    {
        int count = _waypointsContainer.childCount;
        _waypoints = new Transform[count];

        for (int i = 0; i < count; i++)
            _waypoints[i] = _waypointsContainer.GetChild(i);
    }

    private void Update()
    {
        if (_waypoints.Length == 0)
            return;

        Transform target = _waypoints[_currentWaypointIndex];
        transform.position = Vector3.MoveTowards(
            transform.position, target.position, _moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) <= ArrivalThreshold)
            AdvanceWaypoint();
    }

    private void AdvanceWaypoint()
    {
        _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
        FaceCurrentWaypoint();
    }

    private void FaceCurrentWaypoint()
    {
        Vector3 direction = _waypoints[_currentWaypointIndex].position - transform.position;
        if (direction.sqrMagnitude > 0f)
            transform.forward = direction;
    }
}
