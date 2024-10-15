using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour
{
    public Vector3 position = new Vector3(201f, 50f, 510f);
    public GameObject player;
    public GameObject controller;
    private AAController aaController;
    public bool isTutorial = false;

    private void Start()
    {
        aaController = controller.GetComponent<AAController>();
    }
    public void RevivePlayer()
    {
        player.transform.position = position;

        if (isTutorial)
            controller.GetComponent<Tutorial>().phaseNumber = 0;
        else
            aaController.ResetScript();

    }
}
