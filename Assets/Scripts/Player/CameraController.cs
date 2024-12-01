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
    Vector3 down, customRight, customForward, up, forwardRotated;

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
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        down = movePlayer.gOrientationFull;
        customRight = new Vector3(-player.transform.position.z, 0, player.transform.position.x);
        up = (down * -1f).normalized;

        customForward = Vector3.Cross(down, customRight).normalized;


        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, 20f, 150f);

        float alphaPitchRadians = pitch * Mathf.Deg2Rad; // Convert degrees to radians
        float alphaYawRadians = yaw * Mathf.Deg2Rad;

        Debug.Log("yaw: " + alphaYawRadians + ", pitch: " + pitch);

        // Calculate rotations
        Quaternion yawRotation = Quaternion.AngleAxis(yaw, up);
        forwardRotated = yawRotation * customForward;
        Vector3 result = (Mathf.Cos(alphaPitchRadians) * up + Mathf.Sin(alphaPitchRadians) * forwardRotated).normalized;

        // Apply rotation to camera

        //transform.rotation = targetRotation;

        // Follow player with offset
        //transform.position = playerTransform.position + targetRotation * cameraOffset;

        /*transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);*/
        player.transform.rotation = Quaternion.LookRotation(up);

        transform.rotation = Quaternion.LookRotation(result, (down * -1f).normalized);


    }
}
