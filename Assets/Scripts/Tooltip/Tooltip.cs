using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public Sprite sprUnlock;
    public Sprite sprShow;
    public SpriteRenderer srPadlock;

    public TextMeshPro txtText;

    public Int32 Category;
    public Int32 Level;

    public Boolean IsUnlocked;

    public static String GetTooltipCompletedPlayerPrefsKey(Int32 category, Int32 level, Int32 biome)
    {
        return $"TOOLTIP_{biome}_{category}_{level}";
    }

    void OnMouseOver()
    {
        if (IsUnlocked && Input.GetMouseButtonDown(0))
        {
            ShowTooltipWindow(false);
        }
    } 

    public void Unlock()
    {
        ChangeInfoToUnlocked();

        srPadlock.sprite = sprUnlock;

        AudioManager.Instance.Play("unlock", Assets.Scripts.SoundCategory.VFX);

        LeanTween.delayedCall(1.5f, () => ShowTooltipWindow(true));
        LeanTween.delayedCall(1.8f, ChangeAppearanceToUnlocked);

    }

    public void ChangeInfoToUnlocked()
    {
        IsUnlocked = true;
        Int32 biome = PlayerPrefs.GetInt(MainMenuButton.BIOME_KEY, 0);
        PlayerPrefs.SetString(GetTooltipCompletedPlayerPrefsKey(Category, Level, biome), Boolean.TrueString);
    }

    public void ShowTooltipWindow(Boolean dueToUnlocking)
    {
        TooltipWIndowController.Instance.ShowTooltipWindowFor(this, dueToUnlocking);
    }

    public void ChangeAppearanceToUnlocked()
    {
        srPadlock.sprite = sprShow;
        txtText.text = "Poka¿";
        Color green;
        ColorUtility.TryParseHtmlString("#158900", out green);
        txtText.color = green;
        srPadlock.color = green;
    }

    // Start is called before the first frame update
    void Start()
    {
        IsUnlocked = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
