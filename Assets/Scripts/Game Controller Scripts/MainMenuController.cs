using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame() {
        SceneManager.LoadScene("2_Gameplay");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
