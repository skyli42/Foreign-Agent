using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCapture : MonoBehaviour
{
    public GameObject player;
    private bool startCap = false;
    public static float capTime = 5f;
    [HideInInspector]
    public static float currTime;

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
            if (currTime <= 0)
            {
                Debug.Log("Capped");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject == player)
        {
            startCap = false;
            currTime = capTime;
        }
    }
}
