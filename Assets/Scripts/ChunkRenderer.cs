using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class ChunkRenderer : MonoBehaviour
{
    private PerlinNoise3D noise;
    private void Start()
    {
        noise = PerlinNoise3D.Instance;
        Gen();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            Gen();
    }
    void Gen()
    {

        var data = ChunkDataManager.GenerateChunk(new Vector2Int((int)transform.position.x, (int)transform.position.z), noise);

        StartCoroutine(Render(data.Result));
    }
    public IEnumerator Render(ChunkBlockData data)
    {
        /* for(int x =0; x < ChunkBlockData.SIZE; x ++)
         {
             for(int z =0; z < ChunkBlockData.SIZE; z++)
             {
                 for(int y=0; y<ChunkBlockData.HEIGHT; y++)
                 {
                     if(data.GetBlock(x, y, z) == BlockType.Dirt)
                     {
                         var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                         Destroy(cube.GetComponent<BoxCollider>());
                         cube.transform.position = new Vector3(x, y, z);

                     }
                 }

             }

             yield return null;
         }*/
        var task = ChunkMeshBuilder.Build(data);
        while (task.IsCompleted == false)
            yield return null;


        var r = GetComponent<MeshFilter>();
        r.mesh = task.Result;

        yield break;


    }
}
