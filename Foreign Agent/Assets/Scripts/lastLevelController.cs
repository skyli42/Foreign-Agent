using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class lastLevelController : MonoBehaviour
{
    public GameObject player;
    public RPGTalk introTalk;
    private Animator m_Animator;
    public RPGTalk endTalk;
    public GameObject endMenu;
    private bool endPlayed = false;
    public AudioSource victorySound;
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
    void LateUpdate()
    {
        if (!endPlayed && GameController.Instance.numCaptures == GameController.Instance.numCellsInLevel)
        {
            CancelControls();
            endMenu.SetActive(false);
            Time.timeScale = 1.0f;
            endTalk.NewTalk("endLevelStart", "endLevelEnd", endTalk.txtToParse);
            endPlayed = true;
        }
    }
    public void activateEndMenu()
    {
        victorySound.Play();
        endMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void activateCells()
    {
        GameController.Instance.secondInfection = true;
    }
}



