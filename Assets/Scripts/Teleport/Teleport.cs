using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public int id;
    private GameObject GateGroup;
    private TeleportDistributor teleportDistributor;

    void Start()
    {
        GateGroup = this.transform.parent;
        teleportDistributor = GateGroup.GetComponent<TeleportDistributor>();
    }

    private voidOnTriggerEnter()
    {
        teleportDistributor.Teleport(id);
    }
}
