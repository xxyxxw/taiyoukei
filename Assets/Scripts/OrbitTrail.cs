using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(LineRenderer))]
public class OrbitTrail : MonoBehaviour
{
    public CelestialBody body;
    public int maxPoints = 4096;
    public float minSegmentUnit = 0.02f; // ì_ä‘ÇÃç≈è¨ãóó£ [unit]


    LineRenderer lr;
    readonly List<Vector3> pts = new();
    Vector3 lastPos;


    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.positionCount = 0;
        lr.widthMultiplier = 0.01f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lr.receiveShadows = false;
    }


    void LateUpdate()
    {
        if (body == null) return;
        Vector3 p = body.transform.position;
        if (pts.Count == 0 || (p - lastPos).sqrMagnitude > minSegmentUnit * minSegmentUnit)
        {
            pts.Add(p);
            if (pts.Count > maxPoints) pts.RemoveAt(0);
            lr.positionCount = pts.Count;
            lr.SetPositions(pts.ToArray());
            lastPos = p;
        }
    }
}