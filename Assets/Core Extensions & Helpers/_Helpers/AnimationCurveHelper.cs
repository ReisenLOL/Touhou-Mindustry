using System.Collections;
using UnityEngine;

namespace Core.Extensions
{
    public static partial class Helper
    {
        public static AnimationCurve InitializedAnimationCurve => new AnimationCurve().Initialized();
        public static AnimationCurve Initialized(this AnimationCurve a)
        {
            return new AnimationCurve()
            {
                keys = new Keyframe[2]
                {
                    new Keyframe(0f,1f),
                    new Keyframe(1f,1f),
                }
            };
        }
    }
}