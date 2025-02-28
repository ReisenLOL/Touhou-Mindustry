using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Extensions
{
    public static partial class TransformExtensions
    {
        /*public static Transform Rotate(this Transform t, float x, float y)
        {
            if (x == 0 && y == 0)
                return t;
            Quaternion r = t.rotation;
            r *= Quaternion.Euler(y, x, 0);
            t.rotation = r;
            return t;
        }*/
        public static Transform SetParentDecorator(this Transform t, Transform newParent)
        {
            t.SetParent(newParent);
            return t;
        }
        public static Vector3 GetVectorTowards(this Transform t, Vector3 target)
        {
            return target - t.position;
        }
        public static T FindInRootAndChildren<T>(this Transform t)
        {
            if (t.GetComponent<T>() is T foundOnTransform)
            {
                return foundOnTransform;
            }
            if (t.root.GetComponent<T>() is T foundRoot)
            {
                return foundRoot;
            }
            if (t.root.GetComponentInChildren<T>() is T foundInChildren)
            {
                return foundInChildren;
            }
            Debug.LogWarning($"Failed to find Component on {t.name} of type : " + typeof(T).ToString());
            return default(T);
        }
        public static T FindBelow<T>(this Transform t)
        {
            if (t.GetComponent<T>() is T foundOnTransform)
            {
                return foundOnTransform;
            }
            if (t.root.GetComponentInChildren<T>() is T foundInChildren)
            {
                return foundInChildren;
            }
            Debug.LogWarning($"Failed to find Component on {t.name} of type : " + typeof(T).ToString());
            return default(T);
        }
        public static T FindInRootAndChildren<T>(this GameObject g)
        {
            return g.transform.FindInRootAndChildren<T>();
        }
        public static bool TryLayoutRebuild(this Transform t)
        {
            if (t.GetComponent<RectTransform>() is RectTransform rect)
            {
                LayoutRebuilder.MarkLayoutForRebuild(rect);
                return true;
            }
            return false;
        }
        public enum LookDirection2D
        {
            Right,
            Left,
            Up,
            Down
        }
        public static void Lookat2D(this Transform t, Vector2 position, LookDirection2D cardinal = LookDirection2D.Right)
        {
            switch (cardinal)
            {
                case LookDirection2D.Right:
                    t.right = position - (Vector2)t.position;
                    break;
                case LookDirection2D.Left:
                    t.right = -(position - (Vector2)t.position);
                    break;
                case LookDirection2D.Up:
                    t.up = position - (Vector2)t.position;
                    break;
                case LookDirection2D.Down:
                    t.up = -(position - (Vector2)t.position);
                    break;
                default:
                    break;
            }
            if (t.localRotation.eulerAngles.y == 180f)
            {
                t.localRotation = Quaternion.Euler(0f, 0f, 180f);
                return;
            }
            t.localRotation = Quaternion.Euler(0f, 0f, t.localRotation.eulerAngles.z);
        }
        public static void Lookat2DLerp(this Transform t, Vector2 direction, float delta, float angleOffset = 0f)
        {
            //Quaternion deltaRotation = Quaternion.FromToRotation(Vector3.right, direction);
            //t.rotation = deltaRotation;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
            t.rotation = Quaternion.Slerp(t.rotation, Quaternion.AngleAxis(angle, Vector3.forward), delta *
            Time.deltaTime);
        }
    }
}