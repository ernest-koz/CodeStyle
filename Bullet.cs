using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifetime = 3f;
    [SerializeField] private float _speed = 10f;

    private Rigidbody _rigidbody;
    private Coroutine _lifetimeRoutine;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _lifetimeRoutine = StartCoroutine(ReleaseAfterLifetime());
    }

    private void OnDisable()
    {
        if (_lifetimeRoutine != null)
            StopCoroutine(_lifetimeRoutine);

        _lifetimeRoutine = null;
    }

    public void Launch(Vector3 position, Quaternion rotation, Vector3 direction)
    {
        transform.SetPositionAndRotation(position, rotation);
        _rigidbody.velocity = direction * _speed;
    }

    private IEnumerator ReleaseAfterLifetime()
    {
        yield return new WaitForSeconds(_lifetime);

        Destroy(gameObject);
    }
}
