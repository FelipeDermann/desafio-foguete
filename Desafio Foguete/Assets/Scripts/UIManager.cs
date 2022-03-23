using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    public Countdown countdownObj;
    public GameObject buttonGameObj;
    public TextMeshProUGUI altitudeText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayCountdownAnim()
    {
        buttonGameObj.SetActive(false);
        countdownObj.StartCountdown();
    }

    public void SetAltitudeUI(float maxAltitude)
    {
        altitudeText.text = maxAltitude.ToString("F1");
    }
}
