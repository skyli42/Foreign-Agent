using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Pause : MonoBehaviour
{

    public void Display()
    {
        gameObject.SetActive(true);
    }

    public void ReturnToGame()
    {
        gameObject.SetActive(false);
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
