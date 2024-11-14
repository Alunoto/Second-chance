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
        LimitSpeed();

        /*        if (grounded)
                    player.drag = groundDrag;
                else
                    player.drag = 0;*/


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

/*        if (Input.GetKeyDown(reverseGravityKey))
        {
            float oldGravity = Physics.gravity.y;
            Physics.gravity = new Vector3(0, oldGravity * -1, 0);
        }*/
    }

    private void Move()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded)
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

/*        Debug.Log("punkty bazowe: "+ xp +", " + zp);
        Debug.Log("lokalizacja: "+ x + ", " + z);
        Debug.Log("k¹t: " + alpha);
        Debug.Log(player.velocity);*/

/*        if (x * x + z * z < 62500f)
            dirHelper = -1;
        else
            dirHelper = 1;*/
        gOrientation = new Vector3((xp-x), player.transform.position.y * -1, (zp -z));
        Debug.Log(gOrientation);
        //player.velocity = Vector3.zero;
        player.AddForce(gOrientation.normalized * 10f, ForceMode.Acceleration);
        player.freezeRotation = false;
        gOrientation.x = gOrientation.x * 0.5f;
        gOrientation.z = gOrientation.z * 0.5f;
        player.rotation = Quaternion.Euler(gOrientation*-1f);
        player.freezeRotation = true;
    }
}
