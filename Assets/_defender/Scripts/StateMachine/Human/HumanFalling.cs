using System;
using UnityEngine;

public class HumanFalling : IState
{
    readonly Transform _transform;
    readonly float _fallingSpeedMin;
    readonly float _fallingSpeedMax;
    readonly Destructable _destructable;
    bool _dead;
    float _fallingSpeed;

    public HumanFalling(Transform transform, float fallingSpeedMin, float fallingSpeedMax)
    {
        _transform = transform;
        _fallingSpeedMin = fallingSpeedMin;
        _fallingSpeedMax = fallingSpeedMax;
        _destructable = transform.GetComponent<Destructable>();
    }

    public void Tick()
    {
        if (_dead) return;
        Fall();
        CheckForDeath();
    }

    public void OnEnter()
    {
        Debug.Log($"{_transform.name} entering falling state.");
        _fallingSpeed = _fallingSpeedMin;
    }

    public void OnExit()
    {
        Debug.Log($"{_transform.name} existing falling state.");
    }

    void Fall()
    {
        var position = _transform.position;
        position.y -= (_fallingSpeed * Time.deltaTime);
        _transform.position = position;
        _fallingSpeed += (0.5f * Time.deltaTime);
        _fallingSpeed = Math.Min(_fallingSpeed, _fallingSpeedMax);
    }

    void CheckForDeath()
    {
        if (_transform.position.y < -3.9f && Mathf.Approximately(_fallingSpeedMax, _fallingSpeed))
        {
            Debug.Log($"{_transform.name} fell to their death!");
            _destructable.DestroyMe();
        }
    }
}