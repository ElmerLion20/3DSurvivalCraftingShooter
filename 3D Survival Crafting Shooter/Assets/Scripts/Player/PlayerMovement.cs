using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement Instance { get; private set; }

    Rigidbody rb;

    float velocity;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;


    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    private float groundDrag = 0.5f;

    public float jumpForce;
    private float jumpCooldown = 0.25f;
    private float airMultiplier = 0.4f;
    bool readyToJump;

    [Header("Crouch")]
    public float crouchSpeed;
    private float crouchYScale = 0.5f;
    private float startYScale;
    private bool crouching;



    [Header("Ground Check")]
    private float playerHeight = 1;
    public LayerMask whatIsGround;
    private bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;


    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;

        startYScale = transform.localScale.y;
        crouching = false;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = rb.velocity.magnitude;

        switch (state)
        {
            case MovementState.walking:

                break;
            case MovementState.sprinting:

                break;
            case MovementState.crouching:
                break;
            case MovementState.air:
                break;
        }
        //Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
        //Om står still
        if (rb.velocity.magnitude <= 0.05)
        {

        }
        PlayerInput();
        SpeedControl();
        StateHandler();

        if (grounded)
        {
            rb.drag = groundDrag;
            
        }
        else
            rb.drag = 0;

        //Crouch
        if (crouching)
        {
            moveSpeed = crouchSpeed;
        }

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void PlayerInput()
    {
        //Movement
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded && !crouching)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //Crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            crouching = true;
        }

        //stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            crouching = false;
        }
    }

    private void StateHandler()
    {
        //Crouching
        if (Input.GetKey(crouchKey) && !Input.GetKey(sprintKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;

        }

        //Sprinting
        if (grounded && Input.GetKey(sprintKey) && !crouching)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;

        }
        //Walking
        else if (grounded && !crouching)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;


        }
        //Air - ändras inte till crouching
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        //Walk direction looking;
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //Ground
        if (grounded)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        //Air
        else if (!grounded)
        {

            transform.position += moveDirection * moveSpeed * airMultiplier * Time.deltaTime;
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            //Om flatVel större än moveSpeed, vad är max speed, velocity = maxspeed
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void OnDrawGizmosSelected()
    {
        // Set the gizmo color to green.
        Gizmos.color = Color.green;

        // Draw a wire sphere to represent the detection radius.
        //Gizmos.DrawWireSphere(transform.position, noiseRadius);
    }

   private void OnParticleCollision(GameObject other)
   {
       
       PlayerMovement.Instance.transform.GetComponent<HealthSystem>().Damage(10);
       Debug.Log($"Hit by Ice {other.name}");
   }

}
