using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class memoryCellIntroController : MonoBehaviour
{
    public GameObject player;
    public RPGTalk introTalk;
    private Animator m_Animator;

    public Image grayScreen;
    void Start()
    {
        m_Animator = player.GetComponent<Animator>();
        grayScreen.enabled = true;
    }
    public void CancelControls()
    {
        player.GetComponent<PlayerMovement>().dashStart = false;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<companionSpawn>().enabled = false;

        m_Animator.SetBool("IsWalking", false);
        m_Animator.SetBool("IsRunning", false);
        grayScreen.enabled = true;
    }

    //give back the controls to player
    public void GiveBackControls()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<companionSpawn>().enabled = true;
        grayScreen.enabled = false;
    }
    public void activateCells()
    {
        GameController.Instance.secondInfection = true;
    }
}



