using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaDemo : MonoBehaviour
{
    private Material mat;
    [SerializeField] private float minTile;
    [SerializeField] private float maxTile;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        float edgeTimer = Time.time;
        float edgeSize = Mathf.Clamp(Mathf.Cos(edgeTimer) + 0.5f, 0.0f, 1.0f);
        mat.SetFloat("EdgeSize", 1-Mathf.SmoothStep(0.5f, 1.0f, edgeSize));
        float tileSize = Mathf.Clamp(Mathf.Sin(edgeTimer) + 0.5f, 0, 1.0f);
        mat.SetFloat("TileSize", Mathf.SmoothStep(minTile, maxTile, tileSize));
    }
}
