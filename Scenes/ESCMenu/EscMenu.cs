using Godot;
using System;

public partial class EscMenu : Control
{
    [Export] private Button ContinueButton;
    private GameManager gameManager;

    public override void _Ready()
    {
        ContinueButton.Pressed += HandleContinueButtonPressed;

        Visible = false;
    }

    public void Init(GameManager gm)
    {
        gameManager = gm;
    }

    private void HandleContinueButtonPressed()
    {
        
    }
    
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            Visible = !Visible;

            
        }
    }

}
