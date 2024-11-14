using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Size : MonoBehaviour
{
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Vector3 size = renderer.bounds.size;
            Debug.Log("Torus Size: " + size);
            Debug.Log("Width (X): " + size.x + ", Height (Y): " + size.y + ", Depth (Z): " + size.z);
        }
        else
        {
            Debug.LogWarning("Renderer not found on the torus object!");
        }
    }
}
