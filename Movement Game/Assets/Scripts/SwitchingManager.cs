using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerMovement;
using static ThirdPersonCam;

public class SwitchingManager : MonoBehaviour
{
    public ThirdPersonCam RotationScript;
    public PlayerMovement playerMovementScript;

    public GameObject gameMenu;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SwitchCameraLock();
        SwitchMovementLock();
        SwitchRotationLock();
        GameSettingsLock();

        if (Input.GetKeyDown(KeyCode.Alpha1)) moveType = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) moveType = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) moveType = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) moveType = 4;

    }

    private void FixedUpdate()
    {
        switch (moveType)
        {
            case 1:
                MoveWithBySettingPosition();
                break;
            case 2:
                MoveWithMovePosition();
                break;
            case 3:
                MoveWithAddForce();
                break;
            case 4:
                MoveWithAddTorque();
                break;
        }
    }

    private void GameSettingsLock()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) gameMenu.SetActive(!gameMenu.activeInHierarchy);
        if (Input.GetKeyDown(KeyCode.Q)) SceneManager.LoadScene(0);
    }

    private void SwitchCameraLock()
    {
        RotationScript.SwitchCameraStyle(CameraStyle.LockedCam);
        RotationScript.SwitchCameraStyle(CameraStyle.BasicCam);
    }

    private void SwitchMovementLock()
    {
        playerMovementScript.SwitchMovementStyle(MovementStyle.BasicMove);
        playerMovementScript.SwitchMovementStyle(MovementStyle.ForwardMove);
    }

    private void SwitchRotationLock()
    {
        RotationScript.Hops = RotationScript.Hops ? false : true;

    }





}
