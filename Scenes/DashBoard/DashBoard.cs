using Godot;
using System;

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

    private UserDataManager userDataManager;
    private GameManager gameManager;


    public override void _Ready()
	{
		DailyStageButton.Pressed += HandleDailyStageButtonPressed;
        NomalStageButton.Pressed += HandleNomalStageButtonPressed;
        ReviewStageButton.Pressed += HandleReviewStageButtonPressed;
        
        UpdatePlayerInfo();
    }

    public void Init(GameManager gm, UserDataManager udm)
    {
        gameManager = gm;
        userDataManager = udm;
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

    public void HandleDailyStageButtonPressed()
    {
        GD.Print("DailyStageBottonPressed");
        userDataManager.DoneDaily();
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
