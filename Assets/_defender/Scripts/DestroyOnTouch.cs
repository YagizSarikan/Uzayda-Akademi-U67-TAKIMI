using UnityEngine;

public class DestroyOnTouch : MonoBehaviour
{
    [SerializeField] LayerMask _layersToIgnore;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (ShouldIgnoreThisCollision(other)) return;
        if (other.TryGetComponent<Destructable>(out var target))
        {
            target.DestroyMe();
        }
    }

    bool ShouldIgnoreThisCollision(Collider2D other)
    {
        return (_layersToIgnore & (1 << other.gameObject.layer)) > 0;
    }
}
