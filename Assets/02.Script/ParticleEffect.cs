using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ParticleEffect : PoolItem
{
    [SerializeField]
    private float disableTime = 1f;

    private static string Func_DisableObject = "DisableObject";

    [SerializeField]
    private ParticleSystem[] particles;

    private void OnValidate()
    {
        particles = GetComponentsInChildren<ParticleSystem>(true);
    }
    private void OnEnable()
    {
        Invoke(Func_DisableObject, disableTime);

        for(int i=0;i< particles.Length; i++) 
        {
            particles[i].Play();
        }
    }

    private void DisableObject()
    {
        this.gameObject.SetActive(false);
    }
}
