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
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                MenuUIFrame.SetActive(!MenuUIFrame.activeInHierarchy);

                if (Cursor.visible == false){
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
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
