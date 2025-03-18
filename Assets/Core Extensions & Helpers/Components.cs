using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Extensions
{
    public static class Components
    {
        public static List<T> Collect2D<T>(Vector2 position, float size, LayerMask mask)
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(position, size, mask);
            List<T> result = new List<T>();
            foreach (Collider2D collider in hit)
            {
                if (collider.GetComponent<T>() is T component && component != null)
                {
                    if (component == null)
                        continue;
                    result.Add(component);
                }
            }
            result = result.Where(x => (object)x != null).ToList();
            return result;
        }
        public static T[] Collect3D<T>(Vector3 position, float size, LayerMask mask)
        {
            throw new System.Exception("Not Yet Implemented");
        }
    }
}
