using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniTutorial : MonoBehaviour
{
    private const String TUT_SEEN = "TUT_SEEN";

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString(TUT_SEEN, Boolean.FalseString) == Boolean.TrueString)
        {
            GameObject.Destroy(gameObject);
        }

        PlayerPrefs.SetString(TUT_SEEN, Boolean.TrueString);
    }

    public void Closee()
    {
        PlayerPrefs.SetString(TUT_SEEN, Boolean.TrueString);
        GameObject.Destroy(gameObject);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LeanTween.delayedCall(0.15f, Closee);
        }
    }

    
}
