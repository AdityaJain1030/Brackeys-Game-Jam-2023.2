using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class PlayerLight : MonoBehaviour
{
    public float oilRemaining { get; private set; } = 50;
    public PlayerData config;
    // public Light2D playerLight;

    private float secondsSinceLastOilLoss = 0;
    private float secondsPerOilDecrease;
    private float maxLightIntensity;
    // Start is called before the first frame update
    void Start()
    {
        secondsPerOilDecrease = config.oilConsumptionRate / 1;
        // maxLightIntensity = playerLight.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        secondsSinceLastOilLoss += Time.deltaTime;
        if (secondsSinceLastOilLoss >= secondsPerOilDecrease)
        {
            secondsSinceLastOilLoss = 0;
            oilRemaining -= 1;
        }

        if (oilRemaining <= 0)
        {
            // playerLight.intensity = 0;
            // GameObject.Find("Lights").SetActive("False");
            oilRemaining = 0;
        }
        else
        {
            // playerLight.intensity = Mathf.Lerp(config.maxLightDim, maxLightIntensity, oilRemaining / 100);
        }

        DrawOilToScreen();
    }

    private void DrawOilToScreen()
    {
        
    }

    public void addOil(int amount) {
        oilRemaining = Mathf.Max(100, oilRemaining + amount);
    }
}
