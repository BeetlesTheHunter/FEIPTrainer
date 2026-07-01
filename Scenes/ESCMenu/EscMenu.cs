using Godot;
using System;

public partial class EscMenu : Control
{
    [Export] private Button ContinueButton;

    public override void _Ready()
    {
        ContinueButton.Pressed += HandleContinueButtonPressed;

        Visible = false;
    }

    private void HandleContinueButtonPressed()
    {
        GameManager.instance.ReplaceScene(GameManager.instance.dashBoardScreen);
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            Visible = !Visible;

            
        }
    }

}
