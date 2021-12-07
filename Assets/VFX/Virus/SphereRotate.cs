using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MaterialModifier))]
public class SphereRotate : MonoBehaviour
{
    [SerializeField] private Vector3 axis;
    private MaterialModifier materialModifier;
    [SerializeField] private float rotateSpeed = 50.0f;
    [SerializeField] private float baseRotateSpeed = 10.0f;


    void Start()
    {
        materialModifier = GetComponent<MaterialModifier>();
    }

    void Update()
    {
        float rotateModifier = Mathf.Sin(materialModifier.Modifier * Mathf.PI);
        transform.Rotate(axis, (rotateModifier * rotateSpeed) + baseRotateSpeed);
    }
}
