using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Extensions
{
    [CreateAssetMenu(fileName = "Scene String", menuName = "Core/Scriptable Helpers/Scene String")]
    public class SceneStringSO : ScriptableObject
    {
        [field: SerializeField] public string sceneName { get; private set; }
    }
}
