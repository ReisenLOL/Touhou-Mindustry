using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Extensions
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] CursorEntry FallbackCursor;
        List<CursorEntry> entries = new List<CursorEntry>();
        private void Awake()
        {
            SetNewCursor(FallbackCursor);
        }
        public void SetNewCursor(CursorEntry entry)
        {
            if (entry.CursorTexture == null)
            {
                Debug.LogError("Missing Cursor Texture");
                return;
            }
            Cursor.SetCursor(entry.CursorTexture, entry.Hotspot, entry.Mode);
        }
    }
    [System.Serializable]
    public struct CursorEntry
    {
        public CursorEntry(Texture2D tex, Vector2 hotspot, float priority)
        {
            CursorTexture = tex;
            Hotspot = hotspot;
            Mode = CursorMode.ForceSoftware;
            this.priority = priority;
        }
        public Texture2D CursorTexture;
        public Vector2 Hotspot;
        public CursorMode Mode;
        [Range(-1f, 100f)]
        public float priority;
    }
}