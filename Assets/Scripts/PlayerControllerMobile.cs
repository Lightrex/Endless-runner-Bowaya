using UnityEngine;

public class PlayerControllerPrototype : MonoBehaviour
{
    public float jumpForce = 7f;
    public int maxJumpCount = 2;
<<<<<<< Updated upstream
=======
    public float maxHoldTime = 0.3f; // long press for higher jump

    [Header("Coyote Time & Jump Buffer")]
    public float coyoteTime = 0.1f;      
    public float jumpBufferTime = 0.1f;   

>>>>>>> Stashed changes
    private int currentJump = 0;
    private Rigidbody2D rb;

    private bool isHolding = false;
    private float holdStartTime;
    public float maxHoldTime = 0.3f; // long press for higher jump
    private bool jumpButtonReleased = true; // prevent spam jumping

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
#if UNITY_EDITOR
        // Keyboard test: Space = jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpButtonReleased && currentJump < maxJumpCount)
            {
                StartJump();
                isHolding = true;
                holdStartTime = Time.time;
                jumpButtonReleased = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            EndJump();
            isHolding = false;
            jumpButtonReleased = true;
        }
#endif

        HandleTouch();
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
                    if (isLeftSide && jumpButtonReleased && currentJump < maxJumpCount)
                    {
                        StartJump();
                        isHolding = true;
                        holdStartTime = Time.time;
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
    }

    void EndJump()
    {
        // Short press = shorter jump
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
            currentJump = 0;
    }
}