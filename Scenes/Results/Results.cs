using Godot;
using System;

public partial class Results : Control
{
   [Export]  private Button _nextStageButton;
   [Export] private Label CorrectAnswerRate;
   [Export] private Label score;

    private GameManager gameManager;
   

    public override void _Ready()
    {
        _nextStageButton.Pressed += OnNextStagePressed;
        
    }


    public void Init(GameManager gm, int scoreValue, int rate)
    {
        gameManager = gm;

        score.Text = $"獲得スコア:{scoreValue * 1000}";

        CorrectAnswerRate.Text = $"正答率:{rate}%";
    }
    
    private void OnNextStagePressed()
    {

       
        gameManager.DashBoardScreen();
    
          
    }
}