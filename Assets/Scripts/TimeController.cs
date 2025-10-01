using UnityEngine;


public class TimeController : MonoBehaviour
{
    public SolarSystemManager sim;
    public double[] presets = new double[] { 1, 100, 1000, 10000, 100000, 1000000 };
    int idx = 3;


    void Start()
    {
        if (sim != null) sim.timeScale = presets[idx];
    }


    public void Faster()
    {
        idx = Mathf.Clamp(idx + 1, 0, presets.Length - 1);
        if (sim != null) sim.timeScale = presets[idx];
    }


    public void Slower()
    {
        idx = Mathf.Clamp(idx - 1, 0, presets.Length - 1);
        if (sim != null) sim.timeScale = presets[idx];
    }


    public void PauseToggle()
    {
        if (sim == null) return;
        if (Mathf.Approximately((float)sim.timeScale, 0)) sim.timeScale = presets[idx];
        else sim.timeScale = 0;
    }
}