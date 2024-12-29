using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject controller;
    private Revive revive;
    private void Start()
    {
        revive = controller.GetComponent<Revive>();    
    }
    private void OnTriggerEnter()
    {
        revive.position = this.transform.position;
    }
}
