using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public int numCaptures;
    public int numCellsInLevel;
    public GameObject cellsLeftUI;
    private int prevFramenumCaptures;

    public GameObject pauseMenu;
    private bool paused;
    public GameObject player;

    public bool death;
    public Text TargetDestroyedDeath;
    public Text TimeSpentDeath;
    public GameObject deathMenu;
    public GameObject deathAnim;
    private bool alreadyDead = false;

    public Text TargetDestroyed;
    public Text TimeSpent;
    private bool atEnd = false;
    public GameObject endMenu;
    private UnityEngine.EventSystems.EventSystem myEventSystem;

    public GameObject restartLevelDeathButton;
    public GameObject nextLevelEndButton;
    public GameObject returnToGameButton;

    public bool isTutorial = false;
    void Start()
    {
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
        if(Input.GetKeyDown(KeyCode.Escape) && !death)
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
            TargetDestroyed.text = numCaptures.ToString() +" / " + numCellsInLevel.ToString();
            float timeLeft = Time.timeSinceLevelLoad;
            int min = Mathf.FloorToInt(timeLeft / 60);
            int sec = Mathf.FloorToInt(timeLeft % 60);
            TimeSpent.text = min.ToString("00") + ":" + sec.ToString("00");
            endMenu.SetActive(true);
            atEnd = true;
            myEventSystem.SetSelectedGameObject(nextLevelEndButton);
            nextLevelEndButton.GetComponent<Button>().OnSelect(null);
            StartCoroutine(waitTillDissolveDone());
        }
        prevFramenumCaptures = numCaptures;
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
        if (!isTutorial)
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
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            player.gameObject.GetComponentInChildren<Renderer>().enabled = true;
        }
        
    }
    public IEnumerator waitTillDissolveDone()
    {
        yield return new WaitForSeconds(2f);
        if (!isTutorial)
            Time.timeScale = 0;
    }
}
