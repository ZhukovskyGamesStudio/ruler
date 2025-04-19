using System;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager {
    public static Save SaveData;

    private static DecksTableConfig _decksConfig;

    public static void Init(DecksTableConfig decksTableConfig) {
        _decksConfig = decksTableConfig;
        SaveData = Load(_decksConfig);
    }

    public static void ClearSave() {
        PlayerPrefs.DeleteAll();
        SaveData = Load(_decksConfig);
    }

    public static void Save() {
        PlayerPrefs.SetString("save", JsonUtility.ToJson(SaveData));
    }

    private static Save CreateNewSave(DecksTableConfig decksTableConfig) {
        Save sv = new Save {
            bucks = 0,
            effectsVolume = 0.5f,
            musicVolume = 0.5f,
            DeckDatas = new List<DeckData>()
        };
        foreach (DeckConfig deckConfig in decksTableConfig.Decks) {
            sv.DeckDatas.Add(new DeckData(deckConfig.IsAlreadyAvailable, deckConfig.IsAlreadyAvailable));
        }
        sv.records = new Record[3];
        for (int i = 0; i < sv.records.Length; i++) {
            sv.records[i] = new Record(0, 0);
        }

        return sv;
    }

    public static Save Load(DecksTableConfig decksTableConfig) {
        if (!PlayerPrefs.HasKey("save")) {
            return CreateNewSave(decksTableConfig);
        }

        string savedJson = PlayerPrefs.GetString("save");
        return JsonUtility.FromJson<Save>(savedJson);
    }
}

[Serializable]
public class Save {
    public int bucks;
    public Record[] records;
    public List<DeckData> DeckDatas = new List<DeckData>();
    public float effectsVolume;
    public float musicVolume;
}

[Serializable]
public class Record {
    public int score;
    public int time;

    public Record(int sc, int t) {
        score = sc;
        time = t;
    }
}

[Serializable]
public class DeckData {
    public DeckType DeckType;
    public bool isBought;
    public bool isEnabled;

    public DeckData(bool b, bool e) {
        isBought = b;
        isEnabled = e;
    }
}