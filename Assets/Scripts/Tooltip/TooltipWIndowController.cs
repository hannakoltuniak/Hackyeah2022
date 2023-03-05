using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipWIndowController : MonoBehaviour
{
    public GameObject objTooltipWindowPrefab;
    public Transform trCanvas;


    public void ShowTooltipWindowFor(Tooltip tooltip, Boolean dueToUnlocking)
    {
        TooltipWindow window =  Instantiate(objTooltipWindowPrefab, trCanvas).GetComponent<TooltipWindow>();
        window.checkBiomeCompletenessAfterClosing = dueToUnlocking;
        window.Show(KnowledgeDatabase.GetTooltip(tooltip.Level, tooltip.Category));
    }


    public static TooltipWIndowController Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
}
