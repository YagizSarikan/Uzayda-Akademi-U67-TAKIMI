using UnityEngine;

public class HumanIdle : IState
{
    readonly Transform _transform;

    public HumanIdle(Transform transform)
    {
        _transform = transform;
    }
    
    public void Tick()
    {
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }
}