using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerRocket : MonoBehaviour
{
    public ParticleSystem propulsionParticle;

    private void Awake()
    {
        RocketController.LowerPartDetached += Detach;
        RocketController.LaunchStarted += StartPropulsion;
    }

    private void OnDestroy()
    {
        RocketController.LowerPartDetached -= Detach;
        RocketController.LaunchStarted -= StartPropulsion;

    }

    void Detach(Vector3 oldVelocity)
    {
        transform.parent = null;
        
        Rigidbody newRb = gameObject.AddComponent<Rigidbody>();
        newRb.velocity = oldVelocity;
        newRb.angularDrag = 0.1f;
        newRb.drag = 1;
        newRb.interpolation = RigidbodyInterpolation.Interpolate;
        
        propulsionParticle.Stop();
    }

    void StartPropulsion()
    {
        propulsionParticle.Play();
    }
}
