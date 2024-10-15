using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission1 : AAMissionTemplate
{
    void Update()
    {
        phaseSelector(ref phaseNumber);
        void phaseSelector(ref int phaseNumber)
        {
            switch (phaseNumber)
            {
                case 0:
                    mc.message.createNew("Mission 1", false);
                    phaseNumber++;
                    break;
                case 1:
                    if (mc.message.clicked == true)
                    {
                        mc.message.createNew("wooooooooo", true);
                        phaseNumber++;
                    }
                    break;
                default:
                    End();
                    break;
            }
        }
    }
}
