// Patrol.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class antibodyPatrol : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private GameObject player;
    public GameObject[] macrophages;
  
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.avoidancePriority = Random.Range(0, 101);
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = Random.Range(0, points.Length - 1);
        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        
        if (Vector3.Distance(agent.transform.position, player.transform.position) < 1f) 
        {
            Destroy(agent.gameObject);
            for (int i = 0; i < macrophages.Length; i++)
            {
                macrophages[i].GetComponent<FieldOfView>().viewRadius += 0.5f;
            }
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
}

