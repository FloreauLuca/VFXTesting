using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluid3D
{
    int N;
    float dt;
    float diff;
    float visc;

    float[] s;
    public float[] density;

    float[] Vx;
    float[] Vy;
    float[] Vz;

    float[] Vx0;
    float[] Vy0;
    float[] Vz0;

    public Fluid3D(int diffusion, int viscosity, float dt)
    {
        this.N = Globals.CUBE_SIZE;
        this.dt = dt;
        this.diff = diffusion;
        this.visc = viscosity;

        this.s = new float[Globals.CUBE_SIZE * Globals.CUBE_SIZE * Globals.CUBE_SIZE];
        this.density = new float[Globals.CUBE_SIZE * Globals.CUBE_SIZE * Globals.CUBE_SIZE];

        this.Vx = new float[Globals.CUBE_SIZE * Globals.CUBE_SIZE * Globals.CUBE_SIZE];
        this.Vy = new float[Globals.CUBE_SIZE * Globals.CUBE_SIZE * Globals.CUBE_SIZE];
        this.Vz = new float[Globals.CUBE_SIZE * Globals.CUBE_SIZE * Globals.CUBE_SIZE];

        this.Vx0 = new float[Globals.CUBE_SIZE * Globals.CUBE_SIZE * Globals.CUBE_SIZE];
        this.Vy0 = new float[Globals.CUBE_SIZE * Globals.CUBE_SIZE * Globals.CUBE_SIZE];
        this.Vz0 = new float[Globals.CUBE_SIZE * Globals.CUBE_SIZE * Globals.CUBE_SIZE];
    }

    public void AddDensity(int x, int y, int z, float amount)
    {
        density[Globals.Index(x, y, z)] += amount;
    }

    public void AddVelocity(int x, int y, int z, float amountX, float amountY, float amountZ)
    {
        int index = Globals.Index(x, y, z);

        this.Vx[index] += amountX;
        this.Vy[index] += amountY;
        this.Vz[index] += amountZ;
    }

    public void Step()
    {
        float visc = this.visc;
        float diff = this.diff;
        float dt = this.dt;
        float[] Vx = this.Vx;
        float[] Vy = this.Vy;
        float[] Vz = this.Vz;
        float[] Vx0 = this.Vx0;
        float[] Vy0 = this.Vy0;
        float[] Vz0 = this.Vz0;
        float[] s = this.s;
        float[] density = this.density;

        diffuse(1, ref Vx0, Vx, visc, dt, 4, N);
        diffuse(2, ref Vy0, Vy, visc, dt, 4, N);
        diffuse(3, ref Vz0, Vz, visc, dt, 4, N);

        project(ref Vx0, ref Vy0, ref Vz0, ref Vx, ref Vy, 4, N);

        advect(1, ref Vx, Vx0, Vx0, Vy0, Vz0, dt, N);
        advect(2, ref Vy, Vy0, Vx0, Vy0, Vz0, dt, N);
        advect(3, ref Vz, Vz0, Vx0, Vy0, Vz0, dt, N);

        project(ref Vx, ref Vy, ref Vz, ref Vx0, ref Vy0, 4, N);

        diffuse(0, ref s, density, diff, dt, 4, N);
        advect(0, ref density, s, Vx, Vy, Vz, dt, N);
        FadeD();
    }

    void FadeD()
    {
        for (int i = 0; i < density.Length; i++)
        {
            density[i] = Mathf.Clamp(density[i] - 0.01f, 0, 100);
        }
    }

    static void set_bnd(int b, ref float[] x, int N)
    {
        for (int j = 1; j < N - 1; j++)
        {
            for (int i = 1; i < N - 1; i++)
            {
                x[Globals.Index(i, j, 0)] = b == 3 ? -x[Globals.Index(i, j, 1)] : x[Globals.Index(i, j, 1)];
                x[Globals.Index(i, j, N - 1)] = b == 3 ? -x[Globals.Index(i, j, N - 2)] : x[Globals.Index(i, j, N - 2)];
            }
        }
        for (int k = 1; k < N - 1; k++)
        {
            for (int i = 1; i < N - 1; i++)
            {
                x[Globals.Index(i, 0, k)] = b == 2 ? -x[Globals.Index(i, 1, k)] : x[Globals.Index(i, 1, k)];
                x[Globals.Index(i, N - 1, k)] = b == 2 ? -x[Globals.Index(i, N - 2, k)] : x[Globals.Index(i, N - 2, k)];
            }
        }
        for (int k = 1; k < N - 1; k++)
        {
            for (int j = 1; j < N - 1; j++)
            {
                x[Globals.Index(0, j, k)] = b == 1 ? -x[Globals.Index(1, j, k)] : x[Globals.Index(1, j, k)];
                x[Globals.Index(N - 1, j, k)] = b == 1 ? -x[Globals.Index(N - 2, j, k)] : x[Globals.Index(N - 2, j, k)];
            }
        }

        x[Globals.Index(0, 0, 0)] = 0.33f * (x[Globals.Index(1, 0, 0)]
                                      + x[Globals.Index(0, 1, 0)]
                                      + x[Globals.Index(0, 0, 1)]);
        x[Globals.Index(0, N - 1, 0)] = 0.33f * (x[Globals.Index(1, N - 1, 0)]
                                      + x[Globals.Index(0, N - 2, 0)]
                                      + x[Globals.Index(0, N - 1, 1)]);
        x[Globals.Index(0, 0, N - 1)] = 0.33f * (x[Globals.Index(1, 0, N - 1)]
                                      + x[Globals.Index(0, 1, N - 1)]
                                      + x[Globals.Index(0, 0, N)]);
        x[Globals.Index(0, N - 1, N - 1)] = 0.33f * (x[Globals.Index(1, N - 1, N - 1)]
                                      + x[Globals.Index(0, N - 2, N - 1)]
                                      + x[Globals.Index(0, N - 1, N - 2)]);
        x[Globals.Index(N - 1, 0, 0)] = 0.33f * (x[Globals.Index(N - 2, 0, 0)]
                                      + x[Globals.Index(N - 1, 1, 0)]
                                      + x[Globals.Index(N - 1, 0, 1)]);
        x[Globals.Index(N - 1, N - 1, 0)] = 0.33f * (x[Globals.Index(N - 2, N - 1, 0)]
                                      + x[Globals.Index(N - 1, N - 2, 0)]
                                      + x[Globals.Index(N - 1, N - 1, 1)]);
        x[Globals.Index(N - 1, 0, N - 1)] = 0.33f * (x[Globals.Index(N - 2, 0, N - 1)]
                                      + x[Globals.Index(N - 1, 1, N - 1)]
                                      + x[Globals.Index(N - 1, 0, N - 2)]);
        x[Globals.Index(N - 1, N - 1, N - 1)] = 0.33f * (x[Globals.Index(N - 2, N - 1, N - 1)]
                                      + x[Globals.Index(N - 1, N - 2, N - 1)]
                                      + x[Globals.Index(N - 1, N - 1, N - 2)]);
    }

    static void lin_solve(int b, ref float[] x, float[] x0, float a, float c, int iter, int N)
    {
        float cRecip = 1.0f / c;
        for (int k = 0; k < iter; k++)
        {
            for (int m = 1; m < N - 1; m++)
            {
                for (int j = 1; j < N - 1; j++)
                {
                    for (int i = 1; i < N - 1; i++)
                    {
                        x[Globals.Index(i, j, m)] =
                            (x0[Globals.Index(i, j, m)]
                             + a * (x[Globals.Index(i + 1, j, m)]
                                    + x[Globals.Index(i - 1, j, m)]
                                    + x[Globals.Index(i, j + 1, m)]
                                    + x[Globals.Index(i, j - 1, m)]
                                    + x[Globals.Index(i, j, m + 1)]
                                    + x[Globals.Index(i, j, m - 1)]
                             )) * cRecip;
                    }
                }
            }
            set_bnd(b, ref x, N);
        }
    }

    static void diffuse(int b, ref float[] x, float[] x0, float diff, float dt, int iter, int N)
    {
        float a = dt * diff * (N - 2) * (N - 2);
        lin_solve(b, ref x, x0, a, 1 + 6 * a, iter, N);
    }

    static void project(ref float[] velocX, ref float[] velocY, ref float[] velocZ, ref float[] p, ref float[] div, int iter, int N)
    {
        for (int k = 1; k < N - 1; k++)
        {
            for (int j = 1; j < N - 1; j++)
            {
                for (int i = 1; i < N - 1; i++)
                {
                    div[Globals.Index(i, j, k)] = -0.5f * (
                        velocX[Globals.Index(i + 1, j, k)]
                        - velocX[Globals.Index(i - 1, j, k)]
                        + velocY[Globals.Index(i, j + 1, k)]
                        - velocY[Globals.Index(i, j - 1, k)]
                        + velocZ[Globals.Index(i, j, k + 1)]
                        - velocZ[Globals.Index(i, j, k - 1)]
                    ) / N;
                    p[Globals.Index(i, j, k)] = 0;
                }
            }
        }
        set_bnd(0, ref div, N);
        set_bnd(0, ref p, N);
        lin_solve(0, ref p, div, 1, 6, iter, N);

        for (int k = 1; k < N - 1; k++)
        {
            for (int j = 1; j < N - 1; j++)
            {
                for (int i = 1; i < N - 1; i++)
                {
                    velocX[Globals.Index(i, j, k)] -= 0.5f * (p[Globals.Index(i + 1, j, k)]
                                                              - p[Globals.Index(i - 1, j, k)]) * N;
                    velocY[Globals.Index(i, j, k)] -= 0.5f * (p[Globals.Index(i, j + 1, k)]
                                                              - p[Globals.Index(i, j - 1, k)]) * N;
                    velocZ[Globals.Index(i, j, k)] -= 0.5f * (p[Globals.Index(i, j, k + 1)]
                                                              - p[Globals.Index(i, j, k - 1)]) * N;
                }
            }
        }
        set_bnd(1, ref velocX, N);
        set_bnd(2, ref velocY, N);
        set_bnd(3, ref velocZ, N);
    }

    static void advect(int b, ref float[] d, float[] d0, float[] velocX, float[] velocY, float[] velocZ, float dt, int N)
    {
        float i0, i1, j0, j1, k0, k1;

        float dtx = dt * (N - 2);
        float dty = dt * (N - 2);
        float dtz = dt * (N - 2);

        float s0, s1, t0, t1, u0, u1;
        float tmp1, tmp2, tmp3, x, y, z;

        float Nfloat = N;
        float ifloat, jfloat, kfloat;
        int i, j, k;

        for (k = 1, kfloat = 1; k < N - 1; k++, kfloat++)
        {
            for (j = 1, jfloat = 1; j < N - 1; j++, jfloat++)
            {
                for (i = 1, ifloat = 1; i < N - 1; i++, ifloat++)
                {
                    tmp1 = dtx * velocX[Globals.Index(i, j, k)];
                    tmp2 = dty * velocY[Globals.Index(i, j, k)];
                    tmp3 = dtz * velocZ[Globals.Index(i, j, k)];
                    x = ifloat - tmp1;
                    y = jfloat - tmp2;
                    z = kfloat - tmp3;

                    if (x < 0.5f) x = 0.5f;
                    if (x > Nfloat + 0.5f) x = Nfloat + 0.5f;
                    i0 = Mathf.Floor(x);
                    i1 = i0 + 1.0f;
                    if (y < 0.5f) y = 0.5f;
                    if (y > Nfloat + 0.5f) y = Nfloat + 0.5f;
                    j0 = Mathf.Floor(y);
                    j1 = j0 + 1.0f;
                    if (z < 0.5f) z = 0.5f;
                    if (z > Nfloat + 0.5f) z = Nfloat + 0.5f;
                    k0 = Mathf.Floor(z);
                    k1 = k0 + 1.0f;

                    s1 = x - i0;
                    s0 = 1.0f - s1;
                    t1 = y - j0;
                    t0 = 1.0f - t1;
                    u1 = z - k0;
                    u0 = 1.0f - u1;

                    int i0i = (int)i0;
                    int i1i = (int)i1;
                    int j0i = (int)j0;
                    int j1i = (int)j1;
                    int k0i = (int)k0;
                    int k1i = (int)k1;

                    d[Globals.Index(i, j, k)] =

                        s0 * (t0 * (u0 * d0[Globals.Index(i0i, j0i, k0i)]
                                    + u1 * d0[Globals.Index(i0i, j0i, k1i)])
                            + (t1 * (u0 * d0[Globals.Index(i0i, j1i, k0i)]
                                    + u1 * d0[Globals.Index(i0i, j1i, k1i)])))
                       + s1 * (t0 * (u0 * d0[Globals.Index(i1i, j0i, k0i)]
                                    + u1 * d0[Globals.Index(i1i, j0i, k1i)])
                            + (t1 * (u0 * d0[Globals.Index(i1i, j1i, k0i)]
                                    + u1 * d0[Globals.Index(i1i, j1i, k1i)])));
                }
            }
        }
        set_bnd(b, ref d, N);
    }
}
