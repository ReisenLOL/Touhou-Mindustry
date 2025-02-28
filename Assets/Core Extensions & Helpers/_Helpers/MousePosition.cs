using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Extensions
{
    public static partial class Helper
    {
        public static Vector2 MousePosition => GetMousePosition();
        private static Vector2 GetMousePosition()
        {
            if (Camera.main != null)
            {
                Vector2 mouseXY = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                return mouseXY;
            }
            Debug.LogError("Failed to find mouse position, See MousePosition.cs");
            return Vector2.zero;
        }
    }
}
