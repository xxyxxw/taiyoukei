using UnityEngine;
using System.Collections.Generic;


public class SolarSystemManager : MonoBehaviour
{
    [Header("Bodies in simulation")]
    public List<CelestialBody> bodies = new List<CelestialBody>();


    [Header("Scales")]
    public float distanceScale = 1000f; // 1 unit = 1000 km
    public float radiusVisualScale = 500f; // 見やすさ用


    [Header("Time")]
    public double timeScale = 10000; // 実秒に対する倍率
    public double fixedDt = 60; // 物理ステップ [s]
    public bool useAdaptiveSubsteps = true; // 近接時の安定化


    double accumulator;


    void Start()
    {
        foreach (var b in bodies)
        {
            if (b.data == null) continue;
            b.positionKm = b.data.initialPositionKm;
            b.velocityKmPerSec = b.data.initialVelocityKmPerSec;


            if (b.visual == null)
            {
                var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.SetParent(b.transform, false);
                b.visual = sphere.transform;
                var mr = sphere.GetComponent<MeshRenderer>();
                mr.material = new Material(Shader.Find("Standard"));
                mr.material.color = b.data.color;
                if (b.data.albedo) mr.material.mainTexture = b.data.albedo;
            }


            float scaledRadius = Mathf.Max(0.05f, b.RadiusKm / distanceScale * radiusVisualScale);
            b.visual.localScale = Vector3.one * scaledRadius * 2f; // 直径
        }
    }


    void FixedUpdate()
    {
        // 可変サブステップ
        double dtSim = fixedDt * timeScale; // [s] per FixedUpdate
        int sub = 1;
        if (useAdaptiveSubsteps)
        {
            // 雑: 最大速度や最近接距離に応じて調整しても良い
            if (timeScale > 100000) sub = 2;
            if (timeScale > 500000) sub = 4;
            if (timeScale > 2000000) sub = 8;
        }
        double step = dtSim / sub;


        for (int i = 0; i < sub; i++)
        {
            RK4.Step(bodies, step);
        }


        // 表示更新
        foreach (var b in bodies)
        {
            b.transform.position = b.positionKm / distanceScale; // km -> unit
        }
    }
}