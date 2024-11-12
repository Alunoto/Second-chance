using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDistributor : MonoBehaviour
{
    public GameObject player;
    private Teleport[] teleports;
    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        teleports = this.getComponentsInChildren<Teleport>();

        foreach(Teleport teleport in teleports)
        {
            teleport.id = i;
            i++;
        }
    }

    public void Teleport(int id)
    {
        if (i <= 1)
            return;
        do
        {
            int rand = Random.Range(0, i);
        } while (rand == id)
        
        player.transform.position = teleports[rand].transform.position;
    }
}
