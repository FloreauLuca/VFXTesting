using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDFDemo : MonoBehaviour
{
    private Material material;

    [SerializeField] private float radius;
    [SerializeField] private float speed;
    private float timer = 0.0f;
    private float currentRadius = 0.0f;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        timer += Time.deltaTime * speed;
        float delta = 2 * Mathf.PI / 3.0f;
        currentRadius = (Mathf.Sin(timer) + 1) / 2.0f * radius;
        float posADelta = 0;
        Vector2 posA = new Vector2(Mathf.Cos(timer), Mathf.Sin(timer)) * currentRadius;
        float posBDelta = delta;
        Vector2 posB = new Vector2(Mathf.Cos(timer + posBDelta), Mathf.Sin(timer + posBDelta)) * currentRadius;
        float posCDelta = delta * 2.0f;
        Vector2 posC = new Vector2(Mathf.Cos(timer + posCDelta), Mathf.Sin(timer + posCDelta)) * currentRadius;
        material.SetVector("_PointA", posA);
        material.SetVector("_PointB", posB);
        material.SetVector("_PointC", posC);
    }
}
