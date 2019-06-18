using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class macrophageCollision : MonoBehaviour
{
    public List<GameObject> Bcells;
    private NavMeshAgent agent;
    private float normalSpeed;
    public GameObject stage;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        normalSpeed = agent.speed;

    }
    private void OnCollisionEnter(Collision other)
    {
        if (!plasmaSpawn.activated && other.gameObject.tag == "companion")
        {
            other.collider.gameObject.SetActive(false);
            agent.speed = 0;
            plasmaSpawn.activated = true;//probably temp until T helper are implemented
            SimpleSonarShader_Parent parent = stage.GetComponent<SimpleSonarShader_Parent>();
            if (parent) parent.StartSonarRing(transform.position, 5);
            StartCoroutine(unfreezePosition());
        }
        if (other.gameObject.tag == "Player")
        {
            GameController.death = true;
        }

    }
    IEnumerator unfreezePosition()
    {
        yield return new WaitForSeconds(3f);
        agent.speed = normalSpeed;
    }


}
