using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    [SerializeField]private int maxHeath;
    [SerializeField]private int maxMagic;
    [SerializeField]private int remainingHealth;
    [SerializeField]private int remainingMagic;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float randomNumber = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private InputSystem_Actions input;
    private Camera mainCam;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        input = new InputSystem_Actions();
        mainCam = Camera.main;
    }

    void OnEnable()
    {
        input.Player.Enable();
    }

    void OnDisable()
    {
        input.Player.Disable();
    }

    void Update()
    {
        movement = input.Player.Move.ReadValue<Vector2>().normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;

        Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = mouseWorldPos - rb.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        rb.MoveRotation(angle- 90f);
        
    }
}




// [SerializeField]private int maxHeath;
//     [SerializeField]private int maxMagic;
//     [SerializeField]private int remainingHealth;
//     [SerializeField]private int remainingMagic;