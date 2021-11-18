using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VoxelVisualization : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    private float[,,] voxels = new float[Globals.CUBE_SIZE, Globals.CUBE_SIZE, Globals.CUBE_SIZE];
    private GameObject[,,] cubes = new GameObject[Globals.CUBE_SIZE, Globals.CUBE_SIZE, Globals.CUBE_SIZE];
    
    void Start()
    {
        for (int x = 0; x < Globals.CUBE_SIZE; x++)
        {
            for (int y = 0; y < Globals.CUBE_SIZE; y++)
            {
                for (int z = 0; z < Globals.CUBE_SIZE; z++)
                {
                    cubes[x, y, z] = Instantiate(cubePrefab, new Vector3((x * (10.0f/ Globals.CUBE_SIZE)) - 5.0f, (y * (10.0f / Globals.CUBE_SIZE)) - 5.0f, (z * (10.0f / Globals.CUBE_SIZE)) - 5.0f), Quaternion.identity, transform);
                    cubes[x, y, z].transform.localScale = Vector3.one * (10.0f / Globals.CUBE_SIZE);
                }
            }
        }
    }

    void Update()
    {
        //Vector3Int center = new Vector3Int(Globals.CUBE_SIZE / 2, Globals.CUBE_SIZE / 2, Globals.CUBE_SIZE / 2);
        //for (int x = 0; x < Globals.CUBE_SIZE; x++)
        //{
        //    for (int y = 0; y < Globals.CUBE_SIZE; y++)
        //    {
        //        for (int z = 0; z < Globals.CUBE_SIZE; z++)
        //        {
        //            float dist = (center - new Vector3Int(x, y, z)).magnitude;
        //            voxels[x, y, z] = 1.0f - (dist / 5.0f);
        //        }
        //    }
        //}
        for (int x = 0; x < Globals.CUBE_SIZE; x++)
        {
            for (int y = 0; y < Globals.CUBE_SIZE; y++)
            {
                for (int z = 0; z < Globals.CUBE_SIZE; z++)
                {
                    if (voxels[x, y, z] == 0)
                    {
                        cubes[x, y, z].SetActive(false);
                    }
                    else
                    {
                        cubes[x, y, z].SetActive(true);
                        cubes[x, y, z].GetComponent<Renderer>().material.color = new Color(1, 1, 1, voxels[x, y, z]);
                    }
                }
            }
        }
    }

    public void Draw(float[,,] voxelsToToDraw)
    {
        voxels = voxelsToToDraw;
    }
}
