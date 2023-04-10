using UnityEngine;
using Random = UnityEngine.Random;

public class LanderProjectile : MonoBehaviour
{
    [SerializeField] float _speed = 3f, _duration = 3f;

    Transform _transform;
    Vector3 _direction;
    float _destroyTime;
    bool OutOfFuel => Time.time >= _destroyTime;

    void OnEnable()
    {
        _transform = transform;
        _destroyTime = Time.time + _duration;
        _direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
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
}
