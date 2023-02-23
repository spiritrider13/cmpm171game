using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject Pause;
    public void PanelOpener()
    {
        if(Pause != null)
        {
            bool isActive = Pause.activeSelf;
            Pause.SetActive(!isActive);
            Debug.Log("uhoh");
        }
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
