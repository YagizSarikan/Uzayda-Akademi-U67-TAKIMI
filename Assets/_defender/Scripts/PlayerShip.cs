using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerShip : MonoBehaviour
{
    public float Speed => _speed;
    public float Direction => _direction;
    
    [SerializeField] Sprite[] _sprites;
    [SerializeField] GameObject _engineThrust;
    [SerializeField] AudioClip _fireSound, _explosionSound;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] float _speed = 5f, _verticalSpeed = 500f;
    [SerializeField] Vector2 _legalAltitude = new Vector2(-4.65f, 3f);
    [SerializeField] Gun _gun;

    IUserInput _userInput;
    Transform _transform;
    Rigidbody2D _rigidbody;
    bool _isThrusting;
    int _direction;
    Vector2 _moveInput, _thrustInput;
    int _sprite;
    Camera _mainCamera;
    float _hyperspaceDelay;

    void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
        _userInput = UserInput.Instance;
        _mainCamera = Camera.main;
    }

    void OnEnable()
    {
        _userInput.OnMoveReceived += HandleOnMove;
        _userInput.OnThrustReceived += HandleOnThrust;
        _userInput.OnFlipDirectionPressed += HandleOnFlipDirection;
        _userInput.OnFirePressed += HandleOnFire;
        _userInput.OnHyperspacePressed += HandleOnHyperspace;
        _direction = 1;
        UpdateEngineThruster();
    }

    void OnDisable()
    {
        _userInput.OnMoveReceived -= HandleOnMove;
        _userInput.OnThrustReceived -= HandleOnThrust;
        _userInput.OnFlipDirectionPressed -= HandleOnFlipDirection;
        _userInput.OnFirePressed -= HandleOnFire;
        _userInput.OnHyperspacePressed -= HandleOnHyperspace;
    }

    void FixedUpdate()
    {
        Vector2 velocity = Vector2.zero;

        if (_userInput.UpPressed && _transform.position.y < _legalAltitude.y)
        {
            velocity.y = _verticalSpeed * Time.fixedDeltaTime;
        }
        else if (_userInput.DownPressed && _transform.position.y > _legalAltitude.x)
        {
            velocity.y = _verticalSpeed * Time.fixedDeltaTime * -1f;
        }

        _rigidbody.velocity = velocity;
    }

    void HandleOnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    void HandleOnThrust(InputValue value)
    {
        _thrustInput = value.Get<Vector2>();
        UpdateEngineThruster();
    }

    void HandleOnFlipDirection()
    {
        var rotation = _transform.localRotation;
        rotation.y = rotation.y == 0 ? 180 : 0;
        _transform.localRotation = rotation;
        _direction *= -1;
    }

    void HandleOnFire()
    {
        _gun.FireGun();
    }

    void HandleOnHyperspace()
    {
        if (Time.time < _hyperspaceDelay) return;
        _hyperspaceDelay = Time.time + 0.25f;
        var legalEntryAltitude = _mainCamera.WorldToViewportPoint(_legalAltitude);
        var reentryPosition = Vector3.zero;
        reentryPosition.x = Random.Range(0.1f, 0.9f);
        reentryPosition.y = Mathf.Clamp(Random.Range(0.1f, 0.9f), legalEntryAltitude.x, legalEntryAltitude.y);
        reentryPosition = _mainCamera.ViewportToWorldPoint(reentryPosition);
        reentryPosition.z = 0;
        _transform.position = reentryPosition;
    }

    void UpdateEngineThruster()
    {
        _engineThrust.SetActive(_userInput.IsThrusting);
        _sprite = _userInput.IsThrusting ? 1 : 0;
        _renderer.sprite = _sprites[_sprite];
    }
}
