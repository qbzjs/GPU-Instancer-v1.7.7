using System.Collections;
using System.Collections.Generic;
using Invector.vMelee;
using Unity.Mathematics;
using UnityEngine;

public class Particle_Spawner_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hitRecoil_ParticleSystem;
    
    public void SpawnRecoilHit_ParticleSystem(vHitInfo hitInfo)
    {
        GameObject spanwedParticleSystem = Instantiate(hitRecoil_ParticleSystem, hitInfo.hitPoint, quaternion.identity);
        PlayParticleSystem(spanwedParticleSystem);
    }
    
    public void PlayParticleSystem(GameObject ps)
    {
        ps.GetComponent<ParticleSystem>().Play();
    }

    public void RemoveParticleSystems(GameObject ps)
    {
        Debug.Log("Particle system is destroyed.");
        Destroy(ps);
    }
}