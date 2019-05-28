using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCapture : MonoBehaviour
{
    public GameObject player;
    private bool startCap = false;
    public float capTime = 5f;
    [HideInInspector]
    public float currTime;
    private bool capped = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            startCap = true;
            currTime = capTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startCap)
        {
            currTime -= Time.deltaTime;
            if (currTime <= 0 && !capped)
            {
                Debug.Log("Capped");
                Score.numCaptures += 1; //temporary
                capped = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject == player)
        {
            if (currTime > 0)
            {
                startCap = false;
            }
            currTime = capTime;
        }
    }
}
