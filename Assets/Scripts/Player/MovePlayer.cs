using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode reverseGravityKey = KeyCode.LeftControl;

    [Header("Ground check")]
    public float playerHeight;
    bool grounded;

    [Header("Gravity")]
    private float x, y, z, alpha, xp, zp;
    Vector3 gOrientation;
    int dirHelper;
    public GameObject body;
    int reverseGravity = 1;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody player;
    void Start()
    {
        player = GetComponent<Rigidbody>(); 
        player.freezeRotation = true;
        player.useGravity = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is the torus
        if (collision.gameObject.CompareTag("Torus"))
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Torus"))
        {
            grounded = false;
        }
    }

    void Update()
    {
        GetInput();

        if (grounded)
        {
            player.drag = groundDrag;
            LimitSpeed();
        }
        else
            player.drag = 0.5f;


    }

    private void FixedUpdate()
    {
        Move();
        ApplyGravity();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(reverseGravityKey))
        {
            reverseGravity = reverseGravity * -1;
        }
    }

    private void Move()
    {
        moveDirection = body.transform.forward * verticalInput + body.transform.right * horizontalInput;//orientation.right * horizontalInput;

        Debug.Log("forward: " + body.transform.forward + ", rotation: " + body.transform.rotation);
        if (grounded)
            player.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else
            player.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void LimitSpeed()
    {
        Vector3 flatVel = new Vector3(player.velocity.x, 0f, player.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            player.velocity = new Vector3(limitedVel.x, player.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //player.velocity = new Vector3(player.velocity.x, 0f, player.velocity.z);

        player.AddForce(gOrientation * -0.01f * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    public void StopMovement()
    {
        player.velocity = Vector3.zero;          // Stops linear movement
        player.angularVelocity = Vector3.zero;    // Stops any rotation
    }

    private void ApplyGravity()
    {
        x = player.transform.position.x;
        z = player.transform.position.z;
        alpha = Mathf.Atan((Mathf.Sqrt(x*x)) / Mathf.Sqrt(z*z));

        xp = 250 * Mathf.Sin(alpha);
        zp = 250 * Mathf.Cos(alpha);

        if (x < 0f)
            xp = xp * -1; 

        if (z < 0f)
            zp = zp * -1;

        gOrientation = new Vector3((xp - x), player.transform.position.y * -1, (zp - z)) * reverseGravity;

        gOrientation.Normalize();
        Debug.Log(gOrientation);
        //player.velocity = Vector3.zero;
        player.AddForce(gOrientation * 10f, ForceMode.Acceleration);
        alpha = Mathf.Acos((gOrientation.z * 1)/(Mathf.Sqrt(gOrientation.x * gOrientation.x + gOrientation.z * gOrientation.z)));
        alpha = alpha*180f/Mathf.PI;
        if (gOrientation.x < 0f)
        {
            alpha = 360 - alpha;
        }

        player.freezeRotation = false;
        player.rotation = Quaternion.LookRotation(gOrientation);
        //player.rotation = Quaternion.Euler(gOrientation.y * -90f - 90f, 0f, alpha - alpha*Mathf.Abs(gOrientation.y));
        //Quaternion targetRotation = Quaternion.LookRotation(player.transform.forward, -gOrientation.normalized);

        // Smoothly rotate the cylinder toward the target rotation
        //player.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        player.freezeRotation = true;
    }
}
