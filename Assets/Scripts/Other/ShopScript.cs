using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopScript : MonoBehaviour {
    public Text nameText;
    public Transform[] places;

    public Text scoreText;
    public Text cashText;

    public Text costText;
    public AudioSource BuySound;

    public GameObject Towel;
    public GameObject BuyButton;
    
    private int cost;
    private DeckConfig _deck;
    private int _selectedDeckIndex;

    public static ShopScript Instance;

    private void Awake() {
        Instance = this;
    }

    public void OpenBuyOffer(DeckConfig tmpDeck) {
        BuyButton.SetActive(true);
        ShowTowel(false);
        _deck = tmpDeck;
        nameText.text = _deck.name;
        scoreText.text = "x" + _deck.scoreGet.ToString();
        cashText.text = "x" + _deck.cashGet.ToString();
        cost = _deck.cost;
        costText.text = cost.ToString() + "®";

        foreach (Transform t in places) {
            foreach (Transform child in t.transform) {
                Destroy(child.gameObject);
            }

            GameObject a = Instantiate(_deck.Cards[Random.Range(0, _deck.Cards.Length)], t);
            a.GetComponent<Image>().raycastTarget = false;
        }
    }

    public void Buy() {
        if (SaveManager.SaveData.bucks >= cost) {
            BuyButton.SetActive(false);
            DecksManager.Instance.BuyDeck(_deck.DeckType);
            SaveManager.SaveData.bucks -= cost;
            BuySound.Play();
            SaveManager.Save();
            DecksManager.Instance.Recast();
        } else {
            ShowTowel(true);
        }
    }

    public void ShowTowel(bool IsOpen) {
        Towel.SetActive(IsOpen);
    }
}