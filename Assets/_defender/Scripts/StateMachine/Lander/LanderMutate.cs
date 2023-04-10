using UnityEngine;

public class LanderMutate : IState
{
    readonly Transform _transform;

    public LanderMutate(Transform transform)
    {
        _transform = transform;
    }

    public void Tick()
    {
    }

    public void OnEnter()
    {
        Debug.Log($"{_transform.name} entering mutate state.");
        if (!_transform.TryGetComponent<MutatableMob>(out var mob)) return;
        mob.Mutate();
    }

    public void OnExit()
    {
    }
}