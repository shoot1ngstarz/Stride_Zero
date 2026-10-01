using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    // Old input system axis names (Edit > Project Settings > Input Manager)
    [SerializeField] private string horizontalAxis = "Horizontal";
    [SerializeField] private string verticalAxis = "Vertical";

    private Vector2 movement;
    private Rigidbody2D rb;

    [Header("Animation")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] idleSprites;
    [SerializeField] private Sprite[] walkUpSprites;
    [SerializeField] private Sprite[] walkDownSprites;
    [SerializeField] private Sprite[] walkLeftSprites;
    [SerializeField] private Sprite[] walkRightSprites;
    [SerializeField] private float animationSpeed = 0.1f; // seconds per frame

    private float animationTimer = 0f;
    private int currentFrame = 0;
    private Sprite[] lastAnimation = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // OLD input system
        float x = Input.GetAxisRaw(horizontalAxis);
        float y = Input.GetAxisRaw(verticalAxis);

        movement = new Vector2(x, y);

        // Optional: normalize so diagonal isn't faster
        if (movement.sqrMagnitude > 1f)
            movement = movement.normalized;

        HandleAnimation();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    private void HandleAnimation()
    {
        if (spriteRenderer == null) return;

        Sprite[] currentAnimation = idleSprites;

        if (movement.y > 0.1f)
            currentAnimation = walkUpSprites;
        else if (movement.y < -0.1f)
            currentAnimation = walkDownSprites;
        else if (movement.x < -0.1f)
            currentAnimation = walkLeftSprites;
        else if (movement.x > 0.1f)
            currentAnimation = walkRightSprites;

        if (currentAnimation != lastAnimation)
        {
            lastAnimation = currentAnimation;
            currentFrame = 0;
            animationTimer = 0f;

            if (currentAnimation != null && currentAnimation.Length > 0)
                spriteRenderer.sprite = currentAnimation[currentFrame];
        }

        if (currentAnimation == null || currentAnimation.Length == 0) return;

        animationTimer += Time.deltaTime;
        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;
            currentFrame = (currentFrame + 1) % currentAnimation.Length;
            spriteRenderer.sprite = currentAnimation[currentFrame];
        }
    }
}