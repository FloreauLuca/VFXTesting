using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidSimultation3D : MonoBehaviour
{
    private VoxelVisualization voxelVis;

    private Fluid3D fluid;
    private float[,,] voxels = new float[Globals.CUBE_SIZE, Globals.CUBE_SIZE, Globals.CUBE_SIZE];

    void Start()
    {
        voxelVis = FindObjectOfType<VoxelVisualization>();
        fluid = new Fluid3D(0, 0, 0.1f);
    }
    
    void Update()
    {
        //Vector3Int center = new Vector3Int(Globals.CUBE_SIZE / 2, Globals.CUBE_SIZE / 2, Globals.CUBE_SIZE / 2);
        //for (int x = -2; x < 2; x++)
        //{
        //    for (int y = -2; y < 2; y++)
        //    {
        //        for (int z = -2; z < 2; z++)
        //        {
        //            Vector3Int pos = center + new Vector3Int(x, y, z);
        //            fluid.AddDensity(pos.x, pos.y, pos.z, 300);

        //        }
        //    }
        //}
        fluid.AddDensity(Globals.CUBE_SIZE/2, Globals.CUBE_SIZE / 2, Globals.CUBE_SIZE / 2, 300);
        fluid.AddVelocity(Globals.CUBE_SIZE / 2, Globals.CUBE_SIZE / 2, Globals.CUBE_SIZE / 2, 0, -10, 0);

        fluid.Step();
        RenderD();

        voxelVis.Draw(voxels);

    }

    void RenderD()
    {
        for (int x = 0; x < Globals.CUBE_SIZE; x++)
        {
            for (int y = 0; y < Globals.CUBE_SIZE; y++)
            {
                for (int z = 0; z < Globals.CUBE_SIZE; z++)
                {
                    float d = fluid.density[Globals.Index(x, y, z)] / 100.0f;
                    voxels[x, y, z] = Mathf.Clamp(d, 0, 1);
                }
            }
        }
    }
}
