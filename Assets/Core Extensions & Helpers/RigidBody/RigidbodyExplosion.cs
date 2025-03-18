using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Extensions;

namespace Core.Extensions
{
    #region 2D
    public static partial class RigidbodyExplosion
    {
        public enum ProximityMode
        {
            ForcedOne,
            Distance
        }
        public static List<Rigidbody2D> Explode(Vector2 position, float addedVerticalVelocity, float radius, LayerMask layers, float velocityForce, ProximityMode proximity, float torqueMultiplier = 1f)
        {
            List<Rigidbody2D> output = new();
            foreach (var item in Physics2D.OverlapCircleAll(position, radius, layers))
            {
                if (item != null && item.transform != null && item.transform.GetComponent<Rigidbody2D>() is Rigidbody2D rb && rb != null)
                {
                    float xTorque = (item.transform.position.x - position.x).Sign();
                    float yTorque = (item.transform.position.y - position.y).Sign() * 1.2f;
                    Vector2 direction = -(position - rb.position).normalized * velocityForce.Spread();
                    direction += new Vector2(0f, addedVerticalVelocity);
                    switch (proximity)
                    {
                        case ProximityMode.ForcedOne:
                            break;
                        case ProximityMode.Distance:
                            float lerp = (position - rb.position).magnitude / radius.Max(0.1f);
                            direction = Vector2.Lerp(direction, Vector2.zero, lerp);
                            break;
                        default:
                            break;
                    }
                    rb.AddTorque(-xTorque * yTorque * torqueMultiplier);
                    if (rb.linearVelocity.magnitude + direction.magnitude < direction.magnitude * 3)
                    {
                        rb.linearVelocity += direction;
                    }
                    output.Add(rb);
                }
            }
            return output;
        }
    }
    #endregion
    #region 3D
    public static partial class RigidbodyExplosion
    {

    }
    #endregion
}
