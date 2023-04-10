using System;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance { get; private set; }

    [SerializeField] GameObject _explosionPrefab, _beamInPrefab;

    public GameObject ExplosionPrefab => _explosionPrefab;
    public GameObject BeamInPrefab => _beamInPrefab;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
