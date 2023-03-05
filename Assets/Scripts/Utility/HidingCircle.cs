using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingCircle : MonoBehaviour
{
    public SpriteRenderer srHidingCircle;

    private Boolean _isShown = false;
    public void Show()
    {
        if(_isShown)
        {
            return;
        }

        _isShown = true;
        LeanTween.scale(srHidingCircle.gameObject, new Vector3(30, 30, 30), 1f)
            .setEaseInSine();
    }

    public void Hide()
    {
        //_isShown = false;
        LeanTween.scale(srHidingCircle.gameObject, Vector3.one, 1f)
            .setEaseOutSine();
    }


    public static HidingCircle Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }


}
