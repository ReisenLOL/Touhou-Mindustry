using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Extensions
{
    public static class IntExtensions
    {
        public static int Clamp(this int i, int min, int max)
        {
            return Mathf.Clamp(i, min, max);
        }
        public static int Min(this int i, int min)
        {
            return Mathf.Min(i, min);
        }
        public static int Max(this int i, int max)
        {
            return Mathf.Max(i, max);
        }
        public static int Abs(this int i)
        {
            return Mathf.Abs(i);
        }
        public static int Scramble(this int i, int seed, int upperLimit, int scrambler = 3378)
        {
            int hash = scrambler.ToString().GetHashCode();
            int seedHash = seed.ToString().GetHashCode();

            int factor = (seedHash * hash);
            return ((i * factor).Abs() % upperLimit);
        }
        public static bool IsWithin(this int i, int min, int max)
        {
            return i >= min && i <= max;
        }
        public static int RandomBetween(this int i, int min, int max)
        {
            i = Random.Range(min, max);
            return i;
        }
        public static int Spread(this int i, float percentage = 5f)
        {
            return (int)((float)i * (Random.Range(1 - percentage.Clamp(0f, 100f) * FloatExtensions.Percent, 1 + percentage.Clamp(0f, 100f) * FloatExtensions.Percent)));
        }
        public static int MultiplyAndFloor(this int i, float multiplier)
        {
            return (int)((float)i).Multiply(multiplier).Floor();
        }
        public static float MultiplyAndFloorAsFloat(this int i, float multiplier)
        {
            return (float)MultiplyAndFloor(i, multiplier);
        }
    }
}