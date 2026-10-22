using Godot;
using System;

public partial class EscMenu : Control
{
    [Export] private Button ContinueButton;
    [Export] private Button TitleButton;

    private GameManager gameManager;

    public override void _Ready()
    {
        ContinueButton.Pressed += HandleContinueButtonPressed;
        TitleButton.Pressed += HandleTitleButtonPressed;

        Visible = false;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            ToggleVisibility();
            GetViewport().SetInputAsHandled();
        }
    }

    public void Init(GameManager gm)
    {
        gameManager = gm;
    }

    private void HandleContinueButtonPressed()
    {
        ToggleVisibility();
    }

    private void HandleTitleButtonPressed()
    {
        ToggleVisibility();
        //To-DO 戻る前に警告をやるべき。

        gameManager.DashBoardScreen();
        
    }

    private void ToggleVisibility()
    {
        Visible = !Visible;
    }

    
}