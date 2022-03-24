using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public static Action<float> RocketRotationChanged;
    
    public static UIManager Instance;
    
    public Countdown countdownObj;
    public GameObject startButtonGameObj;
    public GameObject restartButtonGameObj;
    public GameObject rotationSliderGameObj;
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
        startButtonGameObj.SetActive(false);
        rotationSliderGameObj.SetActive(false);
        countdownObj.StartCountdown();
    }

    public void SetAltitudeUI(float maxAltitude)
    {
        altitudeText.text = maxAltitude.ToString("F1");
    }

    public void ChangeRocketRotation(float sliderValue)
    {
        RocketRotationChanged(sliderValue);
    }

    public void ShowRestartButton()
    {
        restartButtonGameObj.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
