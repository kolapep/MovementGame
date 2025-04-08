using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerMovement;
using static ThirdPersonCam;

public class SwitchingManager : MonoBehaviour
{
    // SCRIPT: SWITCHES PLAYER MECHANICS AND UI

    [Header("Scripts")]
    public ThirdPersonCam RotationScript;
    public PlayerMovement playerMovementScript;

    private int playerMovementMechStage = 1;

    [Header("UI Objects")]
    public TextMeshProUGUI MovementStatusText;

    public float airTimeDelay;
    public GameObject Jumpmessage;

    private void Start()
    {
        IceStage();
        playerMovementMechStage = 1;
        Jumpmessage.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) playerMovementMechStage = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) playerMovementMechStage = 2;

    }

    private void FixedUpdate()
    {
        switch (playerMovementMechStage)
        {
            case 1:
                IceStage();
                break;
            case 2:
                WaterStage();
                break;
        }
    }
    IEnumerator AirStage()
    {
        Jumpmessage.SetActive(true);
        playerMovementScript.isGrounded = true;
        playerMovementScript.readyToJump = true;

        yield return new WaitForSeconds(airTimeDelay);

        Jumpmessage.SetActive(false);
        playerMovementMechStage = 2;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("IceStage"))
        {
            playerMovementMechStage = 1;
        }
        if (other.gameObject.CompareTag("WaterStage"))
        {
            playerMovementMechStage = 2;
        }
        if (other.gameObject.CompareTag("AirStage"))
        {
            AirStage();
        }
    }
    public void IceStage()
    {
        playerMovementScript.PlayerCanJump = true;
        MovementStatusText.text = "Rotate player with A and D, Press Space to Jump";
        playerMovementScript.SwitchPlayerState(playerStyle.Ice);
        ManualRotation();
    }
    public void WaterStage()
    {
        playerMovementScript.PlayerCanJump = false;
        MovementStatusText.text = "Rotate player with Mouse";
        playerMovementScript.SwitchPlayerState(playerStyle.Water);
        MouseControlDrive();
    }

    private void MouseControlDrive()
    {
        RotationScript.SwitchCameraStyle(CameraStyle.LockedCam);

        playerMovementScript.SwitchMovementStyle(MovementStyle.ForwardMove);

        RotationScript.Hops = false;

    }

    private void MouseControlFree()
    {
        RotationScript.SwitchCameraStyle(CameraStyle.LockedCam);

        playerMovementScript.SwitchMovementStyle(MovementStyle.BasicMove);

        RotationScript.Hops = false;

    }

    private void ManualRotation()
    {
        RotationScript.SwitchCameraStyle(CameraStyle.BasicCam);

        playerMovementScript.SwitchMovementStyle(MovementStyle.ForwardMove);

        RotationScript.Hops = false;

    }

    private void JumpToRotate()
    {
        RotationScript.SwitchCameraStyle(CameraStyle.BasicCam);

        playerMovementScript.SwitchMovementStyle(MovementStyle.ForwardMove);

        RotationScript.Hops = true;

    }






}
