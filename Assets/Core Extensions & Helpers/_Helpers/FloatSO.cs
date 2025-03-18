using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Extensions
{
    [CreateAssetMenu(fileName ="Float Value", menuName ="Core/Scriptable Helpers/Float SO")]
    public class FloatSO : ScriptableObject
    {
        [SerializeField] float value = 0f;
        public static implicit operator float(FloatSO s) { return s.value; }
    }
}
