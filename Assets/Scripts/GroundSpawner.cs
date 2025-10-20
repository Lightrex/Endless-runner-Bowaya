using UnityEngine;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundPrefab;
    public float groundLength = 20f;
    public int numberOfTiles = 3;
    public Transform player;

    private Queue<GameObject> activeTiles = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            Vector3 pos = new Vector3(i * groundLength, -2f, 0);
            GameObject g = Instantiate(groundPrefab, pos, Quaternion.identity);
            activeTiles.Enqueue(g);
        }
    }

    void Update()
    {
        GameObject firstTile = activeTiles.Peek();
        if (player.position.x - firstTile.transform.position.x > groundLength)
        {
            GameObject oldTile = activeTiles.Dequeue();
            oldTile.transform.position += new Vector3(groundLength * numberOfTiles, 0, 0);
            activeTiles.Enqueue(oldTile);
        }
    }
}
