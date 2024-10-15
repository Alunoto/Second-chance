using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    protected MessageConnector mc;
    public int phaseNumber = 0;

    private void Start()
    {
        //this.enabled = false; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        mc = GetComponent<MessageConnector>();
    }

    void Update()
    {
       phaseSelector(ref phaseNumber);
    }

    void phaseSelector(ref int phaseNumber)
    {
        switch (phaseNumber)
        {
            case 0:
                mc.message.createNew("Welcome to the world", false);
                phaseNumber++;
                break;
            case 1:
                if (mc.message.clicked == true)
                {
                    mc.message.createNew("move", true);
                    phaseNumber++;  
                }
                break;
            case 2:
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    mc.message.createNew("jump", true);
                    phaseNumber++;
                }
                break;
            case 3:
                if(Input.GetButtonDown("Jump"))
                {
                    mc.message.createNew("fly", true);
                    phaseNumber++;
                }
                break;
            case 4:
                if (Input.GetButtonDown("Fire3"))
                {
                    phaseNumber++;
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
}
