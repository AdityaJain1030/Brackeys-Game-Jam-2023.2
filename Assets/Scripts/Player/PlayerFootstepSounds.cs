using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepSounds : MonoBehaviour
{
    [SerializeField] public AudioClip[] walkingClip;

    private AudioSource footstepAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        footstepAudioSource = gameObject.transform.Find("FootstepSFX").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void footstep() {
        footstepAudioSource.clip = walkingClip[Random.Range(0,walkingClip.Length)];
        footstepAudioSource.Play();
    }
}
