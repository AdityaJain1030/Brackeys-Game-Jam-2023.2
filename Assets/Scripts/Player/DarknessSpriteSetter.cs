using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessSpriteSetter : MonoBehaviour
{
    [SerializeField] public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().sprite = player.GetComponent<SpriteRenderer>().sprite;
        GetComponent<SpriteRenderer>().flipX = player.GetComponent<SpriteRenderer>().flipX;
    }
}
