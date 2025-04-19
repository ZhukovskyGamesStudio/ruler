using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class DecksManager : MonoBehaviour {
    public GameObject X;

    [SerializeField]
    private DollarScr _dollarButtonPrefab;

    public Text ScoreText;
    public Text CashText;
    public Text WalletText;

    public Transform CardsHandler;
    public Toggle toggle;
    public AudioSource SwitchSound;
    public DeckScript[] Decks;
    public DecksTableConfig DecksTableConfig;
    /**********/

    [HideInInspector]
    public int decksOnAmount;

    [HideInInspector]
    public float scoreCnt;

    [HideInInspector]
    public float cashCnt;

    private List<GameObject> _cards;

    public static DecksManager Instance;

    private void Awake() {
        Instance = this;
        SaveManager.Init(DecksTableConfig);
    }

    private void Start() {
        ReLoadDecks();
    }

    public void Recast() {
        foreach (Transform child in CardsHandler.transform) {
            Destroy(child.gameObject);
        }

        _cards = new List<GameObject>();

        bool onlyBought = toggle.isOn;

        foreach (var deckConfig in DecksTableConfig.Decks) {
            DeckData data = SaveManager.SaveData.DeckDatas.First(d => d.DeckType == deckConfig.DeckType);
            if (onlyBought && !data.isBought) {
                continue;
            }

            GameObject card = Instantiate(deckConfig.Cards[Random.Range(0, deckConfig.Cards.Length)], CardsHandler);
            _cards.Add(card);

            if (!data.isBought) {
                DollarScr d = Instantiate(_dollarButtonPrefab, card.transform);
                d.SetData(deckConfig);
                card.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
            }

            if (card) {
                card.GetComponent<CardScript>().enabled = false;
                card.GetComponent<EventTrigger>().enabled = false;
                if (data.isBought) {
                    GameObject x = Instantiate(X, card.transform);
                    Xscript xsc = x.GetComponent<Xscript>();
                    xsc.DeckType = deckConfig.DeckType;
                    xsc.CardImg = card.GetComponent<Image>();
                    xsc.Change(data.isEnabled);
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
        List<DeckType> enabledDecks = GetEnabledDecks();
        foreach (DeckConfig deckConfig in DecksTableConfig.Decks.Where(deckConfig => enabledDecks.Contains(deckConfig.DeckType))) {
            decksOnAmount++;
            scoreCnt += deckConfig.scoreGet;
            cashCnt += deckConfig.cashGet;
        }

        ScoreText.text = "x" + scoreCnt.ToString();
        CashText.text = "x" + cashCnt.ToString();
        WalletText.text = SaveManager.SaveData.bucks.ToString();
    }

    public void ReLoadDecks() {
        Recast();
    }

    public void BuyDeck(DeckType deckType) {
        DeckData d = SaveManager.SaveData.DeckDatas.First(d => d.DeckType == deckType);
        d.isBought = true;
        d.isEnabled = true;
        SaveManager.Save();
    }

    public bool TryToggleDeck(DeckType deckType) {
        bool isEnabled = IsDeckEnabled(deckType);
        if ((decksOnAmount <= 2 || isEnabled) && !isEnabled) {
            return IsDeckEnabled(deckType);
        }

        DeckData data = SaveManager.SaveData.DeckDatas.First(d => d.DeckType == deckType);
        data.isEnabled = !data.isEnabled;
        SwitchDeckSound();
        ReCount();
        SaveManager.Save();
        return IsDeckEnabled(deckType);
    }

    public bool IsDeckEnabled(DeckType deckType) {
        DeckData data = SaveManager.SaveData.DeckDatas.First(d => d.DeckType == deckType);
        return data.isEnabled;
    }

    public List<DeckType> GetEnabledDecks() {
        return SaveManager.SaveData.DeckDatas.Where(d => d.isEnabled).Select(d => d.DeckType).ToList();
    }

    public List<DeckConfig> GetEnabledDecksConfigs() {
        return DecksTableConfig.Decks.Where(deckConfig => SaveManager.SaveData.DeckDatas.Any(d => d.DeckType == deckConfig.DeckType && d.isEnabled)).ToList();
    }

    private void SwitchDeckSound() {
        SwitchSound.Play();
    }
}