using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBackground : MonoBehaviour
{  
    void Start()
    {
        GetComponent<Renderer>().material.mainTexture = WebcamMotionCapture.instance.webcamTexture;
    }
}
