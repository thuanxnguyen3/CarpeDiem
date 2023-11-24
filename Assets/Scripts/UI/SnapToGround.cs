using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SnapToGround : MonoBehaviour
{
    [MenuItem("Custom/Snap To Ground %g")]
    public static void Ground()
    {
        foreach(var transform in Selection.transforms)
        {
            RaycastHit hitInfo;
            var hits = Physics.RaycastAll(transform.position + Vector3.up, Vector3.down, 10000f);
            foreach(var hit in hits)
            {
                if (hit.collider.gameObject == transform.gameObject)
                    continue;

                transform.position = hit.point;
                break;
            }
        }
    }
}
