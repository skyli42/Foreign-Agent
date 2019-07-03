using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialController2 : MonoBehaviour
{

    public GameObject wall;
    public GameObject player;
    public GameObject tutBCell;
    public RPGTalk plasmaSpawnTalk;
    public GameObject tutCell;
    public RPGTalk deathTalk;
    public RPGTalk companionTalk;
    public RPGTalk THelperTalk;
    private bool ThelpPlayed = false;
    private bool plasmaSpawnPlayed = false;
    private bool companionTalkPlayed = false;
    public GameObject finalCell;
    public GameObject checkpoint;
    public GameObject macrophage;
    public RPGTalk endTalk;
    public GameObject endMenu;
    private bool endPlayed = false;
    public GameObject pointer;
    public GameObject wallFirst;
    public GameObject wallThird;
    private Animator m_Animator;
    public Image spacebar;
	public Image grayScreen;
    public AudioSource victorySound;

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
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
		grayScreen.enabled = false;

	}

	public void DestroyWall()
    {
        wall.SetActive(false);
    }
    public void DestroyWallFirst()
    {
        wallFirst.SetActive(false);
    }
    public void DestroyWallThird()
    {
        wallThird.SetActive(false);
    }

    void LateUpdate()
    {
        if (!plasmaSpawnPlayed && tutBCell.GetComponent<plasmaSpawn>().Bcellcollision)
        {
            CancelControls();
            hidePointer1();
            plasmaSpawnTalk.NewTalk("plasmaSpawnStart", "plasmaSpawnEnd", plasmaSpawnTalk.txtToParse);
            plasmaSpawnPlayed = true;
        }
        if (!companionTalkPlayed && tutCell.GetComponentInChildren<CellCapture>().capped)
        {
            CancelControls();
            hidePointer1();
            companionTalk.NewTalk("companionStart", "companionEnd", companionTalk.txtToParse);
            companionTalkPlayed = true;
        }
        if (GameController.Instance.death)
        {

            macrophage.GetComponent<FieldOfView>().detected = false;
            macrophage.GetComponent<Patrol>().chaseStart = false;
            player.transform.position = checkpoint.transform.position;
            CancelControls();
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            deathTalk.NewTalk("deathStart", "deathEnd", deathTalk.txtToParse);
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            GameController.Instance.death = false;
        }
        if (plasmaSpawn.Instance.activated && !ThelpPlayed)
        {
            CancelControls();
            hideSpace();
            THelperTalk.NewTalk("THelperStart", "THelperEnd", THelperTalk.txtToParse);
            ThelpPlayed = true;
        }
        if (!endPlayed && finalCell.GetComponentInChildren<CellCapture>().capped)
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
   
    public void flashPointer()
    {
        if (pointer.activeSelf)
            pointer.SetActive(false);
        else
            pointer.SetActive(true);
    }
    public void displayPointer1()
    {
        InvokeRepeating("flashPointer", 0.0f, 0.75f);
    }
    public void deactivatepointer(GameObject pointer)
    {
        CancelInvoke();
        pointer.SetActive(false);
        pointer.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        pointer.transform.position =  new Vector3(1.88f, 1f, -29f);

    }
    public void hidePointer1()
    {
        deactivatepointer(pointer);
    }
    public void displaySpace()
    {
        spacebar.gameObject.SetActive(true);
    }
    public void hideSpace()
    {
        spacebar.gameObject.SetActive(false);
    }

}
