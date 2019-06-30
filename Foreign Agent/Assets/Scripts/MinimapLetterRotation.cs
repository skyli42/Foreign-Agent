using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapLetterRotation : MonoBehaviour
{

    void LateUpdate()
    {
        //transform.rotation = rotation;
        transform.localRotation = Quaternion.Euler(-transform.parent.rotation.eulerAngles + new Vector3(90, 180, 180));
    }
}
