using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : MonoBehaviour
{
    public GameObject player;
    public GameObject controller;
    private AAController aaController;
    private Revive revive;

    private void Start()
    {
        aaController = controller.GetComponent<AAController>();
        revive = controller.GetComponent<Revive>();
    }
    private void OnTriggerEnter()
    {
        revive.isTutorial = aaController.isTutorial;

        if (aaController.isTutorial)
        {
            revive.isTutorial = true;

        }

        revive.RevivePlayer();
        Debug.Log("Player entered the volume!");
    }
}
