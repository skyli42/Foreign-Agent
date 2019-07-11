using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialController3 : MonoBehaviour
{
    public GameObject player;
    public GameObject tutCell;
    public RPGTalk deathTalk;
   
    public GameObject checkpoint;
    public GameObject Tcell;
    public Transform[] largerPatrol;
    public RPGTalk endTalk;
    public GameObject endMenu;
    private Animator m_Animator;
    private bool endPlayed = false;
    public AudioSource victorySound;
	public Image grayScreen;
    private bool diedAlready = false;
    void Start()
    {
		grayScreen.enabled = true;
		m_Animator = player.GetComponent<Animator>();
    }
    public void CancelControls()
    {
        player.GetComponent<PlayerMovement>().dashStart = false;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<companionSpawn>().enabled = false;
        m_Animator.SetBool("IsWalking", false);
        m_Animator.SetBool("IsRunning", false);
		//  player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		grayScreen.enabled = true;

	}

	//give back the controls to player
	public void GiveBackControls()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<companionSpawn>().enabled = true;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
		grayScreen.enabled = false;

	}


	void LateUpdate()
    {
        if (GameController.Instance.death)
        {
            player.transform.position = checkpoint.transform.position;
            CancelControls();
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            deathTalk.NewTalk("deathStart", "deathEnd", deathTalk.txtToParse);
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            GameController.Instance.death = false;
            if (!diedAlready)
            {
                Tcell.GetComponent<TcellPatrol>().points = largerPatrol;
                Tcell.GetComponent<TcellPatrol>().destPoint = 0;
            }
            diedAlready = true;
        }

        if (!endPlayed && tutCell.GetComponentInChildren<CellCapture>().capped)
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



}
