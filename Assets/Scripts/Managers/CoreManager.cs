﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CoreManager : MonoBehaviour {
    public static int[] previousPams;

    public Transform EndGameCanvas;
    public Transform[] HandPos;

    public Text scoreText;
    public Text cashText;
    public GameObject DoubleButton;
    public GameObject AlreadyDoubledText;

    public Text quoteText;
    public Text timeText;

    public GameObject BuckPrefab;
    public GameObject BackButton;
    public GameObject PauseButton;
    public GameObject[] progress;

    public DropField DF;
    private DeckScript[] Decks;

    public AudioSource BuckCollected;
    public AudioSource DeckDone;

    [Space(20)]
    [Min(1)]
    public int time;

    [Min(1)]
    public int addtime;

    [Min(1)]
    public float buckChance;

    // Переменные внутри скрипта
    [HideInInspector]
    public DeckScript curDeck;

    [HideInInspector]
    public GameObject[] MainCards;

    [HideInInspector]
    public int correctCnt;

    [HideInInspector]
    public int scoreCnt;

    [HideInInspector]
    public int solvedDecksCnt;

    [HideInInspector]
    public int bucksEarned;

    [HideInInspector]
    public int timeSpent;

    [HideInInspector]
    public int seconds;

    [HideInInspector]
    public bool zenBl;

    private float scoreMultiplayer;

    private int falseCnt;

    /**********/
    public void Start() {
        DF = GetComponent<DropField>();
        MainCards = new GameObject[4];
    }

    public void CreateLevel(bool zen) {
        zenBl = zen;
        StopAllCoroutines();

        Decks = DecksManager.Instance.Decks;
        Reload();
        Restart(true);

        if (zen) {
            seconds = 0;
            addtime = 0;
            BackButton.SetActive(true);
            PauseButton.SetActive(false);
        } else {
            timeSpent = 0;
            scoreCnt = 0;
            addtime = 30;
            seconds = 60;

            PauseButton.SetActive(true);
            BackButton.SetActive(false);
        }

        StartCoroutine(Timer());
    }

    public void СollectedBuck(int value) {
        BuckCollected.Play();

        bucksEarned += value;

        StartCoroutine(QuoteShow(2));
    }

    public void CorrectCounter(bool isTrue) {
        if (isTrue) {
            StartCoroutine(QuoteShow(3));
            correctCnt++;
            if (correctCnt == 10) {
                if (falseCnt == 0)
                    AchievementsManager.GetTheAchivement(AchievementsManager.bornLucky_achieve, 100.0f);
                falseCnt = 0;
                FinishSequence();
            }
        } else {
            correctCnt = 0;
            falseCnt++;
        }

        UpdateProgress();
        Stash(false);
    }

    public void FinishSequence() {
        int rulesDone;
        if (PlayerPrefs.HasKey("rulesDone"))
            rulesDone = PlayerPrefs.GetInt("rulesDone");
        else
            rulesDone = 0;
        rulesDone += 1;

        AchievementsManager.GetTheAchivement(AchievementsManager.rules10_achieve, rulesDone / 10.0f > 1 ? 100.0f : 100.0f * rulesDone / 10.0f);
        AchievementsManager.GetTheAchivement(AchievementsManager.rules50_achieve, rulesDone / 50.0f > 1 ? 100.0f : 100.0f * rulesDone / 50.0f);
        AchievementsManager.GetTheAchivement(AchievementsManager.rules250_achieve,
            rulesDone / 250.0f > 1 ? 100.0f : 100.0f * rulesDone / 250.0f);
        AchievementsManager.GetTheAchivement(AchievementsManager.rules1000_achieve,
            rulesDone / 1000.0f > 1 ? 100.0f : 100.0f * rulesDone / 1000.0f);

        PlayerPrefs.SetInt("rulesDone", rulesDone);

        scoreCnt += (int)(seconds * scoreMultiplayer);
        seconds += addtime;
        timeText.text = (seconds / 60 < 10 ? "0" : "") + seconds / 60 + " : " + (seconds % 60 < 10 ? "0" : "") + seconds % 60;

        DeckDone.Play();

        Restart(false);
    }

    private void OnApplicationQuit() {
        EndGame();
    }

    public void EndGame() {
        MainMenuButtons.Instance.Canvases[1].gameObject.SetActive(false);

        Record curRec = new Record(scoreCnt, timeSpent);

        AlreadyDoubledText.SetActive(false);
        DoubleButton.SetActive(true);
        scoreText.text = scoreCnt.ToString();
        cashText.text = bucksEarned + "®";

        MainMenuButtons.Instance.SendRecord(scoreCnt);
        if (scoreCnt > SaveManager.SaveData.records[2].score) {
            if (curRec.score > SaveManager.SaveData.records[1].score) {
                if (curRec.score > SaveManager.SaveData.records[0].score) {
                    SaveManager.SaveData.records[2] = SaveManager.SaveData.records[1];
                    SaveManager.SaveData.records[1] = SaveManager.SaveData.records[0];
                    SaveManager.SaveData.records[0] = curRec;
                } else {
                    SaveManager.SaveData.records[2] = SaveManager.SaveData.records[1];
                    SaveManager.SaveData.records[1] = curRec;
                }
            } else
                SaveManager.SaveData.records[2] = curRec;
        }

        SaveManager.SaveData.bucks += bucksEarned;

        SaveManager.Save();
        MainMenuButtons.Instance.UpdateRecords();
        MainMenuButtons.Instance.To(3);
    }

    public void DoubleBucks() {
        // MMB.WatchDoubleAd();
    }

    public void OnUserDoubledReward(object sender, System.EventArgs args) {
        int bucks = PlayerPrefs.GetInt("bucks");
        bucks += bucksEarned;
        PlayerPrefs.SetInt("bucks", bucks);
        cashText.text = (bucksEarned * 2).ToString() + "®";
        AlreadyDoubledText.SetActive(true);
        DoubleButton.SetActive(false);
    }

    public void Restart(bool first) {
        List<DeckConfig> decks = DecksManager.Instance.GetEnabledDecksConfigs();
        List<DeckScript> decksInGame = decks.Select(DeckRuleFactory.GetRuleByType).ToList();

        falseCnt = 0;
        curDeck = decksInGame[Random.Range(0, decksInGame.Count)];
        curDeck.GetRule(Random.Range(50, 150));

        StopCoroutine(QuoteShow(first ? 0 : 1));
        StartCoroutine(QuoteShow(first ? 0 : 1));

        CorrectCounter(false);
        DF.Clear();
        previousPams = null;
    }

    public void Reload() {
        buckChance = 0;
        List<DeckType> decks = DecksManager.Instance.GetEnabledDecks();
        foreach (DeckConfig variable in DecksManager.Instance.DecksTableConfig.Decks.Where(variable => decks.Contains(variable.DeckType))) {
            buckChance += variable.cashGet;
            scoreMultiplayer += variable.scoreGet;
        }
    }

    public void Stash(bool isPunished) {
        if (isPunished) {
            for (int i = 0; i < HandPos.Length; i++) {
                if (MainCards[i].GetComponent<CardScript>().Check(false)) {
                    correctCnt = 0;
                    UpdateProgress();
                }
            }
        }

        for (int i = 0; i < HandPos.Length; i++) {
            foreach (Transform child in HandPos[i].transform) {
                Destroy(child.gameObject);
            }

            MainCards[i] = Instantiate(curDeck.Cards[Random.Range(0, curDeck.Cards.Count)], HandPos[i]);
            //if (!zenBl) {
            if (Random.Range(0, 100) <= buckChance) {
                Instantiate(BuckPrefab, MainCards[i].transform);
                MainCards[i].GetComponent<CardScript>().withBuck = true;
            }
            //}
        }
    }

    public void UpdateProgress() {
        for (int i = 0; i < progress.Length; i++) {
            if (i < correctCnt)
                progress[i].SetActive(true);
            else
                progress[i].SetActive(false);
        }
    }

    public void StartCors() {
        StartCoroutine(Timer());
    }

    public IEnumerator Timer() {
        if (zenBl) {
            while (true) {
                timeText.text = (seconds / 60 < 10 ? "0" : "") + seconds / 60 + " : " + (seconds % 60 < 10 ? "0" : "") + seconds % 60;
                seconds++;
                yield return new WaitForSeconds(1);
            }
        } else {
            while (seconds > 0) {
                timeText.text = (seconds / 60 < 10 ? "0" : "") + seconds / 60 + " : " + (seconds % 60 < 10 ? "0" : "") + seconds % 60;
                timeSpent++;
                seconds--;
                yield return new WaitForSeconds(1);
            }

            EndGame();
        }

        yield return false;
    }

    public IEnumerator QuoteShow(int what) {
        if (what == 3 && Random.Range(0, 3) != 0)
            yield break;
        switch (what) {
            case 0:
                switch (Random.Range(0, 6)) {
                    case 0:
                        quoteText.text = "START!";
                        break;
                    case 1:
                        quoteText.text = "Let's begin!";
                        break;
                    case 2:
                        quoteText.text = "GO!";
                        break;
                    case 3:
                        quoteText.text = "FIRE!";
                        break;
                    case 4:
                        quoteText.text = "!END.";
                        break;
                    case 5:
                        quoteText.text = "Kick off!";
                        break;
                }

                break;
            case 1:
                switch (Random.Range(0, 6)) {
                    case 0:
                        if (!zenBl)
                            quoteText.text = "No time, watch the timer!";
                        else
                            quoteText.text = "Timer is your friend today!";
                        break;
                    case 1:
                        quoteText.text = "Masterpiece of reasoning!";
                        break;
                    case 2:
                        if (!zenBl)
                            quoteText.text = "Inductive genius with " + scoreCnt.ToString() + " points!";
                        else
                            quoteText.text = "Inductive genius!";
                        break;
                    case 3:
                        if (!zenBl)
                            quoteText.text = "Faster, faster, FASTER!";
                        else
                            quoteText.text = "Zen, zeeen, zeeeeeen~";
                        break;
                    case 4:
                        if (!zenBl)
                            quoteText.text = "Let the " + scoreCnt.ToString() + " points be your inspiration!";
                        else
                            quoteText.text = "Let Zen be your inspiration!";
                        break;
                    case 5:
                        if (!zenBl)
                            quoteText.text = scoreCnt.ToString() + " points are the reason to increase self-esteem.";
                        else
                            quoteText.text = "Zen is the reason to increase self-esteem.";
                        break;
                }

                break;
            case 2:
                switch (Random.Range(0, 7)) {
                    case 0:
                        quoteText.text = bucksEarned.ToString() + "® in my pockets, " + bucksEarned.ToString() + "®...";
                        break;
                    case 1:
                        quoteText.text = "Cash cash ca-a-ash!!";
                        break;
                    case 2:
                        quoteText.text = "Can I invest them? ";
                        break;
                    case 3:
                        quoteText.text = bucksEarned.ToString() + "® — nothing is better than a fresh bill.";
                        break;
                    case 4:
                        quoteText.text = bucksEarned.ToString() + "® — already ®illionare.";
                        break;
                    case 5:
                        quoteText.text = "® -> ®® -> ®®®®!";
                        break;
                    case 6:
                        quoteText.text = "Is it just a small bill card?.. Nah.";
                        break;
                }

                break;
            case 3:
                switch (Random.Range(0, 7)) {
                    case 0:
                        quoteText.text = "You're doing awesome!";
                        break;
                    case 1:
                        quoteText.text = "Right, right, aaaaand right!";
                        break;
                    case 2:
                        quoteText.text = "But mistakes are cooler actually.";
                        break;
                    case 3:
                        quoteText.text = "You have ideas, don't you?";
                        break;
                    case 4:
                        quoteText.text = "Pretty inductive.";
                        break;
                    case 5:
                        quoteText.text = "Don't rely much on the first thought.";
                        break;
                    case 6:
                        quoteText.text = "Be wise and screw this up instead!";
                        break;
                }

                break;
        }

        quoteText.color = new Color(quoteText.color.r, quoteText.color.g, quoteText.color.b, 1);
        float time = 0;
        while (time < 5f) {
            time += Time.deltaTime;
            quoteText.color = new Color(quoteText.color.r, quoteText.color.g, quoteText.color.b, (5f - time) / 5f);
            yield return new WaitForEndOfFrame();
        }

        yield return false;
    }
}

[System.Serializable]
public class Simplifier {
    public bool returnTru = false;
    public bool noRandom = false;
    public int ruleIndex = 0;
    public int caseIndex = 0;
}