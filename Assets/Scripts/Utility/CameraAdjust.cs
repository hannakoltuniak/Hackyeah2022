using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjust : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Single reference = 1920f / 1080f;
        Single kaletas  = 1280f / 1024f;
        Single ratio = (Single)Screen.width / (Single)Screen.height;

        if(ratio < (reference - 0.0777f))
        {
            Single diffBetweenKaleta = ratio - (kaletas - 0.01f);
            Single part = Mathf.Clamp01(1f  - diffBetweenKaleta);
            Single upFov = 18 * part;
            Camera.main.fieldOfView = 60 + upFov;
            //Single upFov = 18 * 

        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
