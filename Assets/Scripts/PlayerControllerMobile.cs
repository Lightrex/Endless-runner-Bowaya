using UnityEngine;

public class PlayerControllerPrototype : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 7f;
    public int maxJumpCount = 2;
    public float maxHoldTime = 0.3f; // long press for higher jump

    [Header("Coyote Time & Jump Buffer")]
    public float coyoteTime = 0.1f;       // grace time after leaving ground
    public float jumpBufferTime = 0.1f;   // grace time before landing

    private int currentJump = 0;
    private Rigidbody2D rb;

    // Input handling
    private bool isHolding = false;
    private float holdStartTime;
    private bool jumpButtonReleased = true;

    // Timers for coyote time and jump buffer
    private float coyoteTimer;
    private float jumpBufferTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        coyoteTimer -= Time.deltaTime;
        jumpBufferTimer -= Time.deltaTime;

#if UNITY_EDITOR
        // --- Keyboard Testing ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferTimer = jumpBufferTime; // store jump input
            jumpButtonReleased = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            EndJump();
            isHolding = false;
            jumpButtonReleased = true;
        }
#endif

        HandleTouch();

        // Check if we can jump (buffered + coyote support)
        if (jumpBufferTimer > 0 && (currentJump < maxJumpCount || coyoteTimer > 0))
        {
            StartJump();
            isHolding = true;
            holdStartTime = Time.time;
            jumpBufferTimer = 0; // consume buffered jump
        }
    }

    void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            bool isLeftSide = t.position.x < Screen.width / 2;

            switch (t.phase)
            {
                case TouchPhase.Began:
                    if (isLeftSide)
                    {
                        jumpBufferTimer = jumpBufferTime; // buffer touch
                        jumpButtonReleased = false;
                    }
                    break;

                case TouchPhase.Ended:
                    if (isHolding)
                    {
                        EndJump();
                        isHolding = false;
                        jumpButtonReleased = true;
                    }
                    break;
            }
        }
    }

    void StartJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        currentJump++;
        coyoteTimer = 0f; // reset coyote time after using it
    }

    void EndJump()
    {
        if (rb.velocity.y > 0)
        {
            float heldTime = Time.time - holdStartTime;
            if (heldTime < maxHoldTime)
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            currentJump = 0;
            coyoteTimer = coyoteTime; // reset coyote window when touching ground
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Start coyote timer when leaving ground
            coyoteTimer = coyoteTime;
        }
    }
}
