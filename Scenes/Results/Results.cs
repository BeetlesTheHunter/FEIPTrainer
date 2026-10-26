using Godot;
using System;

public partial class Results : Control
{
   [Export]  private Button _nextStageButton;
   [Export] private Label CorrectAnswerRate;
   [Export] private Label score;

    /// <summary>「次へ」ボタンが押されたとき。遷移先は GameManager 側で決める。</summary>
    public event Action NextStagePressed;
   

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

       
        NextStagePressed?.Invoke();
    
          
    }
}