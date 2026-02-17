using UnityEngine;
using UnityEngine.InputSystem;

public class playerCasting : MonoBehaviour
{
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private float spellSpeed = 12f;

    [SerializeField] private Transform aimPivot;
    [SerializeField] private Transform firePoint;

    private Camera mainCam;
    private bool hasAimed = false;

    void Awake()
    {
        mainCam = Camera.main;
        aimPivot.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            hasAimed = true;
            RotateAimPivot();
            CastSpell();
            return;
        }

        if (hasAimed)
            RotateAimPivot();
    }

    void CastSpell()
    {
        Quaternion projRot = firePoint.rotation * Quaternion.Euler(0f, 0f, 90f);
        GameObject spell = Instantiate(spellPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = spell.GetComponent<Rigidbody2D>();
        rb.linearVelocity = (Vector2)firePoint.right * spellSpeed;
    }

    void RotateAimPivot()
    {
        Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = mouseWorldPos - (Vector2)aimPivot.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        aimPivot.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}



