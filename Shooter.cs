using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Periodically spawns pooled bullets toward <see cref="_target"/>.
/// </summary>
public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _target;
    [SerializeField] private float _bulletSpeed = 10f;
    [SerializeField] private float _shootInterval = 1f;
    [SerializeField] private int _poolDefaultCapacity = 20;
    [SerializeField] private int _poolMaxSize = 50;

    private ObjectPool<GameObject> _pool;
    private Coroutine _shootingRoutine;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: CreateBullet,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: Destroy,
            collectionCheck: true,
            defaultCapacity: _poolDefaultCapacity,
            maxSize: _poolMaxSize);
    }

    private void OnEnable() => _shootingRoutine = StartCoroutine(ShootingRoutine());

    private void OnDisable()
    {
        if (_shootingRoutine != null)
            StopCoroutine(_shootingRoutine);
        _shootingRoutine = null;
    }

    private GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(_bulletPrefab);
        if (bullet.TryGetComponent(out Bullet bulletComponent))
            bulletComponent.Initialize(_pool);
        return bullet;
    }

    private static void OnGet(GameObject bullet) => bullet.SetActive(true);
    private static void OnRelease(GameObject bullet) => bullet.SetActive(false);

    private IEnumerator ShootingRoutine()
    {
        while (enabled)
        {
            Shoot();
            yield return new WaitForSeconds(_shootInterval);
        }
    }

    private void Shoot()
    {
        if (_target == null || _bulletPrefab == null)
            return;

        Vector3 direction = (_target.position - transform.position).normalized;
        GameObject bullet = _pool.Get();
        bullet.transform.SetPositionAndRotation(
            transform.position + direction,
            Quaternion.LookRotation(direction));

        if (bullet.TryGetComponent(out Rigidbody rb))
            rb.velocity = direction * _bulletSpeed;
    }
}
