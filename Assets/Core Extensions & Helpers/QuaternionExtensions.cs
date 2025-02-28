using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Extensions
{
    public static class QuaternionExtensions
    {
        public static Quaternion LerpTowards(this Quaternion q, Quaternion target, float delta)
        {
            return Quaternion.Lerp(q, target, delta * Time.deltaTime);
        }
        public static Quaternion LerpTowards(this Quaternion q, Vector3 target, float delta)
        {
            return Quaternion.Lerp(q, Quaternion.Euler(target.normalized), delta * Time.deltaTime);
        }
    }
}
