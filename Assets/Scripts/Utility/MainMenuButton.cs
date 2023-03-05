using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public Int32 Biome;

    // Start is called before the first frame update
    void Start()
    {

    }

    public const String BIOME_KEY = "BIOM";

    Boolean _clicked = false;
    public void Click()
    {
        _clicked = true;
        AudioManager.Instance.Play("clickerson", SoundCategory.VFX);

        PlayerPrefs.SetInt(BIOME_KEY, Biome);
        GameObject.FindObjectOfType<InitialGrey>().LoadScene("Level");
    }

    Boolean _banTween = false;
    public void ent()
    {
        if (_banTween || _clicked)
            return;

        //print("Enter");
        _banTween = true;

        LeanTween.scale(gameObject, new Vector3(1.06f, 1.06f, 1.06f), 0.25f)
            .setEaseOutSine();


    }

    public void ex()
    {

        if (_banTween)
        {
            //print("Exit");
            LeanTween.delayedCall(0.35f, () =>
            {
                if (gameObject)
                    LeanTween.scale(gameObject, Vector3.one, 0.25f)
                        .setEaseOutSine();

                _banTween = false;
            });
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
