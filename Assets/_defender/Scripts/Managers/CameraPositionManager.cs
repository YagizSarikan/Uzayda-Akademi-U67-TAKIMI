using System;
using UnityEngine;

public class CameraPositionManager : MonoBehaviour
{
        [SerializeField] Vector3 _leftPosition, _rightPosition;
        [SerializeField] float _lerpSpeed = 10f;
        
        Transform _transform;
        GameManager _gameManager;
        PlayerShip PlayerShip => _gameManager.PlayerShip;

        void Awake()
        {
                _transform = transform;
                _gameManager = FindObjectOfType<GameManager>();
        }

        void LateUpdate()
        {
                if (PlayerShip == null) return;
                var desiredPosition = PlayerShip.Direction > 0 ? _leftPosition : _rightPosition;
                if (Vector3.Distance(desiredPosition, _transform.position) > float.Epsilon)
                {
                        _transform.position = Vector3.Lerp(_transform.position, desiredPosition,
                                _lerpSpeed * Time.deltaTime);
                }
        }
}
