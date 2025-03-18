using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Extensions
{
    #region Helper
    public static partial class Helper
    {

    }
    #endregion
    public static class VectorExtensions
    {
        public static Vector3 ToXZ(this Vector3 v)
        {
            return new()
            {
                x = v.x,
                y = 0f,
                z = v.z,
            };
        }
        public static Vector2 ToX(this Vector2 v)
        {
            return new()
            {
                x = v.x,
                y = 0f
            };
        }
        public static Vector3 Abs(this Vector3 v)
        {
            return new()
            {
                x = Mathf.Abs(v.x),
                y = Mathf.Abs(v.y),
                z = Mathf.Abs(v.z)
            };
        }
        public static Vector3 Multiply(this Vector3 v, float magnitude)
        {
            return new(v.x * magnitude, v.y * magnitude, v.z * magnitude);
        }
        public static Vector2 Multiply(this Vector2 v, float magnitude)
        {
            return new(v.x * magnitude, v.y * magnitude);
        }
        public static Vector3 RoundToInt(this Vector3 v)
        {
            return new Vector3(v.x.Round(), v.y.Round(), v.z.Round());
        }
        public static Vector2 RoundToInt(this Vector2 v)
        {
            return new Vector2(v.x.Round(), v.y.Round());
        }
        public static float DistanceTo(this Vector3 v, Vector3 position)
        {
            return Vector3.Distance(v, position);
        }
        public static float SqDistance(this Vector3 v, Vector3 position)
        {
            return (v - position).sqrMagnitude;
        }
        public static float DistanceTo(this Vector2 v, Vector2 position)
        {
            return Vector2.Distance(v, position);
        }
        public static float SquareDistanceTo(this Vector2 v, Vector2 position)
        {
            return (position - v).sqrMagnitude;
        }
        public static bool SquareDistanceToGreaterThan(this Vector3 v, Vector3 position, float magnitude)
        {
            return (position - v).sqrMagnitude > magnitude * magnitude;
        }
        public static bool SquareDistanceToLessThan(this Vector3 v, Vector3 position, float magnitude)
        {
            return !SquareDistanceToGreaterThan(v, position, magnitude);
        }
        public static bool SquareDistanceToGreaterThan(this Vector2 v, Vector2 position, float magnitude)
        {
            return (position - v).sqrMagnitude > magnitude * magnitude;
        }
        public static bool SquareDistanceToLessThan(this Vector2 v, Vector2 position, float magnitude)
        {
            return !SquareDistanceToGreaterThan(v, position, magnitude);
        }
        public static float RandomBetweenXY(this Vector2 v)
        {
            return Random.Range(v.x, v.y);
        }
        public static Vector3 MoveTowards(this Vector3 v, Vector3 worldPosition, float speed)
        {
            return Vector3.MoveTowards(v, worldPosition, speed * Time.deltaTime);
        }
        public static Vector2 MoveTowards(this Vector2 v, Vector2 worldPosition, float speed)
        {
            return Vector2.MoveTowards(v, worldPosition, speed * Time.deltaTime);
        }
        public static Vector2 LerpTowardsWithDeltaTime(this Vector2 v, Vector2 worldPosition, float speed, float deltaTime)
        {
            return Vector2.Lerp(v, worldPosition, speed * deltaTime);
        }
        public static Vector2 LerpTowards(this Vector2 v, Vector2 worldPosition, float speed)
        {
            return Vector2.Lerp(v, worldPosition, speed * Time.deltaTime);
        }
        public static Vector3 X(this Vector3 v, float x)
        {
            return new(x, v.y, v.z);
        }
        public static Vector3 Y(this Vector3 v, float y)
        {
            return new(v.x, y, v.z);
        }
        public static Vector3 Z(this Vector3 v, float z)
        {
            return new(v.x, v.y, z);
        }
        public static float CalculateTravelTime(this Vector2 v, float speed)
        {
            return v.magnitude / speed.Max(0.01f);
        }
        public static bool Overlaps(this BoundsInt b, BoundsInt other)
        {
            if (b.Contains(other.min) || b.Contains(other.max))
                return true;
            return false;
        }
        public static Vector2Int GetRandom(this Vector2Int v, Vector2Int min, Vector2Int max)
        {
            return v = new(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
        }
        public static int RandomBetweenXY(this Vector2Int v)
        {
            return Random.Range(v.x, v.y);
        }
        public static Vector2 RandomFromZero(this Vector2 v)
        {
            return new(Random.Range(0f, v.x), Random.Range(0f, v.y));
        }
        public static Vector2Int GetRandom(this Vector2Int v, Vector3Int min, Vector3Int max)
        {
            return GetRandom(v, (Vector2Int)min, (Vector2Int)max);
        }
        public static Vector2 RandomSign(this Vector2 v)
        {
            return new(Random.Range(0, 2) * 2 - 1, Random.Range(0, 2) * 2 - 1);
        }
        public static BoundsInt ToInt(this Bounds b)
        {
            BoundsInt result = new BoundsInt();
            result.min = new(b.min.x.ToInt(), b.min.y.ToInt());
            result.max = new(b.max.x.ToInt(), b.max.y.ToInt());
            return result;
        }
        public static Vector2 RandomCircleDirection(this Vector3 v, float radius, float radiusMax = -1)
        {
            if (radiusMax == -1)
            {
                radiusMax = radius;
            }
            return RandomCircleDirection(v, new Vector2(radius, radiusMax));
        }
        public static Vector2 RandomCircleDirection(this Vector2 v, float radius, float radiusMax = -1)
        {
            Vector3 v3 = (Vector3)v;
            if (radiusMax == -1)
            {
                radiusMax = radius;
            }
            return RandomCircleDirection(v3, new(radius, radiusMax));
        }
        public static Vector2 RandomCircleDirection(this Vector3 v, Vector2 radiusRange)
        {
            Vector2 point = v;
            point += Random.insideUnitCircle.normalized * radiusRange.RandomBetweenXY();
            return point;
        }
        public static Vector2 RandomCircleDirection(this Vector2 v, Vector2 radiusRange)
        {
            Vector2 point = v;
            point += Random.insideUnitCircle.normalized * radiusRange.RandomBetweenXY();
            return point;
        }
        public static Vector2 Rotate2D(this Vector2 v, float angle)
        {
            v = Quaternion.AngleAxis(angle, Vector3.forward) * v;
            return v;
        }
        public static float Angle(this Vector2 v, Vector2? other = null)
        {
            Vector2 compare = other ?? Vector2.right;
            return Vector2.SignedAngle(v, compare);
        }
        public static Vector2 Sign(this Vector2 v)
        {
            return new(Mathf.Sign(v.x), Mathf.Sign(v.y));
        }
        public static Vector2 ClampInside(this Vector2 v, Bounds bounds)
        {
            Vector2 vector;
            vector.x = v.x.Clamp(bounds.min.x, bounds.max.x);
            vector.y = v.y.Clamp(bounds.min.y, bounds.max.y);
            return vector;
        }
        public static Vector2 ClampInside2D(this Vector3 v, Bounds bounds)
        {
            return ((Vector2)v).ClampInside(bounds);
        }

        public static Vector2 Bounce(this Vector2 v, Vector2 normal, float bounce)
        {
            return (v.normalized - 2 * (Vector2.Dot(v.normalized, normal)) * normal).normalized * bounce * v.magnitude;
        }
        public static Vector2 Clamp(this Vector2 v, float min, float max)
        {
            return v.normalized * v.magnitude.Clamp(min, max);
        }
        public static Vector2 Squared(this Vector2 v)
        {
            return v * v;
        }
        public static float TravelTime(this Vector2 v, float speed) => v.magnitude / speed;
        public static Vector2 Floor(this Vector2 v) => new(v.x.Floor(), v.y.Floor());
        public static Vector2 Quantize(this Vector2 v, float steps) => (v * steps).Floor() / steps;
        public static Vector2 ScaleToMagnitude(this Vector2 v, float magnitude)
        {
            Vector2 direction = v.normalized * magnitude;
            return direction;
        }
        public static Vector2 RandomWithin(this Bounds b, Vector2 center)
        {
            Vector2 v = center;
            Vector2 extends = ((Vector2)b.extents);
            v += extends.RandomFromZero() * extends.RandomSign();
            return v;
        }
        public static string BoundsToString(this Bounds b)
        {
            string text = "";
            text += $"x: {b.min.x.ToString()} : {b.max.x.ToString()}##".Color(Color.red).ReplaceLineBreaks("##");
            text += $"y: {b.min.y.ToString()} : {b.max.y.ToString()}".Color(Color.green);
            return text;
        }
    }
}
