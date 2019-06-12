using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
	}

	// Update is called once per frame
	void Update()
    {
		transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
	}
}
