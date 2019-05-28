using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public GameObject endCanvas;
    public static int numCaptures;
    public int numCellsInLevel;
    // Start is called before the first frame update
    void Start()
    {
        numCaptures = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (numCaptures == numCellsInLevel)
        {
            endCanvas.SetActive(true);
        }
    }
}
