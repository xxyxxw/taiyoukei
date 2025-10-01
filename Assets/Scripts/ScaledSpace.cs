using UnityEngine;


// îºåaÇæÇØï î{ó¶Ç≈å©Ç‚Ç∑Ç≠Ç∑ÇÈè¨ï®ÅiîCà”Åj
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