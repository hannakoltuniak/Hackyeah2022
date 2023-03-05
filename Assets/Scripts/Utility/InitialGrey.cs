using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitialGrey : MonoBehaviour
{
    private Image bkg;

    public const Single GREY_TIME = 0.33f;

    // Start is called before the first frame update
    void Start()
    {
        bkg = GetComponent<Image>();
        bkg.enabled = true;
        LeanTween.delayedCall(0.3f, () => LeanTween.value(1f, 0f, GREY_TIME)
            .setOnUpdate((float val) =>
            {
                bkg.color = new Color(bkg.color.r, bkg.color.g, bkg.color.b, val);
            })
            .setOnComplete(() =>
            {
                bkg.enabled = false;
                bkg.raycastTarget = false;

            }));


    }

    public void LoadScene(string sceneName)
    {
        bkg.enabled = true;
        bkg.raycastTarget = true;
        LeanTween.value(0f, 1f, GREY_TIME)
             .setOnUpdate((float val) =>
             {
                 bkg.color = new Color(bkg.color.r, bkg.color.g, bkg.color.b, val);
             })
             .setOnComplete(() =>
             {

                 SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
             });

    }

    // Update is called once per frame
    void Update()
    {

    }
}
