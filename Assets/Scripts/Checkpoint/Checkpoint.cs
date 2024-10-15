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
        Debug.Log("Player entered the checkpoint volume!");
        revive.position = this.transform.position;
    }
}
