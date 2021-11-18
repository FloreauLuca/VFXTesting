using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    [Header("LightDemo")]
    [SerializeField] private Material matGradient;

    [SerializeField] private float manualTime;
    [SerializeField] private float timeSpeed = 1.0f;

    [SerializeField] private Transform light;
    [SerializeField] private Quaternion startRot;
    [SerializeField] private Quaternion endRot;

    [Header("TileDemo")]
    [SerializeField] private Material gizehMat;
    [SerializeField] private float minTile;
    [SerializeField] private float maxTile;

    [SerializeField] private Transform camera;
    [SerializeField] private Quaternion startCam;
    [SerializeField] private Quaternion endCam;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        manualTime = Time.time * timeSpeed;
        float timer = (Mathf.Cos(manualTime) + 1) / 2.0f;
        Quaternion currentRot = Quaternion.Lerp(startRot, endRot, timer);
        light.rotation = currentRot;
        float skyTimer = (Mathf.Cos(manualTime*2.0f) + 1) / 2.0f;
        matGradient.SetFloat("GradientLerp", skyTimer);
        camera.rotation = Quaternion.Lerp(startCam, endCam, timer);
        float currrentTile = Mathf.Lerp(minTile, maxTile, timer);
        gizehMat.SetFloat("Tiling", currrentTile);
        gizehMat.SetFloat("RandomTiling", currrentTile);
    }
}
