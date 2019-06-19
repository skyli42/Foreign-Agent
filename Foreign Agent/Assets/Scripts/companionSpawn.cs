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
    public  int numCompanions;
    private List<Collider> enemiesList = new List<Collider>();
    private List<NavMeshAgent> companionList = new List<NavMeshAgent>();

    public GameObject companionUI;

    private void Start()
    {
        Instance = this;
        numCompanions = 0;
    }
    void Update()
    {
        companionUI.GetComponent<TextMeshProUGUI>().text = "Companions: " + numCompanions.ToString();
        for (int i = 0; i < enemiesList.Count; i++)
        {
            if (companionList[i].isActiveAndEnabled)
            {
                companionList[i].destination = enemiesList[i].transform.position;
            }
        }
        if (Input.GetKeyDown("space") && numCompanions > 0)
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, 25, enemyMask);
            float minDist = float.MaxValue;
            Collider closestEnemy = null;
            for (int i = 0; i < enemies.Length; i++)
            {
                float dist = Vector3.Distance(enemies[i].transform.position, gameObject.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closestEnemy = enemies[i];
                }
            }
            if (closestEnemy != null)
            {
                Vector3 spawn = (gameObject.transform.position - Vector3.Normalize(gameObject.transform.position - closestEnemy.transform.position));
                numCompanions -= 1;
                agent = Instantiate(companionVirus, spawn, Quaternion.identity).GetComponent<NavMeshAgent>();
               
                companionList.Add(agent);
                enemiesList.Add(closestEnemy);
                agent.autoBraking = false;
                agent.stoppingDistance = 0;
                agent.speed = 10;
                agent.acceleration = 20;
                agent.destination = closestEnemy.transform.position;
            }
        }
       
    }
}