using Godot;
using System;

public partial class DashBoard : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export] private Button DailyStageButton;
    [Export] private Button NomalStageButton;
    [Export] private Button ReviewStageButton;
    [Export] private Button CategoryStageButton;

    private UserDataManager userDataManager;
    private GameManager gameManager;


    public override void _Ready()
	{
		DailyStageButton.Pressed += HandleDailyStageButtonPressed;
        NomalStageButton.Pressed += HandleNomalStageButtonPressed;
        ReviewStageButton.Pressed += HandleReviewStageButtonPressed;
        CategoryStageButton.Pressed += HandleCategoryStageButtonPressed;

    }

    public void Init(GameManager gm, UserDataManager udm)
    {
        gameManager = gm;
        userDataManager = udm;
    }

    public void HandleDailyStageButtonPressed()
    {
        GD.Print("DailyStageBottonPressed");
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

    public void HandleCategoryStageButtonPressed()
    {
        GD.Print("CategoryStageButtonPressed");
        gameManager.CategorySelectScreen();
    }
}
