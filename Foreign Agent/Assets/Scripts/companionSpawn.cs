using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class companionSpawn : MonoBehaviour
{
    public static companionSpawn Instance;
    public GameObject companionVirus;
    private NavMeshAgent agent;
    private float distance;
    public LayerMask enemyMask;
    public int numCompanions;
    private List<Collider> enemiesList = new List<Collider>();
    public List<NavMeshAgent> companionList = new List<NavMeshAgent>();
    public int prevFrameNumCompanions;
    public GameObject companionUI;
    public Dictionary<NavMeshAgent, GameObject> companionToEnemy;

    private void Start()
    {
        Instance = this;
        numCompanions = 0;
        prevFrameNumCompanions = numCompanions;
        companionToEnemy = new Dictionary<NavMeshAgent, GameObject>();
        companionList = new List<NavMeshAgent>();

    }
    
    void Update()
    {
        if (prevFrameNumCompanions == numCompanions - 1) // Aidan why would you do this to me
        {
            Debug.Log("Spawning new Companion (from companionSpawn)");
            bool validSpawn = false;
            int tries = 0;
            Vector3 spawn = new Vector3(0, 0, 0);
            while (!validSpawn && tries < 10000)
            {

                spawn = Random.insideUnitSphere * 2 + transform.position;
                if (spawn.y < 1f || spawn.y > 1.5f)
                {
                    tries++;
                }
                else
                {
                    Collider[] colliders = Physics.OverlapSphere(spawn, 1f);
                    bool collisionFound = false;
                    foreach (Collider col in colliders)
                    {
                        // If this collider is tagged "Obstacle"
                        if (col.tag == "Obstacle" || col.tag == "companion" || col.tag == "Player")
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
            }
            if (validSpawn)
            {
                Debug.Log(spawn);
                agent = Instantiate(companionVirus, spawn, Quaternion.identity).GetComponent<NavMeshAgent>();

                companionToEnemy.Add(agent, gameObject);
                agent.avoidancePriority = Random.Range(0, 101);
                agent.autoBraking = true;
                companionList.Add(agent);
                agent.stoppingDistance = 2;
            }

        }
        prevFrameNumCompanions = numCompanions;
        companionUI.GetComponent<TextMeshProUGUI>().text = "Companions: " + numCompanions.ToString();
        List<NavMeshAgent> markedForRemoval = new List<NavMeshAgent>();

        foreach (KeyValuePair<NavMeshAgent, GameObject> entry in companionToEnemy)
        {
            if (!entry.Key.isActiveAndEnabled)
            {
                markedForRemoval.Add(entry.Key);
            }
            else
                entry.Key.destination = entry.Value.transform.position;
        }
        foreach (NavMeshAgent removedAgent in markedForRemoval)
        {
            companionToEnemy.Remove(removedAgent);
            companionList.Remove(removedAgent);
        }

        numCompanions = Mathf.Min(companionList.Count, numCompanions);
        if (Input.GetKeyDown("space") && numCompanions > 0)
        {
           
            NavMeshAgent agent2 = null;
            foreach (KeyValuePair<NavMeshAgent, GameObject> entry in companionToEnemy)
            {
                if (entry.Value == gameObject)
                {
                    agent2 = entry.Key;
                    break;
                }
            }
            if (agent2 != null)
            {
                Collider closestEnemy = getObjectsDist(agent2);
                if (closestEnemy != null)
                {
                    companionToEnemy[agent2] = closestEnemy.gameObject;
                    numCompanions -= 1;
                    agent2.autoBraking = false;
                    agent2.stoppingDistance = 0;
                    agent2.speed = 10;
                    agent2.acceleration = 20;
                    agent2.radius = 0.5f;
                    agent2.isStopped = false;
                    agent2.destination = closestEnemy.transform.position;
                }

            }
        }
    }
    public Collider getObjectsDist(NavMeshAgent agent2)
    {
        NavMeshPath path = new NavMeshPath();
        float minDist = float.MaxValue;
        Collider closestEnemy = null;
        Collider[] enemies = Physics.OverlapSphere(agent2.transform.position, 25, enemyMask);
        for (int i = 0; i < enemies.Length; i++)
        {
            Transform trans = enemies[i].transform;
            NavMesh.CalculatePath(agent2.transform.position, trans.position, agent2.areaMask, path);
            float dist = 0f;
            for (int j = 0; j < path.corners.Length - 1; j++)
            {
                dist += Vector3.Distance(path.corners[j], path.corners[j + 1]);
            }
            if(dist < minDist)
            {
                minDist = dist;
                closestEnemy = enemies[i];
            }
            
        }
        return closestEnemy;
    }
    
}

