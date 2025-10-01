using UnityEngine;
using System;

[Serializable]
public struct KeplerElements
{
    public double semiMajorAxisKm;   // a
    public double eccentricity;      // e
    public double inclinationDeg;    // i
    public double longAscNodeDeg;    // Ω
    public double argPeriapsisDeg;   // ω
    public double meanAnomalyDeg;    // M at epoch
}

public static class Kepler
{
    const double DEG2RAD = Math.PI / 180.0;

    // 2体近似: 中心体のμ=G(M+m) ~ GM を渡す
    public static void ElementsToState(
        KeplerElements el, double muKm3PerS2,
        out Vector3 positionKm, out Vector3 velocityKmPerSec)
    {
        // Kepler方程式（Newton-Raphson）
        double M = el.meanAnomalyDeg * DEG2RAD;
        double e = el.eccentricity;
        double E = M; // 初期値
        for (int it = 0; it < 20; it++)
        {
            double f = E - e * Math.Sin(E) - M;
            double fp = 1 - e * Math.Cos(E);
            double dE = -f / fp;
            E += dE;
            if (Math.Abs(dE) < 1e-12) break;
        }

        double a = el.semiMajorAxisKm;
        double cosE = Math.Cos(E);
        double sinE = Math.Sin(E);
        double r = a * (1 - e * cosE);
        double x_orb = a * (cosE - e);
        double y_orb = a * Math.Sqrt(1 - e * e) * sinE;
        double n = Math.Sqrt(muKm3PerS2 / (a * a * a));
        double vx_orb = -a * n * sinE / (1 - e * cosE);
        double vy_orb = a * n * Math.Sqrt(1 - e * e) * cosE / (1 - e * cosE);

        // 角度は別名にしてスコープ衝突を回避
        double inc = el.inclinationDeg * DEG2RAD;
        double O = el.longAscNodeDeg * DEG2RAD;
        double w = el.argPeriapsisDeg * DEG2RAD;

        // 回転: perifocal -> inertial (3-1-3)
        double cosO = Math.Cos(O), sinO = Math.Sin(O);
        double cosi = Math.Cos(inc), sini = Math.Sin(inc);
        double cosw = Math.Cos(w), sinw = Math.Sin(w);

        double R11 = cosO * cosw - sinO * sinw * cosi;
        double R12 = -cosO * sinw - sinO * cosw * cosi;
        double R13 = sinO * sini;
        double R21 = sinO * cosw + cosO * sinw * cosi;
        double R22 = -sinO * sinw + cosO * cosw * cosi;
        double R23 = -cosO * sini;
        double R31 = sinw * sini;
        double R32 = cosw * sini;
        double R33 = cosi;

        positionKm = new Vector3(
            (float)(R11 * x_orb + R12 * y_orb),
            (float)(R13 * x_orb + R23 * y_orb),
            (float)(R31 * x_orb + R32 * y_orb)
        );
        velocityKmPerSec = new Vector3(
            (float)(R11 * vx_orb + R12 * vy_orb),
            (float)(R13 * vx_orb + R23 * vy_orb),
            (float)(R31 * vx_orb + R32 * vy_orb)
        );
    }
}
