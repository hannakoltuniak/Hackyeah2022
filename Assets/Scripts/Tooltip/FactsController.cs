using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class FactsController : MonoBehaviour
{

    public TextMeshPro txtFactText;

    // Start is called before the first frame update
    private Color _textColor;
    Int32 biome;
    void Start()
    {
        _textColor = txtFactText.color;
        biome = PlayerPrefs.GetInt(MainMenuButton.BIOME_KEY, 0);
        StartCoroutine(Facting());
        txtFactText.color = Color.clear;
        txtFactText.text = GetFactsForCurrentBiome()[currentFact];
    }

    Int32 currentFact = 0;
    Boolean isFirstFact = true;
    private IEnumerator Facting()
    {
        if (!isFirstFact)
        {
            currentFact++;
            currentFact = currentFact % (GetFactsForCurrentBiome().Count);
            txtFactText.text = GetFactsForCurrentBiome()[currentFact];
        }
        else isFirstFact = false;
        Color colNew = _textColor;
        colNew.a = 0f;
        LeanTween.value(0, 1f, 1f)
            .setOnUpdate((Single val) =>
            {
                colNew.a = val;
                txtFactText.color = colNew;
            });

        yield return new WaitForSeconds(UnityEngine.Random.Range(11f, 16f));

        LeanTween.value(colNew.a, 0f, 1f)
            .setOnUpdate((Single val) =>
            {
                colNew.a = val;
                txtFactText.color = colNew;
            });

        yield return new WaitForSeconds(1.1f);
        StartCoroutine(Facting());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private List<String> GetFactsForCurrentBiome() => (biome == 0 ? _waterFacts : _energyFacts);


    private List<String> _waterFacts = new List<String>
    {
        "Na pi�ciominutowy prysznic zu�ywasz �rednio 200 litr�w wody",
        @"Wed�ug danych statystycznych, przeci�tny Polak zu�ywa oko�o 150 litr�w wody dziennie. Przyk�adowo zu�ywamy:
 20-40 litr�w do sp�ukiwania toalety
 4-7 litr�w do mycia naczy�
 40-90 litr�w na cykl p�ukania pralki",
        @"Statystyczny Polak zu�ywa dziennie 92 l wody. Gdyby zmieni� cho� kilka codziennych nawyk�w, m�g�by zmniejszy� jej pob�r nawet czterokrotnie",
        @"Z danych GUS wynika, �e najwi�cej, bo a� 72% wody wykorzystywanej jest w Polsce na potrzeby gospodarki narodowej (w przemy�le)",
        "Korzystanie z prysznica zamiast k�pieli w wannie pozwala na zmniejszenie zu�ycia tygodniowego o oko�o 798 litr�w!",
        "Je�li z kranu co sekund� spada kropla wody, to w ci�gu jednej doby zostanie zu�yte 8 litr�w wody. ",
        "Je�li w toalecie permanentnie sp�ywa ma�a stru�ka wody do muszli, to w ci�gu doby zu�yte zostanie 216 litr�w wody, a w przypadku du�ej stru�ki wody � 864 litry.",
        "Zakr�cenie kranu w trakcie mycia z�b�w oszcz�dza oko�o 11 litr�w wody",
        "Do produkcji 1 kilograma wo�owiny\npotrzeba 15 000 litr�w wody",
    };

    private List<String> _energyFacts = new List<String>
    {
        "Samoch�d ze �redni� liczb� pasa�er�w 1,3 emituje ok. 140 g/km dwutlenku w�gla na osob�, natomiast nowy autobus z silnikiem wysokopr�nym, kt�rym jedzie �rednio 80 os�b � 0,006 g/km tlenku w�gla na osob�",
        "Czajnik jest u�ywany kilka razy dziennie, co w sumie daje oko�o 0,33 godziny na dob�. Moc czajnika to �rednio 2000 W. Dziennie zu�ywa on zatem 0,66 kWh. Roczne zu�ycie energii elektrycznej przez czajnik elektryczny to 240 kWh",
        "W Europie jest kilka pomnik�w kaloryfera. Jeden z najwi�kszych znajduje si� w polskim mie�cie St�pork�w, po�o�onym w wojew�dztwie �wi�tokrzyskim.",
        "Najprostsza droga licz�ca 260 km znajduje si� na granicy Zjednoczonych Emirat�w Arabskich i Arabii Saudyjskiej",
        "W czasie burzy napi�cie pomi�dzy Ziemi� a chmur� dochodzi do 100 000 000 V. Pr�d p�yn�cy w b�yskawicy ma nat�enie w szczycie oko�o 10 000 A a czasami znacznie wi�cej",
        "Niekt�re zwierz�ta potrafi� wytwarza� napi�cie elektryczne. Na przyk�ad p�aszczki elektryczne i w�gorze elektryczne. Zapas energii w�gorza w jego ogonie, wystarczy�by do rozb�y�ni�cia kilkunastu �ar�wek.",
        "Masowe wydarzenia ekologiczne polegaj�ce na zbiorowym nieu�ywaniu urz�dze� elektrycznych maj� negatywny wp�yw na r�wnowag� gospodarki pr�dowej poniewa� elektrownie maj� problem, by szybko dostosowa� si� do ponownego nag�ego wzrostu. ",
        "Zak�adaj�c, �e pozostawiamy �adowark� w gniazdku przez ca�� dob� to w ci�gu miesi�ca zu�yje ona od 0,07 do 0,36 kWh",
        "Pr�d elektryczny biegnie z pr�dko�ci� oko�o 300 000 km na sekund� w pr�ni, natomiast w przewodach pr�dko�� ta jest oko�o po�ow� mniejsza. Impuls elektryczny mo�e wi�c w ci�gu jednej sekundy obiec kul� ziemsk� doko�a cztery razy.",
    };

}
