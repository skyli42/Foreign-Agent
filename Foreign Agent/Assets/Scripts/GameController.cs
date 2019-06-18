using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static int numCaptures;
    public int numCellsInLevel;
    public GameObject cellsLeftUI;
    private int prevFramenumCaptures;

    public GameObject pauseMenu;
    private bool paused;
    public GameObject player;

    public static bool death;
    public GameObject deathMenu;
    public GameObject deathAnim;
    private bool alreadyDead = false;

    public Text TargetDestroyed;
    public Text TimeSpent;
    private bool atEnd = false;
    public GameObject endMenu;
    void Start()
    {
        death = false;
        numCaptures = 0;
        prevFramenumCaptures = numCaptures;
        cellsLeftUI.GetComponent<TextMeshProUGUI>().text = "Cells Left: " + (numCellsInLevel - numCaptures).ToString();
    }
    // Update is called once per frame
    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.Escape) && !death)
        {
            paused = togglePause();
            if (paused)
                pauseMenu.GetComponent<Pause>().Display();
            else
                pauseMenu.GetComponent<Pause>().ReturnToGame();
        }
        if (death && !alreadyDead)
        {
            
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
            TimeSpent.text = min.ToString("00") + ":" + sec.ToString();
            endMenu.SetActive(true);
            atEnd = true;
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
        bool isTutorial;
        isTutorial = SceneManager.GetActiveScene().buildIndex == 0;
        
        Instantiate(deathAnim, player.GetComponent<Collider>().bounds.center, Quaternion.identity);
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        player.gameObject.GetComponent<Renderer>().enabled = false;
        if (!isTutorial)
        {
            yield return new WaitForSeconds(1.2f);
            deathMenu.SetActive(true);
        }
        else
        {
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            player.gameObject.GetComponent<Renderer>().enabled = true;
        }
        
    }
    public IEnumerator waitTillDissolveDone()
    {
        bool isTutorial;
        isTutorial = SceneManager.GetActiveScene().buildIndex == 0;
        yield return new WaitForSeconds(1.4f);
        if (!isTutorial)
            Time.timeScale = 0;
    }
}
