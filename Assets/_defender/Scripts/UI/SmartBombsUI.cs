using UnityEngine;

public class SmartBombsUI : MonoBehaviour
{
    [SerializeField] GameObject _smartBombPrefab;
    Transform _transform;
    GameManager _gameManager;

    void Awake()
    {
        _transform = transform;
        _gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        _gameManager.SmartBombsChanged += OnSmartBombsChanged;
    }

    void OnSmartBombsChanged(int bombs)
    {
        while (_transform.childCount > bombs)
        {
            Transform bomb = _transform.GetChild(0);
            bomb.SetParent(null);
            Destroy(bomb.gameObject);
        }

        while (_transform.childCount < bombs)
        {
            Instantiate(_smartBombPrefab, _transform);
        }
    }
}
