using UnityEngine;

public class LanderAbductHuman : IState
{
    readonly Transform _transform;
    readonly float _mutateHeight;
    readonly float _speed;
    GameObject _destination;

    public LanderAbductHuman(Transform transform, float mutateHeight, float speed)
    {
        _transform = transform;
        _mutateHeight = mutateHeight;
        _speed = speed;
    }
    
    public void Tick()
    {
        _transform.MoveTowardsTarget(_destination.transform, _speed);
    }

    public void OnEnter()
    {
        Debug.Log($"{_transform.name} entering abduct human state.");
        SetDestination();
    }

    public void OnExit()
    {
        Debug.Log($"{_transform.name} existing abduct human state.");
        if (_destination)
        {
            Object.Destroy(_destination);
        }
    }

    void SetDestination()
    {
        var position = _transform.position;
        position.y = _mutateHeight;
        _destination = new GameObject($"{_transform.name} destination");
        _destination.AddComponent<MobScroller>();
        _destination.transform.position = position;
    }
}