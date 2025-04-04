using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class ThirdPersonCam : MonoBehaviour
{
    //Script bases from Dave / GameDevelopment. Controls the third person camera movement in relationship to the player object and inputs.

    public PlayerMovement playerMovementScript;

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

    public CameraStyle currentStyle;
    public GameObject BasicCam;
    public TextMeshProUGUI CamStatusText;

    //Rotation Depending on Grounding
    public bool Hops;
    public TextMeshProUGUI HopsText;


    public enum CameraStyle
    {
        Basic,
        Manual,
        noRotation
    }

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");

        //Locks Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Text UI
        CamStatusText.text = "Player Direction: Follows Camera";
        Hops = false;
        HopsText.text = "Hops: False";

    }

    public void Update()
    {
        SwitchCameraVeiw();
        TextUI();
        PlayerRotation();

    }

    private void SwitchCameraVeiw()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyle(CameraStyle.Manual);
        if (Input.GetKeyDown(KeyCode.Alpha5)) Hops = Hops ? false : true;
    }
    private void TextUI()
    {
        if (Hops == true)
        {
            HopsText.text = "Hops: True";
        }
        else
        {
            HopsText.text = "Hops: False";
        }

        if (currentStyle == CameraStyle.Basic)
        {
            if (Hops == true)
            {
                CamStatusText.text = "Player Direction: Manual Only";
            }
            else
            {
                CamStatusText.text = "Player Direction: Follows Camera";
            }
        }
        else if (currentStyle == CameraStyle.Manual)
        {
            if (Hops == true)
            {
                CamStatusText.text = "Player Direction: Manual Only";
            }
            else
            {
                CamStatusText.text = "Player Direction: Manual Rotation";
            }
        }
    }
    private void PlayerRotation()
    {
        if (!Hops || playerMovementScript.isGrounded == false)
        {

            //Smooth Rotation of player object
            moveInput = moveAction.ReadValue<Vector2>(); //Grabs the input values
            float horizontalInput = moveInput.x; ;
            float verticleInput = moveInput.y;
            Vector3 inputDir = orientation.forward * verticleInput + orientation.right * horizontalInput; //Sets the input direction to the origional orientation plus the player input

            if (inputDir != Vector3.zero) //If there is player input, change forward direction of player object to the input direction
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

            if (currentStyle == CameraStyle.Basic)
            {
                //Sets the rotation orientation
                Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z); //calculates where cam direction is based on the camera's x/z axis to the player object's
                orientation.forward = viewDir.normalized; //Sets orientation front to cam direction

            }
            else if (currentStyle == CameraStyle.Manual)
            {
                orientation.forward = playerObj.forward;
            }
        }
    }
    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        currentStyle = newStyle;
    }

}
