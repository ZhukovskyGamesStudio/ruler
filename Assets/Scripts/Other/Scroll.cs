using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour {
    public Transform CorrectScrollTransform;
    public Transform wrongCards;
    public Transform correctCards;
    public ScrollRect ScrollRectObj;

    /**********/

    public void Revert() {
        StopCoroutine(GentleRevert());
        StartCoroutine(GentleRevert());
    }

    public void Clear() {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    public void WrongCardsMover() {
        wrongCards.position = new Vector3(correctCards.position.x, wrongCards.position.y, wrongCards.position.z);
    }

    public IEnumerator GentleRevert() {
        float was = ScrollRectObj.horizontalNormalizedPosition;
        float time = 0;
        float MaxTime = 0.2f;
        while (time <= MaxTime) {
            time += Time.deltaTime;
            ScrollRectObj.horizontalNormalizedPosition = was + (1 - was) * time / MaxTime;
            yield return new WaitForEndOfFrame();
        }

        ScrollRectObj.horizontalNormalizedPosition = 1;
        yield return null;
    }
}