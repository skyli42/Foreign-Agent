using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
    }
}
