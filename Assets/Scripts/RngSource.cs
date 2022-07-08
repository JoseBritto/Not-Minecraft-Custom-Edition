using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RngSource : MonoBehaviour
{
    public static RngSource Instance;

    #region Editor Exposed

    [SerializeField]
    private int seed;

    [SerializeField]
    private FastNoiseLite.NoiseType noiseSourceType;

    [SerializeField]
    private FastNoiseLite.FractalType fractalType;

    [SerializeField]
    private float frequency = 0.01f;

    [SerializeField]
    private float lacunarity = 0.5f;

    [SerializeField]
    private int octaves = 3;

    [SerializeField]
    private float fractalGain = 0.5f;

    [SerializeField]
    private float Amplitude = 10f;
    #endregion


    public int Seed => seed;

    private FastNoiseLite noiseSource;

    private void Awake()
    {
        // This should be moved to be called beofre world generation. This script should also be destroyed after player exit world.
        Initiialize(seed);
    }


    public void Initiialize(int seed)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        noiseSource = new FastNoiseLite(seed);
        noiseSource.SetNoiseType(noiseSourceType);
        noiseSource.SetFractalType(fractalType);

        noiseSource.SetFrequency(frequency);
        noiseSource.SetFractalLacunarity(lacunarity);
        noiseSource.SetFractalOctaves(octaves);
        noiseSource.SetFractalGain(fractalGain);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            Initiialize(seed);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns>A value between 1  and -1</returns>
    public float GetSurfaceNoise2D(int x, int z)
    {
        return noiseSource.GetNoise(x, z) * Amplitude;
    }

}
