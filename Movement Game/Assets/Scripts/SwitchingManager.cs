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

    }

    //private void FixedUpdate()
    //{
    //    switch (moveType)
    //    {
    //        case 1:
    //            MoveWithBySettingPosition();
    //            break;
    //        case 2:
    //            MoveWithMovePosition();
    //            break;
    //        case 3:
    //            MoveWithAddForce();
    //            break;
    //        case 4:
    //            MoveWithAddTorque();
    //            break;
    //    }
    //}

    private void GameSettingsLock()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) gameMenu.SetActive(!gameMenu.activeInHierarchy);
        if (Input.GetKeyDown(KeyCode.Q)) SceneManager.LoadScene(0);
    }

    private void SwitchCameraLock()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) RotationScript.SwitchCameraStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Alpha2)) RotationScript.SwitchCameraStyle(CameraStyle.Manual);

    }

    private void SwitchMovementLock()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3)) playerMovementScript.SwitchMovementStyle(MovementStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Alpha4)) playerMovementScript.SwitchMovementStyle(MovementStyle.noRotation);
    }

    private void SwitchRotationLock()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5)) RotationScript.Hops = RotationScript.Hops ? false : true;

    }





}
