using UnityEngine;

namespace CameraControl
{
    public static class PerlinHelper
    {
        /// <summary>
        /// Calculate a perlin noise value
        /// </summary>
        /// <param name="newSeed">The seed to use</param>
        /// <param name="frequency">How quickly does the shake change</param>
        /// <param name="strength">How strong is the shake</param>
        /// <returns>A random float between 0 and 1</returns>
        public static float GetPerlin(float newSeed, float frequency, float strength)
        {
            var noise = Mathf.PerlinNoise(newSeed + Time.time * frequency, newSeed + Time.time * frequency);
            noise = noise * 2 - 1;
            return noise * strength;
        }
    }
}