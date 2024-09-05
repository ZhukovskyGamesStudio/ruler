using UnityEngine;
using UnityEngine.UI;

public class Translate : MonoBehaviour {
    // Use this for initialization
    void Start() {
        Text text = gameObject.GetComponent<Text>();
        if (text) text.text = TranslateManager.Translate(text.text);
    }
}