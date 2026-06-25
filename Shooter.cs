using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _target;
    [SerializeField] private float _shootInterval = 1f;

    private WaitForSeconds _shootWait;
    private Coroutine _shootingRoutine;

    private void Awake()
    {
        _shootWait = new WaitForSeconds(_shootInterval);
    }

    private void OnEnable() =>
        _shootingRoutine = StartCoroutine(ShootingRoutine());

    private void OnDisable()
    {
        if (_shootingRoutine != null)
            StopCoroutine(_shootingRoutine);

        _shootingRoutine = null;
    }

    private void Shoot()
    {
        if (_target == null || _bulletPrefab == null)
            return;

        Vector3 direction = (_target.position - transform.position).normalized;

        Bullet bullet = Instantiate(
            _bulletPrefab,
            transform.position + direction,
            Quaternion.LookRotation(direction));

        bullet.Launch(direction);
    }

    private IEnumerator ShootingRoutine()
    {
        while (enabled)
        {
            Shoot();
            yield return _shootWait;
        }
    }
}
