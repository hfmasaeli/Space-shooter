using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadSinglePlayerGame()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadCoopMode()
    {
        SceneManager.LoadScene(2);
    }
}
