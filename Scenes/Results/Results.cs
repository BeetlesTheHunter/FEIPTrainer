using Godot;
using System;

public partial class Results : Control
{
   [Export]  private Button _nextStageButton;
   [Export] private Label CorrectAnswerRate;
   [Export] private Label score;
   

    public override void _Ready()
    {
        _nextStageButton.Pressed += OnNextStagePressed;
        
    }


    public void lnit(int scoreValue,int  rate)
    {
        score.Text = $"獲得スコア:{scoreValue}";

        CorrectAnswerRate.Text = $"正答率:{rate}%";
    }
    
    private void OnNextStagePressed()
    {

       
            GameManager.Instance.ReplaceScene(GameManager.Instance.dashBoardScreen);
    
          
    }
}