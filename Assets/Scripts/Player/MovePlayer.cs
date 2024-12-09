using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    [Header("Objects")]
    public NewInputs inputs;
    private InputAction move;
    private InputAction reverse;
    private InputAction jump;
    Rigidbody player;

    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;


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
    public int reverseGravity = 1;
    public bool customGravity = true;

    public delegate void FunctionTriggeredHandler();
    public static event FunctionTriggeredHandler OnJumpTriggered, OnReverseTriggered;


    void Start()
    {
        player = GetComponent<Rigidbody>();
        player.constraints = RigidbodyConstraints.FreezeRotation;
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
        var actionMap = inputs.asset.actionMaps[0];
        actionMap.Disable();
        actionMap.Enable();
    }

    public void ChangeGravity()
    {
        if (customGravity)
        {
            customGravity = false;
            player.useGravity = true;
        }
        else
        {
            customGravity = true;
            player.useGravity = false;
        }
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
        if (customGravity)
            reverseGravity = reverseGravity * -1;
        else
            Physics.gravity = Physics.gravity * -1f;

        OnReverseTriggered?.Invoke();
    }

    private void Jump(InputAction.CallbackContext callbackContext)
    {
        if (readyToJump && grounded)
        {
            readyToJump = false;
            player.AddForce(gOrientation * -1f * jumpForce, ForceMode.Impulse);
            Invoke(nameof(ResetJump), jumpCooldown);
            OnJumpTriggered?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        Move();
        if (customGravity)
            ApplyGravity();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

    }

    private void Move()
    {
        moveDirection = body.transform.forward * verticalInput + body.transform.right * horizontalInput;

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
        player.velocity = Vector3.zero;
        player.angularVelocity = Vector3.zero;
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
            zp = 250 * Mathf.Cos(alpha) * -1;
        else
            zp = 250 * Mathf.Cos(alpha);

        gOrientation = new Vector3((xp - x), (player.transform.position.y - 1000f) * -1, (zp - z)) * reverseGravity;
        gOrientationFull = gOrientation;

        gOrientation.Normalize();
        //Debug.Log("x: " + x + ", xp: " + xp + ", z: " + z + ", zp: " + zp + ", gravity: " + gOrientation);

        player.AddForce(gOrientation * 10f, ForceMode.Acceleration);
    }
}
