using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


public class companionFollow : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject player;
    public float radius = 3f;
    private float distance;
    public float followDistance;
    public LayerMask enemyMask;
    private bool spacePressed = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);

    }

    void Update()
    {
        if (Input.GetKeyDown("space") || spacePressed)
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, 25, enemyMask);
            float minDist = float.MaxValue;
            Collider closestEnemy = null;
            for (int i = 0; i < enemies.Length; i++)
            {
                float dist = Vector3.Distance(enemies[i].transform.position, agent.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closestEnemy = enemies[i];
                }
            }
            if (closestEnemy != null)
            {
                agent.stoppingDistance = 0;
                agent.speed = 20;
                agent.destination = closestEnemy.transform.position;
                spacePressed = true;
            }
        }
        else if(!spacePressed)
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            if (followDistance <= distance)
            {
                agent.destination = player.transform.position;
                agent.stoppingDistance = radius;
            }
        }

    }
}