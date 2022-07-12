using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
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
        if (Input.GetKeyUp(KeyCode.G))
            Gen();
    }
    void Gen()
    {        
        StartCoroutine(Render());
    }
    public IEnumerator Render()
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

        var dataTask = ChunkDataManager.GenerateChunk(new Vector2Int((int)transform.position.x, (int)transform.position.z), noise);
        
        while (dataTask.IsCompleted == false)
        {
            yield return new WaitForFixedUpdate();
        }
        //task1.Wait();
        
        var data = dataTask.Result;

        var task = ChunkMeshBuilder.Build(data);

        var r = GetComponent<MeshFilter>();

        while (task.IsCompleted == false)
            yield return new WaitForFixedUpdate();


        r.mesh = task.Result;

        GetComponent<MeshCollider>().sharedMesh = task.Result;

        yield break;


    }
}
