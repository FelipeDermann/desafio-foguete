using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    public float rocketSpeed;
    public float secondsOfFuel;

    public GameObject lowerPartGameObj;

    private Rigidbody _rb;
    private bool _detached = false;
    private bool _moving = false;
    private bool _maxAltitudeSaved = false;

    private float _maxAltitude;
    
    private void Awake()
    {
        Countdown.StartRocketLaunch += StartLaunch;

        _rb = GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        Countdown.StartRocketLaunch -= StartLaunch;
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

        StartCoroutine(SpeedUp());
    }

    IEnumerator SpeedUp()
    {
        _moving = true;
        _rb.isKinematic = false;
        
        float time = 0;
        float d = 0;
        
        while (time < secondsOfFuel)
        {
            d += Time.deltaTime;
            time = Mathf.Lerp(0, secondsOfFuel, d/secondsOfFuel);
            
            _rb.AddForce(Vector3.up * rocketSpeed, ForceMode.Force);
            
            if (time >= secondsOfFuel * 0.5f && !_detached) DetachLowerPart(_rb.velocity);
            
            yield return null;
        }
        
        
        _rb.drag = 1;
    }

    void DetachLowerPart(Vector3 oldVelocity)
    {
        _detached = true;
        lowerPartGameObj.transform.parent = null;
        
        Rigidbody newRb = lowerPartGameObj.AddComponent<Rigidbody>();
        newRb.velocity = oldVelocity;
        newRb.angularDrag = 0.1f;
        newRb.drag = 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (_detached) _moving = false;
        }
    }
}
