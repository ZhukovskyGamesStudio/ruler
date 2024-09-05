using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Xscript : MonoBehaviour {
    public int deckIndex;
    public Text Xtext;
    public Image CardImg;
    DecksManager DM;

    private void Start() {
        DM = GameObject.Find("DecksCanvas").GetComponent<DecksManager>();
    }

    public void DisEnable() {
        bool isEnabled = !DM.Decks[deckIndex].IsEnb;

        if ((DM.decksOnAmount > 2 && !isEnabled) || isEnabled) {
            DM.SwitchDeckSound();
            DM.Decks[deckIndex].IsEnb = isEnabled;
            Change(isEnabled);
            DM.ReCount();
            DM.ReSaveDecks();
        }
    }

    public void Change(bool isEnabled) {
        Color tmp = gameObject.GetComponent<Image>().color;
        CardImg.color = new Color(tmp.r, tmp.g, tmp.b, isEnabled ? 1f : 100f / 255f);
        Xtext.text = isEnabled ? "" : "x";
    }
}