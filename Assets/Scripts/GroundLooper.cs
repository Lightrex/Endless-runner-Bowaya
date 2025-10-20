using UnityEngine;

public class GroundLooper : MonoBehaviour
{
    public float loopPoint = -20f;
    public float resetX = 40f;

    void Update()
    {
        transform.Translate(Vector2.left * GameManager.Instance.currentSpeed * Time.deltaTime);

        if (transform.position.x < loopPoint)
            transform.position += new Vector3(resetX, 0, 0);
    }
}
