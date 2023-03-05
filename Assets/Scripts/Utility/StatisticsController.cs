using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatisticsController : MonoBehaviour
{
    public TextMeshProUGUI txtTilesMatched;
    public TextMeshProUGUI txtBiomes;
    public TextMeshProUGUI txtLevels;
    public TextMeshProUGUI txtFour;
    public TextMeshProUGUI txtFive;

    private const String TilesMatched = "TilesMatched";
    private const String LevelsCollected = "LevelsCollected";
    private const String COMPLETED_BIOMES = "COMPLETED_BIOMES";
    private const String FOUR_MATCHES = "FOUR_MATCHES";
    private const String FIVE_MATCHES = "FIVE_MATCHES";

    //public static StatisticsController Instance;

    //private void Awake()
    //{
    //    if(Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(this);
    //    }
    //    else
    //    {
    //        Destroy(this);
    //    }
    //}

    public static void AddToStatistic(Int32 howMuch, StatisticKind kind)
    {
        PlayerPrefs.SetInt(KindToKey(kind), PlayerPrefs.GetInt(KindToKey(kind), 0) + howMuch);
    }


    private static String KindToKey(StatisticKind kind)
    {
        if (kind == StatisticKind.TilesMatched)
            return TilesMatched;
        else if (kind == StatisticKind.LevelsCompleted)
            return LevelsCollected;
        else if (kind == StatisticKind.CompletedBiomes)
            return COMPLETED_BIOMES;
        else if (kind == StatisticKind.FourMatches)
            return FOUR_MATCHES;
        else if (kind == StatisticKind.FiveMatches)
            return FIVE_MATCHES;

        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        txtTilesMatched.text = PlayerPrefs.GetInt(KindToKey(StatisticKind.TilesMatched), 0).ToString();
        txtFour.text = PlayerPrefs.GetInt(KindToKey(StatisticKind.FourMatches), 0).ToString();
        txtFive.text = PlayerPrefs.GetInt(KindToKey(StatisticKind.FiveMatches), 0).ToString();

        Int32 levelsCompleted = 0;
        Int32 biomesCompleted = 0;

        Int32 biome = 0;
        Boolean allComplete = true;
        for (Int32 cat = 0; cat < 3; cat++)
        {
            for (Int32 lvl = 0; lvl < 2; lvl++)
            {
                Boolean isUnlocked = Boolean.Parse(PlayerPrefs.GetString(Tooltip.GetTooltipCompletedPlayerPrefsKey(cat, lvl, biome), Boolean.FalseString));
                if (isUnlocked)
                    levelsCompleted++;
                else allComplete = false;
            }
        }

        if (allComplete)
            biomesCompleted++;

        biome = 1;
        allComplete = true;

        for (Int32 cat = 0; cat < 3; cat++)
        {
            for (Int32 lvl = 0; lvl < 2; lvl++)
            {
                Boolean isUnlocked = Boolean.Parse(PlayerPrefs.GetString(Tooltip.GetTooltipCompletedPlayerPrefsKey(cat, lvl, biome), Boolean.FalseString));
                if (isUnlocked)
                    levelsCompleted++;
                else allComplete = false;

            }
        }

        if (allComplete)
            biomesCompleted++;

        txtBiomes.text = biomesCompleted.ToString();
        txtLevels.text = levelsCompleted.ToString();
    }

}


public enum StatisticKind
{
    TilesMatched = 1,
    LevelsCompleted = 2,
    CompletedBiomes = 3,
    FourMatches = 4,
    FiveMatches = 5
}