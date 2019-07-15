using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
	// Start is called before the first frame update
	private int count;
    void Start()
    {
		count = 0;
    }

	void Update()
	{
		if (Input.GetKeyDown("space")) {
			ScreenCapture.CaptureScreenshot("./screenshot"+count+".png");
			count++;
		}
		
	}
}
