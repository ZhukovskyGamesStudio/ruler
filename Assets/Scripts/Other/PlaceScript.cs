using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceScript : MonoBehaviour {
    GameObject[] children;
    int curChildren;

    /**********/
    public void Start() {
        children = new GameObject[5];
    }

    public void Change() {
        if (transform.childCount > 0) {
            curChildren = (curChildren + 1) % transform.childCount;
            children[curChildren].transform.SetAsFirstSibling();
        }
    }

    public void Count() {
        curChildren = 0;
        int i = 0;
        foreach (Transform child in transform) {
            if (i >= 5)
                Destroy(child.gameObject);
            else
                children[i] = child.gameObject;
            i++;
        }
    }
}