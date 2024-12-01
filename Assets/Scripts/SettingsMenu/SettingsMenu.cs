using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject panel;
    public Slider brightnessSlider;
    public Image brightnessOverlay;

    void Start()
    {
        panel.SetActive(false);
        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
        brightnessSlider.value = 0.5f; // Default brightness
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        panel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        panel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void OnBrightnessChanged(float value)
    {
        Color overlayColor = brightnessOverlay.color;
        overlayColor.a = (int)value*0.5f;
        brightnessOverlay.color = overlayColor;
    }
}
