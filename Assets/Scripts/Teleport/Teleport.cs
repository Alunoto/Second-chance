using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public int id;
    private TeleportDistributor2 teleportDistributor;
    public delegate void FunctionTriggeredHandler(int id);
    public static event FunctionTriggeredHandler OnTeleportTriggered;

    void Start()
    {
        teleportDistributor = GetComponentInParent<TeleportDistributor2>();
    }

    private void OnTriggerEnter()
    {
        if (!teleportDistributor.isTeleported)
        {
            teleportDistributor.Teleport(id);
            OnTeleportTriggered.Invoke(id);
        }
        else
            teleportDistributor.isTeleported = false;
    }
}
