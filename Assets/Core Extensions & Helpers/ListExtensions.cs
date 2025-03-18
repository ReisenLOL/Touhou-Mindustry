using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Extensions
{
    public enum ListAddMode
    {
        Default,
        NoDuplicates
    }
    public static class ListExtensions
    {
        public static bool AddIfDoesntExist<T>(this List<T> l, T other)
        {
            if (!l.Contains(other))
            {
                l.Add(other);
                return true;
            }
            return false;
        }
        public static void AddList<T>(this List<T> l, List<T> other, ListAddMode addMode = ListAddMode.NoDuplicates)
        {
            if (l.GetType() == other.GetType())
            {
                foreach (T item in other)
                {
                    if (other != null)
                    {
                        if (addMode == ListAddMode.NoDuplicates && l.Contains(item))
                            continue;

                        l.Add(item);
                    }
                }
            }
        }
        public static void ClearNull<T>(this List<T> l)
        {
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i] == null)
                {
                    l.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}