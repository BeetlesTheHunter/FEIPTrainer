using Godot;
using System;

public partial class DashBoard : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export] private Button DailyStageButton;
    [Export] private Button NomalStageButton;
    [Export] private Button ReviewStageButton;
	[Export] private Label ScoreLabel;

    private UserDataManager userDataManager;
    private GameManager gameManager;

    public void Init(GameManager gameManager, UserDataManager userDataManager)
    {
        this.gameManager = gameManager;
        this.userDataManager = userDataManager;
    }

    public override void _Ready()
	{
        ScoreLabel.Text = $"総スコア：{userDataManager.getTotalScore()}";
		DailyStageButton.Pressed += HandleDailyStageButtonPressed;
        NomalStageButton.Pressed += HandleNomalStageButtonPressed;
        ReviewStageButton.Pressed += HandleReviewStageButtonPressed;

    }

    public void HandleDailyStageButtonPressed()
    {
        GD.Print("DailyStageBottonPressed");
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
