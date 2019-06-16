using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject menu;
    private bool paused;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            paused = togglePause();
            if (paused)
                menu.GetComponent<Pause>().Display();
            else
                menu.GetComponent<Pause>().ReturnToGame();
        }
    }
    
    public bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            return (true);
        }
    }
}
