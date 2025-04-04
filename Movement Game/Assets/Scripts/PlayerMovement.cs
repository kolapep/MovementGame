using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static ThirdPersonCam;

public class PlayerMovement : MonoBehaviour
{
    //Player Controller

    public ThirdPersonCam RotationScript;
    public SwitchingManager switchingManagerScript;


    [Header("Player Refrences")]
    //Player Information
    public Transform orientation;
    Rigidbody rb;

    //Action Input references
    float horizonalInput;
    float verticalInput;
    Vector3 moveDirection;
    InputAction moveAction;
    private Vector2 moveInput;
    InputAction jumpAction;

    public MovementStyle currentStyle;

    [Header("Movement")]
    public float moveSpeed; //MOVE SPEED
    public float maxSpeed; //SPEED LIMIT

    [Header("Jump")]
    public float jumpForce; //JUMP HEIGHT
    public float jumpCoolDown; //JUMP COOL DOWN
    public float airMultiplier; //AIR MOVE SPEED MULTIPLIER
    public bool readyToJump;

    [Header("Ground Check")]
    public LayerMask IsGround;
    public float playerHeight;
    public float GroundDrag; //DRAG OR PLAYER SLIP
    public bool isGrounded;

    [Header("UI and Objects")]
    public TextMeshProUGUI MovementStatusText;

    public GameObject splashAudioClip;
    public ParticleSystem icePartic;


    public enum MovementStyle
    {
        Basic,
        noRotation
    }

    private void Start()
    {
        //Locate and Set Action Refrences
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        //Locate and Set Player Refrences
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;

        //Text UI
        MovementStatusText.text = "Move Forward: W A D";

        //Audio And Particles
        icePartic.Play();
        splashAudioClip.SetActive(false);

    }

    void Update()
    {
        MyInput();

        GroundCheck();

        SpeedControl();

        Partical();

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {

        //Move Player
        moveInput = moveAction.ReadValue<Vector2>(); //Grabs the move acion value
        horizonalInput = moveInput.x;
        verticalInput = moveInput.y;

        //Jump Player
        if (readyToJump && isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            readyToJump = false;
            Jump();
            ResetJump();

        }
    }

    private void Partical()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            icePartic.Play();
        }
 
    }

    private void MovePlayer()
    {
        if (currentStyle == MovementStyle.Basic){
            //Move 
            moveDirection = orientation.forward * verticalInput + orientation.right * horizonalInput;
        }
        else if (currentStyle == MovementStyle.noRotation){
            //Calculate and Move in the direction the player is looking
            moveDirection = orientation.forward * verticalInput;
        }

        //Move
        if (isGrounded){
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);  //Apply Movement
        }
        else if (!isGrounded) 
        { 
            if (RotationScript.Hops == true)
            {
                if (!isGrounded && !Input.GetKeyDown(KeyCode.W) || !Input.GetKeyDown(KeyCode.UpArrow))
                {
                    rb.AddForce(moveDirection.normalized * moveSpeed * 0f * airMultiplier, ForceMode.Force);  //Apply No Movement in Air
                }
            }
            else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);  //Apply Movement + Air speed
            }
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        //Limits players velocity to a max value
        if (flatVel.magnitude > moveSpeed + 1)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            
        }
    }

    public void GroundCheck()
    {
        //Ground Check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.05f + 0.5f, IsGround);

        //Applies if player is isGrounded
        if (isGrounded)
        {
            rb.linearDamping = GroundDrag/2; //Player drag
            splashAudioClip.SetActive(true); //Splash Audio
        }
        else
        {
            rb.linearDamping = 0;
            splashAudioClip.SetActive(false);
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); //Set y Velocity to Zero
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); //Apply Jump
        Debug.Log("Jump!");
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    public void SwitchMovementStyle(MovementStyle newStyle)
    {

        if (newStyle == MovementStyle.Basic) MovementStatusText.text = "Move Forward: W A D";
        if (newStyle == MovementStyle.noRotation) MovementStatusText.text = "Move Forward: W";

        currentStyle = newStyle;

    }

}
