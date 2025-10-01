using UnityEngine;


[RequireComponent(typeof(MeshRenderer))]
public class CelestialBody : MonoBehaviour
{
    public BodyData data;


    [Header("State (km / km/s)")]
    public Vector3 positionKm;
    public Vector3 velocityKmPerSec;


    [Header("Runtime refs")]
    public Transform visual; // ‹…‘Ì


    public double Mass => data != null ? data.massKg : 0.0;
    public float RadiusKm => data != null ? data.radiusKm : 1000f;
}