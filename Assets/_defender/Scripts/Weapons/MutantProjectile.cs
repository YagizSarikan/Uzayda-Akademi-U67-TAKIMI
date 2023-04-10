using UnityEngine;
using Random = UnityEngine.Random;

public class MutantProjectile : MonoBehaviour
{
    [SerializeField] float _speed = 5f, _duration = 2f;

    Transform _transform, _target;
    Vector3 _direction;
    float _destroyTime;

    bool OutOfFuel => Time.time >= _destroyTime;

    void Awake()
    {
        _transform = transform;
    }

    public void Init(Transform target)
    {
        _target = target;
        _destroyTime = Time.time + _duration;
        SetDirection();
    }

    void Update()
    {
        if (OutOfFuel)
        {
            Destroy(gameObject);
            return;
        }

        _transform.position += _direction * (_speed * Time.deltaTime);
    }

    void SetDirection()
    {
        var targetPosition = _target.position;
        targetPosition.y += Random.Range(-2f, 2f);
        _direction = targetPosition - _transform.position;
        _direction.Normalize();
    }
}
