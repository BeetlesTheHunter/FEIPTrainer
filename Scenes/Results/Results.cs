using Godot;
using System;

public partial class Results : Control
{
    [Export] private Button _nextStageButton;   // 「ダッシュボードへ戻る」ボタン
    [Export] private Label CorrectAnswerRate;   // 正解数の表示
    [Export] private Label score;               // 不正解数の表示

    public override void _Ready()
    {
        _nextStageButton.Pressed += OnReturnToDashboardPressed;
    }

    /// <summary>QuizScene から呼ばれる。正解数と不正解数を表示する。</summary>
    public void Init(int correct, int wrong)
    {
        int total = correct + wrong;
        int rate = total > 0 ? Mathf.RoundToInt(correct * 100f / total) : 0;

        CorrectAnswerRate.Text = $"正解: {correct}問  (正答率 {rate}%)";
        score.Text = $"不正解: {wrong}問";
    }

    private void OnReturnToDashboardPressed()
    {
        GameManager.Instance.ReplaceScene(GameManager.Instance.dashBoardScreen);
    }
}
