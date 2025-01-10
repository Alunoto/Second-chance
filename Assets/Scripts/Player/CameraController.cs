using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using static UnityEngine.LightAnchor;

public class CameraController : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public GameObject player;
    private MovePlayer movePlayer;
    public Vector3 customDown, customRight, customForward, customUp, forwardRotated;
    public bool freezeRotation = false;

    public Transform orientation;

    float yaw, pitch;
    void Start()
    {
        movePlayer = player.GetComponent<MovePlayer>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        if (freezeRotation)
            return;
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        if (movePlayer.customGravity)
        {
            customDown = movePlayer.gOrientation.normalized * movePlayer.reverseGravity;
            customRight = new Vector3(-player.transform.position.z, 0, player.transform.position.x).normalized;
            customUp = (customDown * -1f).normalized;
            customForward = Vector3.Cross(customDown, customRight).normalized;
        }
        else
        {
            customDown = new Vector3(0f, -1f, 0f);
            customRight = new Vector3(1f, 0f, 0f);
            customUp = (customDown * -1f).normalized;
            customForward = Vector3.Cross(customDown, customRight).normalized;
        }

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, 20f, 150f);

        float alphaPitchRadians = pitch * Mathf.Deg2Rad;
        float alphaYawRadians = yaw * Mathf.Deg2Rad;

        Quaternion yawRotation = Quaternion.AngleAxis(yaw, customUp);
        forwardRotated = yawRotation * customForward;
        Vector3 result = (Mathf.Cos(alphaPitchRadians) * customUp + Mathf.Sin(alphaPitchRadians) * forwardRotated).normalized;

        transform.rotation = Quaternion.LookRotation(result, customUp);
    }
}
