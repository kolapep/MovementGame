using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static ThirdPersonCam;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // SCRIPT: MOVES PLAYER

    [Header("Scripts")]
    public ThirdPersonCam RotationScript;
    public SwitchingManager switchingManagerScript;

    [Header("Player Refrences")]
    //Player Information
    public Transform orientation;
    Rigidbody rb;
    public GameObject playerObjWater;
    public GameObject playerObjIce;

    //Action Input references
    float horizonalInput;
    float verticalInput;
    Vector3 moveDirection;
    InputAction moveAction;
    private Vector2 moveInput;
    InputAction jumpAction;

    public MovementStyle currentStyle;
    public playerStyle currentPlayerStyle;

    [Header("Movement")]
    public float moveSpeed; //MOVE SPEED
    public float maxSpeed; //SPEED LIMIT

    [Header("Jump")]
    public float jumpForce; //JUMP HEIGHT
    public float jumpCoolDown; //JUMP COOL DOWN
    public float airMultiplier; //AIR MOVE SPEED MULTIPLIER
    public bool readyToJump;

    [Header("Ground")]
    public LayerMask IsGround;
    public float playerHeight;
    public float GroundDrag; //DRAG OR PLAYER SLIP
    public bool isGrounded;

    [Header("Fall Damage")]
    private float fallTimer = 0;
    public int fallDamageLength; //FALL DAMAGE ADJUSTER
    public float deathScreenDelay;
    public GameObject deathScreen;
    public Transform respawnLocation;

    [Header("Character Animations")]

    public GameObject introText;
    public GameObject splashAudioClip;
    public ParticleSystem icePartic;
    public AudioSource backgroundMusic;

    public GameObject musicButtonOn;
    public GameObject musicButtonOff;
    public bool musicIsMuted;

    public bool PlayerCanJump;
    public bool solidBody;

    public enum MovementStyle
    {
        BasicMove,
        ForwardMove,
    }

    public enum playerStyle
    {
        Ice,
        Water
    }

    private void Start()
    {
        deathScreen.SetActive(false);
        introText.SetActive(false);

        //Locate and Set Action Refrences
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        //Locate and Set Player Refrences
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;

        musicIsMuted = false;
        musicButtonOff.SetActive(false);

        //Audio And Particles
        icePartic.Play();
        splashAudioClip.SetActive(false);

    }

    IEnumerator DeathScene()
    {
        musicIsMuted = true;
        deathScreen.SetActive(true);
        introText.SetActive(true);

        yield return new WaitForSeconds(deathScreenDelay);

        musicIsMuted = false;
        Respawn();
        deathScreen.SetActive(false);
        introText.SetActive(false);

    }

    public void Respawn()
    {
        transform.position = respawnLocation.position;
    }

    void Update()
    {
        MyInput();

        GroundCheck();

        SpeedControl();

        Partical();

        FallDamage();

        SwitchMusic();

        PlayerBody();

        if (PlayerCanJump == false)
        {
            readyToJump = false;
        }

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        if (deathScreen) //If player is not dead, player can move and jump
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
       
    }

    public void MusicButton()
    {
        if (musicIsMuted == true) //Turns Audio Off to On
        {
            musicIsMuted = false;
            musicButtonOff.SetActive(false);
            musicButtonOn.SetActive(true);
        }
        else //Turns Audio On to Off
        {
            musicIsMuted = true;
            musicButtonOn.SetActive(false);
            musicButtonOff.SetActive(true);
        }
    }

    public void SwitchMusic()
    {
        if (musicIsMuted == true){
            backgroundMusic.mute = true;
        }

        else if (musicIsMuted == false) {
            backgroundMusic.mute = false;
        }
    }
    public void PlayerBody()
    {
        if (currentPlayerStyle == playerStyle.Ice)
        {
            playerObjIce.SetActive(true);
            playerObjWater.SetActive(false);

        }
        if (currentPlayerStyle == playerStyle.Water)
        {
            playerObjWater.SetActive(true);
            playerObjIce.SetActive(false);
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
        if (currentStyle == MovementStyle.BasicMove)
        {
            //Move 
            moveDirection = orientation.forward * verticalInput + orientation.right * horizonalInput;
        }
        else if (currentStyle == MovementStyle.ForwardMove)
        {
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

    private void FallDamage()
    {
        FallTimer();

        Vector3 linearVelocity = rb.linearVelocity;
        float fallingspeed = linearVelocity.magnitude;

        if (isGrounded && fallingspeed > 20)
        {
            if(fallTimer > fallDamageLength * 0.1)
            {
                StartCoroutine(DeathScene());
            }

            fallTimer = 0;
        }
    }
    private void FallTimer()
    {
        if (!isGrounded) 
        {
            fallTimer += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BadEnd"))
        {
            SceneManager.LoadScene(2);

        }
        if (collision.gameObject.CompareTag("GoodEnd"))
        {
            SceneManager.LoadScene(3);

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
        currentStyle = newStyle;
    }

    public void SwitchPlayerState(playerStyle newStyle)
    {
        currentPlayerStyle = newStyle;
    }

}
