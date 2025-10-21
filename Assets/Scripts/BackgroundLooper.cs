using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
    private float width;

    void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Use the shared game speed from GameManager
        float speed = GameManager.Instance.currentSpeed;

        // Move background to the left
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Loop background
        if (transform.position.x < -width)
        {
            Vector2 newPos = new Vector2(transform.position.x + 2 * width, transform.position.y);
            transform.position = newPos;
        }
    }
}
