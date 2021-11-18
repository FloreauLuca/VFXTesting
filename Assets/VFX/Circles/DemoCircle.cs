using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoCircle : MonoBehaviour
{
    [SerializeField] private Renderer character;
    [SerializeField] private Renderer ground;
    [SerializeField] private float speed = 0.3f; 
    [SerializeField] private float animSpeed = 0.2f;
    private float completion;
    [SerializeField] private AnimationCurve curve;
    private Material charaMat;
    private Material groundMat;
    private Vector3 pos;
    private float startTime = 0.0f;

    private void Start()
    {
        charaMat = character.material;
        groundMat = ground.material;
        charaMat.SetFloat("_Completion", completion);
        groundMat.SetFloat("_Completion", completion);
        pos = character.transform.position;
    }

    void Update()
    {
        character.transform.position = pos + Vector3.up * curve.Evaluate(Time.time * animSpeed);
        if (curve.Evaluate(Time.time * animSpeed) < 0)
        {
            completion = -Mathf.Cos(startTime+Time.time * speed) / 2 + 0.5f;
            groundMat.SetFloat("_Completion", completion);
        }
        else
        {
            completion = -Mathf.Cos(Time.time * speed) / 2 + 0.5f;
            charaMat.SetFloat("_Completion", completion);
            startTime = Time.time;
        }
        
    }
}
