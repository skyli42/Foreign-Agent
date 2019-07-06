using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TcellPatrol : MonoBehaviour
{
    public Transform[] points;
    [HideInInspector]
    public int destPoint = 0;
    private NavMeshAgent agent;
    public LayerMask cellMask;
    private Vector3 previousPoint;
    private bool firstCall = true;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
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
    // Update is called once per frame
    void Update()
    {
        Collider[] cells = Physics.OverlapSphere(transform.position, 25, cellMask);
        float minDist = float.MaxValue;
        Collider closestCell = null;
        for (int i = 0; i < cells.Length; i++)
        {
            float dist = Vector3.Distance(cells[i].transform.position, gameObject.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestCell = cells[i];
            }
        }
        if (closestCell != null)
        {
            if (closestCell.gameObject.GetComponent<CellCapture>().startCap)
            {
                if (firstCall)
                {
                    previousPoint = agent.destination;
                    firstCall = false;
                }
                agent.destination = closestCell.transform.position;
                
            }
            else if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GotoNextPoint();
                firstCall = true;
            }
            else if (agent.destination.x == closestCell.transform.position.x && agent.destination.z == closestCell.transform.position.z)
            {
                agent.destination = previousPoint;
                firstCall = true;
            }
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "companion")
        {
            Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }

    }
}
