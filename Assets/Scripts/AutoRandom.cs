namespace Games.Bingo
{
    using UnityEngine;
    public static class AutoRandom
    {
        public static uint initialSeed;
        private static uint seed;
        private static uint counter = 0;
        public static int Range(int min, int max)
        {
            UpdateSeed();
            return (int)Remap(seed, int.MinValue, int.MaxValue, min, max);
        }
        public static float Range(float min, float max)
        {
            UpdateSeed();
            return Remap(seed, int.MinValue, int.MaxValue, min, max);
        }
        public static float Value()
        {
            UpdateSeed();
            return Remap(seed, int.MinValue, int.MaxValue, 0f, 1f);
        }
        private static void UpdateSeed()
        {
            seed = MixSeed(seed, ++counter);
        }
        public static void ResetSeed()
        {
            seed = GenerateNewSeed();
            counter = 0;
        }
        private static uint GenerateNewSeed()
        {
            uint newSeed = (uint)System.DateTime.Now.Ticks;
            return newSeed;
        }
        private static uint MixSeed(uint seed, uint counter)
        {
            uint mixedSeed = seed ^ counter;
            mixedSeed ^= (mixedSeed << 13);
            mixedSeed ^= (mixedSeed >> 17);
            mixedSeed ^= (mixedSeed << 5);
            return mixedSeed;
        }
        private static float Remap(uint value, int originalMin, int originalMax, float newMin, float newMax)
        {
            float normalizedValue = Mathf.InverseLerp(originalMin, originalMax, (int)value);
            return Mathf.Lerp(newMin, newMax, normalizedValue);
        }
    }
}