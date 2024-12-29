using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour
{
    public Vector3 position = new Vector3(201f, 50f, 510f);
    public GameObject player;
    public GameObject controller;
    public GameObject tp;
    private Teleport[] teleports;
    private AAController aaController;
    private MovePlayer movePlayer;
    public bool isTutorial = false;

    private void Start()
    {
        aaController = controller.GetComponent<AAController>();
    }
    public void RevivePlayer()
    {
        player.transform.position = position;
        player.GetComponent<MovePlayer>().StopMovement();
        movePlayer = player.GetComponent<MovePlayer>();

        movePlayer.reverseGravity = 1;
        if (!movePlayer.customGravity)
            movePlayer.ChangeGravity();

        if (isTutorial)
            controller.GetComponent<Tutorial>().phaseNumber = 0;
        else
            aaController.ResetScript();

        teleports = tp.GetComponentsInChildren<Teleport>();

        for(int i = 0; i < teleports.Length; i++)
         {
             teleports[i].gameObject.SetActive(true);
         }
    }
}
