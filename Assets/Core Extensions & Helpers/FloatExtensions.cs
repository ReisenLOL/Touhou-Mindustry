using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Core.Extensions
{
    public static class FloatExtensions
    {
        public static readonly float Percent = 0.01f;
        public static float LerpTime(this float f, float target, float delta)
        {
            return Mathf.Lerp(f, target, delta * Time.deltaTime);
        }
        public static float Lerp(this float f, float target, float lerp)
        {
            return Mathf.Lerp(f, target, lerp);
        }
        public static float Clamp(this float f, float min, float max)
        {
            return Mathf.Clamp(f, min, max);
        }
        public static float Clamp(this float f, Vector2 range)
        {
            return Clamp(f, range.x, range.y);
        }
        public static float AboveOrZero(this float f)
        {
            return Mathf.Max(f, 0f);
        }
        public static bool IsBelowOrZero(this float f)
        {
            return f <= 0f;
        }
        public static float Round(this float f)
        {
            return Mathf.Round(f);
        }
        public static float Min(this float f, float min)
        {
            return Mathf.Min(f, min);
        }
        public static float Max(this float f, float max)
        {
            return Mathf.Max(f, max);
        }
        public static float Absolute(this float f)
        {
            return Mathf.Abs(f);
        }
        public static float AbsoluteNegative(this float f)
        {
            return 0f - f.Absolute();
        }
        public static float AbsoluteBetween(this float f, float target)
        {
            return Mathf.Abs(f - target);
        }
        public static float Spread(this float f, float percentage = 5f)
        {
            if (percentage < 1f)
                return f;
            return f * UnityEngine.Random.Range(1 - percentage.Clamp(0f, 100f) * Percent, 1 + percentage.Clamp(0f, 100f) * Percent);
        }
        public static float Sign(this float f)
        {
            return Mathf.Sign(f);
        }
        public static int SignInt(this float f)
        {
            return (int)Mathf.Sign(f);
        }
        public static float ToFloat(this bool b)
        {
            return b ? 1f : 0f;
        }
        public static float AddRandomBetween(this float f, float min, float max)
        {
            return f + UnityEngine.Random.Range(min, max);
        }
        public static int ToInt(this float f)
        {
            return Mathf.FloorToInt(f);
        }
        public static float Multiply(this float f, float value)
        {
            return f = f * value;
        }
        public static float Percentify(this float f)
        {
            return f * 0.01f;
        }
        public static float Ceil(this float f)
        {
            return Mathf.Ceil(f);
        }
        public static float Floor(this float f)
        {
            return Mathf.Floor(f);
        }
        public static bool IsBetween(this float f, float a, float b)
        {
            if (a > b)
            {
                float c = a;
                a = b;
                b = c;
            }
            return f > a && f <= b;
        }
        public static bool IsBetween(this float f, Vector2 range)
        {
            return IsBetween(f, range.x, range.y);
        }
        public static float RandomPositiveNegativeRange(this float f)
        {
            return Random.Range(-f.Absolute(), f.Absolute());
        }
        public static float Quantize(this float f, float steps)
        {
            if (f == 0f)
            {
                return 0f;
            }
            steps = steps.Max(1f);
            float value = Mathf.Floor(f.Absolute() * steps.Absolute()) / steps.Absolute();
            return value * f.SignInt();
        }
        public static float ReverseQuantize(this float f, float stepSize)
        {
            if (f == 0f)
            {
                return 0f;
            }
            stepSize = stepSize.Max(0.1f);
            float value = Mathf.Floor(f.Absolute() / stepSize.Absolute()) * stepSize.Absolute();
            return value * f.SignInt();
        }
        public static float Squared(this float f) => f * f;
        public static float Mean(this float f, float other)
        {
            return (f + other) * 0.5f;
        }
        public static float AsFloat(this bool b, float trueValue, float falseValue)
        {
            if (b)
            {
                return trueValue;
            }
            return falseValue;
        }
        public static IEnumerable<float> StepFromTo(this float steps, float from, float to)
        {
            for (float i = from; i <= to; i += steps)
            {
                yield return i;
            }
        }
        public static IEnumerable<float> StepThroughCurve(this AnimationCurve curve, float timeStep, int maximumIterations)
        {
            for (float i = 0f; i <= curve.length || i <= maximumIterations * timeStep; i += timeStep)
            {
                yield return curve.Evaluate(i);
            }
        }
        public static float Duration(this AnimationCurve c)
        {
            return c.keys[c.length - 1].time;
        }
        public static float MoveTowards(this float f, float other, float step)
        {
            return Mathf.MoveTowards(f, other, step);
        }
    }
}