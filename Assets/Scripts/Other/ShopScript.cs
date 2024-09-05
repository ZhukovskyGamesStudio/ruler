using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour {
    public Text nameText;
    public Transform[] places;

    public Text scoreText;
    public Text cashText;

    public Text costText;
    public AudioSource BuySound;

    public GameObject Towel;
    public GameObject BuyButton;

    public DecksManager DM;
    private int cost;
    private DeckScript deck;
    private int deckIndex;

    public void MakeVitrina(DeckScript tmpDeck, int tmpdeckIndex) {
        BuyButton.SetActive(true);
        ShowTowel(false);
        deck = tmpDeck;
        deckIndex = tmpdeckIndex;
        nameText.text = deck.name;
        scoreText.text = "x" + deck.scoreGet.ToString();
        cashText.text = "x" + deck.cashGet.ToString();
        cost = deck.cost;
        costText.text = cost.ToString() + "®";

        for (int i = 0; i < places.Length; i++) {
            foreach (Transform child in places[i].transform) {
                Destroy(child.gameObject);
            }

            GameObject a = Instantiate(deck.Cards[Random.Range(0, deck.Cards.Length)], places[i]);
            a.GetComponent<Image>().raycastTarget = false;
        }
    }

    public void Buy() {
        if (DM.bucksCnt >= cost) {
            BuyButton.SetActive(false);
            DM.Decks[deckIndex].isBought = true;
            DM.bucksCnt -= cost;
            BuySound.Play();
            DM.ReSaveDecks();
            DM.Recast();
        } else {
            ShowTowel(true);
        }
    }

    public void ShowTowel(bool IsOpen) {
        Towel.SetActive(IsOpen);
    }
}