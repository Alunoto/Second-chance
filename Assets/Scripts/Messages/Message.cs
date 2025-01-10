using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Message : MonoBehaviour
{
    public string message;
    public TMP_Text text;
    public RectTransform panelTransform;
    public GameObject button;   
    public GameObject panel;
    private bool minimize = false;
    public bool clicked = false;
    private Vector3 scaleLarge;
    private Vector3 scaleSmall;
    public GameObject playerCamera;
    CameraController cameraController;

    void Start()    
    {
        scaleLarge.x = 0.5f;
        scaleLarge.y = 0.5f;
        scaleSmall.x = 0.2f;
        scaleSmall.y = 0.2f;
        panel.SetActive(false);
        cameraController = playerCamera.GetComponent<CameraController>();
    }

    private void Update()
    {
        if (!clicked)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
    }

    public void ButtonAction()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        clicked = true;
        if (minimize)
        {
            moveToCorner();
            panelTransform.localScale = scaleSmall;
            button.SetActive(false);
        }
        else
        {
            panel.SetActive(false); 
        }
        cameraController.freezeRotation = false;
    }

    public void createNew(string mes, bool min)
    {
        clicked = false;
        moveToCenter();
        panelTransform.localScale = scaleLarge;
        text.text = mes;
        button.SetActive(true);
        panel.SetActive(true);
        minimize = min;
        cameraController.freezeRotation = true;
    }

    private void moveToCenter()
    {
        panelTransform.anchorMin = new Vector2(0.5f, 0.5f);
        panelTransform.anchorMax = new Vector2(0.5f, 0.5f);
        panelTransform.pivot = new Vector2(0.5f, 0.5f);
        panelTransform.anchoredPosition = Vector2.zero;
    }

    private void moveToCorner()
    {
        panelTransform.anchorMin = new Vector2(1, 1);
        panelTransform.anchorMax = new Vector2(1, 1);
        panelTransform.pivot = new Vector2(1, 1);
        panelTransform.anchoredPosition = Vector2.zero;
    }

}
