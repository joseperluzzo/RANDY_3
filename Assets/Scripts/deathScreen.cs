using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deathScreen : MonoBehaviour
{
    public void respawn()
    {
        SceneManager.LoadScene("randyNivel1");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}