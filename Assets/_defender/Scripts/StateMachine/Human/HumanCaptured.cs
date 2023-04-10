using UnityEngine;

public class HumanCaptured : IState
{
    readonly Transform _transform;

    public HumanCaptured(Transform transform)
    {
        _transform = transform;
    }

    public void Tick()
    {
        if (Helpers.EvenFrame) return;
        _transform.localPosition = Constants.RidingPosition;
    }

    public void OnEnter()
    {
        Debug.Log($"{_transform.name} entered captured state.");
        _transform.localPosition = Constants.RidingPosition;
    }

    public void OnExit()
    {
        Debug.Log($"{_transform.name} exiting cpatured state.");
    }
}