using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using static SwitchingManager;


public class ThirdPersonCam : MonoBehaviour
{
    // SCRIPT: ROTATES CAMERA AND PLAYER
    //Script bases from Dave / GameDevelopment. Controls the third person camera movement in relationship to the player object and inputs.

    [Header("Scripts")]
    public PlayerMovement playerMovementScript;
    public SwitchingManager switchingManagerScript;

    [Header("Player Refrences")]
    //Player Information
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    //Action Input references
    public float rotationSpeed;
    InputAction moveAction;
    private Vector2 moveInput;

    [Header("Camera Refrences")]
    public GameObject BasicCam;
    public CameraStyle currentStyle;
    public bool Hops;

    public enum CameraStyle
    {
        LockedCam,
        BasicCam,
      
    }

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");

        Hops = false;

    }

    public void Update()
    {
        PlayerRotation();

    }
    private void PlayerRotation()
    {
        // If Player is Grounded or If Locked Hop Rotation is False, Rotate the Player / Cam
        if (!Hops || playerMovementScript.isGrounded == false)
        {
            //Smooth Rotation of player object (Visual Rotation)
            moveInput = moveAction.ReadValue<Vector2>(); 
            float horizontalInput = moveInput.x; 
            float verticleInput = moveInput.y; //Grabs the input values 

            Vector3 inputDir = orientation.forward * verticleInput + orientation.right * horizontalInput; //Sets the input direction to the origional orientation plus the player input
            if (inputDir != Vector3.zero) //If there is player input, change forward direction of player object to the input direction 
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

            CamRotation();

        }
    }
    private void CamRotation()
    {
        //Links orientation front to cam direction
        if (currentStyle == CameraStyle.LockedCam)
        {
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z); //calculates where cam direction is based on the camera's x/z axis to the player object's
            orientation.forward = viewDir.normalized;
        }

        //Unlinks orientation front to cam direction
        else if (currentStyle == CameraStyle.BasicCam)
        {
            orientation.forward = playerObj.forward;
        }
    }
    public void SwitchCameraStyle(CameraStyle newStyle)
    {
        currentStyle = newStyle;
    }

}
