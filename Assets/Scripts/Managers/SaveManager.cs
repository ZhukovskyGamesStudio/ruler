using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager {
    public static void Save(bool isErasing, Save toSave) {
        if (isErasing) {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("tutorialComplete", 1);
        } else {
            PlayerPrefs.SetInt("bucks", toSave.bucks);
            PlayerPrefs.SetInt("decksAmount", toSave.deckDatas.Length);

            for (int i = 0; i < toSave.deckDatas.Length; i++) {
                PlayerPrefs.SetInt("isBought" + i.ToString(), toSave.deckDatas[i].isBought ? 1 : 0);
                PlayerPrefs.SetInt("isEnabled" + i.ToString(), toSave.deckDatas[i].isEnabled ? 1 : 0);
            }

            PlayerPrefs.SetInt("recordsAmount", toSave.records.Length);
            for (int i = 0; i < toSave.records.Length; i++) {
                PlayerPrefs.SetInt("recordScore" + i.ToString(), toSave.records[i].score);
                PlayerPrefs.SetInt("recordTime" + i.ToString(), toSave.records[i].time);
            }

            PlayerPrefs.SetFloat("effectsVolume", toSave.effectsVolume);
            PlayerPrefs.SetFloat("musicVolume", toSave.musicVolume);
        }
    }

    public static Save Load() {
        Save sv = new Save {
            bucks = PlayerPrefs.GetInt("bucks"),
            effectsVolume = PlayerPrefs.GetFloat("effectsVolume"),
            musicVolume = PlayerPrefs.GetFloat("musicVolume")
        };

        int recordsAmount = PlayerPrefs.GetInt("recordsAmount");
        if (recordsAmount > 0) {
            sv.records = new Record[recordsAmount];
            for (int i = 0; i < recordsAmount; i++)
                sv.records[i] = new Record(PlayerPrefs.GetInt("recordScore" + i.ToString()), PlayerPrefs.GetInt("recordTime" + i.ToString()));
        }

        int deckAmount = PlayerPrefs.GetInt("decksAmount");
        if (deckAmount > 0) {
            sv.deckDatas = new DeckData[deckAmount];
            for (int i = 0; i < deckAmount; i++)
                sv.deckDatas[i] = new DeckData(PlayerPrefs.GetInt("isBought" + i.ToString()) == 1,
                    PlayerPrefs.GetInt("isEnabled" + i.ToString()) == 1);
        }

        sv.effectsVolume = PlayerPrefs.GetFloat("effectsVolume");
        sv.musicVolume = PlayerPrefs.GetFloat("musicVolume");
        return sv;
    }

    public static void CreateSave(DecksManager DM) {
        Save sv = new Save {
            bucks = 0,
            effectsVolume = 0.5f,
            musicVolume = 0.5f
        };

        int amount = DM.Decks.Length;

        sv.deckDatas = new DeckData[amount];
        for (int i = 0; i < amount; i++)
            if (i == 0 || i == 1)
                sv.deckDatas[i] = new DeckData(true, true);
            else
                sv.deckDatas[i] = new DeckData(false, false);

        sv.records = new Record[3];
        for (int i = 0; i < sv.records.Length; i++)
            sv.records[i] = new Record(0, 0);

        Save(false, sv);
        Debug.Log("save created!");
    }
}

public class Save {
    public int bucks;
    public Record[] records;
    public DeckData[] deckDatas;
    public float effectsVolume;
    public float musicVolume;
}

public class Record {
    public int score;
    public int time;

    public Record(int sc, int t) {
        score = sc;
        time = t;
    }
}

public class DeckData {
    public bool isBought;
    public bool isEnabled;

    public DeckData(bool b, bool e) {
        isBought = b;
        isEnabled = e;
    }
}