using UnityEngine;

public static class AchievementsManager {
    public const string rules10_achieve = "CgkIzqeEhsMMEAIQBA";
    public const string rules50_achieve = "CgkIzqeEhsMMEAIQBQ";
    public const string rules250_achieve = "CgkIzqeEhsMMEAIQBg";
    public const string rules1000_achieve = "CgkIzqeEhsMMEAIQBw";

    public const string decks3_achieve = "CgkIzqeEhsMMEAIQCA";
    public const string decks5_achieve = "CgkIzqeEhsMMEAIQCQ";
    public const string decks7_achieve = "CgkIzqeEhsMMEAIQCg";
    public const string decks10_achieve = "CgkIzqeEhsMMEAIQCw";

    public const string tutorial_achieve = "CgkIzqeEhsMMEAIQAg";
    public const string tutorialSecond_achieve = "CgkIzqeEhsMMEAIQDQ";
    public const string bornLucky_achieve = "CgkIzqeEhsMMEAIQDA";

    public static void GetTheAchivement(string id, float progress) {
        Social.ReportProgress(id, progress, (bool sucess) => { });
    }
}