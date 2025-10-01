using UnityEngine;


// ���a�����ʔ{���Ō��₷�����鏬���i�C�Ӂj
public class ScaledSpace : MonoBehaviour
{
    public CelestialBody body;
    public SolarSystemManager sim;


    void LateUpdate()
    {
        if (body == null || sim == null) return;
        float scaledRadius = Mathf.Max(0.05f, body.RadiusKm / sim.distanceScale * sim.radiusVisualScale);
        if (body.visual != null) body.visual.localScale = Vector3.one * scaledRadius * 2f;
    }
}