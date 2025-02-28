using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static string Capitalized(this string s)
        {
            return s[0].ToString().ToUpper() + s.Substring(1);
        }
        private static string Color(this string str, string clr) => string.Format("<color={0}>{1}</color>", clr, str);
        public static string Color(this string str, Color32 color)
        {
            string stringColor = ColorUtility.ToHtmlStringRGBA(color).ToString();
            str = str.Color("#" + stringColor);
            return str;
        }
        public static string Color(this string str, byte r, byte g, byte b, byte alpha = 255)
        {
            Color32 color = new(r, g, b, alpha);
            return str.Color(color);
        }
        public static string ReplaceLineBreaks(this string s, string lineBreakSequence)
        {
            s = s.Replace(lineBreakSequence, "\n");
            return s;
        }
        public static IEnumerable<char> StringChop(this string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                yield return s[i];
            }
        }
    }
}
