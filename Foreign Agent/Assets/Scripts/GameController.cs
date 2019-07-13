using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Cinemachine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public int numCaptures;
    public int numCellsInLevel;
    public GameObject cellsLeftUI;
    private int prevFramenumCaptures;

    Rigidbody m_Rigidbody;
    Quaternion m_Rotation = Quaternion.identity;

    public bool secondInfection;
    public GameObject stage;
    private SimpleSonarShader_Parent parent;

    public GameObject pauseMenu;
    private bool paused;
    public GameObject player;

    public bool death;
    public Text TargetDestroyedDeath;
    public Text TimeSpentDeath;
    public GameObject deathMenu;
    public GameObject deathAnim;
    private bool alreadyDead = false;
    public AudioSource deathAudio;
    public Text TargetDestroyed;
    public Text TimeSpent;
    private bool atEnd = false;
    public GameObject endMenu;
    private UnityEngine.EventSystems.EventSystem myEventSystem;

    public GameObject restartLevelDeathButton;
    public GameObject nextLevelEndButton;
    public GameObject returnToGameButton;

    public GameObject portalAnim;
    Animator m_Animator;
    public bool isTutorial = false;
    public bool isIntro = false;
    private bool memoryActivated = false;

    public AudioSource victorySound;

    private CinemachineVirtualCamera vcam;
    
    void Start()
    {
        vcam = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        m_Animator = player.GetComponent<Animator>();
        parent = stage.GetComponent<SimpleSonarShader_Parent>();
        Instance = this;
        death = false;
        numCaptures = 0;
        prevFramenumCaptures = numCaptures;
        cellsLeftUI.GetComponent<TextMeshProUGUI>().text = "Cells Left: " + (numCellsInLevel - numCaptures).ToString();
        myEventSystem = GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>();
      
    }
    // Update is called once per frame
    void Update()
    {
        if (Instance.secondInfection && !memoryActivated)
        {
            plasmaSpawn.Instance.activated = true;//probably temp until T helper are implemented
            if (parent)
            {
                Debug.Log("sonar");
                parent.StartSonarRing(stage.transform.position, 5);
                memoryActivated = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !death)
        {
            paused = togglePause();
            if (paused)
            {
                myEventSystem.SetSelectedGameObject(returnToGameButton);
                returnToGameButton.GetComponent<Button>().OnSelect(null);
                pauseMenu.GetComponent<Pause>().Display();
            }
            else
                pauseMenu.GetComponent<Pause>().ReturnToGame();
        }
        if (death && !alreadyDead)
        {
            myEventSystem.SetSelectedGameObject(restartLevelDeathButton);
            restartLevelDeathButton.GetComponent<Button>().OnSelect(null);
            alreadyDead = true;
            deathAudio.Play();
            StartCoroutine(waitTillDeathDone());

        }
        else if (!death && alreadyDead)
            alreadyDead = false;

        if (prevFramenumCaptures != numCaptures)
        {
            cellsLeftUI.GetComponent<TextMeshProUGUI>().text = "Cells Left: " + (numCellsInLevel - numCaptures).ToString();
        }

        if (numCaptures == numCellsInLevel && !atEnd)
        {
            TargetDestroyed.text = numCaptures.ToString() + " / " + numCellsInLevel.ToString();
            float timeLeft = Time.timeSinceLevelLoad;
            int min = Mathf.FloorToInt(timeLeft / 60);
            int sec = Mathf.FloorToInt(timeLeft % 60);
            TimeSpent.text = min.ToString("00") + ":" + sec.ToString("00");

            atEnd = true;
            myEventSystem.SetSelectedGameObject(nextLevelEndButton);
            nextLevelEndButton.GetComponent<Button>().OnSelect(null);

            bool validSpawn = false;
            int tries = 0;

            Vector3 spawn = new Vector3(0, 0, 0);
            while (!validSpawn && tries < 15000)
            {

                spawn = Random.insideUnitSphere * 3.25f + player.transform.position;
                if (spawn.y < 1.5f || spawn.y > 2f)
                {
                    tries++;
                }
                else
                {
                    if (GroundCheck(spawn))
                    {
                        Collider[] colliders = Physics.OverlapSphere(spawn, 0.75f);
                        bool collisionFound = false;
                        foreach (Collider col in colliders)
                        {
                            // If this collider is tagged "Obstacle"
                            if (col.tag == "Obstacle" || col.tag == "HumanCell")
                            {
                                // Then this position is not a valid spawn position
                                validSpawn = false;
                                collisionFound = true;
                                tries += 1;
                                break;
                            }
                        }
                        if (!collisionFound)
                        {
                            validSpawn = true;
                        }
                    }
                }
            }
            if (validSpawn)
            {
                GameObject portal = Instantiate(portalAnim, spawn, portalAnim.transform.rotation);
                StartCoroutine(movePlayerToPortal(portal));
                // player.transform.position = Vector3.MoveTowards(player.transform.position, spawn, Time.deltaTime);
            }

        }
        prevFramenumCaptures = numCaptures;
    }
    private bool GroundCheck(Vector3 spawn)
    {
        RaycastHit hit;
        float distance = 2.5f;
        Vector3 dir = new Vector3(0, -1);

        return (Physics.Raycast(spawn, dir, out hit, distance));
    }

    public bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            return (true);
        }
    }
    public IEnumerator waitTillDeathDone()
    {
        Instantiate(deathAnim, player.GetComponent<Collider>().bounds.center, Quaternion.identity);
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        player.gameObject.GetComponentInChildren<Renderer>().enabled = false;
        if (!isTutorial || isIntro)
        {
            yield return new WaitForSeconds(1.2f);
            TargetDestroyedDeath.text = numCaptures.ToString() + " / " + numCellsInLevel.ToString();
            float timeLeft = Time.timeSinceLevelLoad;
            int min = Mathf.FloorToInt(timeLeft / 60);
            int sec = Mathf.FloorToInt(timeLeft % 60);
            TimeSpentDeath.text = min.ToString("00") + ":" + sec.ToString("00");
            deathMenu.SetActive(true);
        }
        else
        {
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            player.gameObject.GetComponentInChildren<Renderer>().enabled = true;
        }

    }

    public IEnumerator movePlayerToPortal(GameObject portal)
    {
        player.GetComponent<Collider>().enabled = false;
        //player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<companionSpawn>().enabled = false;
       // player.GetComponent<Collider>().enabled = false;
        m_Rigidbody = player.GetComponent<Rigidbody>();
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        m_Rigidbody.useGravity = false;

        m_Animator.SetBool("IsWalking", false);
        m_Animator.SetBool("IsRunning", false);
        m_Animator.SetBool("IsJumping", true);
        m_Rigidbody = player.GetComponent<Rigidbody>();

        while (Mathf.Abs(player.transform.position.x - portal.transform.position.x) > 0.02f || Mathf.Abs(player.transform.position.z - portal.transform.position.z) > 0.02f)
        {
            Vector3 targetDir = portal.transform.position - player.transform.position;
            targetDir.Normalize();
            Vector3 desiredForward = Vector3.RotateTowards(player.transform.forward, new Vector3(targetDir.x, 0f, targetDir.z), 20f * Time.deltaTime, 0f);
            m_Rotation = Quaternion.LookRotation(desiredForward);
            m_Rigidbody.MoveRotation(m_Rotation);
            player.transform.position = Vector3.MoveTowards(player.transform.position, portal.transform.position, Time.deltaTime * 2f);
            yield return null;
        }
        vcam.m_Follow = null;

        while (player.transform.position.y > -2f)
        {
            player.transform.Translate(Vector3.down * Time.deltaTime * 7.5f, Space.World);
            yield return null;
        }
        // player.GetComponent<PlayerMovement>().enabled = true;
        //player.GetComponent<companionSpawn>().enabled = true;
        //player.GetComponent<Collider>().enabled = true; 
        m_Animator.SetBool("IsJumping", false);

        if (!isTutorial)
        {
            victorySound.Play();
            endMenu.SetActive(true);
            Time.timeScale = 0;
        }


    }

}
