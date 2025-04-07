using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIScriptManager : MonoBehaviour
{
    [Header("UI Assets")]
    public GameObject MenuUIFrame;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            MenuUIFrame.SetActive(false);
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PointsData.playerScore = 0;

        }
        else
        {
            MenuUIFrame.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MenuUIFrame.SetActive(!MenuUIFrame.activeInHierarchy);

                if (MenuUIFrame == true){
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else if (MenuUIFrame == false)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
       
    }

    public void LoadIntro()
    {
        SceneManager.LoadScene(0);
    }
    public void Gameplay()
    {
        SceneManager.LoadScene(1);
    }
   

}
