using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float baseSpeed = 2f;
    public float speedIncreaseInterval = 15f;
    public float speedIncreaseAmount = 0.5f;
    [HideInInspector] public float currentSpeed;

    private float timer = 0f;

    void Awake() => Instance = this;

    void Start() => currentSpeed = baseSpeed;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= speedIncreaseInterval)
        {
            currentSpeed += speedIncreaseAmount;
            timer = 0f;
        }
    }
}
