﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class DeckScript {
    public int[] PamsVariety;
    public List<GameObject> Cards;

    protected DeckConfig _deckConfig;
    /**********/

    protected int[] numbers;
    protected int step;
    protected int sequenceIndex;

    public DeckScript(DeckConfig config) {
        _deckConfig = config;
        Cards = config.Cards.ToList();
        PamsVariety = config.PamsVariety.ToArray();
    }

    public void GenerateNumbers() {
        numbers = new int[10];
        for (int i = 0; i < numbers.Length; i++)
            numbers[i] = Random.Range(5, 25);
    }

    public void GetRule(int seed) {
        sequenceIndex = Random.Range(0, 100) + seed;

        numbers = new int[10];
        for (int i = 0; i < numbers.Length; i++)
            numbers[i] = Random.Range(10, 50);

        step = 0;
    }

    public abstract bool CheckRule(int[] Pams, int[] PreviousPams, bool isStepping = false, int seed = 0);

    protected void Log(string smth, bool showStep = false) {
        Debug.Log(smth + (showStep ? (", Step: " + step) : ""));
    }
}