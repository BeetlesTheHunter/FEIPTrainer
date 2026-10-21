using Godot;
using System;

public partial class DashBoard : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export] private Button DailyStageButton;
    [Export] private Button NomalStageButton;
    [Export] private Button ReviewStageButton;
    [Export] private Button CategoryStageButton;


    public override void _Ready()
	{
		DailyStageButton.Pressed += HandleDailyStageButtonPressed;
        NomalStageButton.Pressed += HandleNomalStageButtonPressed;
        ReviewStageButton.Pressed += HandleReviewStageButtonPressed;
        CategoryStageButton.Pressed += HandleCategoryStageButtonPressed;


    }

    public void HandleDailyStageButtonPressed()
    {
        GD.Print("DailyStageBottonPressed");
    }

    public void HandleNomalStageButtonPressed()
    {
        GD.Print("NomalStageBottonPressed");
        GameManager.Instance.ReplaceScene(GameManager.Instance.quizScreen);
    }

    public void HandleReviewStageButtonPressed()
    {
        GD.Print("ReviewStageBottonPressed");
    }

    public void HandleCategoryStageButtonPressed()
    {
        GD.Print("CategoryStageButtonPressed");
        var scene = GD.Load<PackedScene>("res://Scenes/CategorySelect/CategorySelect.tscn");
        GameManager.Instance.ReplaceScene(scene);
    }
}
