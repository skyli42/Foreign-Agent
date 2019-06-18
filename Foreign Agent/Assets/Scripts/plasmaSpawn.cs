using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plasmaSpawn : MonoBehaviour
{
    public static bool activated;
    private bool alreadySpawned = false;
    public int numAntibodies = 3;
    public GameObject antibody;
    public float radius = 3f;
    public Transform[] spawnPoints;
    public GameObject[] macrophages; //list of macrophages so I can loop through and change fov in antibody control. Not a great way to do it probably
    public GameObject activationAlert;

    public Material activeMat;
    private bool Bcellcollision = false;
    // Update is called once per frame
    private void Start()
    {
        activated = false;
    }
    void Update()
    {
        if (activated && !alreadySpawned)
        {

            gameObject.GetComponent<Renderer>().material = activeMat;
            InvokeRepeating("activateAlert", 0.0f, 1f);
            Invoke("deactivateAlert", 5f);
            if (!Bcellcollision)
            {
                for (int i = 0; i < numAntibodies; i++)
                {
                    float angle = i * Mathf.PI * 2 / numAntibodies;
                    float x = Mathf.Cos(angle) * radius;
                    float z = Mathf.Sin(angle) * radius;
                    Vector3 pos = transform.position + new Vector3(x, 0, z);
                    float angleDegrees = -angle * Mathf.Rad2Deg;
                    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
                    GameObject antibodySpawn = (GameObject)Instantiate(antibody, pos, rot);
                    antibodySpawn.GetComponent<antibodyPatrol>().points = spawnPoints;
                    antibodySpawn.GetComponent<antibodyPatrol>().macrophages = macrophages;
                   
                }
            }
            alreadySpawned = true;
        }

        
       
    }
    void activateAlert()
    {
        if (activationAlert.activeSelf)
            activationAlert.SetActive(false);
        else
            activationAlert.SetActive(true);
    }
    void deactivateAlert()
    {
        CancelInvoke();
        activationAlert.SetActive(false);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!plasmaSpawn.activated && other.gameObject.tag == "Player" && !Bcellcollision)
        {
            //plasmaSpawn.activated = true;//probably temp until T helper are implemented
            gameObject.GetComponent<Renderer>().material = activeMat;
            for (int i = 0; i < numAntibodies; i++)
            {
                float angle = i * Mathf.PI * 2 / numAntibodies;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                Vector3 pos = transform.position + new Vector3(x, 0, z);
                float angleDegrees = -angle * Mathf.Rad2Deg;
                Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
                GameObject antibodySpawn = (GameObject)Instantiate(antibody, pos, rot);
                antibodySpawn.GetComponent<antibodyPatrol>().points = spawnPoints;
                antibodySpawn.GetComponent<antibodyPatrol>().macrophages = macrophages;
            }
            Bcellcollision = true;
        }

    }
}
