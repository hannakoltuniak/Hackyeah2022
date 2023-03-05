using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    private const String INTRO_SHOWED_INITIALLY = "INTRO_SHOWED_INITIALLY";

    public GameObject objContent;
    public GameObject objWindow;

    public Image imgFiller;

    // Start is called before the first frame update
    void Start()
    {
        objContent.SetActive(false);
        objWindow.transform.localScale = Vector3.zero;

        LeanTween.delayedCall(InitialGrey.GREY_TIME * 2f, ShowIfApplies);
    }

    private void ShowIfApplies()
    {
        Boolean showedInitally = Boolean.Parse(PlayerPrefs.GetString(INTRO_SHOWED_INITIALLY, Boolean.FalseString));
        
        if(!showedInitally)
        {
            Show();
        }
    }

    public void Show()
    {
        PlayerPrefs.SetString(INTRO_SHOWED_INITIALLY, Boolean.TrueString);
        imgFiller.color = new Color(imgFiller.color.r, imgFiller.color.g, imgFiller.color.b, 0.5f);

        objContent.SetActive(true);
        LeanTween.scale(objWindow, Vector3.one, 0.5f)
            .setEaseOutSine();

    }

    public void Close()
    {
        LeanTween.scale(objWindow, Vector3.zero, 0.3f)
            .setEaseOutSine()
            .setOnComplete(() =>
            {
                objContent.SetActive(false);
            });

        LeanTween.value(0.5f, 0f, 0.2f)
            .setOnUpdate((Single val) =>
            {
                Color newCol = imgFiller.color;
                newCol.a = val;
                imgFiller.color = newCol;
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
