using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void enterGame()
    {
        SceneManager.LoadScene("OutdoorsScene");
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
