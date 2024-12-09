using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    protected MessageConnector mc;
    public int phaseNumber = 0;
    private bool indicator = false;
    private int prevJump = 1000000, prevRev = 1000000;


    private void Start()
    {
        //this.enabled = false; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        mc = GetComponent<MessageConnector>();
    }

    private void OnEnable()
    {
        // Subscribe to the event
        MovePlayer.OnJumpTriggered += RespondToJump;
        MovePlayer.OnReverseTriggered += RespondToReverse;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        MovePlayer.OnJumpTriggered -= RespondToJump;
        MovePlayer.OnReverseTriggered -= RespondToReverse;
    }

    void Update()
    {
       phaseSelector(ref phaseNumber);   //wazne important
    }

    void phaseSelector(ref int phaseNumber)
    {
        switch (phaseNumber)
        {
            case 0:
                mc.message.createNew("Mission and message system", false);
                phaseNumber++;
                break;
            case 1:
                if (mc.message.clicked == true)
                {
                    mc.message.createNew("To move press w, a, s, d or arrows", true);
                    phaseNumber++;  
                }
                break;
            case 2:
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    mc.message.createNew("To jump press space", true);
                    phaseNumber++;
                }
                break;
            case 3:
                if( indicator )
                {
                    mc.message.createNew("To reverse button press left control or button set in settings", true);
                    phaseNumber++;
                    indicator = false;
                }

                break;
            case 4:
                if (indicator)
                {
                    phaseNumber++;
                    indicator = false;
                }
                break;
            default:
                End();
                break;
        }
    }

    public void End()
    {
        AAController controller = GetComponent<AAController>();
        this.enabled = false;
        controller.NextScript();
    }

    public void RespondToJump()
    {
        if( phaseNumber == 3 )
        {
            indicator = true;
        }
    }

    public void RespondToReverse()
    {
        if (phaseNumber == 4)
        {
            indicator = true;
        }
    }


}
