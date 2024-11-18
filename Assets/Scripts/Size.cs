using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Size : MonoBehaviour
{
    public GameObject capsulePrefab;  // Assign a Capsule prefab in the Inspector
    public int numberOfCapsules = 200;
    public Vector3 spawnArea = new Vector3(900f, 900f, 900f); // Defines the area within which capsules are spawned
    private float x, y, z, alpha, xp, zp;
    Vector3 gOrientation;

    void Start()
    {
        SpawnCapsules();
    }

    void SpawnCapsules()
    {
        for (int i = 0; i < numberOfCapsules; i++)
        {
            // Generate a random position within the defined spawn area
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
                Random.Range(-spawnArea.y / 2, spawnArea.y / 2),
                Random.Range(-spawnArea.z / 2, spawnArea.z / 2)
            );

            // Instantiate a new capsule at the random position with no rotation
            GameObject capsule = Instantiate(capsulePrefab, randomPosition, Quaternion.identity);

            // Add a Rigidbody to the capsule if it doesn’t already have one
            Rigidbody rb = capsule.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = capsule.AddComponent<Rigidbody>();
            }

            // Turn off gravity for the Rigidbody
            rb.useGravity = false;

            x = capsule.transform.position.x;
            z = capsule.transform.position.z;
            alpha = Mathf.Atan((Mathf.Sqrt(x * x)) / Mathf.Sqrt(z * z));

            xp = 250 * Mathf.Sin(alpha);
            zp = 250 * Mathf.Cos(alpha);

            if (x < 0f)
                xp = xp * -1;

            if (z < 0f)
                zp = zp * -1;

            /*        Debug.Log("punkty bazowe: "+ xp +", " + zp);
                    Debug.Log("lokalizacja: "+ x + ", " + z);
                    Debug.Log("k¹t: " + alpha);
                    Debug.Log(player.velocity);*/

            /*        if (x * x + z * z < 62500f)
                        dirHelper = -1;
                    else
                        dirHelper = 1;*/
            gOrientation = new Vector3((xp - x), capsule.transform.position.y * -1, (zp - z));

            gOrientation.Normalize();
            Debug.Log(gOrientation * 10f);
            //player.velocity = Vector3.zero;
            rb.AddForce(gOrientation * 10f, ForceMode.Acceleration);
            alpha = Mathf.Acos((gOrientation.z * 1) / (Mathf.Sqrt(gOrientation.x * gOrientation.x + gOrientation.z * gOrientation.z)));
            alpha = alpha * 180f / Mathf.PI;
            if (gOrientation.x < 0f)
            {
                alpha = 360 - alpha;
            }

            rb.freezeRotation = false;
            capsule.transform.rotation = Quaternion.Euler(gOrientation.y * -90f - 90f, 0f, alpha - alpha * Mathf.Abs(gOrientation.y));
            //Quaternion targetRotation = Quaternion.LookRotation(player.transform.forward, -gOrientation.normalized);

            // Smoothly rotate the cylinder toward the target rotation
            //player.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            rb.freezeRotation = true;
        }
    }
}
