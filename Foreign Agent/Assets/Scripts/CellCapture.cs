using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CellCapture : MonoBehaviour
{
    public GameObject player;
    [HideInInspector]
    public bool startCap = false;
    public float capTime = 5f;
	public Slider slider;
    [HideInInspector]
    public float currTime;
    private bool capped = false;

    void OnTriggerEnter(Collider other)
    {
        if (!capped && other.gameObject == player)
        {
            startCap = true;
            currTime = capTime;
			Debug.Log("starting cap");
        }
        else if (startCap && other.gameObject.CompareTag("Tcell"))
        {
            Debug.Log("death");
            SceneManager.LoadScene(0); //temporary
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startCap && !capped)
        {
            currTime -= Time.deltaTime;
			float complete = (capTime - currTime) / capTime;
			slider.value = complete;
            if (currTime <= 0)
            {
                Debug.Log("Capped");
                companionSpawn.numCompanions += 1;
                Score.numCaptures += 1; //temporary
                capped = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject == player)
        {
            if (currTime > 0 && !capped)
            {
                slider.value = 0;
            }
            currTime = capTime;
            startCap = false;

        }
    }
}
