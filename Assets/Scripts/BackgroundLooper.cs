using UnityEngine;

public class BackgroundLooperWithSpeedUp : MonoBehaviour
{
    public float startSpeed = 2f;      // starting speed
    public float speedIncrease = 0.5f; // how much speed increases every interval
    public float interval = 15f;       // seconds between each speed increase

    private float currentSpeed;
    private float width;
    private float timer;

    void Start()
    {
        currentSpeed = startSpeed;
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        timer = 0f;
    }

    void Update()
    {
        // Move background to the left
        transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);

        // Loop background
        if (transform.position.x < -width)
        {
            Vector2 newPos = new Vector2(transform.position.x + 2 * width, transform.position.y);
            transform.position = newPos;
        }

        // Timer for increasing speed
        timer += Time.deltaTime;
        Debug.Log("Timer: " + timer);
        if (timer >= interval)
        {
            currentSpeed += speedIncrease;
            timer = 0f; // reset timer
        }
    }
}
