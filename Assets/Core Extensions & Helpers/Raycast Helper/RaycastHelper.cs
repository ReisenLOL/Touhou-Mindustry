using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public static class RaycastHelper
    {
        private enum DrawMode
        {
            None,
            OnlyErrors,
            All
        }
        private static DrawMode mode = DrawMode.All;
        public static bool Cast(Vector2 start, Vector2 end, LayerMask mask, out RaycastHit2D hit, float distance = Mathf.Infinity)
        {
            RaycastHit2D foundHit = Physics2D.Raycast(start, end - start, distance, mask);
            if (foundHit.transform != null)
            {
                if (mode == DrawMode.All)
                {
                    Debug.DrawRay(start, (end - start), Color.yellow, 0.1f);
                }
                hit = foundHit;
                return true;
            }
            hit = new();
            if (mode != DrawMode.None)
                Debug.DrawLine(start, end, Color.red, 0.1f);
            return false;
        }
    }
}
