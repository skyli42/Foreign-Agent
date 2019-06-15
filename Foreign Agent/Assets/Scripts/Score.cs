using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Score : MonoBehaviour
{
    public static int numCaptures;
    public int numCellsInLevel;
    public GameObject cellsLeftUI;
    public GameObject endText;
    // Start is called before the first frame update
    void Start()
    {
        numCaptures = 0;
    }

    // Update is called once per frame
    void Update()
    {
        cellsLeftUI.GetComponent<TextMeshProUGUI>().text = "Cells Left: " + (numCellsInLevel - numCaptures).ToString();
        if (numCaptures == numCellsInLevel)
        {
            endText.SetActive(true);
        }
    }
}
