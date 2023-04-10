using System;
using UnityEngine;

public class CatchHuman : MonoBehaviour
{
    [SerializeField] GameObject _bonusPrefab250, _bonusPrefab500;

    Transform _humanContainer;
    Transform _transform;
    Human _human;

    bool CarryingHuman => (_human = GetComponentInChildren<Human>()) != null;
    bool _humanDropped, _humanCaught;
    
    public void Init(Transform humansContainer)
    {
        _humanContainer = humansContainer;
    }

    void Awake()
    {
        _transform = transform;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        TryCatchHuman(other);
    }

    void Update()
    {
        if (Helpers.EvenFrame) return;
        if (!CarryingHuman) return;
        TryReleaseHuman();
    }

    void TryCatchHuman(Collider2D other)
    {
        if (_humanCaught) return;
        if (!other.TryGetComponent<HumanStateMachine>(out var human)) return;
        if (human.CurrentStateType != typeof(HumanFalling)) return;
        if (CarryingHuman) return;
        _humanCaught = true;
        Instantiate(_bonusPrefab500, human.transform.position, Quaternion.identity);
        human.transform.SetParent(_transform);
    }

    void TryReleaseHuman()
    {
        if (_humanDropped) return;
        if (!(_transform.position.y < -3.5f)) return;
        _humanDropped = true;
        Instantiate(_bonusPrefab250, _human.transform.position, Quaternion.identity);
        _human.transform.SetParent(_humanContainer);        
    }
}
