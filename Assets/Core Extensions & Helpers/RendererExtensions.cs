using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Extensions
{
    public static class RendererExtensions
    {
        public static bool IsVisible(this Renderer r, Camera c)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(c);
            return GeometryUtility.TestPlanesAABB(planes, r.bounds);
        }
    }
}
