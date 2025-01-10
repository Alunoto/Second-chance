using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission1 : AAMissionTemplate
{
    private int indicator = -1;
    public GameObject player;
    private MovePlayer movePlayer;
    public GameObject teleportGroup;
    public GameObject arrow;
    private RotateArrow rotateArrow;

    private void OnEnable()
    {
        Teleport.OnTeleportTriggered += RespondToTeleport;
    }

    private void OnDisable()
    {
        Teleport.OnTeleportTriggered -= RespondToTeleport;
    }

    void Update()
    {
        phaseSelector(ref phaseNumber);
        void phaseSelector(ref int phaseNumber)
        {
            switch (phaseNumber)
            {
                case 0:
                    mc.message.createNew("This is your first mission", false);
                    phaseNumber++;
                    break;
                case 1:
                    if (mc.message.clicked == true)
                    {
                        rotateArrow = arrow.GetComponent<RotateArrow>();
                        teleportGroup.SetActive(true);
                        mc.message.createNew("Please go the indicated teleport", true);
                        rotateArrow.targetPos = new Vector3(-170f, 1101f, 33); 
                        phaseNumber++;
                    }
                    break;
                case 2:
                    if (indicator == 0)
                    {
                        mc.message.createNew("Please go the next one", true);
                        rotateArrow.targetPos = new Vector3(-2f, 870f, -289f);
                        phaseNumber++;
                    }
                    break;
                case 3:
                    if (indicator == 2)
                    {
                        mc.message.createNew("Oh hey, you got your gravity back to normal, enjoy it", false);
                        phaseNumber++;
                    }
                    break;
                case 4:
                    if (mc.message.clicked == true)
                    {
                        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 200, player.transform.position.z);
                        mc.message.createNew("What is happening?", false);
                        phaseNumber++;
                    }
                    break;
                case 5:
                    if (mc.message.clicked == true && player.transform.position.y < -100)
                    {
                        mc.message.createNew("Oh hey, you got back to your new world", false);
                        phaseNumber++;
                    }
                    break;
                case 6:
                    if (mc.message.clicked == true)
                    {
                        mc.message.createNew("You can reverse gravity and fly back to torus to play a little, at some point its gravity will intercept you", true);
                        phaseNumber++;
                    }
                    break;
                case 7:
                    if (mc.message.clicked == true && player.transform.position.y > 500)
                    {
                        mc.message.createNew("You got intercepted by thorus gravity", true);
                        movePlayer = player.GetComponent<MovePlayer>();
                        movePlayer.reverseGravity = 1;
                        movePlayer.ChangeGravity();
                        phaseNumber++;
                    }
                    break;
                case 8:
                    break;
                default:
                    End();
                    break;
            }
        }
    }

    private void RespondToTeleport(int id)
    {
        indicator = id;
    }
}
