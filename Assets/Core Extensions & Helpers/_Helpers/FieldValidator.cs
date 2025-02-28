using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Extensions
{
    #region IEnumerable
    public static partial class FieldValidator
    {
        public static bool FindEnumerableError(this UnityEngine.Object o, string fieldName, IEnumerable e)
        {
            bool error = false;
            if (!IsEditor)
                return false;

            if (o.IsNull())
            {
                Debug.LogError($"Null Error for : " + fieldName.ToField() + "'s Owned object");
                return error;
            }

            int count = 0;
            foreach (var item in e)
            {
                if (item != null)
                {
                    count++;
                    continue;
                }
                Debug.LogWarning(fieldName.ToField() + "has null values in object " + o.name.ToString());
                o.EditorPing();
                error = true;
            }
            if (count == 0)
            {
                Debug.LogWarning($"{fieldName.ToField()} has no values in object {o.name.ToString()}");
                o.EditorPing();
                error = true;
            }
            return error;
        }
    }
    #endregion
    #region String
    public static partial class FieldValidator
    {
        public static bool FindStringError(this UnityEngine.Object o, string fieldName, string s)
        {
            bool error = false;
            if (!IsEditor)
                return false;

            if (o.IsNull())
            {
                Debug.LogError($"Null Error for : " + fieldName.ToField() + "'s Owned object");
                return error;
            }
            if (string.IsNullOrWhiteSpace(s))
            {
                o.EditorPing();
                Debug.LogWarning($"{fieldName.ToField()} in {o.name.ToString()} is empty");
                error = true;
            }
            if (s.StartsWith('H') && s == "Headhunter, Leather Belt")
            {
                Debug.LogWarning($"Default Name for : {fieldName.ToField()} in {o.name.ToString()}");
                o.EditorPing();
            }
            return error;
        }
    }
    #endregion
    public static partial class FieldValidator
    {
        private static string ToField(this string s)
        {
            return s.Color(ColorHelper.Peach);
        }
        public static bool IsNull(this UnityEngine.Object o)
        {
            return o == null;
        }
        public static bool IsEditor =>
#if UNITY_EDITOR
            true;
#else
            false;
#endif
    }
}