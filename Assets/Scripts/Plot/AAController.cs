using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAController : MonoBehaviour
{
    public int scriptNumber = 0;
    public AAMissionTemplate[] scripts;
    public bool isTutorial = true;

    void Start()
    {
        scripts = GetComponents<AAMissionTemplate>();
    }

    public void NextScript()
    {
        isTutorial = false;
        if(scripts[scriptNumber].GetType() == typeof(AAMissionTemplate))
        {
            scriptNumber = scriptNumber + 1;
        }
        scripts[scriptNumber].enabled = true;
        scriptNumber = scriptNumber + 1;
    }

    public void ResetScript()
    {
        scripts[scriptNumber - 1].phaseNumber = 0;
    }

}
