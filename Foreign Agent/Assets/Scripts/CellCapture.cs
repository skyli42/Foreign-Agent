﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellCapture : MonoBehaviour
{
    public GameObject player;
    private bool startCap = false;
    public float capTime = 5f;
	public Slider slider;
    [HideInInspector]
    public float currTime;
    private bool capped = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            startCap = true;
            currTime = capTime;
			Debug.Log("starting cap");
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
                startCap = false;
                slider.value = 0;
            }
            currTime = capTime;
         
        }
    }
}
