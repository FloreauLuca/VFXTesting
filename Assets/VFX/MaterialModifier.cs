using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialModifier : MonoBehaviour
{
    [SerializeField] private List<Material> materials;

    [Range(0 ,1)][SerializeField] private float modifier;

    public float Modifier => modifier;

    [SerializeField] private AnimationCurve curve;

    [SerializeField] private float speed = 1;
    void Update()
    {
        modifier = curve.Evaluate(Mathf.Sin(Time.time * speed) * 0.5f + 0.5f);
        foreach (Material material in materials)
        {
            material.SetFloat("_Modifier", modifier);
        }   
    }
}
