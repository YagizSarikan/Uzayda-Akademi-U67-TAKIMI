using UnityEngine;

public class MobScroller : MonoBehaviour
{
    Transform _transform;
    GameManager _gameManager;

    PlayerShip PlayerShip => _gameManager.PlayerShip;
    bool ShouldScrollMob => UserInput.Instance.IsThrusting;
    float ScrollAmount => PlayerShip.Speed * PlayerShip.Direction * -1f;
    
    void Awake()
    {
        _transform = transform;
        _gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (!_gameManager || !PlayerShip) return;
        if (ShouldScrollMob)
        {
            ScrollMob();
        }
    }

    void ScrollMob()
    {
        var position = _transform.position + (Vector3.right * (ScrollAmount * Time.deltaTime));
        var leftEdge = _gameManager.MapWidth * -0.5f;
        var rightEdge = _gameManager.MapWidth * 0.5f;

        if (position.x < leftEdge)
        {
            position.x = rightEdge;
        }
        else if (position.x > rightEdge)
        {
            position.x = leftEdge;
        }
        
        _transform.position = position;
    }
}
