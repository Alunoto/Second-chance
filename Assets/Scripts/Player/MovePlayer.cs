using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    public NewInputs inputs;
    private InputAction move;
    private InputAction reverse;
    private InputAction jump;

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
    public float x, y, z, alpha, xp, zp;
    public Vector3 gOrientation, gOrientationFull;
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

    private void Awake()
    {
        inputs = FindObjectOfType<SettingsMenu>().inputActions;
        if (inputs == null)
        {
            inputs = new NewInputs();
        }
        Debug.Log(inputs);
    }
    public void RefreshBindings()
    {
        // Disable and re-enable the action map to refresh the bindings
        var actionMap = inputs.asset.actionMaps[0]; // Assuming one action map
        actionMap.Disable();
        actionMap.Enable();
    }

    private void OnEnable()
    {
        move = inputs.player.move;
        move.Enable();

        reverse = inputs.player.reverse;
        reverse.Enable();
        reverse.performed += Reverse;

        jump = inputs.player.jump;
        jump.Enable();
        jump.performed += Jump;
    }

    private void OnDisable()
    {
        move.Disable();
        reverse.Disable();
        jump.Disable();
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

    private void Reverse(InputAction.CallbackContext callbackContext)
    {
        reverseGravity = reverseGravity * -1;
    }

    private void Jump(InputAction.CallbackContext callbackContext)
    {
        if (readyToJump && grounded)
        {
            readyToJump = false;
            player.AddForce(gOrientation * -1f * jumpForce, ForceMode.Impulse);
            Invoke(nameof(ResetJump), jumpCooldown);
        }
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

    }

    private void Move()
    {
        moveDirection = body.transform.forward * verticalInput + body.transform.right * horizontalInput;//orientation.right * horizontalInput;

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

        if (x < 0f)
            xp = 250 * Mathf.Sin(alpha) * -1;
        else
            xp = 250 * Mathf.Sin(alpha);

        if (z < 0f)
            zp = 250 * Mathf.Cos(alpha) -1;
        else
            zp = 250 * Mathf.Cos(alpha);

        gOrientation = new Vector3((xp - x), player.transform.position.y * -1, (zp - z)) * reverseGravity;
        gOrientationFull = gOrientation;

        gOrientation.Normalize();

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
