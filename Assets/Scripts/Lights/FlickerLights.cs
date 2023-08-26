using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Experimental.GlobalIllumination;

public class FlickerLights : MonoBehaviour
{
    public float speed;
    public float flickerRadiusDelta;
    public GameObject lightObject;
    private SpotLight[] childLights;
    private float curTime;
    private float[] initRadius;
    // [SerializeField] private SpotLight spotLight;
    // Start is called before the first frame update
    void Start()
    {
        childLights = lightObject.GetComponentsInChildren<SpotLight>();
        initRadius = childLights.Select(light => light.range).ToArray();
        // spotLight = GetComponent<SpotLight>();
    }

    // Update is called once per frame
    void Update()
    {
        //lerp between radius of child lights depeinding on speed
        if (curTime >= 1) 
            curTime -= Time.deltaTime * speed;
        
        else 
            curTime += Time.deltaTime * speed;
        
        for (int i = 0; i < childLights.Length; i++) {
            childLights[i].range = Mathf.Lerp(initRadius[i], initRadius[i]-flickerRadiusDelta, curTime);
        }
    }
}
