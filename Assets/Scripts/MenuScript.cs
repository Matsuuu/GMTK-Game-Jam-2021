using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GoToCredits()
    {
        ChangeScene("CreditsScene");
    }

    public void GoToGameScene()
    {
        ChangeScene("GameScene");
    }

    public void GoToMenuScene()
    {
        ChangeScene("MenuScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
