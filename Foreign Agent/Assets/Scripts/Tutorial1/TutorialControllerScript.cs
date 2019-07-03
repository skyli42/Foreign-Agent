using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
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
	private companionSpawn companionScript;

    public AudioSource victorySound;
	public Image grayScreen;

    void Start()
    {
        m_Animator = player.GetComponent<Animator>();
		companionScript = player.GetComponent<companionSpawn>();
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
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            GameController.Instance.death = false;
			//TODO: reset companion
			int tries = 0;
		
			Vector3 spawn = new Vector3(0, 0, 0);
			Debug.Log("Death");
			while (tries < 5000)
			{
				spawn = Random.insideUnitSphere * 2 + player.transform.position;

				Collider[] colliders = Physics.OverlapSphere(spawn, 0.5f);
				bool collisionFound = false;
				foreach (Collider col in colliders)
				{
					// If this collider is tagged "Obstacle"
					if (col.tag == "Obstacle" || col.tag == "companion" || col.tag == "Player")
					{
						collisionFound = true;
						tries += 1;
						break;
					}
				}
				if (!collisionFound)
				{
					GameObject[] allObjects = GameObject.FindGameObjectsWithTag("companion");
					foreach (GameObject obj in allObjects)
					{
						Destroy(obj);
						
					}
					companionScript.numCompanions = 0;
					companionScript.companionList.Clear();
					companionScript.companionToEnemy.Clear();
					
					NavMeshAgent agent = Instantiate(companionScript.companionVirus, spawn, Quaternion.identity).GetComponent<NavMeshAgent>();

					companionScript.companionToEnemy.Add(agent, player);
					agent.avoidancePriority = Random.Range(0, 101);
					agent.autoBraking = false;
					companionScript.companionList.Add(agent);
					agent.stoppingDistance = 2;
					companionScript.numCompanions = 1;
					companionScript.prevFrameNumCompanions = 1;
					Debug.Log("New Companion Spawned");
					break;
				}
			}
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
        pointer.transform.position = pointer.transform.position + new Vector3(0, 0, 8);

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

