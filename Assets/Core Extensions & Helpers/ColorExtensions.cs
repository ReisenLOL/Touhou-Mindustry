using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Extensions
{
    public struct ColorHelper
    {
        public static Color32 Peach => new Color32(235, 160, 90, 255);
        public static Color32 FullBlue => new Color32(0, 0, 255, 255);
        public static Color32 FullGreen => new Color32(0, 255, 0, 255);
        public static Color32 FullRed => new Color32(255, 0, 0, 255);
        public static Color32 DeepBlue => new Color32(70, 70, 255, 255);
        public static Color32 DeepGreen => new Color32(70, 255, 70, 255);
        public static Color32 DeepRed => new Color32(255, 70, 70, 255);
        public static Color32 PastelBlue => new Color32(140, 158, 255, 255);
        public static Color32 PastelGreen => new Color32(147, 255, 140, 255);
        public static Color32 PastelRed => new Color32(255, 140, 140, 255);
        public static Color32 PastelYellow => new Color32(250, 255, 140, 255);
        public static Color32 PastelPurple => new Color32(252, 140, 255, 255);
        public static Color32 PastelCyan => new Color32(140, 240, 255, 255);
        public static Color32 PastelOrange => new Color32(255, 216, 140, 255);
    }
    public static class ColorExtensions
    {
        public static Color32 Opacity(this Color c, byte alpha = 255)
        {
            Color32 color = c;
            return new(color.r, color.g, color.b, alpha);
        }
    }
}