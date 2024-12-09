using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDistributor2 : MonoBehaviour
{
    public GameObject player;
    private MovePlayer movePlayer;
    private Teleport[] teleports;
    private int i;
    public bool isTeleported = false;
    // Start is called before the first frame update
    void Start()
    {
        teleports = this.GetComponentsInChildren<Teleport>();
        movePlayer = player.GetComponent<MovePlayer>();

       /* for(i = 0; i < teleports.Length; i++)
        {
            teleports[i].id = i;
        }*/
    }

    public void Teleport(int id)
    {
        if (id == 2)
        {
            movePlayer.ChangeGravity();
        }

        if (id == 3)
        {
            id = -1;
        }

        player.transform.position = teleports[id+1].transform.position;
        teleports[id + 1].enabled = false;
        //Debug.Log(teleports[id + 1].transform.position);
        isTeleported = true;
    }
}
