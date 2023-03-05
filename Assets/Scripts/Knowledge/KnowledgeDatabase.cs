using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class KnowledgeDatabase
    {
        public static String GetCategory(Int32 cat)
        {
            Int32 biome = PlayerPrefs.GetInt(MainMenuButton.BIOME_KEY, 0);

            if (biome == 0)
            {
                return _waterCategories[cat];
            }
            else if (biome == 1)
            {
                return _energyCategories[cat];
            }

            Debug.LogError("BAD!!!!!!!");
            return "BLAD";

        }

        public static String GetTooltip(Int32 level, Int32 category)
        {
            Int32 biome = PlayerPrefs.GetInt(MainMenuButton.BIOME_KEY, 0);

            if (biome == 0)
            {
                return _waterTooltips[category, level];
            }
            else if (biome == 1)
            {
                return _energyTooltips[category, level];
            }

            Debug.LogError("BAD!!!!!!!");
            return "BLAD";

        }

        private static String[] _waterCategories = new String[3]
        {
            "Woda pitna",
            "Woda użytkowa",
            "Czystość",
        };

        private static String[,] _waterTooltips = new String[3, 2]
        {
            { "Już dzisiaj możesz przyczynić się do zrównoważonego rozwoju w obszarze wody pitnej! Jeżeli kupujesz wodę mineralną w plastikowych butelkach lub pijesz z nich w pracy to rozważ rozpoczęcie używania butelek z filtrem (lub dzbanków). Dzięki temu ograniczysz" +
                "ilość generowanego plastiku i zminimalizujesz frustrację związaną z noszeniem ciężkich pojemników. Takie rozwiązanie pozwala również zaoszczędzić pieniądze swoje oraz pracodawcy!",
                "W niektórych sytuacjach ciężko będzie Ci zrezygnować kompletnie z wody butelkowanej. Jeżeli kupujesz wodę, to staraj się wybierać butelki szklane. Szklana butelka na wodę jest zupełnie neutralna dla zdrowia. Jej gładka powierzchnia odporna jest na " +
                "zarysowania i bruzdy, w których mogłyby gromadzić się bakterie. Dzięki takiemu wyborowi unikniesz spożywania zbędnych mikroplastików" },

            { @"Wyłącz kran podczas mycia rąk. Czy potrzebujesz wody podczas szorowania rąk? Zaoszczędź kilka litrów wody i wyłącz kran po wyszorowaniu rąk. Dobrym nawykiem jest również zakręcanie kranu podczas mycia zębów. Wyłącz kran po zmoczeniu szczoteczki i pozostaw wyłączony, aż nadejdzie czas płukania zębów. ",
                "Podlewaj rośliny na zewnątrz wczesnym rankiem. Będziesz potrzebował mniej wody, ponieważ niższe temperatury rano oznaczają utratę mniejszej ilości wody do odparowania. Wieczorne nawadnianie nie jest dobrym pomysłem, ponieważ może to sprzyjać rozwojowi pleśni." },
            
            { "Gdy jesteś w łazience czy to w pracy czy w domu, pamiętaj aby używać tyle środków higieny ile jest potrzebnych do wykonania danej czynności. Staraj się wykorzystać jeden listek papieru w celu wytarcia rąk oraz tę samą liczbę dozowań pompki mydła do rąk. W większości przypadków takie ilości w zupełności wystarczą.",
                "Kiedy korzystasz ze środków czystości w opakowaniach, staraj się używać ilości zalecanej przez producenta. Do proszków do prania dołączona jest miarka, która pomoże Ci uniknąć marnowania środku. Jeśli dodamy go więcej nic się nie stanie z praniem, ale proszek szybciej się skończy. To samo tyczy się innych środków czystości. Czasem mniejsza ilość doczyści brud tak samo jak większa ilość." },
        };


        /// <summary>
        /// //////////////////////////////////////////////////////
        /// </summary>
        private static String[] _energyCategories = new String[3]
        {
            "Energia elektryczna",
            "Energia cieplna",
            "Zrównoważony transport",
        };

        private static String[,] _energyTooltips = new String[3, 2]
        {
            { "Czy to w pracy czy w domu, stosując się do kilku zasad możesz ograniczyć zużycie energii elektrycznej. Kończąc pracę wyłączaj komputer oraz monitor – nie pozostawiaj ich w trybie stand-by. Jeśli korzystasz z laptopa – wyciągaj ładowarkę z gniazdka.",
                "Pamiętaj o tym aby gasić światło, tam gdzie nie jest ono potrzebne. Nie drukuj dokumentów, które nie są niezbędne w takiej formie i korzystaj rozsądnie z pozostałych sprzętów, np. klimatyzacji. Gotowanie tylko takiej ilości wody, która jest potrzebna to najlepszy sposób na oszczędności. Naucz się pytać innych, czy nie potrzebują wody na herbatę gdy sam wstawiasz wodę.​" },
            { @"Kiedy decydujesz się na przewietrzenie pomieszczenia, zastosuj się do poniższych rad:
- krótko (w zależności od temperatury na zewnątrz wystarczy kilka-kilkanaście minut)
- przy oknie otwartym na oścież
- wcześniej zakręć zawory na grzejnikach
Po zamknięciu okna temperatura szybko wróci szybko do normy, dzięki ciepłu zakamuflowanemu w ścianach i stropie.", "Może wydawać się to oczywiste, jednak im więcej ubrań nosi się zimą w domu, tym mniej trzeba go ogrzewać. Warto zakładać odpowiednią liczbę warstw odzieży — w zależności od osobistej tolerancji na niskie temperatury. Wówczas maksymalne ogrzewanie danego miejsca przestaje być koniecznością. Zaleca się wybrać wełniany sweter lub zamienić ulubione dżinsy na spodnie o nieco grubszym materiale." },
            { " Każdego dnia planujemy podróże – do pracy, na wczasy, służbowe. Każdego dnia najczęściej wybieramy swój samochód, osobowy czy służbowy. Warto docenić podróż innymi środkami transportu: pociągi, tramwaje i autobusy są w stanie dowieźć nas do celu. Będzie łatwiej i korzystniej dla otoczenia, lepiej dla środowiska i bezpieczniej dla nas samych, podróż innych również będzie przez to bezpieczniejsza i łatwiejsza",
                @"Aby przyczynić się do zrównoważonego transportu poruszając się własnym pojazdem, stosuj się do kilku wskazówek:
- zmieniaj biegi w odpowiednim zakresie obrotów – według specjalistów optymalne wskazania obrotomierza to około 1,5-2,5 tys. obr./min,
- staraj się przewidywać ruchy innych kierowców,
- dostosuj prędkość jazdy do warunków na drodze,
- unikaj gwałtownego hamowania i przyspieszania" },
        };

    }
}
