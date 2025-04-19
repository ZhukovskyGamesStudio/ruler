using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "DeckConfig", menuName = "Scriptable Objects/DeckConfig")]
public class DeckConfig : ScriptableObject {
    public DeckType DeckType;
    public int[] PamsVariety;
    public GameObject[] Cards;

    [Space(20)]
    public new string name;

    public int cost;
    public float scoreGet;
    public float cashGet;
    public bool IsAlreadyAvailable;

    /**********/

    protected int[] numbers;
    protected int step;
    protected int sequenceIndex;

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

    public bool CheckRule(int[] Pams, int[] PreviousPams, bool isStepping = false, int seed = 0) {
        throw new NotImplementedException();
    }

    protected void Log(string smth, bool showStep = false) {
        Debug.Log(smth + (showStep ? (", Step: " + step) : ""));
    }
}
[Serializable]
public enum DeckType {
    Tutorial,
    Colours,
    Dancers,
    Flowers,
    Stars,
    ABCs,
    Domino,
}