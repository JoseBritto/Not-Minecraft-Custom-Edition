using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChunkMeshBuilder 
{
   public static Task<Mesh> Build(ChunkBlockData data)
   {
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for(int x = 0; x < ChunkBlockData.SIZE; x++)
        {
            for(int y =0; y < ChunkBlockData.HEIGHT; y++)
            {
                for(int z =0; z < ChunkBlockData.SIZE; z++)
                {
                    if (data.GetBlock(x, y, z) == BlockType.Air)
                    {
                        continue; // This block is air
                    }

                    if (x == ChunkBlockData.SIZE - 1 || data.GetBlock(x + 1, y, z) == BlockType.Air)
                    {
                        AddRightFace(x, y, z, verts, tris, uvs);
                    }

                    if (x == 0 || data.GetBlock(x - 1, y, z) == BlockType.Air)
                    {
                        AddLeftFace(x, y, z, verts, tris, uvs);
                    }

                    if (z == ChunkBlockData.SIZE - 1 || data.GetBlock(x, y, z + 1) == BlockType.Air)
                    {
                        AddFrontFace(x, y, z, verts, tris, uvs);
                    }

                    if (z == 0 || data.GetBlock(x, y, z - 1) == BlockType.Air)
                    {
                        AddBackFace(x, y, z, verts, tris, uvs);
                    }


                    if (y == ChunkBlockData.HEIGHT - 1 || data.GetBlock(x, y + 1, z) == BlockType.Air)
                    {
                        AddTopFace(x, y, z, verts, tris, uvs);
                    }

                    if (y == 0 || data.GetBlock(x, y - 1, z) == BlockType.Air)
                    {
                        AddBottomFace(x, y, z, verts, tris, uvs);
                    }
                }
            }
        }

        var mesh = new Mesh();

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        mesh.SetUVs(0, uvs);
        //mesh.Optimize();

        return Task.FromResult(mesh);

   }



    private static void AddRightFace(int x, int y, int z, List<Vector3> verts, List<int> tris, List<Vector2> uv = null)
    {
            verts.Add(new Vector3(x + 1, y + 0, z + 0)); // 0
            verts.Add(new Vector3(x + 1, y + 1, z + 0)); // 1
            verts.Add(new Vector3(x + 1, y + 1, z + 1)); // 2
            verts.Add(new Vector3(x + 1, y + 0, z + 1)); // 3

            int count = verts.Count;

            tris.Add(count - 4); // 0
            tris.Add(count - 3); // 1
            tris.Add(count - 2); // 2

            tris.Add(count - 4); // 0
            tris.Add(count - 2); // 2
            tris.Add(count - 1); // 3

            if (uv == null) return;

            uv.Add(new Vector2(0, 0));
            uv.Add(new Vector2(0, 1));
            uv.Add(new Vector2(1, 1));
            uv.Add(new Vector2(1, 0));

    }

    private static void AddLeftFace(int x, int y, int z, List<Vector3> verts, List<int> tris, List<Vector2> uv = null)
    {

            verts.Add(new Vector3(x, y, z)); // 0
            verts.Add(new Vector3(x, y + 1, z)); // 1
            verts.Add(new Vector3(x, y + 1, z + 1)); // 2
            verts.Add(new Vector3(x, y + 0, z + 1)); // 3

            int count = verts.Count;

            tris.Add(count - 2); // 2
            tris.Add(count - 3); // 1        
            tris.Add(count - 4); // 0


            tris.Add(count - 2); // 2
            tris.Add(count - 4); // 0
            tris.Add(count - 1); // 3


            if (uv == null) return;

            uv.Add(new Vector2(0, 0));
            uv.Add(new Vector2(0, 1));
            uv.Add(new Vector2(1, 1));
            uv.Add(new Vector2(1, 0));

    }

    private static void AddTopFace(int x, int y, int z, List<Vector3> verts, List<int> tris, List<Vector2> uv = null)
    {        

            verts.Add(new Vector3(x, y + 1, z)); // 0
            verts.Add(new Vector3(x, y + 1, z + 1)); // 1
            verts.Add(new Vector3(x + 1, y + 1, z + 1)); // 2
            verts.Add(new Vector3(x + 1, y + 1, z)); // 3

            int count = verts.Count;

            tris.Add(count - 4); // 0
            tris.Add(count - 3); // 1
            tris.Add(count - 2); // 2


            tris.Add(count - 4); // 0
            tris.Add(count - 2); // 2
            tris.Add(count - 1); // 3


            if (uv == null) return;

            uv.Add(new Vector2(0, 0));
            uv.Add(new Vector2(0, 1));
            uv.Add(new Vector2(1, 1));
            uv.Add(new Vector2(1, 0));

    }

    private static void AddBottomFace(int x, int y, int z, List<Vector3> verts, List<int> tris, List<Vector2> uv = null)
    {

            verts.Add(new Vector3(x, y, z)); // 0
            verts.Add(new Vector3(x, y, z + 1)); // 1
            verts.Add(new Vector3(x + 1, y, z + 1)); // 2
            verts.Add(new Vector3(x + 1, y, z)); // 3

            int count = verts.Count;


            tris.Add(count - 2); // 2
            tris.Add(count - 3); // 1        
            tris.Add(count - 4); // 0


            tris.Add(count - 2); // 2
            tris.Add(count - 4); // 0
            tris.Add(count - 1); // 3


            if (uv == null) return;

            uv.Add(new Vector2(0, 0));
            uv.Add(new Vector2(0, 1));
            uv.Add(new Vector2(1, 1));
            uv.Add(new Vector2(1, 0));

    }


    private static void AddBackFace(int x, int y, int z, List<Vector3> verts, List<int> tris, List<Vector2> uv = null)
    {

            verts.Add(new Vector3(x, y, z)); // 0
            verts.Add(new Vector3(x, y + 1, z)); // 1
            verts.Add(new Vector3(x + 1, y + 1, z)); // 2
            verts.Add(new Vector3(x + 1, y, z)); // 3

            int count = verts.Count;

            tris.Add(count - 4); // 0
            tris.Add(count - 3); // 1
            tris.Add(count - 2); // 2


            tris.Add(count - 4); // 0
            tris.Add(count - 2); // 2
            tris.Add(count - 1); // 3


            if (uv == null) return;

            uv.Add(new Vector2(0, 0));
            uv.Add(new Vector2(0, 1));
            uv.Add(new Vector2(1, 1));
            uv.Add(new Vector2(1, 0));



    }

    private static void AddFrontFace(int x, int y, int z, List<Vector3> verts, List<int> tris, List<Vector2> uv = null)
    {

            verts.Add(new Vector3(x, y, z + 1)); // 0
            verts.Add(new Vector3(x, y + 1, z + 1)); // 1
            verts.Add(new Vector3(x + 1, y + 1, z + 1)); // 2
            verts.Add(new Vector3(x + 1, y, z + 1)); // 3

            int count = verts.Count;


            tris.Add(count - 2); // 2
            tris.Add(count - 3); // 1        
            tris.Add(count - 4); // 0


            tris.Add(count - 2); // 2
            tris.Add(count - 4); // 0
            tris.Add(count - 1); // 3


            if (uv == null) return;

            uv.Add(new Vector2(0, 0));
            uv.Add(new Vector2(0, 1));
            uv.Add(new Vector2(1, 1));
            uv.Add(new Vector2(1, 0));


    }
}
