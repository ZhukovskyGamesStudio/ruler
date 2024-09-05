using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {
    public static int[] previousPams;
    public DeckScript curDeck;

    public Transform[] HandPos;
    public MainMenuButtons MMB;

    public GameObject StashButton;
    public GameObject ProgressHandler;
    public GameObject[] progress;
    public GameObject[] TutImages;
    public DropField DF;
    public int step;
    public int cardCnt;
    private bool corIncor;

    // Переменные внутри скрипта

    [HideInInspector]
    public GameObject[] MainCards;

    [HideInInspector]
    public int correctCnt;

    [HideInInspector]
    public int solvedCnt;

    public void ReCast() {
        MainCards = new GameObject[4];
        previousPams = null;

        StashButton.SetActive(false);
        ProgressHandler.SetActive(false);
        curDeck.GetRule(0);
        for (int i = 0; i < TutImages.Length; i++) {
            TutImages[i].SetActive(false);
        }

        CorrectCounter(false);
        DF.Clear();
        step = 0;
        Step();
        cardCnt = 0;
    }

    public void Step() {
        cardCnt = 0;
        if (step <= TutImages.Length - 1)
            TutImages[step].SetActive(true);
        if (step == 3)
            StashButton.SetActive(true);
        if (step == 4)
            ProgressHandler.SetActive(true);
    }

    public void Hide(int imgIndex) {
        TutImages[imgIndex].SetActive(false);
        step++;
    }

    public void CorrectCounter(bool isTrue) {
        if (isTrue) {
            if (step > 3)
                correctCnt++;

            if (correctCnt == 10) {
                AchievementsManager.GetTheAchivement(AchievementsManager.tutorial_achieve, 100.0f);
                if (PlayerPrefs.GetInt("tutorialComplete") == 1)
                    AchievementsManager.GetTheAchivement(AchievementsManager.tutorialSecond_achieve, 100.0f);
                Skip();
            }
        } else
            correctCnt = 0;

        UpdateProgress();

        if (step < 2) {
            cardCnt++;
            if (cardCnt >= 3)
                Step();
            if (previousPams == null || previousPams[0] == 3)
                FakeStash(0);
            else
                FakeStash(1);
        } else if (step < 3) {
            cardCnt++;
            if (cardCnt >= 4)
                Step();
            if (Random.Range(0, 4) == 0)
                Stash(false);
            else
                FakeStash(Random.Range(0, 2));
        } else if (step < 4) {
            if (isTrue)
                cardCnt++;
            else
                cardCnt = 0;
            if (cardCnt >= 3)
                Step();
            Stash(false);
        } else {
            if (isTrue)
                cardCnt++;
            else
                cardCnt = 0;
            if (step < 5 && cardCnt >= 3)
                Step();
            if (Random.Range(0, 3) == 0)
                Stash(false);
            else
                FakeStash(Random.Range(0, 2));
        }

        corIncor = isTrue;
    }

    public void FakeStash(int card) {
        for (int i = 0; i < HandPos.Length; i++) {
            foreach (Transform child in HandPos[i].transform)
                Destroy(child.gameObject);
            MainCards[i] = Instantiate(curDeck.Cards[card], HandPos[i]);
            MainCards[i].GetComponent<CardScript>().isTutorial = true;
        }
    }

    public void Skip() {
        PlayerPrefs.SetInt("tutorialComplete", 1);
        MMB.To(0);
        GameObject.Find("AudioManager").GetComponent<AudioManager>().AmbientSwitch(-1);
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
            foreach (Transform child in HandPos[i].transform)
                Destroy(child.gameObject);
            MainCards[i] = Instantiate(curDeck.Cards[Random.Range(0, curDeck.Cards.Length)], HandPos[i]);
            MainCards[i].GetComponent<CardScript>().isTutorial = true;
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
}