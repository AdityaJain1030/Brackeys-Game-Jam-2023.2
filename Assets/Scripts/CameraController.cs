using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform playerTransform;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 cameraPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        cameraPos = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
        transform.position = Vector3.SmoothDamp(transform.position, cameraPos, ref velocity, dampTime);
    }
}
