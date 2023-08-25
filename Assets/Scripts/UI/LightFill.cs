using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFill : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] public Vector3 maxScale;
    [SerializeField] public Vector3 minScale;
    [SerializeField] public float speed = 2.0f;


    private PlayerLight playerLightScript;
    private RectTransform rectTransform;

    private float incrementScaleX;
    [SerializeField] public float oilRemaining;
    

    // Start is called before the first frame update
    void Start()
    {
        playerLightScript = player.GetComponent<PlayerLight>();
        rectTransform = GetComponent<RectTransform>();
        oilRemaining = playerLightScript.oilRemaining;
        incrementScaleX = Mathf.Abs(maxScale.x - minScale.x) / oilRemaining;

        rectTransform.localScale = maxScale;
    }

    // Update is called once per frame
    void Update()
    {
        oilRemaining = playerLightScript.oilRemaining;
        Vector3 fill = minScale;
        fill.x -= incrementScaleX * oilRemaining;
        rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, fill, speed);
    }
}
