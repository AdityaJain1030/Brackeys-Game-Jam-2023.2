using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLightCollection : MonoBehaviour
{
    [SerializeField] public ParticleSystem ps;
    [SerializeField] public GameObject player;
    private PlayerLight playerLight;

    List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
    // Start is called before the first frame update
    void Start()
    {
        playerLight = player.GetComponent<PlayerLight>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleTrigger() 
    {
        int triggeredParticles = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
        for (int i=0; i<triggeredParticles; i++) {
            ParticleSystem.Particle p = particles[i];
            p.remainingLifetime = 0;
            Debug.Log("1 particle collected");
            particles[i] = p;
            playerLight.addOil(1);
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
    }
}
