using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Xscript : MonoBehaviour {
    public DeckType DeckType;
    public Text Xtext;
    public Image CardImg;

    public void DisEnable() {
        bool res = DecksManager.Instance.TryToggleDeck(DeckType);
        Change(res);
    }

    public void Change(bool isEnabled) {
        Color tmp = gameObject.GetComponent<Image>().color;
        CardImg.color = new Color(tmp.r, tmp.g, tmp.b, isEnabled ? 1f : 100f / 255f);
        Xtext.text = isEnabled ? "" : "x";
    }
}