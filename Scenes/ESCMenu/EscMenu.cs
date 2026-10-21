using Godot;
using System;

public partial class EscMenu : Control
{
    [Export] private Button ContinueButton;
    [Export] private Button TitleButton;

    public override void _Ready()
    {
        ContinueButton.Pressed += HandleContinueButtonPressed;
        TitleButton.Pressed += HandleTitleButtonPressed;

        Visible = false;
    }

    // 続ける: メニューを閉じるだけ(シーンは変えない)
    private void HandleContinueButtonPressed()
    {
        Visible = false;
    }

    // タイトル画面に戻る: メニューを閉じてダッシュボードへ戻る
    private void HandleTitleButtonPressed()
    {
        Visible = false;
        GameManager.Instance.ReplaceScene(GameManager.Instance.dashBoardScreen);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            Visible = !Visible;
            GetViewport().SetInputAsHandled();
        }
    }
}
