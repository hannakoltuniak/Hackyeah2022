using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class SustainableColors
    {

        public Color FirstTwoColor;
        public Color SecondTwoColor;
        public Color ThirdTwoColor;


        public SustainableColors()
        {
            if (PlayerPrefs.GetInt(MainMenuButton.BIOME_KEY, 0) == 0)
            {
                ColorUtility.TryParseHtmlString("#4D75FF", out FirstTwoColor);
                ColorUtility.TryParseHtmlString("#FF8766", out SecondTwoColor);
                ColorUtility.TryParseHtmlString("#9ED852", out ThirdTwoColor);
            }
            else if (PlayerPrefs.GetInt(MainMenuButton.BIOME_KEY, 0) == 1)
            {
                ColorUtility.TryParseHtmlString("#F6CC00", out FirstTwoColor);
                ColorUtility.TryParseHtmlString("#E92925", out SecondTwoColor);
                ColorUtility.TryParseHtmlString("#0C91F0", out ThirdTwoColor);
            }
            else
            {
                Debug.LogError("BAD");
            }
        }

    }
}
