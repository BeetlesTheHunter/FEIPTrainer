using Godot;
using System;

public partial class EscMenu : Control
{
    [Export] private Button ContinueButton;

    public override void _Ready()
    {
        ContinueButton.Pressed += HandleContinueButtonPressed;
    }

    private void HandleContinueButtonPressed()
    {
        GameManager.instance.ReplaceScene(GameManager.instance.dashBoardScreen);
    }


}
