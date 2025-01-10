using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateArrow : MonoBehaviour
{
    public Vector3 targetPos = new Vector3(-1700f, 1100f, 0f);
    Vector3 targetVec;
    public GameObject playerCamera;
    CameraController cameraController;
    void Start()
    {
        cameraController = playerCamera.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        targetVec = new Vector3(targetPos.x - transform.position.x, targetPos.y - transform.position.y, targetPos.z - transform.position.z);
        transform.rotation = Quaternion.LookRotation(targetVec, cameraController.customUp);
    }
}
