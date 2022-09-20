using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class CircleDrawer : MonoBehaviour
{
    Renderer renderer;
    [SerializeField] public float radius = 2.0f;
    void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float shaderRad = radius / (transform.localScale.x * 5 * 2);
        renderer.sharedMaterial.SetFloat("_Radius", shaderRad);
    }
}
