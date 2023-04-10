using System;
using TMPro;
using UnityEngine;

public class LanderStateMachine : MonoBehaviour
{
    public event Action<IState> LanderStateChanged = delegate(IState state) {  };
    public Type CurrentStateType => _stateMachine.CurrentState.GetType();
    
    [SerializeField] Vector3 _patrolMin = Constants.PatrolMin;
    [SerializeField] Vector3 _patrolMax = Constants.PatrolMax;
    [SerializeField] float _patrolDistance = 3f, _speed = 0.5f;
    [SerializeField] float _mutateHeight = 3f;
    [SerializeField] float _fireDelayMin = 2f, _fireDelayMax = 5f;
    [SerializeField] GameObject _projectilePrefab;

    Transform _transform;
    Transform _nearestHuman;
    StateMachine _stateMachine;
    RaycastHit2D[] _hits;
    MutatableMob _mutableMob;


    void Awake()
    {
        _transform = transform;
        _mutableMob = gameObject.GetComponent<MutatableMob>();
        _stateMachine = new StateMachine();
        _stateMachine.OnStateChanged += state => LanderStateChanged.Invoke(state);
        var patrol = new LanderPatrol(
            _transform,
            _patrolMin,
            _patrolMax,
            _patrolDistance,
            _speed,
            _fireDelayMin,
            _fireDelayMax,
            _projectilePrefab);

        _hits = new RaycastHit2D[3];
        var intercept = new LanderInterceptHuman(
            _transform, GetNearestHuman, _speed);

        var abduct = new LanderAbductHuman(_transform, _mutateHeight, _speed);

        var mutate = new LanderMutate(_transform);
        
        _stateMachine.AddTransition(
            patrol,
            intercept,
            () => GetNearestHuman() != null);
        
        _stateMachine.AddTransition(
            intercept,
            patrol,
            () => intercept.HumanTargetLost);
        
        _stateMachine.AddTransition(
            intercept,
            abduct,
            () => _mutableMob.HumanPassenger() != null);
        
        _stateMachine.AddTransition(
            abduct,
            mutate,
            () => _transform.position.y >= _mutateHeight);
        
        _stateMachine.AddTransition(
            abduct,
            patrol,
            () => _mutableMob.HumanPassenger() == null);
        
        _stateMachine.SetState(patrol);
    }

    void Update()
    {
        _stateMachine.Tick();
    }

    Transform GetNearestHuman()
    {
        if (_nearestHuman != null) return _nearestHuman;
        var hits = Physics2D.CircleCastNonAlloc(_transform.position, 2f, Vector2.down, _hits, 1f);
        for (var i = 0; i < hits && !_nearestHuman; ++i)
        {
            if (!IsHuman(i, out var human)) continue;
            if (AlreadyCaptured(human)) continue;
            _nearestHuman = human.transform;
        }

        return _nearestHuman;
    }

    bool IsHuman(int i, out Human human)
    {
        return _hits[i].transform.TryGetComponent<Human>(out human);
    }

    bool AlreadyCaptured(Human human)
    {
        var parent = human.transform.parent;
        if (!parent) return false;
        return (parent.TryGetComponent<MutatableMob>(out var mob) ||
                parent.TryGetComponent<PlayerShip>(out var player)) ;
    }
}