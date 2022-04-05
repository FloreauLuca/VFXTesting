using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScreen : MonoBehaviour
{
    [SerializeField] private Material mat;
    private float timer = 0.0f;
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mat.SetVector("_FocalPoint", Camera.main.ScreenToViewportPoint(Input.mousePosition));
            timer = 0.0f;
        }

        timer += Time.deltaTime;
        mat.SetFloat("_Timer", timer);
    }
}
