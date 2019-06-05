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

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        normalSpeed = agent.speed;

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "companion")
        {
            other.collider.gameObject.SetActive(false);
            agent.speed = 0;
            plasmaSpawn.activated = true; //probably temp until T helper are implemented
            StartCoroutine(unfreezePosition());
        }
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(0);
        }

    }
    IEnumerator unfreezePosition()
    {
        yield return new WaitForSeconds(3f);
        agent.speed = normalSpeed;
    }


}
