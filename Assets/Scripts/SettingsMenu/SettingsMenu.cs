using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject panel;
    public Slider brightnessSlider;
    public Image brightnessOverlay;
    public Canvas messageCanvas;
    private Message message;

    public NewInputs inputActions;
    public Button rebindButton;
    public TMP_Text bindingDisplayName;
    private InputAction actionToRebind;

    private void Awake()
    {
        inputActions = FindObjectOfType<MovePlayer>().inputs;
        Debug.Log("eluwina" + inputActions);
        if (inputActions == null)
        {
            inputActions = new NewInputs(); // Create the shared instance
        }

        // Load saved bindings if any
        LoadBindings();
    }

    void Start()
    {
        message = messageCanvas.GetComponent<Message>();
        panel.SetActive(false);
        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
        brightnessSlider.value = 0.5f; // Default brightness

        actionToRebind = inputActions.FindAction("reverse");

        // Update display name with the current binding
        UpdateBindingDisplay();

        // Set up the button click listener
        rebindButton.onClick.AddListener(() => StartRebinding());
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
        if (isPaused)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
    }

    void PauseGame()
    {
        message.panel.SetActive(false);
        panel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        panel.SetActive(false);
        message.panel.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void OnBrightnessChanged(float value)
    {
        Color overlayColor = brightnessOverlay.color;
        overlayColor.a = (int)value*0.5f;
        brightnessOverlay.color = overlayColor;
    }

    void UpdateBindingDisplay()
    {
        // Get the first binding and display its name
        if (actionToRebind != null)
        {
            var binding = actionToRebind.bindings[0];
            bindingDisplayName.text = InputControlPath.ToHumanReadableString(binding.effectivePath);
        }
    }

    public void StartRebinding()
    {
        // Disable the button while rebinding
        rebindButton.interactable = false;

        actionToRebind.Disable();

        // Start rebinding
        actionToRebind.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)// Exclude certain controls if needed
            .OnComplete(operation =>
            {
                Debug.Log("Rebind Complete: " + operation.action.bindings[0].effectivePath);

                // Update UI
                UpdateBindingDisplay();

                SaveBindings();
                LoadBindings();

                // Re-enable the button
                rebindButton.interactable = true;

                // Clean up
                operation.Dispose();
            })
            .Start();
       
    }

    public void SaveBindings()
    {
        string rebindings = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebindings", rebindings);
        PlayerPrefs.Save(); // Ensures the data is written to disk
        FindObjectOfType<MovePlayer>().RefreshBindings();

        Debug.Log("saved");
    }

    public void LoadBindings()
    {
        string savedRebindings = PlayerPrefs.GetString("rebindings", null);
        if (!string.IsNullOrEmpty(savedRebindings))
        {
            Debug.Log("ciagnie");
            inputActions.LoadBindingOverridesFromJson(savedRebindings);
        }
        else
            Debug.Log("ssie");
    }

}
