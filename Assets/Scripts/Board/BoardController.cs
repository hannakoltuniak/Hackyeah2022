using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class BoardController : MonoBehaviour
{

    private const Boolean USE_PREDEFINED_BOARD = false;

    //board z duza iloscia na poczatek combo
    private Int32[,] predefinedBoard = new Int32[BOARD_WIDTH, BOARD_HEIGHT]
{
       { 0,4,0,5,3,0,0},//upper left of array is bottom left . upper right is upper left
       { 0,5,3,4,0,5,1},
       { 2,4,2,0,2,4,3},
       { 3,1,5,0,5,0,5},
       { 4,2,0,2,0,0,4},
       { 5,3,0,0,2,0,5},
       { 0,4,1,0,0,5,0},
       { 1,5,4,2,0,3,1}
};

    //private Int32[,] predefinedBoard = new Int32[BOARD_WIDTH, BOARD_HEIGHT]
    //{
    //   { 0,4,0,5,3,0,0},//upper left of array is bottom left . upper right is upper left
    //   { 0,5,3,4,0,5,1},
    //   { 2,4,2,0,2,4,3},
    //   { 3,1,5,1,5,1,5},
    //   { 4,2,4,2,4,0,4},
    //   { 5,3,0,3,2,3,5},
    //   { 0,4,1,5,1,5,2},
    //   { 1,5,4,2,0,3,1}
    //};

    //private Int32[,] predefinedBoard = new Int32[BOARD_WIDTH, BOARD_HEIGHT]
    //{
    //   { 1,2,0,1,3,1,1},//upper left of array is bottom left . upper right is upper left
    //   { 1,2,0,5,2,0,2},
    //   { 4,1,5,1,2,5,2},
    //   { 4,5,4,2,5,2,4},
    //   { 5,2,3,2,0,2,5},
    //   { 1,0,2,1,1,5,1},
    //   { 0,1,4,2,5,1,4},
    //   { 1,2,0,1,5,2,1}
    //};

    private const Boolean AUTO_PLAY = false;

    public static BoardController Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public const Int32 BOARD_WIDTH = 8;
    public const Int32 BOARD_HEIGHT = 7;
    public const Single FALL_ONE_SLOT_TIME = 0.2f;

    [SerializeField] private GameObject objSlotPrefab;
    [SerializeField] private GameObject objBiomeCompletedPrefab;

    [SerializeField] private List<GameObject> WaterTiles;
    [SerializeField] private List<GameObject> EnergyTiles;
    private List<GameObject> AvailableTilesForBiome => _currentBiome == 0 ? WaterTiles : EnergyTiles;
    private Int32 _currentBiome = 0;

    [SerializeField] private KnowledgeUnlockingController objKnowledgeUnlockingController;

    private List<Slot> _slots = new List<Slot>();


    public Boolean PreventClicking
    {
        get
        {
            if (UIInteractionsToEnd > 0)
                return true;

            return _preventClicking;
        }
        set
        {
            //print(value);
            _preventClicking = value;
        }
    }
    private Boolean _preventClicking;
    public Int32 UIInteractionsToEnd = 0;

    // Start is called before the first frame update
    void Start()
    {
        //fil world with slots which will contain tiles
        IterateBoard((x, y) =>
        {
            GameObject newSlot = Instantiate(objSlotPrefab, transform);
            Slot slot = newSlot.GetComponent<Slot>();
            slot.Init(x, y);
            _slots.Add(slot);
            newSlot.name = $"Slot X{x} Y{y}";
            newSlot.transform.localPosition = new Vector3(x * newSlot.GetComponent<RectTransform>().rect.width, y * newSlot.GetComponent<RectTransform>().rect.height);
        });

        Invoke(nameof(InitiallyFillBoardTillValid), 0f);

        if (AUTO_PLAY)
        {
            StartCoroutine(ap());
        }

        //fill slots with tiles
    }

    private IEnumerator ap()
    {
        yield return null;
        if (PreventClicking)
        {
            StartCoroutine(ap());
        }
        else
        {

            yield return new WaitForSeconds(0.5f);
            var nextMove = GetNextMoveToScoreGroupIfExists();
            if (_firstClicked == null)
            {
                OnSlotClicked(GetSlotAt(nextMove.from.x, nextMove.from.y));
            }
            else
            {
                OnSlotClicked(GetSlotAt(nextMove.to.x, nextMove.to.y));
            }

            yield return new WaitForSeconds(0.35f);
            StartCoroutine(ap());

        }
    }


    private void InitiallyFillBoardTillValid()
    {
        if(USE_PREDEFINED_BOARD)
            Debug.LogError("Ma byc losowa plansza");
        if (AUTO_PLAY)
            Debug.LogError("AUTOPLAY ma byc wylaczony a nie");

        Int32 initialRefills = 0;
        _currentBiome = PlayerPrefs.GetInt(MainMenuButton.BIOME_KEY, 0);

        do
        {
            if (initialRefills > 0)
            {
                IterateBoard((x, y) =>
                {
                    CrushTileInSlot(new Vector2Int(x, y), immediate: true);
                });
                print("Initial board was bad " + initialRefills);
            }

            initialRefills++;
            IterateBoard((x, y) =>
            {
                Tile randomTile;
                getRandomTile();

                void getRandomTile()
                {
                    if (!USE_PREDEFINED_BOARD)
                    {
                        randomTile = AvailableTilesForBiome[UnityEngine.Random.Range(0, AvailableTilesForBiome.Count)].GetComponent<Tile>();
                    }
                    else
                    {
                        randomTile = AvailableTilesForBiome[predefinedBoard[x, y]].GetComponent<Tile>();
                    }
                }

                Slot currentlyFilledSlot = GetSlotAt(x, y);

                //prevent matches at the first form of the board
                if (!USE_PREDEFINED_BOARD)
                {
                    Slot toCheckToLeft = GetSlotAt(x - 2, y);
                    Slot toCheckToDown = GetSlotAt(x, y - 2);

                    while ((toCheckToLeft != null && toCheckToLeft.Tile.TileKind == randomTile.TileKind)
                        || (toCheckToDown != null && toCheckToDown.Tile.TileKind == randomTile.TileKind))
                        getRandomTile();
                }

                GameObject tilego = Instantiate(randomTile.gameObject, currentlyFilledSlot.transform);
                tilego.transform.localPosition = Vector3.zero;

                currentlyFilledSlot.Tile = tilego.GetComponent<Tile>();
            });

        }
        while (GetNextMoveToScoreGroupIfExists().from.x == -1 && GetNextMoveToScoreGroupIfExists().from.y == -1);

        //testing initial fills
        //Invoke(nameof(cleardebug), 0.5f);
        //Invoke(nameof(InitiallyFillBoardTillValid), 1f);

        PreventClicking = false;
    }

    internal void CheckIfBoardCompleted()
    {
        Boolean allCompleted = true;
        Int32 biome = PlayerPrefs.GetInt(MainMenuButton.BIOME_KEY, 0);
        for (Int32 cat = 0; cat < 3; cat++)
        {
            for (Int32 lvl = 0; lvl < 2; lvl++)
            {
                Boolean isUnlocked = Boolean.Parse(PlayerPrefs.GetString(Tooltip.GetTooltipCompletedPlayerPrefsKey(cat, lvl, biome), Boolean.FalseString));
                if (!isUnlocked)
                    allCompleted = false;
            }
        }

        //print("All completed " + allCompleted);

        if (allCompleted)
        {
            Instantiate(objBiomeCompletedPrefab);
            _preventClicking = true;
        }
    }

    private void IterateBoard(Action<Int32, Int32> action)
    {
        for (Int32 x = 0; x < BOARD_WIDTH; x++)
            for (Int32 y = 0; y < BOARD_HEIGHT; y++)
                action(x, y);
    }

    private Slot _firstClicked = null;
    private Slot _secondClicked = null;
    internal void OnSlotClicked(Slot clicked) => StartCoroutine(OnSlotClickedInternal(clicked));
    private IEnumerator OnSlotClickedInternal(Slot clicked)
    {
        if (PreventClicking)
        {
            yield break;
        }

        PreventClicking = true;
        yield return null;

        void SelectFirst()
        {
            AudioManager.Instance.Play("click", Assets.Scripts.SoundCategory.VFX);
            _firstClicked = clicked;
            _firstClicked.ShowSelectedIndicator();
            PreventClicking = false;
        }

        if (_firstClicked == null)
        {
            SelectFirst();
            yield break;
        }
        else
        {
            if (_firstClicked.Equals(clicked))
            {
                //print("Clicked same slot");
                PreventClicking = false;
                yield break;
            }

            if (PositionHelper.IsFirstAdjcaentToSecond(clicked.X, clicked.Y, _firstClicked.X, _firstClicked.Y))
            {
                AudioManager.Instance.Play("roll", Assets.Scripts.SoundCategory.VFX);
                _secondClicked = clicked;
                Vector2 firstToSecondDirectionToKnowWhereExpandIndicator = new Vector2(_secondClicked.X - _firstClicked.X, _secondClicked.Y - _firstClicked.Y);
                _firstClicked.ExpandSelectionTo(firstToSecondDirectionToKnowWhereExpandIndicator);

                GameObject tileFromFirst = _firstClicked.Tile.gameObject;
                GameObject tileFromSecond = _secondClicked.Tile.gameObject;
                Vector3 firstPos = tileFromFirst.transform.position;
                Vector3 secondPos = tileFromSecond.transform.position;

                _firstClicked.SwapWithTile(tileFromSecond.GetComponent<Tile>());
                _secondClicked.SwapWithTile(tileFromFirst.GetComponent<Tile>());

                _firstClicked.Tile.GetComponent<SpriteRenderer>().sortingOrder = -1;
                _secondClicked.Tile.GetComponent<SpriteRenderer>().sortingOrder = 2;

                Single swapTime = 0.42f;
                LeanTween.move(tileFromFirst, secondPos, swapTime)
                    .setEaseOutSine();
                LeanTween.move(tileFromSecond, firstPos, swapTime)
                    .setEaseOutSine();

                yield return new WaitForSeconds(swapTime * 1.15f);

                _firstClicked.RemoveSelectedIndicator();

                yield return StartCoroutine(CheckBoardAndScoreGroups(onNoMatchesCallback: () =>
                {
                    LeanTween.move(tileFromFirst, firstPos, swapTime)
                            .setEaseOutSine();
                    LeanTween.move(tileFromSecond, secondPos, swapTime)
                        .setEaseOutSine();

                    _firstClicked.SwapWithTile(tileFromFirst.GetComponent<Tile>());
                    _secondClicked.SwapWithTile(tileFromSecond.GetComponent<Tile>());

                }));

                _firstClicked = null;
                _secondClicked = null;
                PreventClicking = false;

                yield break;
            }
            else
            {
                //"jak sie scoruje punkciki to lec¹ one do odpowiedniego paska jak coiny w JTJ po zebraniu"
                _firstClicked.CancelSelectedIndicator();

                SelectFirst();

                yield break;
            }
        }
    }


    private IEnumerator RefillBoard()
    {
        yield return null;

        List<List<Int32>> columnsWithNumberStatingHowMuchToFallForEaachTileInThisColumn = new List<List<Int32>>();
        List<Int32> columnFallsForPurposeOfFallingWhenAllZeros = new List<Int32>();
        for (Int32 x = 0; x < BOARD_WIDTH; x++)
        {
            List<Int32> column = new List<Int32>();
            Int32 fallCountThisColumn = 0;
            for (Int32 y = 0; y < BOARD_HEIGHT; y++)
            {
                Boolean isThereTile = GetSlotAt(x, y).Tile != null;
                if (isThereTile)
                {
                    column.Add(fallCountThisColumn);
                }
                else
                {
                    column.Add(0);
                    fallCountThisColumn++;
                }
            }
            columnFallsForPurposeOfFallingWhenAllZeros.Add(fallCountThisColumn);
            columnsWithNumberStatingHowMuchToFallForEaachTileInThisColumn.Add(column);
        }

        for (Int32 x = 0; x < columnsWithNumberStatingHowMuchToFallForEaachTileInThisColumn.Count; x++)
        {
            for (Int32 y = 0; y < columnsWithNumberStatingHowMuchToFallForEaachTileInThisColumn[x].Count; y++)
                if (columnsWithNumberStatingHowMuchToFallForEaachTileInThisColumn[x][y] > 0)
                {
                    Slot from = GetSlotAt(x, y);
                    Slot to = GetSlotAt(x, y - columnsWithNumberStatingHowMuchToFallForEaachTileInThisColumn[x][y]);
                    Vector3 toPos = to.transform.position;
                    if (from.Tile)
                    {
                        LeanTween.move(from.Tile.gameObject, toPos, columnsWithNumberStatingHowMuchToFallForEaachTileInThisColumn[x][y] * FALL_ONE_SLOT_TIME);
                        to.SwapWithTile(from.Tile);
                        from.Tile = null;
                        //print($"Tile {x} {y} spada {columnsWithNumberStatingHowMuchToFallForEaachTileInThisColumn[x][y]} do dolu");
                    }
                }
        }

        //fill empty slots with new tiles and make them fall
        Single maxFallingTime = 0f;
        foreach (var slot in _slots)
        {
            if (!slot.Tile)
            {
                //print(slot.name);
                var go = Instantiate(AvailableTilesForBiome[UnityEngine.Random.Range(0, AvailableTilesForBiome.Count)], slot.transform);
                Int32 slotsCountToFall = columnsWithNumberStatingHowMuchToFallForEaachTileInThisColumn[slot.X][BOARD_HEIGHT - 1];
                if (slotsCountToFall == 0)//grupka 3 przy samej gorze nie spadala sponad planszy tylko respila sie na miejsach od razu
                    slotsCountToFall = columnFallsForPurposeOfFallingWhenAllZeros[slot.X];
                go.transform.localPosition = new Vector3(0, /*slot.transform.position.y + */
                    slotsCountToFall * slot.GetComponent<RectTransform>().rect.height);

                Single fallingTime = slotsCountToFall * FALL_ONE_SLOT_TIME;
                if (fallingTime > maxFallingTime)
                    maxFallingTime = fallingTime;

                LeanTween.moveLocal(go, Vector3.zero, fallingTime);
                //go.transform.localPosition = Vector3.zero;
                slot.Tile = go.GetComponent<Tile>();
            }
        }

        yield return new WaitForSeconds(maxFallingTime + 0.10f);

        yield return StartCoroutine(CheckBoardAndScoreGroups(() =>
        {
            //print("Ended this chain of matching");
            //check if matches can be made in next move

            if (GetNextMoveToScoreGroupIfExists().from.x == -1 && GetNextMoveToScoreGroupIfExists().from.y == -1)
            {
                print("No more possible matches");

                IterateBoard((x, y) =>
                {
                    if (x % 2 == 0 && y % 2 == 0)
                        CrushTileInSlot(new Vector2Int(x, y), immediate: true);
                    else if (x % 2 == 1 && y % 2 == 1)
                        CrushTileInSlot(new Vector2Int(x, y), immediate: true);

                });

                StartCoroutine(RefillBoard());
            }

        }));
    }

    //resulted in match?
    private IEnumerator CheckBoardAndScoreGroups(Action onNoMatchesCallback)
    {
        yield return null;

        var currentDetectedGroupsNaive = new List<(Vector2 sourceTile, Vector2 groupVector)>();
        List<Vector2Int> tilesCoordinatesToIgnoreBecauseTheyAreAlreadyPresentInOtherMatchedGroup = new List<Vector2Int>();

        IterateBoard((x, y) =>
        {
            if (!tilesCoordinatesToIgnoreBecauseTheyAreAlreadyPresentInOtherMatchedGroup.Any(v => v.x == x && v.y == y))
            {
                Vector2 groupDirectionVector = PositionHelper.CheckIfMakesGroup(GetSlotAt(x, y), tilesCoordinatesToIgnoreBecauseTheyAreAlreadyPresentInOtherMatchedGroup);
                if (groupDirectionVector != Vector2.zero)
                {
                    (Vector2 sourceTile, Vector2 groupVector) matchedGroupInfo = (new Vector2(x, y), groupDirectionVector);
                    currentDetectedGroupsNaive.Add(matchedGroupInfo);
                    var positionsToIgnore = PositionHelper.ExtractCoordinatesFromGroupInfo(matchedGroupInfo);
                    tilesCoordinatesToIgnoreBecauseTheyAreAlreadyPresentInOtherMatchedGroup
                        .AddRange(positionsToIgnore);
                }
            }

        });

        foreach (var group in currentDetectedGroupsNaive)
        {
            var coordinatesToToggleBecauseTheyArePresentInMatchedGroup = PositionHelper.ExtractCoordinatesFromGroupInfo(group);
            for (Int32 i = 0; i < coordinatesToToggleBecauseTheyArePresentInMatchedGroup.Count; i++)
            {
                CrushTileInSlot(coordinatesToToggleBecauseTheyArePresentInMatchedGroup[i], false, i == 0 ? coordinatesToToggleBecauseTheyArePresentInMatchedGroup.Count : 0);//at first tile from group we send info about added points
            }
        }

        yield return new WaitForSeconds(0.420f);

        if (currentDetectedGroupsNaive.Count > 0)//we refill with new tiles if any were matched
            yield return StartCoroutine(RefillBoard());
        else onNoMatchesCallback.Invoke();//if no matches; we rollback the seletions

    }

    private void CrushTileInSlot(Vector2Int coords, Boolean immediate = true, Int32 pointsToScore = 0)
    {
        Slot scoredSlot = GetSlotAt(coords.x, coords.y);
        if (immediate)
        {
            GameObject.Destroy(scoredSlot.Tile.gameObject, 0f);
            scoredSlot.Tile = null;

            return;
        }

        SpriteRenderer scoredSlotTileSpriteRenderer = scoredSlot.Tile.GetComponent<SpriteRenderer>();
        Color c = scoredSlotTileSpriteRenderer.color;
        Single h, s, v;
        Color.RGBToHSV(c, out h, out s, out v);
        scoredSlotTileSpriteRenderer.color = Color.HSVToRGB(h, s, v * 0.95f);

        Color c1 = scoredSlot.Tile.TileIcon.color;
        Single h1, s1, v1;
        Color.RGBToHSV(c1, out h1, out s1, out v1);
        scoredSlot.Tile.TileIcon.color = Color.HSVToRGB(h1, s1, v1 * 0.95f);

        //GameObject.Destroy(scoredSlot.Tile.gameObject, immediate ? 0f : 0.420f);
        scoredSlotTileSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        scoredSlotTileSpriteRenderer.sortingOrder = 3;
        scoredSlot.Tile.CircleMask.SetActive(true);
        LeanTween.scale(scoredSlot.Tile.gameObject, new Vector3(0.5f, 0.5f, 0.5f), 0.35f);
        LeanTween.scale(scoredSlot.Tile.CircleMask, new Vector3(0.9f, 0.9f, 0.9f), 0.35f);
        LeanTween.moveLocal(scoredSlot.Tile.gameObject, PositionHelper.RandomPointOnCircleEdge(1f), 0.35f).setEaseOutSine();

        if (pointsToScore > 0)
        {
            AudioManager.Instance.Play("groupForm", SoundCategory.VFX, volume: 0.420f, playOneShot: true, randomPitch: new RandomPitchSoundParameters(0.94f, 1.03f));
        }

        StartCoroutine(SendToCollectionPoint(scoredSlot.Tile, pointsToScore));

        scoredSlot.Tile = null;
    }

    private IEnumerator SendToCollectionPoint(Tile sendant, Int32 pointsToScore)
    {
        Transform collectionPoint = objKnowledgeUnlockingController.GetCollectionPointForTileKind(sendant);

        sendant.AddComponent<SortingGroup>();

        yield return new WaitForSeconds(0.35f);
        Single travelTime = Random.Range(0.5f, 0.69f);

        Vector3 A = sendant.transform.position;
        Vector3 B = collectionPoint.transform.position;
        Vector3 betweenLittleToDown = 0.50f * Vector3.Normalize(B - A) + A;
        betweenLittleToDown.y -= 2f;
        LeanTween.moveSpline(sendant.gameObject,
            new Vector3[5]
            {
                sendant.transform.position,
                sendant.transform.position,

                betweenLittleToDown,

                collectionPoint.transform.position,
                collectionPoint.transform.position,
            }, travelTime)
            .setEaseInSine();

        yield return new WaitForSeconds(travelTime);

        if (pointsToScore > 0)
        {
            StartCoroutine(Pops(pointsToScore));
            collectionPoint.parent.GetComponent<UnlockableKnowledge>().AddProgress(pointsToScore, sendant.TileKind);
        }

        yield return new WaitForSeconds(0.1f);

        Destroy(sendant.gameObject);

    }


    private IEnumerator Pops(Int32 points)
    {
        for (Int32 i = 0; i < points; i++)
        {
            AudioManager.Instance.Play("pop", SoundCategory.VFX, playOneShot: true, randomPitch: new RandomPitchSoundParameters(0.95f, 1.05f), volume: 0.69f);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private (Vector2Int from, Vector2Int to) GetNextMoveToScoreGroupIfExists()
    {
        Vector2Int moveToScore = Vector2Int.zero;
        Vector2Int tileToClick = new Vector2Int(-1, -1);

        IterateBoard((x, y) =>
        {
            if (moveToScore != Vector2Int.zero)
                return;

            if (tileToClick.x != -1 && tileToClick.y != -1)
            {
                return;
            }

            var scoringSwap = PositionHelper.GetMoveThatMakesGroup(GetSlotAt(x, y));
            if (scoringSwap.swapThis != Vector2Int.zero && scoringSwap.withThis != Vector2Int.zero)
            {
                tileToClick = scoringSwap.swapThis;
                moveToScore = scoringSwap.withThis;
            }

        });

        return (tileToClick, moveToScore);
    }

    public Slot GetSlotAt(Int32 x, Int32 y) => _slots.SingleOrDefault(s => s.X == x && s.Y == y);

    // Update is called once per frame
    void Update()
    {

    }
}

