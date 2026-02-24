using UnityEngine;

public class WallDestructionEffect : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.5f;
    [SerializeField] private ParticleSystem particles;

    void Start()
    {
        // Destroy the effect after lifetime
        Destroy(gameObject, lifetime);
    }
}
