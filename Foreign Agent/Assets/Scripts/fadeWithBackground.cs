using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class fadeWithBackground : MonoBehaviour
{
    private Color newColour;
    private Color backColour;

    // Update is called once per frame
    void Update()
    {
        
        if (GetComponent<Graphic>() != null)
        {
            newColour = GetComponent<Graphic>().color;
            backColour = transform.parent.gameObject.GetComponent<Graphic>().color;
            newColour.a = backColour.a;
            GetComponent<Graphic>().color = newColour;
        }
        else
        {

            newColour = GetComponent<TextMeshProUGUI>().color;
            backColour = transform.parent.gameObject.GetComponent<Graphic>().color;
            newColour.a = backColour.a;
            GetComponent<TextMeshProUGUI>().color = newColour;
        }
    }
}
