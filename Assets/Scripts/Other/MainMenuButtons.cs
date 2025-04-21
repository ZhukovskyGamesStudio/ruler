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
    public Text quoteText;
    public Text quoterText;

    [SerializeField]
    private DecksManager _decksManager;

    [SerializeField]
    private AudioManager _audioManager;

    [SerializeField]
    private CoreManager _coreManager;

    [SerializeField]
    private PanelsManager _panelsManager;

    private const string leaderBoard = "CgkIzqeEhsMMEAIQAw";

    //private const string watch_ad = "ca-app-pub-3940256099942544/1712485313";  тестовая реклама
    private const string bucks10_ad = "ca-app-pub-5529160665131462/8199013869";
    private const string doubleBucks_ad = "ca-app-pub-5529160665131462/6367945492";

    //private RewardedAd ad;
    //private RewardedAd ad1;

    private const string gameOver_ad = "ca-app-pub-5529160665131462/5078026508";

    private const string pMoney1 = "app_money_1";

    bool isActive;

    public static MainMenuButtons Instance;
    /**********/

    public void Awake() {
        Instance = this;
    }

    public void DisablePanel(int index) {
        _panelsManager.DisablePanel(index);
    }

    void Update() {
#if UNITY_ANDROID
        if (Application.targetFrameRate != 60)
            Application.targetFrameRate = 60;
#endif
    }

    public void Start() {
#if UNITY_ANDROID
        QualitySettings.vSyncCount = 0;
#endif
        //GetComponent<Camera>().orthographicSize = 400f * ((float)Screen.height / Screen.width) / (800f / 480f);

        ActivateGooglePlay();
        //MobileAds.Initialize(initStatus => { });
        //LoadAd();
        //LoadAd1();

        GoToFirstPanel();

        QuoteShow();
    }

    private void GoToFirstPanel() {
        _panelsManager.DisablePanels();
        if (!PlayerPrefs.HasKey("tutorialComplete")) {
            _panelsManager.TeleportTo(9);
            AudioManager.Instance.AmbientSwitch(-2);
        } else {
            _panelsManager.TeleportTo(0);
        }
    }

    /**********/

    public void To(int canvasIndex) {
        _panelsManager.To(canvasIndex);
    }

    public void ToGame(bool zen) {
        _panelsManager.To(1);
        StartCoroutine(GameLateCreate(zen));
    }

    /**********/

    public void ResetData() {
        SaveManager.ClearSave();
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
/*        Social.ReportScore(score, leaderBoard, (bool sucess) => {
            if (sucess) { } else { }
        });*/
    }

    public void ActivateGooglePlay() {
        if (!isActive) {
            /*
#if UNITY_ANDROID
            PlayGamesPlatform.Activate();
#endif

*/

            //Social.localUser.Authenticate((bool sucess) => { isActive = sucess; });
        }
    }

    public void ShowGlobalRecords() {
        //Social.ShowLeaderboardUI();
    }

    public void ShowAchievementList() {
        //Social.ShowAchievementsUI();
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

    public IEnumerator GameLateCreate(bool zen) {
        yield return new WaitForSeconds(0.15f);
        _coreManager.CreateLevel(zen);
        yield return false;
    }
}