using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public int id;
    private TeleportDistributor2 teleportDistributor;

    void Start()
    {
        teleportDistributor = GetComponentInParent<TeleportDistributor2>();
    }

    private void OnTriggerEnter()
    {
        if (!teleportDistributor.isTeleported)
            teleportDistributor.Teleport(id);
        else
            teleportDistributor.isTeleported = false;
    }
}
