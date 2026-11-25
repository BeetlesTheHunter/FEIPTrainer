using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class DashBoard : Control
{
	// Called when the node enters the scene tree for the first time.
    [Export] private Button DailyStageButton;
    [Export] private Button NomalStageButton;
    [Export] private Button ReviewStageButton;
    [Export] private Label NameLabel;
    [Export] private Label ScoreLabel;
    [Export] private Label RankLabel;
    [Export] private Label LoginDaysLabel;
    [Export] private RankEntry[] RankEntries;
    [Export] private RankEntry RankEntryUser;

    private UserDataManager userDataManager;
    private GameManager gameManager;
    private DatabaseManager databasemanager;

    public void Init(GameManager gameManager, UserDataManager userDataManager, DatabaseManager databasemanager)
    {
        this.gameManager = gameManager;
        this.userDataManager = userDataManager;
        this.databasemanager = databasemanager;
    }

    public override void _Ready()
	{
		DailyStageButton.Pressed += HandleDailyStageButtonPressed;
        NomalStageButton.Pressed += HandleNomalStageButtonPressed;
        ReviewStageButton.Pressed += HandleReviewStageButtonPressed;
        DailyStageButton.Disabled = userDataManager.IsDailyDone();

        
        UpdatePlayerInfo();
        _ = UpdateRankingAsync();
    }

    
    private void UpdatePlayerInfo()
    {
        if (userDataManager == null) return;

        UserData data = userDataManager.userData;
        if (data == null) return;

        NameLabel.Text = data.userName;
        ScoreLabel.Text = $"スコア: {data.totalScore}";
        LoginDaysLabel.Text = $"連続ログイン: {data.consecutiveLoginNumber}日";

        // ランキング機能はまだ未実装のため仮表示
        RankLabel.Text = "順位: -";
    }
    private async Task UpdateRankingAsync()
    {
        if (gameManager == null || RankEntries == null) return;

        
        if (databasemanager == null) return;

    // 1. 上位5人を RankEntry1〜5 に反映
        List<UserData> rankingList = await databasemanager.GetTopRankingAsync(RankEntries.Length);
        for (int i = 0; i < RankEntries.Length; i++)
        {
            if (RankEntries[i] == null) continue;

            if (i < rankingList.Count)
                RankEntries[i].SetData(i + 1, rankingList[i].userName, rankingList[i].totalScore);
            else
                RankEntries[i].SetEmpty(i + 1);
        }

    // 2. 自分の順位を RankEntryUser と左上の RankLabel に反映
        UserData myData = userDataManager?.userData;
        if (myData != null)
        {
            int myRank = await databasemanager.GetUserRankAsync(myData.totalScore);
            if (myRank > 0)
            {
                RankEntryUser?.SetData(myRank, myData.userName, myData.totalScore);
                if (RankLabel != null) RankLabel.Text = $"順位: {myRank}位";
            }
        }
    }

    public void HandleDailyStageButtonPressed()
    {
        GD.Print("DailyStageBottonPressed");
        userDataManager.DoneDaily();
        gameManager.DailyQuizScene();
        
    }

    public void HandleNomalStageButtonPressed()
    {
        GD.Print("NomalStageBottonPressed");
        gameManager.QuizScreen();
    }

    public void HandleReviewStageButtonPressed()
    {
        GD.Print("ReviewStageBottonPressed");
    }
}
