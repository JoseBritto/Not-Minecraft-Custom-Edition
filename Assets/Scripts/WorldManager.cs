using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public Vector2 WorldSize = new Vector2(5, 5);

    public GameObject ChunkPrefab;

    private void Start()
    {
        for(int x = 0; x < WorldSize.x; x++)
        {
            for(int y = 0; y < WorldSize.y; y++)
            {
               var go = Instantiate(ChunkPrefab, new Vector3(x * ChunkBlockData.SIZE, 0, y * ChunkBlockData.SIZE), Quaternion.identity);
                go.transform.parent = transform;
            }
        }
    }
}
