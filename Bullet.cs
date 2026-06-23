using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Projectile that returns itself to its source pool after <see cref="_lifetime"/> seconds.
/// Must be spawned via <see cref="ObjectPool{GameObject}"/> fed by <see cref="Shooter"/>.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifetime = 3f;

    private IObjectPool<GameObject> _pool;
    private float _releaseTime;
    private bool _released;

    public void Initialize(IObjectPool<GameObject> pool) => _pool = pool;

    private void OnEnable()
    {
        _releaseTime = Time.time + _lifetime;
        _released = false;
    }

    private void Update()
    {
        if (_released || Time.time < _releaseTime)
            return;

        _released = true;
        _pool?.Release(gameObject);
    }
}
