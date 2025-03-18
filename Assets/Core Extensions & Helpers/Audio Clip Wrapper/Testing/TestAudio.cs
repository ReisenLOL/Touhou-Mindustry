using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Extensions
{
    public class TestAudio : MonoBehaviour
    {
        [SerializeField] AudioClipWrapper w;
        float nextTick;
        private void Update()
        {
            if (Time.time > nextTick)
            {
                nextTick = Time.time + 0.5f;
                w.Play(transform.position);
            }
        }
    }
}
