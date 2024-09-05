using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DecksManager : MonoBehaviour {
    public GameObject X;
    public GameObject Dollar;

    public Text ScoreText;
    public Text CashText;
    public Text WalletText;

    public Transform CardsHandler;
    public Toggle toggle;
    public AudioSource SwitchSound;
    public DeckScript[] Decks;

    /**********/

    [HideInInspector]
    public int decksOnAmount;

    [HideInInspector]
    public int bucksCnt;

    [HideInInspector]
    public float scoreCnt;

    [HideInInspector]
    public float cashCnt;

    private GameObject[] Cards;
    Save sv;

    private void Start() {
        Cards = new GameObject[Decks.Length];
        ReLoadDecks();
    }

    public void Recast() {
        foreach (Transform child in CardsHandler.transform)
            Destroy(child.gameObject);

        Cards = new GameObject[Decks.Length];

        bool onlyBought = toggle.isOn;

        for (int i = 0; i < Decks.Length; i++) {
            if (onlyBought) {
                if (Decks[i].isBought) {
                    Cards[i] = Instantiate(Decks[i].Cards[Random.Range(0, Decks[i].Cards.Length)], CardsHandler);
                }
            } else {
                Cards[i] = Instantiate(Decks[i].Cards[Random.Range(0, Decks[i].Cards.Length)], CardsHandler);

                if (!Decks[i].isBought) {
                    GameObject d = Instantiate(Dollar, Cards[i].transform);
                    d.GetComponent<DollarScr>().deck = Decks[i];
                    d.GetComponent<DollarScr>().deckIndex = i;
                    Cards[i].GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
                }
            }

            if (Cards[i]) {
                Cards[i].GetComponent<CardScript>().enabled = false;
                Cards[i].GetComponent<EventTrigger>().enabled = false;
                if (Decks[i].isBought) {
                    GameObject x = Instantiate(X, Cards[i].transform);
                    Xscript xsc = x.GetComponent<Xscript>();
                    xsc.deckIndex = i;
                    xsc.CardImg = Cards[i].GetComponent<Image>();
                    xsc.Change(Decks[i].IsEnb);
                }
            }
        }

        if (decksOnAmount > 3)
            AchievementsManager.GetTheAchivement(AchievementsManager.decks3_achieve, 100.0f);
        if (decksOnAmount > 5)
            AchievementsManager.GetTheAchivement(AchievementsManager.decks5_achieve, 100.0f);
        if (decksOnAmount > 7)
            AchievementsManager.GetTheAchivement(AchievementsManager.decks7_achieve, 100.0f);
        if (decksOnAmount > 10)
            AchievementsManager.GetTheAchivement(AchievementsManager.decks10_achieve, 100.0f);

        ReCount();
    }

    public void ReCount() {
        decksOnAmount = 0;
        scoreCnt = 0;
        cashCnt = 0;
        for (int i = 0; i < Decks.Length; i++) {
            if (Decks[i].isBought && Decks[i].IsEnb) {
                decksOnAmount++;
                scoreCnt += Decks[i].scoreGet;
                cashCnt += Decks[i].cashGet;
            }
        }

        ScoreText.text = "x" + scoreCnt.ToString();
        CashText.text = "x" + cashCnt.ToString();
        WalletText.text = bucksCnt.ToString();
    }

    public void ReLoadDecks() {
        sv = SaveManager.Load();

        DeckData[] tmpDatas = new DeckData[Decks.Length];
        for (int i = 0; i < tmpDatas.Length; i++) {
            tmpDatas[i] = new DeckData(false, false);
            if (i < sv.deckDatas.Length)
                tmpDatas[i] = new DeckData(sv.deckDatas[i].isBought, sv.deckDatas[i].isEnabled);
        }

        sv.deckDatas = tmpDatas;

        for (int i = 0; i < Decks.Length; i++) {
            Decks[i].isBought = tmpDatas[i].isBought;
            Decks[i].IsEnb = tmpDatas[i].isEnabled;
        }

        bucksCnt = sv.bucks;
        Recast();
    }

    public void ReSaveDecks() {
        for (int i = 0; i < sv.deckDatas.Length; i++) {
            sv.deckDatas[i].isBought = Decks[i].isBought;
            sv.deckDatas[i].isEnabled = Decks[i].IsEnb;
        }

        sv.bucks = bucksCnt;
        SaveManager.Save(false, sv);
    }

    public void SwitchDeckSound() {
        SwitchSound.Play();
    }
}