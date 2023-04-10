using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] float _speed = 50f;
    [SerializeField] float _duration = 0.25f;

    float _destroyTime;

    bool OutOfFuel => Time.time >= _destroyTime;

    void OnEnable()
    {
        _destroyTime = Time.time + _duration;
        _rigidbody.velocity = transform.right * _speed;
    }

    void Update()
    {
        if (OutOfFuel)
        {
            Destroy(gameObject);
        }
    }

}
