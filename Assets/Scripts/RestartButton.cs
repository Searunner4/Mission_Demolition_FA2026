using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public Button gameButton;
    public string sceneToOpen = "Scenes/_Scene_0";
    public void restartGame()
    {
        Debug.Log("You have clicked the button.");
        SceneManager.LoadScene(sceneToOpen);
    }
}
