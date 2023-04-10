using UnityEngine;
using Random = UnityEngine.Random;

public class Mutant : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] float _fireDelayMin = 1f, _fireDelayMax = 3f;
    [SerializeField] MutantProjectile _projectilePrefab;

    bool CanFire => Time.time >= _fireTime;
    
    Transform _transform, _target;
    float _fireTime;
    GameManager _gameManager;

    void Awake()
    {
        _transform = transform;
        _gameManager = FindObjectOfType<GameManager>();
    }

    void OnEnable()
    {
        _gameManager.PlayerShipSpawned += OnPlayerShipSpawned;
        SetTarget();
        SetFireTime();
    }

    void OnDisable()
    {
        _gameManager.PlayerShipSpawned -= OnPlayerShipSpawned;
    }

    void Update()
    {
        if (_gameManager.PlayerAlive)
        {
            SetTarget();
            if (CanFire)
            {
                Fire();
            }
        }
        _transform.MoveTowardsTarget(_target, _moveSpeed);
    }

    void Fire()
    {
        SetFireTime();
        var projectile = Instantiate(_projectilePrefab, _transform.position, Quaternion.identity);
        projectile.Init(_gameManager.PlayerShip.transform);
    }

    void SetFireTime()
    {
        var delay = Random.Range(_fireDelayMin, _fireDelayMax);
        _fireTime = Time.time + delay;
    }

    void SetTarget()
    {
        if (_gameManager.PlayerAlive)
        {
            _target = _gameManager.PlayerShip.transform;
        }
    }

    void OnPlayerShipSpawned()
    {
        SetTarget();
    }
}
