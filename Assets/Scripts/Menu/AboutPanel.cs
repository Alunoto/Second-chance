using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutPanel : MonoBehaviour
{
    public GameObject panel;

    void Start()
    {
        panel.SetActive(false);
    }

    public void activate()
    {
        panel.SetActive(true);
    }

    public void inactivate()
    {
        panel.SetActive(false); 
    }
}
