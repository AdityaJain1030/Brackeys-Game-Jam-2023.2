using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSound : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public AudioClip swordSwooshClip;

    private AudioSource swordAudioSource;
    void Start()
    {
        swordAudioSource = gameObject.transform.Find("SwordSFX").GetComponent<AudioSource>();
        swordAudioSource.clip = swordSwooshClip;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void swordSwoosh() {
        swordAudioSource.Play();
    }
}
