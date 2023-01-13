using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    ParticleSystem particle;
    ParticleSystem.Particle[] gameEffects;
    
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        gameEffects = new ParticleSystem.Particle[particle.main.maxParticles];
    }
   
}
