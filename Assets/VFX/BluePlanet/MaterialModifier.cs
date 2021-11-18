using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialModifier : MonoBehaviour
{
    [SerializeField] private List<Material> materials;

    [Range(0 ,1)][SerializeField] private float modifier;
    [SerializeField] private AnimationCurve curve;

    void Update()
    {
        modifier = curve.Evaluate(Mathf.Sin(Time.time) * 0.5f + 0.5f);
        foreach (Material material in materials)
        {
            material.SetFloat("_Modifier", modifier);
        }   
    }
}
