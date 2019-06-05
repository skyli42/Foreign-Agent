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

    // Update is called once per frame
    private void Start()
    {
        activated = false;
    }
    void Update()
    {
        if (activated && !alreadySpawned)
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
                alreadySpawned = true;
            }
        }

    }
}
