using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChunkDataManager
{
    public static Task<ChunkBlockData> GenerateChunk(Vector2Int chunkPosition, PerlinNoise3D noise3d)
    {
        var data = new ChunkBlockData(chunkPosition);

        for(int x = 0; x < ChunkBlockData.SIZE; x++)
        {
            for (int z =0; z < ChunkBlockData.SIZE; z++)
            {
                var noise = (int)(noise3d.GetNoise2D(x + chunkPosition.x, z + chunkPosition.y) * (ChunkBlockData.HEIGHT - 1));
                for (int y = noise; y >= 0; y--)
                {
                    data.SetBlock(x, y, z, BlockType.Dirt);

                }

            }
        }

        return Task.FromResult(data);  
    }


   /* public static Mesh GetChunkMesh()
    {

    }   */

}

public class ChunkBlockData
{
    public const int SIZE = 16;
    public const int HEIGHT = 32;

    private readonly BlockType[] blocks;

    public readonly Vector2Int ChunkPosition; // This should be multiplied by 16 on each axis to get its position in the world.

    public ChunkBlockData(Vector2Int chunkPos)
    {
        ChunkPosition = chunkPos;
        blocks = new BlockType[SIZE * HEIGHT * SIZE];
    }

    public BlockType GetBlock(int x, int y, int z)
    {
        return GetBlock(to1D(x, y, z));
    }

    public BlockType GetBlock(int i)
    {
        return blocks[i];
    }

    public void SetBlock(int x, int y, int z, BlockType block)
    {
        SetBlock(to1D(x, y, z), block);
    }

    public void SetBlock(int i, BlockType block)
    {
        blocks[i] = block;
    }



    public static int to1D(int x, int y, int z)
    {
        return x + (z * SIZE) + (y * SIZE * SIZE); 
    }

    public static (int, int, int) to3D(int i)
    {
        int y = i / (SIZE * SIZE);
        i -= y * SIZE * SIZE;

        int z = i / SIZE;
        int x = i % SIZE;

        return (x, y, z);
    }
}

public enum BlockType
{
    Air = 0,
    Dirt = 1
}