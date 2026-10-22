
using Godot;

public partial class StartScreen : Control
{
    [Export] private Button startButton;
    private GameManager gameManager;
   

    public override void _Ready()
    {

        startButton.Pressed += OnStartPressed;
     
    }

    public void Init(GameManager gm)
    {
        gameManager = gm;
    }

    private void OnStartPressed()
    {
       gameManager.DashBoardScreen();
    }

}
