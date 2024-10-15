using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAMissionTemplate : MonoBehaviour
{
    protected MessageConnector mc;
    public int phaseNumber = 0;
    
    void Start()
    {
        mc = GetComponent<MessageConnector>();
        this.enabled = false;
    }
    public void End()
    {
        AAController controller = GetComponent<AAController>();
        this.enabled = false;
        controller.NextScript();
    }

}
