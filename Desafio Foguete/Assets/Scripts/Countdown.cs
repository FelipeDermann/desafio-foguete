using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public static Action StartRocketLaunch;
    
    public TextMeshProUGUI countdownText;

    private int _count;
    private Animator _anim;
    private int _play = Animator.StringToHash("Play");
    private int _stop = Animator.StringToHash("Stop");

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void StartCountdown()
    {
        _count = 3;
        countdownText.enabled = true;
        _anim.SetTrigger(_play);
    }

    public void CheckCount()
    {
        _count -= 1;
        if (_count < 0)
        {
            FinishCount();
            return;
        }
            
        _anim.SetTrigger(_play); 
    }

    public void SetCountdownText()
    {
        string textToShow = _count == 0 ? "LIFT OFF!" : _count.ToString();
        countdownText.text = textToShow;
    }

    void FinishCount()
    {
        countdownText.enabled = false;
        _anim.SetTrigger(_stop);
        
        StartRocketLaunch();
    }
}
