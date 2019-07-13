using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;
public class plasmaSpawn : MonoBehaviour
{
    public static plasmaSpawn Instance;
    public bool activated = false;
    private bool alreadySpawned = false;
    public int numAntibodies = 3;
    public GameObject antibody;
   // public float radius = 3f;
    public Transform[] spawnPoints;
    public GameObject[] macrophages; //list of macrophages so I can loop through and change fov in antibody control. Not a great way to do it probably
    public GameObject activationAlert;
    private bool alertPlayed = false;
    public Material activeMat;
    [HideInInspector]
    public bool Bcellcollision = false;
    public GameObject Bletter;
    public GameObject Pletter;
    private AudioSource activationSound;
    public TextMeshProUGUI nametag;
    // Update is called once per frame
    private void Start()
    {
        Instance = this;
        activationSound = transform.Find("ActivationSound").gameObject.GetComponent<AudioSource>();
    }
    void LateUpdate()
    {
        if (plasmaSpawn.Instance.activated)
        {
            activated = true;
        }

        if (activated && !alreadySpawned)
        {
            nametag.text = "Plasma B Cell";

            gameObject.GetComponent<Renderer>().material = activeMat;
            Bletter.SetActive(false);
            Pletter.SetActive(true);
            if (!Bcellcollision)
            {
                for (int i = 0; i < numAntibodies; i++)
                {
                    //    float angle = i * Mathf.PI * 2 / numAntibodies;
                    //    float x = Mathf.Cos(angle) * radius;
                    //    float z = Mathf.Sin(angle) * radius;
                    //    Vector3 pos = transform.position + new Vector3(x, 0.5f, z);
                    //    float angleDegrees = -angle * Mathf.Rad2Deg;
                    //    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
                    //    GameObject antibodySpawn = (GameObject)Instantiate(antibody, pos, rot);
                    bool validSpawn = false;
                    int tries = 0;

                    Vector3 spawn = new Vector3(0, 0, 0);
                    while (!validSpawn && tries < 15000)
                    {

                        spawn = Random.insideUnitSphere * 2 + transform.position;
                        if (spawn.y < 0f || spawn.y > 1f)
                        {
                            tries++;
                        }
                        Collider[] colliders = Physics.OverlapSphere(spawn, 1.5f);
                        bool collisionFound = false;
                        foreach (Collider col in colliders)
                        {
                            // If this collider is tagged "Obstacle"
                            if (col.tag == "Obstacle" || col.tag == "HumanCell" || col.tag == "antibody" || col.tag == "Player" || col.tag == "Bcell")
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

                    if (validSpawn)
                    {
                        GameObject antibodySpawn = (GameObject)Instantiate(antibody, spawn, Quaternion.identity);
                        antibodySpawn.GetComponent<antibodyPatrol>().points = spawnPoints;
                        antibodySpawn.GetComponent<antibodyPatrol>().macrophages = macrophages;
                    }
                }

            }
            alreadySpawned = true;
            if (plasmaSpawn.Instance.activated && !plasmaSpawn.Instance.alertPlayed)
            {
                InvokeRepeating("activateAlert", 0.0f, 2f);
                Invoke("deactivateAlert", 6f);
                plasmaSpawn.Instance.alertPlayed = true;
            }
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
        if (!activated && other.gameObject.tag == "Player" && !Bcellcollision)
        {
            Bletter.SetActive(false);
            Pletter.SetActive(true);
            activationSound.Play();
            nametag.text = "Plasma B Cell";
            //plasmaSpawn.activated = true;//probably temp until T helper are implemented
            gameObject.GetComponent<Renderer>().material = activeMat;
            for (int i = 0; i < numAntibodies; i++)
            {
                //    float angle = i * Mathf.PI * 2 / numAntibodies;
                //    float x = Mathf.Cos(angle) * radius;
                //    float z = Mathf.Sin(angle) * radius;
                //    Vector3 pos = transform.position + new Vector3(x, 0.5f, z);
                //    float angleDegrees = -angle * Mathf.Rad2Deg;
                //    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
                //    GameObject antibodySpawn = (GameObject)Instantiate(antibody, pos, rot);
                bool validSpawn = false;
                int tries = 0;

                Vector3 spawn = new Vector3(0, 0, 0);
                while (!validSpawn && tries < 15000)
                {

                    spawn = Random.insideUnitSphere * 2 + transform.position;
                    if (spawn.y < 0f || spawn.y > 1f)
                    {
                        tries++;
                    }
                    Collider[] colliders = Physics.OverlapSphere(spawn, 1f);
                    bool collisionFound = false;
                    foreach (Collider col in colliders)
                    {
                        // If this collider is tagged "Obstacle"
                        if (col.tag == "Obstacle" || col.tag == "HumanCell" || col.tag == "antibody" || col.tag == "Player" || col.tag == "Macrophage")
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

                if (validSpawn)
                {
                    GameObject antibodySpawn = (GameObject)Instantiate(antibody, spawn, Quaternion.identity);
                    antibodySpawn.GetComponent<antibodyPatrol>().points = spawnPoints;
                    antibodySpawn.GetComponent<antibodyPatrol>().macrophages = macrophages;
                }
            }

        }
        Bcellcollision = true;
        nametag.text = "Plasma B Cell";

    }

}
