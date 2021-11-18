using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fluid
{
    public int size;
    public float dt;
    public float diff; //diffusion amount
    public float visc; //viscosity

    public float[] s;
    public float[] density;

    public float[] vx;
    public float[] vy;

    public float[] vx0;
    public float[] vy0;

    public Fluid(float dt, float diffusion, float viscosity)
    {
        this.size = Globals.IMAGE_SIZE;
        this.dt = dt;
        this.diff = diffusion;
        this.visc = viscosity;

        this.s = new float[Globals.IMAGE_SIZE * Globals.IMAGE_SIZE];
        this.density = new float[Globals.IMAGE_SIZE * Globals.IMAGE_SIZE];

        this.vx = new float[Globals.IMAGE_SIZE * Globals.IMAGE_SIZE];
        this.vy = new float[Globals.IMAGE_SIZE * Globals.IMAGE_SIZE];

        this.vx0 = new float[Globals.IMAGE_SIZE * Globals.IMAGE_SIZE];
        this.vy0 = new float[Globals.IMAGE_SIZE * Globals.IMAGE_SIZE];
    }

    public void AddDensity(int x, int y, float amount)
    {
        int index = Globals.IX(x, y);
        this.density[index] += amount;
    }

    public void AddVelocity(int x, int y, float amountX, float amountY)
    {
        int index = Globals.IX(x, y);
        this.vx[index] += amountX;
        this.vy[index] += amountY;
    }

    public void Step()
    {
        int N = this.size;
        float visc = this.visc;
        float diff = this.diff;
        float dt = this.dt;
        float[] Vx = this.vx;
        float[] Vy = this.vy;
        float[] Vx0 = this.vx0;
        float[] Vy0 = this.vy0;
        float[] s = this.s;
        float[] density = this.density;

        Diffuse(1, ref Vx0, Vx, visc, dt, 4);
        Diffuse(2, ref Vy0, Vy, visc, dt, 4);

        Project(ref Vx0, ref Vy0, ref Vx, ref Vy, 4);

        Advect(1, ref Vx, Vx0, Vx0, Vy0, dt);
        Advect(2, ref Vy, Vy0, Vx0, Vy0, dt);

        Project(ref Vx, ref Vy, ref Vx0, ref Vy0, 4);

        Diffuse(0, ref s, density, diff, dt, 4);
        Advect(0, ref density, s, Vx, Vy, dt);

        FadeD();
    }

    void FadeD()
    {
        for (int i = 0; i < density.Length; i++)
        {
            density[i]  = Mathf.Clamp(density[i]-0.01f, 0, 100);
        }
    }

    static void LinSolv(int b, ref float[] x, float[] x0, float a, float c, int iter, int N)
    {
        float cRecip = 1.0f / c;
        for (int t = 0; t < iter; t++)
        {
            for (int j = 1; j < N; j++)
            {
                for (int i = 1; i < N - 1; i++)
                {
                    x[Globals.IX(i, j)] =
                        (x0[Globals.IX(i, j)] + a * (
                            x[Globals.IX(i + 1, j)]
                            + x[Globals.IX(i - 1, j)]
                            + x[Globals.IX(i, j + 1)]
                            + x[Globals.IX(i, j - 1)]
                        )) * cRecip;
                }
            }
        }

        SetBND(b, ref x, N);
    }

    static void Diffuse(int b, ref float[] x, float[] x0, float diff, float dt, int iter)
    {
        float a = dt * diff * (Globals.IMAGE_SIZE - 2) * (Globals.IMAGE_SIZE - 2);
        LinSolv(b, ref x, x0, a, 1 + 6 * a, iter, Globals.IMAGE_SIZE);
    }

    static void Advect(int b, ref float[] d, float[] d0, float[] velX, float[] velY, float dt)
    {
        float i0, i1, j0, j1;

        float dtX = dt * (Globals.IMAGE_SIZE - 2);
        float dtY = dt * (Globals.IMAGE_SIZE - 2);

        float s0, s1, t0, t1;
        float tmp1, tmp2, x, y;

        float N = Globals.IMAGE_SIZE;
        float Nfloat = N;
        float ifloat, jfloat;
        int i, j;

        for (j = 1, jfloat = 1; j < N - 1; j++, jfloat++)
        {
            for (i = 1, ifloat = 1; i < N - 1; i++, ifloat++)
            {
                tmp1 = dtX * velX[Globals.IX(i, j)];
                tmp2 = dtY * velY[Globals.IX(i, j)];
                x = ifloat - tmp1;
                y = jfloat - tmp2;

                if (x < 0.5f) x = 0.5f;
                if (x > Nfloat + 0.5f) x = Nfloat + 0.5f;
                i0 = Mathf.Floor(x);
                i1 = i0 + 1.0f;
                if (y < 0.5f) y = 0.5f;
                if (y > Nfloat + 0.5f) y = Nfloat + 0.5f;
                j0 = Mathf.Floor(y);
                j1 = j0 + 1.0f;

                s1 = x - i0;
                s0 = 1.0f - s1;
                t1 = y - j0;
                t0 = 1.0f - t1;

                int i0i = (int)i0;
                int i1i = (int)i1;
                int j0i = (int)j0;
                int j1i = (int)j1;

                d[Globals.IX(i, j)] =
                    s0 * (t0 * d0[Globals.IX(i0i, j0i)]
                          + t1 * d0[Globals.IX(i0i, j1i)])
                    +
                    s1 * (t0 * d0[Globals.IX(i1i, j0i)]
                          + t1 * d0[Globals.IX(i1i, j1i)]);
            }
        }
        SetBND(b, ref d, Globals.IMAGE_SIZE);
    }

    static void Project(ref float[] velocX, ref float[] velocY, ref float[] p, ref float[] div, int iter)
    {
        int N = Globals.IMAGE_SIZE;
        for (int j = 1; j < N - 1; j++)
        {
            for (int i = 1; i < N - 1; i++)
            {
                div[Globals.IX(i, j)] = -0.5f * (
                    velocX[Globals.IX(i + 1, j)]
                    - velocX[Globals.IX(i - 1, j)]
                    + velocY[Globals.IX(i, j + 1)]
                    - velocY[Globals.IX(i, j - 1)]
                ) / N;
                p[Globals.IX(i, j)] = 0;
            }
        }
        SetBND(0, ref div, N);
        SetBND(0, ref p, N);
        LinSolv(0, ref p, div, 1, 6, iter, N);

        for (int j = 1; j < N - 1; j++)
        {
            for (int i = 1; i < N - 1; i++)
            {
                velocX[Globals.IX(i, j)] -= 0.5f * (p[Globals.IX(i + 1, j)]
                                                    - p[Globals.IX(i - 1, j)]) * N;
                velocY[Globals.IX(i, j)] -= 0.5f * (p[Globals.IX(i, j + 1)]
                                                    - p[Globals.IX(i, j - 1)]) * N;
            }
        }
        SetBND(1, ref velocX, N);
        SetBND(2, ref velocY, N);
    }

    static void SetBND(int b, ref float[] x, int N)
    {
        for (int i = 0; i < N - 1; i++)
        {
            x[Globals.IX(i, 0)] = b == 2 ? -x[Globals.IX(i, 1)] : x[Globals.IX(i, 1)];
            x[Globals.IX(i, N - 1)] = b == 2 ? -x[Globals.IX(i, N - 2)] : x[Globals.IX(i, N - 2)];
        }

        for (int j = 0; j < N - 1; j++)
        {
            x[Globals.IX(0, j)] = b == 1 ? -x[Globals.IX(1, j)] : x[Globals.IX(1, j)];
            x[Globals.IX(N - 1, j)] = b == 1 ? -x[Globals.IX(N - 2, j)] : x[Globals.IX(N - 2, j)];
        }

        x[Globals.IX(0, 0)] = 0.5f * (x[Globals.IX(1, 0)] + x[Globals.IX(0, 1)]);
        x[Globals.IX(0, N - 1)] = 0.5f * (x[Globals.IX(1, N - 1)] + x[Globals.IX(0, N - 2)]);
        x[Globals.IX(N - 1, 0)] = 0.5f * (x[Globals.IX(N - 2, 0)] + x[Globals.IX(N - 1, 1)]);
        x[Globals.IX(N - 1, N - 1)] = 0.5f * (x[Globals.IX(N - 2, N - 1)] + x[Globals.IX(N - 1, N - 2)]);
    }
}
