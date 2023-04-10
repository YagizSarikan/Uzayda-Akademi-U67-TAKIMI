using UnityEngine;

public class PlayerLivesUI : MonoBehaviour
{
    [SerializeField] GameObject _playerLifePrefab;
    Transform _transform;
    GameManager _gameManager;

    void Awake()
    {
        _transform = transform;
        _gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        _gameManager.PlayerLivesChanged += OnPlayerLivesChanged;
    }

    void OnPlayerLivesChanged(int lives)
    {
        while (_transform.childCount > lives)
        {
            Transform playerLife = _transform.GetChild(0);
            playerLife.SetParent(null);
            Destroy(playerLife.gameObject);
        }

        while (_transform.childCount < lives)
        {
            Instantiate(_playerLifePrefab, _transform);
        }
    }
}
