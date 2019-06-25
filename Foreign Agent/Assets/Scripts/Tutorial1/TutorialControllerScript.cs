using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialControllerScript : MonoBehaviour
{

    public GameObject wall;
    public GameObject player;
    public Image wasd;
    public Image shift;
    public GameObject tutCell;
    public RPGTalk rpgTalk;
    private bool companionPlayed = false;
    public GameObject finalCell;
    public GameObject checkpoint;
    public GameObject macrophage;
    public RPGTalk endTalk;
    public GameObject endMenu;
    private bool endPlayed = false;
    public GameObject pointer;
    public Image UIpointer;
    public RPGTalk UITalk;
    public RPGTalk deathTalk;
    private Animator m_Animator;

    void Start()
    {
        m_Animator = player.GetComponent<Animator>();
       
    }
    public void CancelControls()
    {
        player.GetComponent<PlayerMovement>().dashStart = false;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<companionSpawn>().enabled = false;
        m_Animator.SetBool("IsWalking", false);
        m_Animator.SetBool("IsRunning", false);
        
    }

    //give back the controls to player
    public void GiveBackControls()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<companionSpawn>().enabled = true;
    }

    public void DestroyWall()
    {
        wall.SetActive(false);
    }

    public void DisplayWasd()
    {
        wasd.gameObject.SetActive(true);
    }
    public void DisplayShift()
    {
        shift.gameObject.SetActive(true);
    }
    public void HideShift()
    {
        shift.gameObject.SetActive(false);
    }
    public void HideWasd()
    {
        wasd.gameObject.SetActive(false);
    }
    void LateUpdate()
    {
        if (!companionPlayed && tutCell.GetComponentInChildren<CellCapture>().capped)
        {
            CancelControls();
            rpgTalk.NewTalk("infectionSuccessStart", "infectionSuccessEnd", rpgTalk.txtToParse);
            companionPlayed = true;


        }
        if (GameController.Instance.death)
        {
            
            macrophage.GetComponent<FieldOfView>().detected = false;
            macrophage.GetComponent<Patrol>().chaseStart = false;
            player.transform.position = checkpoint.transform.position;
            CancelControls();
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            deathTalk.NewTalk("deathStart", "deathEnd", deathTalk.txtToParse);
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GameController.Instance.death = false;
        }
        if (!endPlayed && finalCell.GetComponentInChildren<CellCapture>().capped)
        {
            CancelControls();
            endMenu.SetActive(false);
            Time.timeScale = 1.0f;
            endTalk.NewTalk("endLevelStart", "endLevelEnd", endTalk.txtToParse);
            endPlayed = true;
        }
        if (UIpointer.gameObject.activeSelf)
        {
            Vector3 pos = UIpointer.transform.position;
         
            //calculate what the new Y position will be
            float newY = Mathf.Sin(Time.time * 4) + pos.y;
            //set the object's Y to the new calculated Y
            UIpointer.transform.position = new Vector3(pos.x, newY, pos.z);
        }
    }
    public void activateEndMenu()
    {
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
        pointer.transform.position = pointer.transform.position +  new Vector3 (0, 0, 8);

    }
    public void hidePointer1()
    {
        deactivatepointer(pointer);
    }
    public void UIemphasis()
    {
        UIpointer.gameObject.SetActive(true);
    }
    public void HideUIemphasis()
    {
        UIpointer.gameObject.SetActive(false);
    }
    public void playUITalk()
    {
        CancelControls();
        UIemphasis();
        UITalk.NewTalk("UIemphasisStart", "UIemphasisEnd", UITalk.txtToParse);
    }
    


}
