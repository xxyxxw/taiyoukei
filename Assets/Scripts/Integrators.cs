using UnityEngine;
using System.Collections.Generic;


public static class Gravity
{
    // G [km^3 / (kg s^2)]
    public const double G = 6.67430e-20;


    public static Vector3 AccelOn(int i, IList<CelestialBody> bodies)
    {
        Vector3 ai = Vector3.zero;
        var bi = bodies[i];
        for (int j = 0; j < bodies.Count; j++)
        {
            if (i == j) continue;
            var bj = bodies[j];
            Vector3 r = bj.positionKm - bi.positionKm;
            double dist2 = (double)r.sqrMagnitude + 1e-6; // softening
            double invDist = 1.0 / System.Math.Sqrt(dist2);
            double invDist3 = invDist * invDist * invDist;
            double mu = G * bj.Mass; // GM
            ai += (float)(mu * invDist3) * r;
        }
        return ai; // [km/s^2]
    }
}


public static class RK4
{
    public struct State { public Vector3 x, v; }


    public static void Step(List<CelestialBody> bodies, double dt)
    {
        int n = bodies.Count;
        var x0 = new Vector3[n];
        var v0 = new Vector3[n];
        for (int i = 0; i < n; i++) { x0[i] = bodies[i].positionKm; v0[i] = bodies[i].velocityKmPerSec; }


        var ax1 = new Vector3[n]; var av1 = new Vector3[n];
        var ax2 = new Vector3[n]; var av2 = new Vector3[n];
        var ax3 = new Vector3[n]; var av3 = new Vector3[n];
        var ax4 = new Vector3[n]; var av4 = new Vector3[n];


        // k1
        for (int i = 0; i < n; i++) { av1[i] = Gravity.AccelOn(i, bodies); ax1[i] = v0[i]; }
        // k2
        for (int i = 0; i < n; i++) { bodies[i].positionKm = x0[i] + ax1[i] * (float)(dt * 0.5); bodies[i].velocityKmPerSec = v0[i] + av1[i] * (float)(dt * 0.5); }
        for (int i = 0; i < n; i++) { av2[i] = Gravity.AccelOn(i, bodies); ax2[i] = bodies[i].velocityKmPerSec; }
        // k3
        for (int i = 0; i < n; i++) { bodies[i].positionKm = x0[i] + ax2[i] * (float)(dt * 0.5); bodies[i].velocityKmPerSec = v0[i] + av2[i] * (float)(dt * 0.5); }
        for (int i = 0; i < n; i++) { av3[i] = Gravity.AccelOn(i, bodies); ax3[i] = bodies[i].velocityKmPerSec; }
        // k4
        for (int i = 0; i < n; i++) { bodies[i].positionKm = x0[i] + ax3[i] * (float)dt; bodies[i].velocityKmPerSec = v0[i] + av3[i] * (float)dt; }
        for (int i = 0; i < n; i++) { av4[i] = Gravity.AccelOn(i, bodies); ax4[i] = bodies[i].velocityKmPerSec; }


        // ‡¬
        for (int i = 0; i < n; i++)
        {
            bodies[i].positionKm = x0[i] + (ax1[i] + 2 * ax2[i] + 2 * ax3[i] + ax4[i]) * (float)(dt / 6.0);
            bodies[i].velocityKmPerSec = v0[i] + (av1[i] + 2 * av2[i] + 2 * av3[i] + av4[i]) * (float)(dt / 6.0);
        }
    }
}