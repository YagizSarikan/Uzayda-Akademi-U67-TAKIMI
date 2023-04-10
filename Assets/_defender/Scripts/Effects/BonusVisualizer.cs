using UnityEngine;

public class BonusVisualizer : MonoBehaviour
{
    [SerializeField] Vector2 _moveSpeed;
    [SerializeField] float _fadeTime = 8f;
    [SerializeField] int _points = 500;
    [SerializeField] SpriteRenderer[] _spriteRenderers;
    
    Transform _transform;
    float _timer;

    void Awake()
    {
        _transform = transform;
    }

    void OnEnable()
    {
        SoundManager.Instance.PlayAudioClip(SoundManager.Instance.BonusSound, 0.25f);
        FindObjectOfType<GameManager>().AddPoints(_points);
        Destroy(gameObject, _fadeTime + 0.1f);
        _timer = 0f;
    }

    void Update()
    {
        _transform.position += (Vector3)_moveSpeed * Time.deltaTime;
        foreach (var spriteRenderer in _spriteRenderers)
        {
            var spriteColor = spriteRenderer.material.color;
            spriteColor.a = 1.0f - Mathf.Clamp01(_timer / _fadeTime);
            spriteRenderer.material.color = spriteColor;
            _timer += Time.deltaTime;
        }
    }
}
