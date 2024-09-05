using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollarScr : MonoBehaviour {
    private MainMenuButtons MMB;
    public DeckScript deck;
    public int deckIndex;

    private void Start() {
        MMB = GameObject.Find("Main Camera").GetComponent<MainMenuButtons>();
    }

    public void ToShop() {
        MMB.To(8);
        GameObject.Find("BuyDeckCanvas").GetComponent<ShopScript>().MakeVitrina(deck, deckIndex);
    }
}