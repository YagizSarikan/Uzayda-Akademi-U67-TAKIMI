using UnityEngine;

public class LanderPatrol : IState
{
    readonly Transform _transform;
    readonly Vector3 _patrolMin;
    readonly Vector3 _patrolMax;
    readonly float _patrolDistance;
    readonly float _patrolSpeed;
    readonly float _fireDelayMin;
    readonly float _fireDelayMax;
    readonly GameObject _projectilePrefab;
    GameObject _patrolDestination;
    float _fireDelay;

    bool ShouldFire
    {
        get
        {
            _fireDelay -= Time.deltaTime;
            Debug.Log($"{_transform.name} _fireDelay={_fireDelay}");
            return _fireDelay <= 0f;
        }
    }

    public LanderPatrol(
        Transform transform, 
        Vector3 patrolMin,
        Vector3 patrolMax,
        float patrolDistance,
        float patrolSpeed,
        float fireDelayMin,
        float fireDelayMax,
        GameObject projectilePrefab)
    {
        _transform = transform;
        _patrolMin = patrolMin;
        _patrolMax = patrolMax;
        _patrolDistance = patrolDistance;
        _patrolSpeed = patrolSpeed;
        _fireDelayMin = fireDelayMin;
        _fireDelayMax = fireDelayMax;
        _projectilePrefab = projectilePrefab;
    }
    
    public void Tick()
    {
        var position = _transform.position;
        if (position.FlatDistance(_patrolDestination.transform.position) <= 0.2f)
        {
            Debug.Log($"{_transform.name} reached destination. Getting a new one.");
            GetNewPatrolDestination();
            return;
        }
        _transform.MoveTowardsTarget(_patrolDestination.transform, _patrolSpeed);

        bool shouldFire = ShouldFire;
        Debug.Log($"{_transform.name} ShouldFire={shouldFire}.");
        if (shouldFire)
        {
            Fire();
        }
    }

    public void OnEnter()
    {
        Debug.Log($"{_transform.name} entered patrol state.");
        SetFireDelay();
        GetNewPatrolDestination();
    }

    public void OnExit()
    {
        if (_patrolDestination)
        {
            Object.Destroy(_patrolDestination);
        }
    }

    void GetNewPatrolDestination()
    {
        if (!_patrolDestination)
        {
            _patrolDestination = new GameObject($"{_transform.name} destination");
            _patrolDestination.AddComponent<MobScroller>();
            _patrolDestination.AddComponent<PatrolDestination>();
        }

        Vector3 position = _transform.position;
        position.x = Mathf.Clamp(position.x + Random.Range(-_patrolDistance, _patrolDistance),
            _patrolMin.x, _patrolMax.x);
        position.y = Mathf.Clamp(position.y + Random.Range(-_patrolDistance, _patrolDistance),
            _patrolMin.y, _patrolMax.y);
        _patrolDestination.transform.position = position;
        Debug.Log($"Patrol destination now {_patrolDestination.transform.position}");
    }

    void SetFireDelay()
    {
        _fireDelay = Random.Range(_fireDelayMin, _fireDelayMax);
    }

    void Fire()
    {
        Debug.Log($"{_transform.name} firing projectile.");
        Object.Instantiate(_projectilePrefab, _transform.position, Quaternion.identity);
        SetFireDelay();
    }
}