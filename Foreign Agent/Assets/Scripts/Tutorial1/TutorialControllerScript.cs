using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialControllerScript : MonoBehaviour
{

    public GameObject wall;
    public GameObject player;
    public Image wasd;
    public GameObject tutCell;
    public RPGTalk rpgTalk;
    private bool companionPlayed = false;

   
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
    void Update()
    {
        if (!companionPlayed && tutCell.GetComponentInChildren<CellCapture>().capped)
        {
            CancelControls();
            rpgTalk.NewTalk("10", "11", rpgTalk.txtToParse);
            companionPlayed = true;
  

        }
    }

}
