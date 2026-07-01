using Godot;
using System;

public partial class QuizScene : Control
{
    // Called when the node enters the scene tree for the first time.
    [Export] private Button ButtonA;
    [Export] private Button ButtonB;
    [Export] private Button ButtonC;
    [Export] private Button ButtonD;
    public override void _Ready()
	{
        ButtonA.Pressed += HandleButtonAPressed;
        
        ButtonB.Pressed += HandleButtonBPressed;
        

        ButtonC.Pressed += HandleButtonCPressed;
        

        ButtonD.Pressed += HandleButtonDPressed;
        



    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{

	}
    private void HandleButtonAPressed() 
    {
        GD.Print("A");
    }
    private void HandleButtonBPressed()
    {
        GD.Print("B");
    }
    private void HandleButtonCPressed()
    {
        GD.Print("C");
    }
    private void HandleButtonDPressed()
    {
        GD.Print("D");
    }
}
