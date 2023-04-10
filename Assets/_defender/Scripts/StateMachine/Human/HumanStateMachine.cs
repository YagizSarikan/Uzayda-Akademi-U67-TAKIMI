using System;
using System.IO.IsolatedStorage;
using UnityEngine;

public class HumanStateMachine : MonoBehaviour
{
    public event Action<IState> HumanStateChanged = delegate(IState state) {  };
    public Type CurrentStateType => _stateMachine.CurrentState.GetType();
    
    [SerializeField] float _fallingSpeedMin = 0.25f, _fallingSpeedMax = 1f;

    Transform _transform;
    StateMachine _stateMachine;

    void Awake()
    {
        _transform = transform;
        _stateMachine = new StateMachine();
        _stateMachine.OnStateChanged += state => HumanStateChanged.Invoke(state);
        var idle = new HumanIdle(_transform);
        var captured = new HumanCaptured(_transform);
        var falling = new HumanFalling(_transform, _fallingSpeedMin, _fallingSpeedMax);
        
        _stateMachine.AddTransition(
            idle,
            captured,
            () => IsCaptured() == true);
        
        _stateMachine.AddTransition(
            captured,
            falling,
            () => IsCaptured() == false);
        
        _stateMachine.AddTransition(
            falling,
            idle,
            () => _transform.position.y <= -4f);
        
        _stateMachine.AddTransition(
            falling,
            captured,
            () => IsCaptured() == true);
        
        _stateMachine.SetState(idle);
    }

    void Update()
    {
        _stateMachine.Tick();
    }

    bool IsCaptured()
    {
        var parent = _transform.parent;
        if (!parent) return false;
        return parent.TryGetComponent<MutatableMob>(out var mob) ||
               parent.TryGetComponent<PlayerShip>(out var player);
    }
}