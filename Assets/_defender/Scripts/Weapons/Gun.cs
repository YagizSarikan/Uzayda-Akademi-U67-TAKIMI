using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject _projectilePrefab;
    [SerializeField] float _fireSoundVolume = 0.33f;
    [SerializeField] float _fireDelay = 0.25f;

    float _coolDownTime;
    Transform _transform;

    bool CanFire => Time.time >= _coolDownTime;

    void Awake()
    {
        _transform = transform;
    }

    void OnEnable()
    {
        _coolDownTime = Time.time;
    }

    public void FireGun()
    {
        if (!CanFire) return;
        SoundManager.Instance.PlayAudioClip(SoundManager.Instance.FireSound, _fireSoundVolume);
        var projectile = Instantiate(_projectilePrefab, _transform.position, _transform.rotation);
        projectile.gameObject.SetActive(true);
        _coolDownTime = Time.time + _fireDelay;
    }
}
