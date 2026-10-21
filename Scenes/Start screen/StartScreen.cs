
using Godot;

public partial class StartScreen : Control
{
    [Export] private Button startButton;
   

    public override void _Ready()
    {
       
      

       
        startButton.Pressed += OnStartPressed;
     
    }

    private void OnStartPressed()
    {
        GD.Print("ゲーム開始");
        GameManager.Instance.ReplaceScene(
            GameManager.Instance.dashBoardScreen);
    }

}
