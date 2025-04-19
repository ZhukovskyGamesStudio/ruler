using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour {
    public int[] Pams;

    public bool areYouSpecial;
    public bool isTutorial;

    // Переменные внутри скрипта

    DeckScript Deck;

    [HideInInspector]
    public bool CanBeMoved;

    [HideInInspector]
    public bool CanBeClicked;

    [HideInInspector]
    public bool isCorrect;

    [HideInInspector]
    public bool withBuck;

    Vector3 delta;
    Vector3 startPosition;
    Quaternion startRotation;

    RectTransform rt;
    Image img;
    DropField DF;
    Image DFImage;
    Transform curCanvas;

    GameObject GameCanvas;
    /**********/

    public void Start() {
        GameCanvas = GameObject.Find("GameCanvas");
        GameObject TutorialCanvas = GameObject.Find("TutorialCanvas");
        if (!areYouSpecial && (GameCanvas || TutorialCanvas)) {
            if (isTutorial) {
                curCanvas = TutorialCanvas.transform;
                Deck = DeckRuleFactory.GetRuleByType(TutorialCanvas.GetComponent<TutorialScript>().TutorialDeck);
                DF = TutorialCanvas.GetComponent<DropField>();
            } else {
                curCanvas = GameCanvas.transform;
                Deck = GameCanvas.GetComponent<CoreManager>().curDeck;
                DF = GameCanvas.GetComponent<DropField>();
            }

            DFImage = DF.DFImage;
            DFImage.raycastTarget = false;
        }

        rt = GetComponent<RectTransform>();
        img = GetComponent<Image>();
    }

    public bool Check(bool isStepping) {
        if (isTutorial)
            return Deck.CheckRule(Pams, TutorialScript.previousPams, isStepping);
        else
            return Deck.CheckRule(Pams, CoreManager.previousPams, isStepping);
    }

    public void OnClick() {
        if (!areYouSpecial && CanBeClicked)
            transform.parent.GetComponent<PlaceScript>().Change();
    }

    public void Down() {
        if (!areYouSpecial && CanBeMoved) {
            startPosition = rt.position;
            startRotation = rt.rotation;
            DFImage.raycastTarget = true;
            rt.rotation = new Quaternion(0, 0, 0, 1);
            img.raycastTarget = false;
        }
    }

    public void Drag() {
        if (!areYouSpecial && CanBeMoved)
            rt.position = new Vector3(Input.mousePosition.x * 480 / Screen.width, Input.mousePosition.y * 800 / Screen.height) +
                curCanvas.position - new Vector3(240, 400, 0);
    }

    public void Drop() {
        if (!areYouSpecial && CanBeMoved) {
            if (DropField.inside) {
                CanBeMoved = false;
                bool temp = Check(true);
                DF.InsertCard(gameObject.transform, temp);
                if (!temp)
                    CanBeClicked = true;
                if (withBuck) {
                    GameCanvas.GetComponent<CoreManager>().СollectedBuck(1);

                    if (transform.childCount > 1)
                        Destroy(transform.GetChild(1).gameObject);
                    else
                        Destroy(transform.GetChild(0).gameObject);
                }
            } else {
                CanBeClicked = false;
                rt.position = startPosition;
                rt.rotation = startRotation;
            }

            img.raycastTarget = true;
            DFImage.raycastTarget = false;
        }
    }
}