using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class Patrol : MonoBehaviour
{
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;

    public GameObject player;
    public float timeTillReset = 5.0f;
    private float timer;
    public Material searchingMat;
    public bool chaseStart = false;
    private float orignalSpeed;
    public float detectedSpeed;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        orignalSpeed = agent.speed;
        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;
        timer = 0.0f;

        GotoNextPoint();
	}


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
      
        if (GetComponent<FieldOfView>().detected)
        {
            timer = 0.0f;
            if (agent.speed != 0.0f)
            {
                agent.speed = detectedSpeed;
            }
            agent.destination = player.transform.position;
            chaseStart = true;
        }
        else
        {
            timer += Time.deltaTime;
            if (chaseStart)
            {
                if (timer > timeTillReset)
                {
                    GotoNextPoint();
                    chaseStart = false;
                    GetComponent<FieldOfView>().detected = false;
                }
                else
                {
                    GetComponent<FieldOfView>().visualization.GetComponent<Renderer>().material = searchingMat;
                    agent.destination = player.transform.position; // need to figure out how to do random searching mechanic
                }
            }
            else
            {
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    GotoNextPoint();
            }

        }
       // if (!agent.pathPending && agent.remainingDistance < 0.5f)
           // GotoNextPoint();
    }
}