using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaterSimulation : MonoBehaviour
{
    private ImageVisualization imageVisualization;
    private Color[,] pixels = new Color[Globals.IMAGE_SIZE, Globals.IMAGE_SIZE];

    private Fluid fluid;
    private Vector2Int prevMousePos;

    void Start()
    {
        imageVisualization = FindObjectOfType<ImageVisualization>();
        fluid = new Fluid(0.01f, 0, 0);
    }
    
    void Update()
    {
        for (int i = 0; i < Globals.IMAGE_SIZE; i++)
        {
            for (int j = 0; j < Globals.IMAGE_SIZE; j++)
            {
                pixels[i, j] = Color.black;
            }
        }

        Vector2Int mousePos = imageVisualization.GetMousePosition();
        for (int i = -2; i < 2; i++)
        {
            for (int j = -2; j < 2; j++)
            {
                fluid.AddDensity(mousePos.x + i, mousePos.y + j, 100);
            }
        }
        Vector2 amount = mousePos - prevMousePos;
        amount *= 10;
        fluid.AddVelocity(mousePos.x, mousePos.y, amount.x, amount.y);
        prevMousePos = mousePos;

        //Vector2Int center = new Vector2Int(Globals.IMAGE_SIZE / 2, Globals.IMAGE_SIZE / 2);
        //fluid.AddDensity(center.x, center.y, Random.Range(50, 100));
        //float angle = Mathf.PerlinNoise(Time.time * 0.1f, 0) * Mathf.PI * 2;
        //Vector2 vel = Rotate(Vector2.right, angle);
        //vel *= 10.0f;
        //fluid.AddVelocity(center.x, center.y, vel.x, vel.y);

        fluid.Step();
        RenderD();
        //RenderV();
        imageVisualization.Draw(pixels);
    }
    public static Vector2 Rotate(Vector2 v, float rad)
    {
        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
    public void RenderD()
    {
        for (int i = 0; i < Globals.IMAGE_SIZE; i++)
        {
            for (int j = 0; j < Globals.IMAGE_SIZE; j++)
            {
                float d = fluid.density[Globals.IX(i, j)] / 100.0f;
                pixels[i, j] = new Color(d, d, d);
            }
        }
    }

    public void RenderV()
    {
        for (int i = 0; i < Globals.IMAGE_SIZE; i++)
        {
            for (int j = 0; j < Globals.IMAGE_SIZE; j++)
            {
                float d = new Vector2(fluid.vx[Globals.IX(i, j)], fluid.vx[Globals.IX(i, j)]).magnitude / 100.0f;
                pixels[i, j] = new Color(d, d, d);
            }
        }
    }

}
