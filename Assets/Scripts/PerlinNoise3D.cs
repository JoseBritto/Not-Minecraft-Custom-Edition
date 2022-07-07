using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise3D  : MonoBehaviour
{
    public static PerlinNoise3D Instance;
    public float Scale;
    public int Seed;
    public float Lacunarity = 0.5f;
    public int Octaves = 3;
    public float Frequency = 0.01f;
    public FastNoiseLite.NoiseType noiseType;
    public FastNoiseLite.FractalType fractalType;

    private void Awake()
    {
        Instance = this;
    }

    public float GetNoise(float x, float y, float z)
    {
        float noise = Perlin.Noise((x + Seed) * Scale, (y + Seed) *Scale, (z +Seed) *Scale ); // Must be between -1 and +1. (-0.9 and 0.9) https://github.com/keijiro/PerlinNoise/pull/4#issue-329347969

        noise += 1; noise /= 2; // Make it between 0 and 1.

        return noise;
        /*
                x = x * Scale;
                y = y * Scale;
                z = z * Scale;

                float ab = Mathf.PerlinNoise(x, y);
                float bc = Mathf.PerlinNoise(y, z);
                float ac = Mathf.PerlinNoise(x, z);

                float ba = Mathf.PerlinNoise(y, x);
                float cb = Mathf.PerlinNoise(z, y);
                float ca = Mathf.PerlinNoise(z, x);

                float abc = ab + bc + ac + ba + cb + ca;

                float noise = abc / 6f;*/

    }


    public float GetNoise2D(int x, int y)
    {
        // return (Perlin.Noise((x + Seed) * Scale, (y + Seed) * Scale) + 1) / 2;


        var noise = new FastNoiseLite(Seed);
        noise.SetNoiseType(noiseType);
        noise.SetFractalLacunarity(Lacunarity);
        noise.SetFractalOctaves(Octaves);
        noise.SetFrequency(Frequency);
        noise.SetFractalGain(0.5f);
        noise.SetFractalType(fractalType);
        var val = noise.GetNoise(x, y);
        
        val += 1;
        val /= 2;
        return val;

       // return Mathf.PerlinNoise((x + Seed) * Scale, (y + Seed) * Scale);
    }
}
