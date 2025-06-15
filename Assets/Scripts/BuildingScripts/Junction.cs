using Core.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class Junction : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out MoveResource resource))
        {
            resource.transform.position += (Vector3)resource.lastDirection * 1.5f;
        }
    }
}
