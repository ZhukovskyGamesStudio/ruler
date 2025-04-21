using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelsManager : MonoBehaviour {
    [SerializeField]
    private Transform[] _panels;
    [SerializeField]
    private RectTransform[] _panelsR;
    [SerializeField]
    private RectTransform _panelsContainer;

    [SerializeField]
    private CanvasScaler _canvasScaler;
    
    private int _curCanvas;
    private List<Vector2> _initialPoses = new List<Vector2>();
    private List<Vector2> _calculatedPoses = new List<Vector2>();
    private Vector2 _screenSize;
    private void Awake() {
        CalculateInitialPoses();
        RecalculatePoses();
    }

    public void DisablePanels() {
        foreach (RectTransform t in _panelsR) {
            t.gameObject.SetActive(false);
        }
    }

    private void CalculateInitialPoses() {
        _initialPoses.Clear();
        for (int index = 0; index < _panelsR.Length; index++) {
            RectTransform panelTransform = _panelsR[index];
            panelTransform.anchoredPosition *= 5;
            _initialPoses.Add(panelTransform.anchoredPosition);
        }
    }

    private void RecalculatePoses() {
        Vector2 referenceRect = _canvasScaler.referenceResolution;
        _screenSize = new Vector2(Screen.width, Screen.height);
        
        _calculatedPoses.Clear();
        for (int index = 0; index < _initialPoses.Count; index++) {
            Vector2 pose = _initialPoses[index];
            Vector2 redrawn = new Vector2(pose.x / referenceRect.x * _screenSize.x, pose.y / referenceRect.y * _screenSize.y);
            _panelsR[index].anchoredPosition = redrawn;
            _calculatedPoses.Add(redrawn);
        }
        _panelsContainer.anchoredPosition = _calculatedPoses[_curCanvas] * -1;
    }

    private void Update() {
        if(!Mathf.Approximately(_screenSize.x, Screen.width) || !Mathf.Approximately(_screenSize.y, Screen.height)) {
            RecalculatePoses();
        }
    }

    public void DisablePanel(int index) {
        _panels[index].gameObject.SetActive(false);
    }

    public void To(int panelIndex) {
        _panels[panelIndex].gameObject.SetActive(true);

        TryInitNextPanel(panelIndex);

        _panels[panelIndex].gameObject.SetActive(true);

        Vector2 fin = _calculatedPoses[panelIndex] * -1;
        StartCoroutine(CamMovement(fin, panelIndex));
    }

    private void TryInitNextPanel(int panelIndex) {
        if (panelIndex == 7) {
            DecksManager.Instance.ReLoadDecks();
        }
        if(_panels[panelIndex].GetComponent<Panel>() != null) {
            _panels[panelIndex].GetComponent<Panel>().Init();
        }

        if (panelIndex == 9)
            _panels[9].GetComponent<TutorialScript>().ReCast();
    }

    private IEnumerator CamMovement(Vector3 finPos, int nextIndex) {
        finPos.z = 0;
        Vector3 startPos = _panelsContainer.anchoredPosition;
        float moveTime = 0.3f;
        float time = 0;
        while (time < moveTime) {
            _panelsContainer.anchoredPosition = Vector2.Lerp(startPos, finPos, time / moveTime);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (_curCanvas != nextIndex && nextIndex != 2)
            _panels[_curCanvas].gameObject.SetActive(false);
        _curCanvas = nextIndex;
        _panelsContainer.anchoredPosition = finPos;

        yield return false;
    }
    
    public void TeleportTo(int panelIndex) {
        if (_curCanvas != panelIndex && panelIndex != 2) {
            _panels[_curCanvas].gameObject.SetActive(false);
        }
        TryInitNextPanel(panelIndex);
        
        Vector2 fin = _calculatedPoses[panelIndex] * -1;
        _panelsContainer.anchoredPosition = fin;
        _panels[panelIndex].gameObject.SetActive(true);
    }
}