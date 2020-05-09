using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    
public void PlayGame()
    {
        
        SceneManager.LoadScene("_Scene_0");
        
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void RePlayGame()
    {
        Main.S.Reset();
        SceneManager.LoadScene("_Scene_0");        
    }

    public void RePlayLevel()
    {
        Main.heroLife = 3;
        SceneManager.LoadScene("_Scene_0");
    }

}