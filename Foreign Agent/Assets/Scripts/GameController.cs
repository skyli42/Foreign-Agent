using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool paused;
    public GameObject player;

    public static bool death;
    public GameObject deathMenu;
    public GameObject deathAnim;
    private bool alreadyDead = false;
    void Start()
    {
        death = false;
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
}
