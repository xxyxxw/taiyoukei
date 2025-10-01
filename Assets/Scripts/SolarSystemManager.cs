using UnityEngine;
using System.Collections.Generic;


public class SolarSystemManager : MonoBehaviour
{
    [Header("Bodies in simulation")]
    public List<CelestialBody> bodies = new List<CelestialBody>();


    [Header("Scales")]
    public float distanceScale = 1000f; // 1 unit = 1000 km
    public float radiusVisualScale = 500f; // ���₷���p


    [Header("Time")]
    public double timeScale = 10000; // ���b�ɑ΂���{��
    public double fixedDt = 60; // �����X�e�b�v [s]
    public bool useAdaptiveSubsteps = true; // �ߐڎ��̈��艻


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
            b.visual.localScale = Vector3.one * scaledRadius * 2f; // ���a
        }
    }


    void FixedUpdate()
    {
        // �σT�u�X�e�b�v
        double dtSim = fixedDt * timeScale; // [s] per FixedUpdate
        int sub = 1;
        if (useAdaptiveSubsteps)
        {
            // �G: �ő呬�x��ŋߐڋ����ɉ����Ē������Ă��ǂ�
            if (timeScale > 100000) sub = 2;
            if (timeScale > 500000) sub = 4;
            if (timeScale > 2000000) sub = 8;
        }
        double step = dtSim / sub;


        for (int i = 0; i < sub; i++)
        {
            RK4.Step(bodies, step);
        }


        // �\���X�V
        foreach (var b in bodies)
        {
            b.transform.position = b.positionKm / distanceScale; // km -> unit
        }
    }
}