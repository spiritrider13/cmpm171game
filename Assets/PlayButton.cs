using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void PlayGame()
    {
        if ((SceneManager.GetActiveScene().buildIndex == 2) || (SceneManager.GetActiveScene().buildIndex == 3)){
            SceneManager.LoadScene(1);
        }
        else{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
