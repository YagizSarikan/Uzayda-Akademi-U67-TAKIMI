using System;
using UnityEngine;

public class MutatableMob : MonoBehaviour
{
    public event Action<MutatableMob, GameObject> OnMutate = delegate(MutatableMob mob, GameObject mutant) {  };
    [SerializeField] GameObject _mutantMobPrefab;

    Transform _transform;

    public GameObject MutantMobPrefab => _mutantMobPrefab;

    public void DropHumanPassenger(Transform humanContainer)
    {
        var human = HumanPassenger();
        if (!human) return;
        human.transform.SetParent(humanContainer);
    }

    public Transform HumanPassenger()
    {
        foreach (Transform child in _transform)
        {
            if (child.TryGetComponent<Human>(out var human))
            {
                return child;
            }
        }

        return null;
    }

    public void Mutate()
    {
        var humanPassenger = HumanPassenger();
        if (humanPassenger && humanPassenger.TryGetComponent<Destructable>(out var human))
        {
            Debug.Log($"{name} mutating. Destroying human passenger {human.name}.");
            human.DestroyMe();
        }

        var mutant = Instantiate(_mutantMobPrefab, _transform.position, Quaternion.identity);
        OnMutate(this, mutant);
    }

    void Awake()
    {
        _transform = transform;
    }
}
