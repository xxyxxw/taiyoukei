using UnityEngine;


[CreateAssetMenu(menuName = "SolarSystem/BodyData", fileName = "BodyData")]
public class BodyData : ScriptableObject
{
    [Header("Basic")]
    public string bodyName;
    public bool isCentral; // ‘¾—z‚È‚Ç
    public float radiusKm; // ŽÀ”¼Œa [km]
    public double massKg; // Ž¿—Ê [kg]


    [Header("Initial State @ Epoch J2000 (approx)")]
    public Vector3 initialPositionKm; // heliocentric, ecliptic‹ßŽ—, [km]
    public Vector3 initialVelocityKmPerSec; // [km/s]


    [Header("Visual")]
    public Color color = Color.white;
    public Texture2D albedo; // ”CˆÓ
}