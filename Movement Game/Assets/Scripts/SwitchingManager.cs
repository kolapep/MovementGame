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
    public TextMeshProUGUI HopsText;
    public TextMeshProUGUI CamStatusText;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1)) playerMovementMechStage = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) playerMovementMechStage = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) playerMovementMechStage = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) playerMovementMechStage = 4;

    }

    private void FixedUpdate()
    {
        switch (playerMovementMechStage)
        {
            case 1:
                MouseControlDrive();
                break;
            case 2:
                MouseControlFree();
                break;
            case 3:
                ManualRotation();
                break;
            case 4:
                JumpToRotate();
                break;
        }
    }

    private void MouseControlDrive()
    {
        RotationScript.SwitchCameraStyle(CameraStyle.LockedCam);
        CamStatusText.text = "Player Direction: Camera";

        playerMovementScript.SwitchMovementStyle(MovementStyle.ForwardMove);
        MovementStatusText.text = "Move Forward: W";

        RotationScript.Hops = false;
        HopsText.text = "Hops: False";

    }

    private void MouseControlFree()
    {
        RotationScript.SwitchCameraStyle(CameraStyle.LockedCam);
        CamStatusText.text = "Player Direction: Camera";

        playerMovementScript.SwitchMovementStyle(MovementStyle.BasicMove);
        MovementStatusText.text = "Move Forward: W A D";

        RotationScript.Hops = false;
        HopsText.text = "Hops: False";

    }

    private void ManualRotation()
    {
        RotationScript.SwitchCameraStyle(CameraStyle.BasicCam);
        CamStatusText.text = "Player Direction: Manual";

        playerMovementScript.SwitchMovementStyle(MovementStyle.ForwardMove);
        MovementStatusText.text = "Move Forward: W";

        RotationScript.Hops = false;
        HopsText.text = "Hops: False";

    }

    private void JumpToRotate()
    {
        RotationScript.SwitchCameraStyle(CameraStyle.BasicCam);
        CamStatusText.text = "Player Direction: Manual";

        playerMovementScript.SwitchMovementStyle(MovementStyle.ForwardMove);
        MovementStatusText.text = "Move Forward: W";

        RotationScript.Hops = true;
        HopsText.text = "Hops: True";

    }






}
