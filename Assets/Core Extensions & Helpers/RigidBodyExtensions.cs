using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Extensions
{
    public static class RigidBodyExtensions
    {
        public static Vector3 VelocityTowardsXZ(this Rigidbody rb, Vector3 direction, float delta)
        {
            return rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, new(direction.x, rb.linearVelocity.y, direction.z), delta * Time.deltaTime);
        }
        public static Vector2 VelocityTowardsX(this Rigidbody2D rb, Vector2 direction, float delta)
        {
            return rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, new(direction.x, rb.linearVelocity.y), delta * Time.deltaTime);
        }
        public static Vector2 VelocityTowards(this Rigidbody2D rb, Vector2 direction, float delta)
        {
            return rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, direction, delta * Time.deltaTime);
        }
        public static Vector2 VelocityScale(this Rigidbody2D rb, Vector2 direction, float scale)
        {
            return rb.linearVelocity = direction * scale;
        }
        public static Vector2 VelocityTowardsWithoutDelta(this Rigidbody2D rb, Vector2 direction, float delta)
        {
            return rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, direction, delta);
        }
        public static Vector2 ScaleVelocityVector(this Rigidbody2D rb, float time, float mix = 1f)
        {
            return rb.linearVelocity * time * mix;
        }
        public static Vector2 PredictPosition(this Rigidbody2D rb, float time, float mix = 1f)
        {
            Vector2 position = rb.position;
            Vector2 direction = rb.ScaleVelocityVector(time, mix);
            Debug.DrawLine(position, position + direction, Color.cyan.Opacity(255),0.25f);
            return position + direction;
        }
    }
}