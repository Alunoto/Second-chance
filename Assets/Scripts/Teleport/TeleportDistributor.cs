using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDistributor : MonoBehaviour
{
    public GameObject player;
    private Teleport[] teleports;
    private int i;
    public bool isTeleported = false;
    // Start is called before the first frame update
    void Start()
    {
        teleports = this.GetComponentsInChildren<Teleport>();

        for(i = 0; i < teleports.Length; i++)
        {
            teleports[i].id = i;
        }
    }

    public void Teleport(int id)
    {
        if (i <= 1)
            return;
        Debug.Log(id);
        int rand = id;
        do
        {
            rand = Random.Range(0, i);
        } while (rand == id);

        Debug.Log(rand);
        Debug.Log(teleports[id].transform.position);
        Debug.Log(player.transform.position);
        Debug.Log(teleports[rand].transform.position);

        player.transform.position = teleports[rand].transform.position;
        isTeleported = true;
    }
}
