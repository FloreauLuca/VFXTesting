using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObjPosModifier : MonoBehaviour
{
    [SerializeField] private Material mat;
    void Update()
    {
        mat.SetVector("_ObjPos", transform.position);
    }
}
