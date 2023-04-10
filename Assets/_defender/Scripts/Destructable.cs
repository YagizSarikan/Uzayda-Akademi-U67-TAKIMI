using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] int _points = 150;

    GameManager _gameManager;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    [ContextMenu("Destroy Me")]
    public void DestroyMe()
    {
        Instantiate(EffectsManager.Instance.ExplosionPrefab, transform.position, Quaternion.identity);
        _gameManager.AddPoints(_points);
        _gameManager.ComponentDestroyed(gameObject);
    }
}
