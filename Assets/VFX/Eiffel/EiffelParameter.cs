using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MaterialModifier))]
public class EiffelParameter : MonoBehaviour
{
    [SerializeField] private Material material;
    private MaterialModifier materialModifier;
    [SerializeField] private AnimationCurve heightCurve;
    [SerializeField] private AnimationCurve powerCurve;
    [SerializeField] private AnimationCurve thicknessCurve;
    [SerializeField] private AnimationCurve tillingCurve;

    void Start()
    {
        materialModifier = GetComponent<MaterialModifier>();
    }

    void Update()
    {
        float modifier = Mathf.Sin(materialModifier.Modifier - 1) * 0.5f + 0.5f;
        material.SetFloat("Height_", heightCurve.Evaluate(modifier));
        material.SetFloat("Power_", powerCurve.Evaluate(modifier));
        material.SetFloat("Thickness_", thicknessCurve.Evaluate(modifier));
        material.SetFloat("Tilling_", tillingCurve.Evaluate(modifier));
    }
}
