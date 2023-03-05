using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectButton : MonoBehaviour
{
    public ActionKind ActionKind = 0;
    public TextMeshPro txtText;

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.Instance.Play("clickerson", SoundCategory.VFX);

            if (ActionKind == ActionKind.None)
            {
                Debug.LogError("BAd");
            }
            else if (ActionKind == ActionKind.MainMenu)
            {
                GameObject.FindObjectOfType<InitialGrey>().LoadScene("Menu");
            }
            else if (ActionKind == ActionKind.Sound)
            {
                AudioManager.Instance.ToggleSound();
                Refreshh();
            }
            else
            {
                Debug.LogError("Bad");
            }


        }

    }

    // Start is called before the first frame update
    void Start()
    {
        Refreshh();
    }

    private void Refreshh()
    {
        if (ActionKind == ActionKind.Sound)
        {
            Boolean ismuted = AudioManager.Instance.IsMuted();
            if (ismuted)
            {
                txtText.text = "W³¹cz dŸwiêk";
            }
            else
            {
                txtText.text = "Wycisz dŸwiêk";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum ActionKind
{
    None = 0,
    MainMenu = 1,
    Sound = 2
}
