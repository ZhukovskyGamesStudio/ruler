using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropField : MonoBehaviour {
    public static bool inside;

    public Transform Scroll;
    public Transform CorrectCards;
    public Transform WrongCards;

    public GameObject PlacePrefab;
    public GameObject LastPlace;
    public GameObject EmptySpacePrefab;
    public GameObject EmptySpace;

    public Image DFImage;

    public AudioSource PaperSource;

    public AudioClip PaperSound;
    /**********/

    public void In() {
        inside = true;
    }

    public void Out() {
        inside = false;
    }

    public void InsertCard(Transform card, bool isItCorrect) {
        PaperSource.PlayOneShot(PaperSound);
        if (isItCorrect) {
            card.SetParent(CorrectCards.transform);
            card.localRotation = new Quaternion(0, 0, 0, 1);

            LastPlace = Instantiate(PlacePrefab, WrongCards);
            EmptySpace.transform.SetAsLastSibling();

            if (GetComponent<GameManager>())
                GameManager.previousPams = card.GetComponent<CardScript>().Pams;
            else
                TutorialScript.previousPams = card.GetComponent<CardScript>().Pams;
        } else {
            card.SetParent(LastPlace.transform);
            card.localRotation = new Quaternion(0, 0, Random.Range(-0.3f, 0.3f) / Mathf.PI, 1);

            card.SetAsFirstSibling();
            LastPlace.GetComponent<PlaceScript>().Count();
        }

        card.localPosition = new Vector3(0, 0, 0);
        if (GetComponent<GameManager>())
            GetComponent<GameManager>().CorrectCounter(isItCorrect);
        else
            GetComponent<TutorialScript>().CorrectCounter(isItCorrect);
        Scroll.GetComponent<Scroll>().Revert();
        card.gameObject.GetComponent<EventTrigger>().enabled = false;
    }

    public void Clear() {
        foreach (Transform child in CorrectCards)
            Destroy(child.gameObject);
        foreach (Transform child in WrongCards)
            Destroy(child.gameObject);
        LastPlace = Instantiate(PlacePrefab, WrongCards);
        EmptySpace = Instantiate(EmptySpacePrefab, CorrectCards);
    }
}