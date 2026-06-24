using Godot;
using System;

public partial class Results : Control
{
   [Export]  private Button _nextStageButton;

    public override void _Ready()
    {
        _nextStageButton.Pressed += OnNextStagePressed;
    }

    
    private void OnNextStagePressed()
    {

       
            GameManager.instance.ReplaceScene(GameManager.instance.dashBoardScreen);
    
          
    }
}