using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    AudioSource _audioSource;
    
    public static SoundManager Instance { get; private set; }

    [SerializeField] AudioClip _fireSound, _startSound, _beamInSound, _bonusSound;

    public AudioClip FireSound => _fireSound;
    public AudioClip StartSound => _startSound;
    public AudioClip BeamInSound => _beamInSound;
    public AudioClip BonusSound => _bonusSound;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(AudioClip clip, float volume = 1f)
    {
        _audioSource.PlayOneShot(clip, volume);
    }
}