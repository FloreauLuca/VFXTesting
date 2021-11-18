using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseVisualization : MonoBehaviour
{
    private ImageVisualization imageVisualization;
    private Color[,] pixels = new Color[Globals.IMAGE_SIZE, Globals.IMAGE_SIZE];
    
    void Start()
    {
        imageVisualization = FindObjectOfType<ImageVisualization>();
    }

    void Update()
    {
        pixels[imageVisualization.GetMousePosition().x, imageVisualization.GetMousePosition().y] = Color.red;
        imageVisualization.Draw(pixels);
    }
}
