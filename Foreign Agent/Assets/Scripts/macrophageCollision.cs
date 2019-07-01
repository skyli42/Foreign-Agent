using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class macrophageCollision : MonoBehaviour
{
    private NavMeshAgent agent;
    private float normalSpeed;
    public GameObject stage;
    private SimpleSonarShader_Parent parent;
    private bool isFrozen = false;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        normalSpeed = agent.speed;
        parent = stage.GetComponent<SimpleSonarShader_Parent>();

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "companion")
        {
            other.collider.gameObject.SetActive(false);
            if(agent.speed == 0)
            {
                isFrozen = true;
            }
            agent.speed = 0;
            if (plasmaSpawn.Instance != null && !plasmaSpawn.Instance.activated)
            {
                plasmaSpawn.Instance.activated = true;//probably temp until T helper are implemented
                
               
                if (parent)
                {
                    Debug.Log("sonar");
                    parent.StartSonarRing(transform.position, 5);
                }
            }
            StartCoroutine(unfreezePosition());
        }
        if (other.gameObject.tag == "Player")
        {
            GameController.Instance.death = true;
        }

    }
    IEnumerator unfreezePosition()
    {
        yield return new WaitForSeconds(3f);
        if(!isFrozen)
            agent.speed = normalSpeed;
        isFrozen = false;
    }


}
