using System;
using UnityEngine;

public class LanderInterceptHuman : IState
{
    public bool HumanTargetLost
    {
        get
        {
            if (!_nearestHuman) return true;
            var parent = _nearestHuman.parent;
            if (!parent) return false;
            if (!parent.TryGetComponent<MutatableMob>(out var mutatableMob)) return false;
            return mutatableMob.transform != _transform;
        }
        
    }
    
    readonly Transform _transform;
    readonly Func<Transform> _getNearestHuman;
    readonly float _interceptSpeed;
    Transform _nearestHuman;


    public LanderInterceptHuman(
        Transform transform,
        Func<Transform> getNearestHuman,
        float interceptSpeed)
    {
        _transform = transform;
        _getNearestHuman = getNearestHuman;
        _interceptSpeed = interceptSpeed;
    }

    public void Tick()
    {
        if (!_transform || !_nearestHuman) return;
        
        _transform.MoveTowardsTarget(_nearestHuman, _interceptSpeed);

        if (!(_transform.position.FlatDistance(_nearestHuman.position) < 0.5f)) return;
        
        Debug.Log($"{_transform.name} capturing {_nearestHuman.name}.");
        _nearestHuman.SetParent(_transform);
    }

    public void OnEnter()
    {
        Debug.Log($"{_transform.name} entering intercept state.");
        _nearestHuman = _getNearestHuman();
    }

    public void OnExit()
    {
        Debug.Log($"{_transform.name} exiting intercept state.");
    }
}