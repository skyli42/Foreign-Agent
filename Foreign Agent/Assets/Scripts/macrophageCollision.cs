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
    private AudioSource sonar;
    private AudioSource companionDeath;
    public GameObject companionAnim;
	private Vector3 frozenPos;
	private float shakeSpeed = 0.05f;
    public GameObject stunEffect;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        normalSpeed = agent.speed;
        parent = stage.GetComponent<SimpleSonarShader_Parent>();
        sonar = transform.Find("Sonar").gameObject.GetComponent<AudioSource>();
        companionDeath = transform.Find("companionDeath").gameObject.GetComponent<AudioSource>();

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "companion")
        {
			frozenPos = transform.position;
            stunEffect.SetActive(true);
            companionDeath.Play();
            Instantiate(companionAnim, other.collider.bounds.center, Quaternion.identity);
            other.collider.gameObject.SetActive(false);
            if(agent.speed == 0)
            {
                isFrozen = true;
                stunEffect.SetActive(true);
            }
			agent.speed = 0;
			shakeSpeed = 0.05f;
            if (plasmaSpawn.Instance != null && !plasmaSpawn.Instance.activated)
            {
                plasmaSpawn.Instance.activated = true;//probably temp until T helper are implemented
                
               
                if (parent)
                {
                    Debug.Log("sonar");
                    sonar.Play();
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
	private void Update()
	{
		if (agent.speed == 0)
		{
			Debug.Log("frozen");
			shakeSpeed -= 0.05f*(Time.deltaTime/3);
			transform.position = frozenPos + UnityEngine.Random.insideUnitSphere * shakeSpeed;
		}
        else
        {
            if(agent.speed == 0)
            {
                isFrozen = true;
            }
        }
		//else
		//{
		//	Debug.Log("not frozen");
		//}
	}
	IEnumerator unfreezePosition()
    {
        yield return new WaitForSeconds(3f);
        if (!isFrozen)
        {
            agent.speed = normalSpeed;
            stunEffect.SetActive(false);
        }
		isFrozen = false;
	}


}
