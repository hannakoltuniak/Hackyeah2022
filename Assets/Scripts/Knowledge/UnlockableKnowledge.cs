using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UnlockableKnowledge : MonoBehaviour
{
    public SpriteRenderer srProgressBar;
    public SpriteRenderer srReminder;

    public List<Tooltip> Tooltips;

    public Transform trPointCollectionPoint;
    public TextMeshPro txtText;
    public List<Int32> tileKindsThatMakeProgress = new List<Int32>();
    public Color BarColor;

    private Int32 _currentProgressPoints = 0;
    private Int32[] _maxProgressPointsPerLevel = new Int32[2]
        { 10, 15};
    //{ 25, 35};
    private Int32 GetMaxProgressPointsForCurrentLevel() => _maxProgressPointsPerLevel[_currentLevel];

    private Int32 _currentLevel = 0;

    private const String MAX_LEVEL_TEXT = "Maks. poziom";

    //-15 -0.73
    public void AddProgress(Int32 progressPoints, Int32 kind)
    {
        StatisticsController.AddToStatistic(progressPoints, StatisticKind.TilesMatched);
        if (progressPoints == 4)
            StatisticsController.AddToStatistic(1, StatisticKind.FourMatches);
        else if (progressPoints == 5)
            StatisticsController.AddToStatistic(1, StatisticKind.FiveMatches);

        if (IsMaxLevel)
            return;

        if (_levellingUp)
        {
            LeanTween.value(0, 4, 0.5f)
                .setOnUpdate((Single val) =>
                {
                    Int32 valc = (Int32)val;
                    if (valc % 2 == 0)
                        srReminder.gameObject.SetActive(false);
                    else
                        srReminder.gameObject.SetActive(true);

                });
            return;
        }

        //print($"Scored {progressPoints} points of kind {kind}!");

        AddProgressBarTweenToQueue(_currentProgressPoints, _currentProgressPoints + progressPoints);
        _currentProgressPoints += progressPoints;

    }

    private Queue<(Int32 before, Int32 after)> progressBarStatesQueue = new Queue<(Int32 before, Int32 after)>();
    private void AddProgressBarTweenToQueue(Int32 before, Int32 after)
    {
        progressBarStatesQueue.Enqueue((before, after));
    }

    private Int32 GetCategoryIndex()
    {
        if (tileKindsThatMakeProgress.Any(k => k == 0) || tileKindsThatMakeProgress.Any(k => k == 1))
        {
            return 0;
        }
        else if (tileKindsThatMakeProgress.Any(k => k == 2) || tileKindsThatMakeProgress.Any(k => k == 3))
        {
            return 1;
        }
        else if (tileKindsThatMakeProgress.Any(k => k == 4) || tileKindsThatMakeProgress.Any(k => k == 5))
        {
            return 2;
        }

        return 0;
    }

    private String _initialText;
    private BoxCollider2D collider;
    void Start()
    {
        txtText.text = KnowledgeDatabase.GetCategory(GetCategoryIndex());

        srReminder.gameObject.SetActive(false);
        collider = GetComponent<BoxCollider2D>();
        _initialText = txtText.text;

        collider.enabled = false;



        SustainableColors sc = new SustainableColors();
        if (tileKindsThatMakeProgress.Any(k => k == 0) || tileKindsThatMakeProgress.Any(k => k == 1))
        {
            srProgressBar.color = sc.FirstTwoColor;
        }
        else if (tileKindsThatMakeProgress.Any(k => k == 2) || tileKindsThatMakeProgress.Any(k => k == 3))
        {
            srProgressBar.color = sc.SecondTwoColor;
        }
        else if (tileKindsThatMakeProgress.Any(k => k == 4) || tileKindsThatMakeProgress.Any(k => k == 5))
        {
            srProgressBar.color = sc.ThirdTwoColor;
        }

        Int32 biome = PlayerPrefs.GetInt(MainMenuButton.BIOME_KEY, 0);
        Boolean lvl1Unlocked = Boolean.Parse(PlayerPrefs.GetString(Tooltip.GetTooltipCompletedPlayerPrefsKey(GetCategoryIndex(), 0, biome), Boolean.FalseString));
        Boolean lvl2Unlocked = Boolean.Parse(PlayerPrefs.GetString(Tooltip.GetTooltipCompletedPlayerPrefsKey(GetCategoryIndex(), 1, biome), Boolean.FalseString));

        if (lvl1Unlocked)
        {
            _currentLevel = 1;
            Tooltips[0].ChangeInfoToUnlocked();
            Tooltips[0].ChangeAppearanceToUnlocked();
        }

        if (lvl2Unlocked)
        {
            _currentLevel = 2;
            Tooltips[1].ChangeInfoToUnlocked();
            Tooltips[1].ChangeAppearanceToUnlocked();
            srProgressBar.transform.localPosition = new Vector3((-15f + 14.27f), 0);
            txtText.text = MAX_LEVEL_TEXT;
        }

        //srProgressBar.color = BarColor;

    }

    private Boolean IsMaxLevel => _currentLevel >= 2;

    void OnMouseOver()
    {
        if (_levellingUp && Input.GetMouseButtonDown(0))
        {
            collider.enabled = false;

            LeanTween.cancel(gameObject);
            LeanTween.cancel(srProgressBar.gameObject);
            LeanTween.cancel(txtText.gameObject);

            AudioManager.Instance.Play("clickerson", SoundCategory.VFX);

            if (_currentLevel < 2)
            {
                LeanTween.moveLocal(srProgressBar.gameObject, new Vector2(-15f, 0), BAR_TWEENING_TIME)
                    .setEaseOutSine();
            }


            LeanTween.delayedCall(BAR_TWEENING_TIME + 0.20f, () =>
            {
                Tooltips[_currentLevel - 1].Unlock();
                HidingCircle.Instance.Hide();

                void changeToText(String text)
                {
                    LeanTween.value(1f, 0f, TEXT_TRANSITION_TIME)
                        .setOnUpdate((Single val) =>
                        {
                            txtText.color = new Color(txtText.color.r, txtText.color.g, txtText.color.b, val);

                        })
                        .setOnComplete(() =>
                        {
                            txtText.text = text;
                            LeanTween.value(0f, 1f, TEXT_TRANSITION_TIME)
                                .setOnUpdate((Single val) =>
                                {
                                    txtText.color = new Color(txtText.color.r, txtText.color.g, txtText.color.b, val);
                                });

                        });
                }


                if (_currentLevel < 2)
                {
                    changeToText(_initialText);
                }
                else
                {
                    changeToText(MAX_LEVEL_TEXT);

                }

                BoardController.Instance.UIInteractionsToEnd--;
                _levellingUp = false;
                _isTweening = false;
            });

        }
    }

    private Boolean _isTweening = false;
    private Boolean _levellingUp = false;
    private const Single BAR_TWEENING_TIME = 0.420f;
    private const Single TEXT_TRANSITION_TIME = 1f;
    void Update()
    {
        if (progressBarStatesQueue.Count > 0 && !_isTweening && !_levellingUp)
        {
            _isTweening = true;
            var lastQueuedItem = progressBarStatesQueue.Dequeue();
            //print($"Popped {lastQueuedItem.after} in {name}");
            Single percentage = (Math.Clamp((Single)lastQueuedItem.after, 0, GetMaxProgressPointsForCurrentLevel()) / (Single)GetMaxProgressPointsForCurrentLevel());

            LeanTween.moveLocal(srProgressBar.gameObject, new Vector2(-15f + Math.Clamp((percentage * 14.27f), Single.MinValue, 14.27f), 0), BAR_TWEENING_TIME)
                .setEaseOutSine()
                .setOnComplete(() =>
                {
                    if (percentage < 1f)
                        _isTweening = false;
                });

            if (percentage >= 1f)
            {
                _levellingUp = true;

                //BoardController.Instance.UIInteractionsToEnd++;
                _currentLevel++;
                progressBarStatesQueue.Clear();

                LeanTween.delayedCall(BAR_TWEENING_TIME + 0.1f, () =>
                {
                    _currentProgressPoints = 0;
                    percentage = 0;

                    collider.enabled = true;
                    Single halfPingPong = 0.15f;
                    LeanTween.scale(gameObject, new Vector3(1.05f, 1.05f, 1.05f), halfPingPong)
                        .setLoopPingPong(1);
                    AudioManager.Instance.Play("knowledgeBarFull", SoundCategory.VFX, volume: 0.34f);

                    progressBarStatesQueue.Clear();

                    LeanTween.delayedCall(halfPingPong * 2, () =>
                    {
                        if (collider.enabled)
                            LeanTween.value(1f, 0f, TEXT_TRANSITION_TIME)
                                .setOnUpdate((Single val) =>
                                {
                                    txtText.color = new Color(txtText.color.r, txtText.color.g, txtText.color.b, val);
                                })
                                .setOnComplete(() =>
                                {
                                    txtText.text = "Nowy poziom!";
                                    if (collider.enabled)
                                        LeanTween.value(0f, 1f, TEXT_TRANSITION_TIME)
                                        .setOnUpdate((Single val) =>
                                        {
                                            txtText.color = new Color(txtText.color.r, txtText.color.g, txtText.color.b, val);
                                            HidingCircle.Instance.Show();

                                        })
                                        .setOnComplete(() =>
                                        {
                                            if (collider.enabled)
                                                LeanTween.value(1f, 0f, TEXT_TRANSITION_TIME)
                                                .setOnUpdate((Single val) =>
                                                {
                                                    txtText.color = new Color(txtText.color.r, txtText.color.g, txtText.color.b, val);

                                                })
                                                .setOnComplete(() =>
                                                {
                                                    txtText.text = "Kliknij tutaj aby odebraæ nagrodê!";
                                                    if (collider.enabled)
                                                        LeanTween.value(0f, 1f, TEXT_TRANSITION_TIME)
                                                        .setOnUpdate((Single val) =>
                                                        {
                                                            txtText.color = new Color(txtText.color.r, txtText.color.g, txtText.color.b, val);

                                                        })
                                                        .setOnComplete(() => { });

                                                });
                                        });
                                });
                    });

                });
            }
        }

    }
}
