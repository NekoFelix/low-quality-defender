using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaParticleTriggers : MonoBehaviour
{
    ParticleSystem _particleSystem;
    Enemy _enemy;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    private void OnEnable()
    {
        _enemy = FindObjectOfType<Enemy>();
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()
    {
        if (!_enemy) { return; }
        _particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        Debug.Log("Name: " + _particleSystem.name);
    }
}
