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
    public GameObject tutCell;
    public RPGTalk rpgTalk;
    private bool companionPlayed = false;
    public GameObject finalCell;
    public GameObject checkpoint;
    public GameObject macrophage;
    public RPGTalk endTalk;
    public GameObject endMenu;
    private bool endPlayed = false;
    public void CancelControls()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<companionSpawn>().enabled = false;
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
    public void HideWasd()
    {
        wasd.gameObject.SetActive(false);
    }
    void LateUpdate()
    {
        if (!companionPlayed && tutCell.GetComponentInChildren<CellCapture>().capped)
        {
            CancelControls();
            rpgTalk.NewTalk("10", "11", rpgTalk.txtToParse);
            companionPlayed = true;


        }
        if (GameController.death)
        {
            
            macrophage.GetComponent<FieldOfView>().detected = false;
            macrophage.GetComponent<Patrol>().chaseStart = false;
            player.transform.position = checkpoint.transform.position;
            CancelControls();
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            rpgTalk.NewTalk("17", "17", rpgTalk.txtToParse);
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GameController.death = false;
        }
        if (!endPlayed && finalCell.GetComponentInChildren<CellCapture>().capped)
        {
            CancelControls();
            endMenu.SetActive(false);
            endTalk.NewTalk("19", "19", endTalk.txtToParse);
            endPlayed = true;
        }
        
    }
    public void activateEndMenu()
    {
        endMenu.SetActive(true);
    }
    
    

}
