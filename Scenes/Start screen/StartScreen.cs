using Godot;

public partial class StartScreen : Control
{
    [Export] private Button startButton;
    private GameManager gameManager;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public override void _Ready()
    {
        startButton.Pressed += OnStartPressed;
    }

    private void OnStartPressed()
    {
       gameManager.DashBoardScreen();
    }
}

