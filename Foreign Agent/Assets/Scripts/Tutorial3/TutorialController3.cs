﻿using System.Collections;
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
    private bool endPlayed = false;

    public void CancelControls()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<companionSpawn>().enabled = false;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    //give back the controls to player
    public void GiveBackControls()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<companionSpawn>().enabled = true;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }


    void LateUpdate()
    {
        if (GameController.Instance.death)
        {
            player.transform.position = checkpoint.transform.position;
            CancelControls();
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            deathTalk.NewTalk("deathStart", "deathEnd", deathTalk.txtToParse);
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GameController.Instance.death = false;
            Tcell.GetComponent<TcellPatrol>().points = largerPatrol;
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
        endMenu.SetActive(true);
        Time.timeScale = 0f;
    }



}