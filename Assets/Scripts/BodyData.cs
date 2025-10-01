using UnityEngine;


[CreateAssetMenu(menuName = "SolarSystem/BodyData", fileName = "BodyData")]
public class BodyData : ScriptableObject
{
    [Header("Basic")]
    public string bodyName;
    public bool isCentral; // ���z�Ȃ�
    public float radiusKm; // �����a [km]
    public double massKg; // ���� [kg]


    [Header("Initial State @ Epoch J2000 (approx)")]
    public Vector3 initialPositionKm; // heliocentric, ecliptic�ߎ�, [km]
    public Vector3 initialVelocityKmPerSec; // [km/s]


    [Header("Visual")]
    public Color color = Color.white;
    public Texture2D albedo; // �C��
}