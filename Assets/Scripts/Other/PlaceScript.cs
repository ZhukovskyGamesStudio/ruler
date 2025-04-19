using UnityEngine;

public class PlaceScript : MonoBehaviour {
    private GameObject[] _children;
    private int _curChildren;

    /**********/
    public void Start() {
        _children = new GameObject[5];
    }

    public void Change() {
        if (transform.childCount > 0) {
            _curChildren = (_curChildren + 1) % transform.childCount;
            _children[_curChildren].transform.SetAsFirstSibling();
        }
    }

    public void Count() {
        _curChildren = 0;
        int i = 0;
        foreach (Transform child in transform) {
            if (i >= 5)
                Destroy(child.gameObject);
            else
                _children[i] = child.gameObject;
            i++;
        }
    }
}