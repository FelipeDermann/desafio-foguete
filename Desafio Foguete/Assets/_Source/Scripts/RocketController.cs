using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    public static Action LaunchStarted;
    public static Action<Vector3> LowerPartDetached;
    
    public float rocketSpeed;
    public float secondsOfFuel;

    public ParticleSystem propulsionParticle;

    private Rigidbody _rb;
    private ConstantForce _constForce;
    private bool _detached = false;
    private bool _moving = false;
    private bool _maxAltitudeSaved = false;

    private float _maxAltitude;

    private Animator _anim;
    private AudioSource _audioSrc;
    private int OpenParachute = Animator.StringToHash("OpenParachute");

    private void Awake()
    {
        Countdown.StartRocketLaunch += StartLaunch;
        UIManager.RocketRotationChanged += ChangeRotation;

        _rb = GetComponent<Rigidbody>();
        _constForce = GetComponent<ConstantForce>();
        _anim = GetComponent<Animator>();
        _audioSrc = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        Countdown.StartRocketLaunch -= StartLaunch;
        UIManager.RocketRotationChanged -= ChangeRotation;
    }

    void Update()
    {
        if (!_moving) return;

        FaceMovementDirection();
        CheckAltitude();
    }

    void CheckAltitude()
    {
        if (_maxAltitudeSaved) return;
        
        if (_rb.velocity.y < 0)
        {
            _maxAltitudeSaved = true;
            UIManager.Instance.SetAltitudeUI(_maxAltitude);
            _anim.SetTrigger(OpenParachute);    
            _rb.drag = 1;

            return;
        }
        _maxAltitude = transform.position.y;
    }

    void FaceMovementDirection()
    {
        Quaternion desiredRotation = Quaternion.LookRotation(_rb.velocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime);
    }
    
    void StartLaunch()
    {
        Debug.Log("Starting rocket launch!");

        StartCoroutine(GainSpeed());
    }

    IEnumerator GainSpeed()
    {
        LaunchStarted();
        _audioSrc.Play();
        
        _moving = true;
        _rb.isKinematic = false;
        
        float time = 0;
        float cForceToGive = 0;
        float d = 0;
        
        while (time < secondsOfFuel)
        {
            d += Time.deltaTime;
            time = Mathf.Lerp(0, secondsOfFuel, d/secondsOfFuel);
            
            _rb.AddForce(transform.forward * rocketSpeed, ForceMode.Force);

            cForceToGive = Mathf.Lerp(0,0.8f, d/4);
            Vector3 constForce = Vector3.zero;
            constForce.x = cForceToGive;
            _constForce.force = constForce;
            
            if (time >= secondsOfFuel * 0.5f && !_detached)
            {
                DetachLowerPart(_rb.velocity);
                propulsionParticle.Play();

                //restart the sound so that it seems that the second propulsion
                //started when the lower one stopped
                _audioSrc.Stop();
                _audioSrc.Play();
            }
            yield return null;
        }

        _rb.useGravity = true;
        _audioSrc.Stop();
        propulsionParticle.Stop();
    }

    void DetachLowerPart(Vector3 oldVelocity)
    {
        _detached = true;
        LowerPartDetached(oldVelocity);
    }

    void ChangeRotation(float sliderValue)
    {
        
        float xRot = Mathf.Lerp(-160, -20, sliderValue);
        var rotation = transform.rotation;
        
        Vector3 newRot = new Vector3(xRot, 90, rotation.z);
        rotation = Quaternion.Euler(newRot);
        
        transform.rotation = rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (_detached && _moving)
            {
                _rb.drag = 0;
                _moving = false;
            }
        }
    }
}
