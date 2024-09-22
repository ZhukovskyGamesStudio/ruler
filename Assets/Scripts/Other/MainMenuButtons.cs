using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// 
/// <summary>
/// ca-app-pub-5529160665131462~6255376802
/// 
/// ca-app-pub-5529160665131462/8199013869
/// 
/// ca-app-pub-5529160665131462/5078026508
/// </summary>
public class MainMenuButtons : MonoBehaviour {
    public Transform[] Canvases;

    public Text[] Records;
    public Text[] RecordsTime;
    public Text quoteText;
    public Text quoterText;

    public GameManager GM;
    public DecksManager DM;

    private const string leaderBoard = "CgkIzqeEhsMMEAIQAw";

    //private const string watch_ad = "ca-app-pub-3940256099942544/1712485313";  тестовая реклама
    private const string bucks10_ad = "ca-app-pub-5529160665131462/8199013869";
    private const string doubleBucks_ad = "ca-app-pub-5529160665131462/6367945492";

    //private RewardedAd ad;
    //private RewardedAd ad1;

    private const string gameOver_ad = "ca-app-pub-5529160665131462/5078026508";

    private const string pMoney1 = "app_money_1";

    Transform mainCam;
    int curCanvas;

    bool isActive;
    /**********/

    void Update() {
#if UNITY_ANDROID
        if (60 != Application.targetFrameRate)
            Application.targetFrameRate = 60;

#endif
    }

    public void Awake() {
        for (int i = 0; i < Canvases.Length; i++)
            Canvases[i].gameObject.SetActive(false);
    }

    public void Start() {
#if UNITY_ANDROID
        QualitySettings.vSyncCount = 0;
#endif
        GetComponent<Camera>().orthographicSize = 400f * ((float)Screen.height / Screen.width) / (800f / 480f);
        mainCam = GetComponent<Transform>();
        curCanvas = 0;

        ActivateGooglePlay();
        //MobileAds.Initialize(initStatus => { });
        //LoadAd();
        //LoadAd1();

        if (!PlayerPrefs.HasKey("tutorialComplete")) {
            To(9);
            GameObject.Find("AudioManager").GetComponent<AudioManager>().AmbientSwitch(-2);
            SaveManager.CreateSave(DM);
        } else
            To(0);

        if (!PlayerPrefs.HasKey("bucks"))
            SaveManager.CreateSave(DM);

        QuoteShow();
    }

    /**********/

    public void To(int canvasIndex) {
        Canvases[canvasIndex].gameObject.SetActive(true);
        StartCoroutine(CamMovement(
            new Vector3(Canvases[canvasIndex].position.x, Canvases[canvasIndex].position.y, mainCam.transform.position.z), canvasIndex));
    }

    public void ToGame(bool zen) {
        To(1);
        StartCoroutine(GameLateCreate(zen));
    }

    /**********/

    public void ResetData() {
        PlayerPrefs.DeleteAll();
        SaveManager.CreateSave(DM);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void QuoteShow() {
        switch (Random.Range(0, 6)) {
            case 0:
                quoteText.text = "\"" + "some cool quote" + "\"";
                quoterText.text = "some guy";
                break;
            case 1:
                quoteText.text = "\"" + "Life is a continuous exercise in creative problem solving." + "\"";
                quoterText.text = "Michael J. Gelb";
                break;
            case 2:
                quoteText.text = "\"" + "People in good moods are better at inductive reasoning." + "\"";
                quoterText.text = "Peter Salovey";
                break;

            case 3:
                quoteText.text = "\"" + "Truth will sooner come out of error than from confusion." + "\"";
                quoterText.text = "Francis Bacon";
                break;
            case 4:
                quoteText.text = "\"" + "No problem can withstand the assault of sustained thinking." + "\"";
                quoterText.text = "Voltaire";
                break;
            case 5:
                quoteText.text = "\"" + "If you make the same guess often enough it becomes a Scientific Fact." + " This is inductive method." +
                                 "\"";
                quoterText.text = "C.S. Lewis";
                break;
        }
    }

    public void UpdateRecords() {
        Save sv = SaveManager.Load();
        for (int i = 0; i < Records.Length; i++) {
            Records[i].text = "Record";
            RecordsTime[i].text = "";
        }

        int t;
        for (int i = 0; i < sv.records.Length; i++) {
            if (sv.records[i].score > 0) {
                Records[i].text = sv.records[i].score.ToString();
                t = sv.records[i].time;
                RecordsTime[i].text = (t / 60 < 10 ? "0" : "") + t / 60 + " : " + (t % 60 < 10 ? "0" : "") + t % 60;
            }
        }
    }

    /**********/
   /* private void LoadAd() {
        ad = new RewardedAd(bucks10_ad);

        AdRequest request = new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice("A118218DAE8D3296")
            .Build();

        ad.LoadAd(request);
    }

    private void LoadAd1() {
        ad1 = new RewardedAd(doubleBucks_ad);

        AdRequest request = new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice("A118218DAE8D3296")
            .Build();

        ad1.LoadAd(request);
    }*/

    /*
    public void WatchAd() {
        if (ad.IsLoaded()) {
            ad.Show();
            ad.OnUserEarnedReward += OnUserEarnedReward;
            LoadAd();
        }
    }

    public void WatchDoubleAd() {
        if (ad1.IsLoaded()) {
            ad1.Show();
            ad1.OnUserEarnedReward += GM.OnUserDoubledReward;
            LoadAd1();
        }
    }*/

    public void OnUserEarnedReward(object sender, System.EventArgs args) {
        int bucks = PlayerPrefs.GetInt("bucks");
        bucks += 10;
        PlayerPrefs.SetInt("bucks", bucks);
    }

    /**********/

    public void SendRecord(int score) {
        Social.ReportScore(score, leaderBoard, (bool sucess) => {
            if (sucess) { } else { }
        });
    }

    public void ActivateGooglePlay() {
        if (!isActive) {
            
            /*
#if UNITY_ANDROID
            PlayGamesPlatform.Activate();
#endif

*/

            Social.localUser.Authenticate((bool sucess) => { isActive = sucess; });
        }
    }

    public void ShowGlobalRecords() {
        Social.ShowLeaderboardUI();
    }

    public void ShowAchievementList() {
        Social.ShowAchievementsUI();
    }

    /**********/

/*
	private void OnApplicationQuit()
	{
#if UNITY_ANDROID
		PlayGamesPlatform.Instance. SignOut();
#endif
	}*/

    /**********/

    public IEnumerator CamMovement(Vector3 finPos, int nextIndex) {
        if (nextIndex == 4)
            UpdateRecords();
        if (nextIndex == 7) {
            DM.ReLoadDecks();
        }

        if (nextIndex == 9)
            Canvases[9].GetComponent<TutorialScript>().ReCast();

        Canvases[nextIndex].gameObject.SetActive(true);
        Vector3 startPos = mainCam.position;
        float MoveTime = 0.3f;
        float time = 0;
        while (time < MoveTime) {
            mainCam.position = Vector3.Lerp(startPos, finPos, time / MoveTime);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (curCanvas != nextIndex && nextIndex != 2)
            Canvases[curCanvas].gameObject.SetActive(false);
        curCanvas = nextIndex;
        mainCam.position = finPos;

        yield return false;
    }

    public IEnumerator GameLateCreate(bool zen) {
        yield return new WaitForSeconds(0.15f);
        GM.CreateLevel(zen);
        yield return false;
    }
}