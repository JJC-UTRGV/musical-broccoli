using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private int maxHeath;
    [SerializeField] private int maxMagic;
    [SerializeField] private int remainingHealth;
    [SerializeField] private int remainingMagic;

    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private Sprite idleUp;
    [SerializeField] private Sprite idleRight;
    [SerializeField] private Sprite idleDown;
    [SerializeField] private Sprite idleLeft;

    [SerializeField] private Transform spriteVisual; 
    [SerializeField] private float bobAmount = 0.03f;
    [SerializeField] private float bobSpeed = 14f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    private InputSystem_Actions input;
    private Camera mainCam;

    private Vector3 spriteBaseLocalPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        input = new InputSystem_Actions();
        mainCam = Camera.main;

        
        if (spriteVisual == null)
            spriteVisual = transform.Find("Visual/Sprite");

        
        spriteRenderer = (spriteVisual != null)
            ? spriteVisual.GetComponent<SpriteRenderer>()
            : GetComponentInChildren<SpriteRenderer>();

        if (spriteVisual != null)
            spriteBaseLocalPos = spriteVisual.localPosition;
    }

    void OnEnable() => input.Player.Enable();
    void OnDisable() => input.Player.Disable();

    void Update()
    {
        movement = input.Player.Move.ReadValue<Vector2>();

        
        if (movement.sqrMagnitude > 1f) movement = movement.normalized;

        UpdateDirection();
        UpdateBobbing();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    void UpdateDirection()
    {
        if (spriteRenderer == null || mainCam == null || Mouse.current == null) return;

        Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 delta = mouseWorldPos - rb.position;

        
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            spriteRenderer.sprite = (delta.x >= 0f) ? idleRight : idleLeft;
        else
            spriteRenderer.sprite = (delta.y >= 0f) ? idleUp : idleDown;
    }

    void UpdateBobbing()
    {
        if (spriteVisual == null) return;

        bool isMoving = movement.sqrMagnitude > 0.001f;

        if (isMoving)
        {
            float bob = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
            spriteVisual.localPosition = spriteBaseLocalPos + new Vector3(0f, bob, 0f);
        }
        else
        {
            spriteVisual.localPosition = spriteBaseLocalPos;
        }
    }
}

