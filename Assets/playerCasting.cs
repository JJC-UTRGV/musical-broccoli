using UnityEngine;
using UnityEngine.InputSystem;

public class playerCasting : MonoBehaviour
{
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private float spellSpeed = 12f;
    [SerializeField] private Transform firePoint;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            CastSpell();
        }
    }

    void CastSpell()
    {
        GameObject spell = Instantiate(
            spellPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody2D rb = spell.GetComponent<Rigidbody2D>();
        rb.linearVelocity = firePoint.up * spellSpeed;
    }
}

