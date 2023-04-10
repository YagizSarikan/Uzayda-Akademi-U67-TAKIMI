using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MobManager : MonoBehaviour
{
    public int RemainingHumans => _humans.Count;
    public int RemainingEnemies => _waves.Sum(w => w.RemainingEnemies);
    public Transform HumansContainer => _humansContainer;
    
    [SerializeField] WaveManager[] _waves;
    [SerializeField] Transform _humansContainer;

    GameManager _gameManager;
    List<GameObject> _humans;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _gameManager.EntityDestroyed += OnEntityDestroyed;
        _humans = new List<GameObject>();
        foreach (Transform human in _humansContainer)
        {
            _humans.Add(human.gameObject);
        }
        Debug.Log($"Mob manager found {_humans.Count} humans.");
    }

    void OnEntityDestroyed(GameObject entity)
    {
        if (!_humans.Contains(entity)) return;
        _humans.Remove(entity);
        Debug.Log($"{name} removing {entity.name}. {_humans.Count} humans remain.");
        if (_humans.Count < 1)
        {
            MutateMobs();
        }
    }

    void MutateMobs()
    {
        Debug.Log($"Mutating mobs in {_waves.Length} waves");
        foreach (var wave in _waves)
        {
            wave.MutateMobs();
        }
    }
}
