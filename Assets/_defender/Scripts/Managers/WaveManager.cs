using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int RemainingEnemies => _enemies.Count;

    public void MutateMobs()
    {
        Debug.Log($"Mutating mobs in {gameObject.name}");
        while (_enemies.Any(MutateMob)) { }
    }

    [SerializeField] float _waveDelay = 0f;

    GameManager _gameManager;

    List<GameObject> _enemies;

    IEnumerator Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _gameManager.EntityDestroyed += OnEntityDestroyed;
        _enemies = new List<GameObject>();
        foreach (Transform enemy in transform)
        {
            if (enemy.TryGetComponent<MutatableMob>(out var mutatableMob))
            {
                mutatableMob.OnMutate += HandleOnMutate;
            }
            _enemies.Add(enemy.gameObject);
        }

        yield return new WaitForSeconds(_waveDelay);
        SoundManager.Instance.PlayAudioClip(SoundManager.Instance.BeamInSound);
        
        // instantiate beam-in particle effect
        foreach (var enemy in _enemies)
        {
            Instantiate(
                EffectsManager.Instance.BeamInPrefab,
                enemy.transform.position, 
                Quaternion.identity);
        }
        
        yield return new WaitForSeconds(1f);
        foreach (var enemy in _enemies)
        {
            enemy.SetActive(true);
        }
    }

    void HandleOnMutate(MutatableMob mob, GameObject mutant)
    {
        Debug.Log($"{name} HandleOnMutate({mob.name}, {mutant.name}. Adding to enemies list.");
        mob.OnMutate -= HandleOnMutate;
        mutant.transform.SetParent(transform);
        mob.GetComponent<Destructable>().DestroyMe();
    }

    void OnEntityDestroyed(GameObject entity)
    {
        if (!_enemies.Contains(entity)) return;
        Debug.Log($"{name} removing {entity.name} from enemies.");
        _enemies.Remove(entity);
    }

    bool MutateMob(GameObject mob)
    {
        if (!mob.TryGetComponent<MutatableMob>(out var mutatableMob)) return false;
        mutatableMob.Mutate();
        return true;
    }
}
