using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour, IUserInput
{
    public static IUserInput Instance;
    
    public event Action<InputValue> OnMoveReceived;
    public event Action<InputValue> OnThrustReceived;
    public event Action OnFlipDirectionPressed;
    public event Action OnFirePressed;
    public event Action OnStartPressed;
    public event Action OnSubmitPressed;
    public event Action OnSmartBombPressed;
    public event Action OnHyperspacePressed;

    public Vector2 MoveInput => _moveInput;
    public bool UpPressed => _moveInput.y > 0;
    public bool DownPressed => _moveInput.y < 0;
    public bool IsThrusting => _thrustInput.x >= 0.5f;
    
    Vector2 _moveInput;
    Vector2 _thrustInput;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        OnMoveReceived?.Invoke(value);
    }

    void OnThrust(InputValue value)
    {
        _thrustInput = value.Get<Vector2>();
        OnThrustReceived?.Invoke(value);
    }

    void OnFlipDirection() => OnFlipDirectionPressed?.Invoke();
    void OnFire() => OnFirePressed?.Invoke();
    void OnStart() => OnStartPressed?.Invoke();
    void OnSubmit() => OnSubmitPressed?.Invoke();
    void OnSmartBomb() => OnSmartBombPressed?.Invoke();
    void OnHyperspace() => OnHyperspacePressed?.Invoke();
}

