using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] Renderer _renderer;
    
    float _scrollSpeedFactor = 1f;
    IUserInput _userInput;
    GameManager _gameManager;
    static readonly int MainTex = Shader.PropertyToID("_MainTex");

    PlayerShip PlayerShip => _gameManager.PlayerShip;

    void Awake()
    {
        _userInput = UserInput.Instance;
        _gameManager = FindObjectOfType<GameManager>();
        _scrollSpeedFactor = _renderer.material.GetTextureScale(MainTex).x / transform.localScale.x;
    }

    void LateUpdate()
    {
        if (!PlayerShip || !_userInput.IsThrusting) return;
        var currentTextureOffset = _renderer.material.GetTextureOffset(MainTex);
        var distanceToScroll = Time.deltaTime * PlayerShip.Speed * PlayerShip.Direction * _scrollSpeedFactor;
        var newOffset = currentTextureOffset + Vector2.right * distanceToScroll;
        _renderer.material.SetTextureOffset(MainTex, newOffset);
    }
}
