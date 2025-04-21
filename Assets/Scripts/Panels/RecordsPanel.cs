using UnityEngine;
using UnityEngine.UI;

public class RecordsPanel : Panel {
    [SerializeField]
    private Text[] _records;

    [SerializeField]
    private Text[] _recordsTime;

    public override void Init() {
        UpdateRecords();
    }

    private void UpdateRecords() {
        foreach (Text t in _records) {
            t.text = "Record";
            t.text = "";
        }

        Record[] records = SaveManager.SaveData.records;
        for (int i = 0; i < records.Length; i++) {
            if (records[i].score <= 0) {
                continue;
            }

            _records[i].text = records[i].score.ToString();
            int t = records[i].time;
            _recordsTime[i].text = (t / 60 < 10 ? "0" : "") + t / 60 + " : " + (t % 60 < 10 ? "0" : "") + t % 60;
        }
    }
}