using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Globals
{
    public static int CUBE_SIZE = 32;
    public static int IMAGE_SIZE = 64;
    public static int IX(int x, int y)
    {
        x = Mathf.Clamp(x, 0, Globals.IMAGE_SIZE - 1);
        y = Mathf.Clamp(y, 0, Globals.IMAGE_SIZE - 1);
        return x + y * IMAGE_SIZE;
    }
    public static int Index(int x, int y, int z)
    {
        x = Mathf.Clamp(x, 0, Globals.CUBE_SIZE - 1);
        y = Mathf.Clamp(y, 0, Globals.CUBE_SIZE - 1);
        z = Mathf.Clamp(z, 0, Globals.CUBE_SIZE - 1);
        return x + y * CUBE_SIZE + z * CUBE_SIZE * CUBE_SIZE;
    }
}

public class ImageVisualization : MonoBehaviour
{
    private Renderer image;
    private Texture2D texture;


    private Color[,] pixels = new Color[Globals.IMAGE_SIZE, Globals.IMAGE_SIZE];

    // Start is called before the first frame update
    void Start()
    {
        texture = new Texture2D(Globals.IMAGE_SIZE, Globals.IMAGE_SIZE);
        texture.filterMode = FilterMode.Point;
        GetComponent<Renderer>().material.SetTexture("_HeightMap", texture);
        GetComponent<Renderer>().material.SetTexture("_MainTexture", texture);
    }

    // Update is called once per frame
    void Update()
    {
        for (int x = 0; x < Globals.IMAGE_SIZE; x++)
        {
            for (int y = 0; y < Globals.IMAGE_SIZE; y++)
            {
                texture.SetPixel(x,y,pixels[x,y]);
            }
        }
        texture.Apply();
    }

    public void Draw(Color[,] pixelsToDraw)
    {
        pixels = pixelsToDraw;
    }

    public Vector2Int GetMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition *= -1.0f;
        mousePosition += new Vector3(5.0f, 0, 5.0f);
        mousePosition *= (float)Globals.IMAGE_SIZE / 10.0f;
        Vector2Int mousePosInt = new Vector2Int((int) Mathf.Clamp(mousePosition.x, 0, Globals.IMAGE_SIZE-1), (int) Mathf.Clamp(mousePosition.z, 0, Globals.IMAGE_SIZE - 1));
        return mousePosInt;
    }
}
