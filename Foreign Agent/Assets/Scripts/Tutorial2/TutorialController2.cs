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

    public void DestroyWall()
    {
        wall.SetActive(false);
    }

    void LateUpdate()
    {
        if (!plasmaSpawnPlayed && tutBCell.GetComponent<plasmaSpawn>().Bcellcollision)
        {
            CancelControls();
            plasmaSpawnTalk.NewTalk("plasmaSpawnStart", "plasmaSpawnEnd", plasmaSpawnTalk.txtToParse);
            plasmaSpawnPlayed = true;
        }
        if (!companionTalkPlayed && tutCell.GetComponentInChildren<CellCapture>().capped)
        {
            CancelControls();
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
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GameController.Instance.death = false;
        }
        if (plasmaSpawn.Instance.activated && !ThelpPlayed)
        {
            CancelControls();
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
        endMenu.SetActive(true);
        Time.timeScale = 0f;
    }



}
