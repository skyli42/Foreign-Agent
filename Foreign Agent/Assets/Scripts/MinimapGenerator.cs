using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinimapGenerator : MonoBehaviour
{
	public GameObject wallParent;
	public Material mat;
	// Start is called before the first frame update
    void Start()
    {
		for (int i = 0; i < wallParent.transform.childCount; i++)
		{
			GameObject wall = Instantiate(wallParent.transform.GetChild(i).gameObject);
            wall.transform.parent = wallParent.transform.GetChild(i);
			wall.layer = 12;
			wall.GetComponent<Renderer>().material = mat;
            if (wall.GetComponent<NavMeshObstacle>() != null)
            {
                wall.GetComponent<NavMeshObstacle>().enabled = false;
            }
		}

	}

}
