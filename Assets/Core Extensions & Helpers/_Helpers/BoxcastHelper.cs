using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Extensions
{
    public static partial class Helper
    {
        /*
        static public int DirectionalCollectBox(RaycastHit2D[] result, Vector2 origin, Vector2 target, float linearOffset, float reach, float width, float addedAngle, int mask = -1, bool drawDebug = false)
        {
            return DirectionalBoxcastNonAlloc(origin, target, linearOffset, reach, width, mask, result, addedAngle, drawDebug);
        }
        static public int DirectionalBoxcastNonAlloc(Vector2 origin, Vector2 target, float linearOffset, float reach, float width, int mask, RaycastHit2D[] result, float addedAngle = 0, bool drawDebug = false)
        {
            Vector2 directionCenter = (target - origin).normalized * ((reach / 2f) + linearOffset);
            if (drawDebug)
            {
                Helper.DrawBoxCast(origin + directionCenter, new Vector2(reach, width), Vector2.right.Angle(directionCenter), Vector2.zero, 0f, 0);
            }
            return Physics2D.BoxCastNonAlloc(origin + directionCenter, new Vector2(reach, width), Vector2.right.Angle(directionCenter), Vector2.zero, result);
        }*/
        #region Boxcast Wrapper
        static public RaycastHit2D DrawBoxCast(Vector2 origen, Vector2 size, float angle, Vector2 direction, float distance, int mask, float debugTime = 0.5f)
        {
            RaycastHit2D hit = Physics2D.BoxCast(origen, size, angle, direction, distance, mask);

            //Setting up the points to draw the cast
            Vector2 p1, p2, p3, p4, p5, p6, p7, p8;
            float w = size.x * 0.5f;
            float h = size.y * 0.5f;
            p1 = new Vector2(-w, h);
            p2 = new Vector2(w, h);
            p3 = new Vector2(w, -h);
            p4 = new Vector2(-w, -h);

            Quaternion q = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
            p1 = q * p1;
            p2 = q * p2;
            p3 = q * p3;
            p4 = q * p4;

            p1 += origen;
            p2 += origen;
            p3 += origen;
            p4 += origen;

            Vector2 realDistance = direction.normalized * distance;
            p5 = p1 + realDistance;
            p6 = p2 + realDistance;
            p7 = p3 + realDistance;
            p8 = p4 + realDistance;

            //Drawing the cast
            Color castColor = hit ? Color.red : Color.green;
            Debug.DrawLine(p1, p2, castColor, debugTime);
            Debug.DrawLine(p2, p3, castColor, debugTime);
            Debug.DrawLine(p3, p4, castColor, debugTime);
            Debug.DrawLine(p4, p1, castColor, debugTime);

            Debug.DrawLine(p5, p6, castColor, debugTime);
            Debug.DrawLine(p6, p7, castColor, debugTime);
            Debug.DrawLine(p7, p8, castColor, debugTime);
            Debug.DrawLine(p8, p5, castColor, debugTime);

            Debug.DrawLine(p1, p5, Color.grey, debugTime);
            Debug.DrawLine(p2, p6, Color.grey, debugTime);
            Debug.DrawLine(p3, p7, Color.grey, debugTime);
            Debug.DrawLine(p4, p8, Color.grey, debugTime);
            if (hit)
            {
                Debug.DrawLine(hit.point, hit.point + hit.normal.normalized * 0.2f, Color.yellow, debugTime);
            }

            return hit;
        }
        #endregion
    }
}
