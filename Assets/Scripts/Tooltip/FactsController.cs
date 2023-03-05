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
        "Na piêciominutowy prysznic zu¿ywasz œrednio 200 litrów wody",
        @"Wed³ug danych statystycznych, przeciêtny Polak zu¿ywa oko³o 150 litrów wody dziennie. Przyk³adowo zu¿ywamy:
 20-40 litrów do sp³ukiwania toalety
 4-7 litrów do mycia naczyñ
 40-90 litrów na cykl p³ukania pralki",
        @"Statystyczny Polak zu¿ywa dziennie 92 l wody. Gdyby zmieni³ choæ kilka codziennych nawyków, móg³by zmniejszyæ jej pobór nawet czterokrotnie",
        @"Z danych GUS wynika, ¿e najwiêcej, bo a¿ 72% wody wykorzystywanej jest w Polsce na potrzeby gospodarki narodowej (w przemyœle)",
        "Korzystanie z prysznica zamiast k¹pieli w wannie pozwala na zmniejszenie zu¿ycia tygodniowego o oko³o 798 litrów!",
        "Jeœli z kranu co sekundê spada kropla wody, to w ci¹gu jednej doby zostanie zu¿yte 8 litrów wody. ",
        "Jeœli w toalecie permanentnie sp³ywa ma³a stru¿ka wody do muszli, to w ci¹gu doby zu¿yte zostanie 216 litrów wody, a w przypadku du¿ej stru¿ki wody – 864 litry.",
        "Zakrêcenie kranu w trakcie mycia zêbów oszczêdza oko³o 11 litrów wody",
        "Do produkcji 1 kilograma wo³owiny\npotrzeba 15 000 litrów wody",
    };

    private List<String> _energyFacts = new List<String>
    {
        "Samochód ze œredni¹ liczb¹ pasa¿erów 1,3 emituje ok. 140 g/km dwutlenku wêgla na osobê, natomiast nowy autobus z silnikiem wysokoprê¿nym, którym jedzie œrednio 80 osób – 0,006 g/km tlenku wêgla na osobê",
        "Czajnik jest u¿ywany kilka razy dziennie, co w sumie daje oko³o 0,33 godziny na dobê. Moc czajnika to œrednio 2000 W. Dziennie zu¿ywa on zatem 0,66 kWh. Roczne zu¿ycie energii elektrycznej przez czajnik elektryczny to 240 kWh",
        "W Europie jest kilka pomników kaloryfera. Jeden z najwiêkszych znajduje siê w polskim mieœcie St¹porków, po³o¿onym w województwie œwiêtokrzyskim.",
        "Najprostsza droga licz¹ca 260 km znajduje siê na granicy Zjednoczonych Emiratów Arabskich i Arabii Saudyjskiej",
        "W czasie burzy napiêcie pomiêdzy Ziemi¹ a chmur¹ dochodzi do 100 000 000 V. Pr¹d p³yn¹cy w b³yskawicy ma natê¿enie w szczycie oko³o 10 000 A a czasami znacznie wiêcej",
        "Niektóre zwierzêta potrafi¹ wytwarzaæ napiêcie elektryczne. Na przyk³ad p³aszczki elektryczne i wêgorze elektryczne. Zapas energii wêgorza w jego ogonie, wystarczy³by do rozb³yœniêcia kilkunastu ¿arówek.",
        "Masowe wydarzenia ekologiczne polegaj¹ce na zbiorowym nieu¿ywaniu urz¹dzeñ elektrycznych maj¹ negatywny wp³yw na równowagê gospodarki pr¹dowej poniewa¿ elektrownie maj¹ problem, by szybko dostosowaæ siê do ponownego nag³ego wzrostu. ",
        "Zak³adaj¹c, ¿e pozostawiamy ³adowarkê w gniazdku przez ca³¹ dobê to w ci¹gu miesi¹ca zu¿yje ona od 0,07 do 0,36 kWh",
        "Pr¹d elektryczny biegnie z prêdkoœci¹ oko³o 300 000 km na sekundê w pró¿ni, natomiast w przewodach prêdkoœæ ta jest oko³o po³owê mniejsza. Impuls elektryczny mo¿e wiêc w ci¹gu jednej sekundy obiec kulê ziemsk¹ doko³a cztery razy.",
    };

}
