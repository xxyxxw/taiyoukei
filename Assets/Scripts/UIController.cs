using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    public SolarSystemManager sim;
    public Text timeScaleText;
    public Button fasterBtn, slowerBtn, pauseBtn;


    void Start()
    {
        var tc = sim.GetComponent<TimeController>();
        fasterBtn.onClick.AddListener(tc.Faster);
        slowerBtn.onClick.AddListener(tc.Slower);
        pauseBtn.onClick.AddListener(tc.PauseToggle);
    }


    void Update()
    {
        if (sim != null && timeScaleText != null)
        {
            timeScaleText.text = $"time x{sim.timeScale:0}";
        }
    }
}