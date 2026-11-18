using Godot;

public partial class GameManager : Node
{
    [Export] private PackedScene dashBoardScene;
    [Export] private PackedScene quizScene;
    [Export] private PackedScene resultsScene;
    [Export] private PackedScene startScene;

    [Export] private PackedScene ESCMenuOverlay;
    
    [Export] private PackedScene questionDataManagerScene;
    [Export] private PackedScene userDataManagerScene;
    [Export] private PackedScene databaseManagerScene;

    private QuestionDataManager questionDataManager;
    private UserDataManager userDataManager;
    private DatabaseManager databaseManager;
    private GameManager gameManager;
    
    private Node currentLoadedScene;

    public override void _Ready()
    {
        questionDataManager = questionDataManagerScene.Instantiate() as QuestionDataManager;
        AddChild(questionDataManager);

        userDataManager = userDataManagerScene.Instantiate() as UserDataManager;
        AddChild(userDataManager);

        databaseManager = databaseManagerScene.Instantiate() as DatabaseManager;
        AddChild(databaseManager);

        gameManager = this;

        StartScreen();
        ESCMenuScreen();


    }

    public void StartScreen()
    {
        StartScreen scene = startScene.Instantiate() as StartScreen;
        scene.Init(gameManager);
        ReplaceScene(scene);
        
    }

    public void DashBoardScreen()
    {
        DashBoard scene = dashBoardScene.Instantiate() as DashBoard;
        scene.Init(gameManager, userDataManager);
        ReplaceScene(scene);
    }

    public void QuizScreen()
    {
        QuizScene scene = quizScene.Instantiate() as QuizScene;
        scene.Init(gameManager, questionDataManager, array:null);
        ReplaceScene(scene);

    }

    public void ResultScreen(int score, int rate)
    {
        Results scene = resultsScene.Instantiate() as Results;
        scene.Init(gameManager, score, rate);
        ReplaceScene(scene);
    }

    public void ESCMenuScreen()
    {
        EscMenu scene = ESCMenuOverlay.Instantiate() as EscMenu;
        scene.Init(gameManager);
        GetContentViewport().AddChild(scene);
    }

    private void ReplaceScene(Node scene)
    {
        if (currentLoadedScene != null)
        {
            currentLoadedScene.QueueFree();
        }

        currentLoadedScene = scene;
        
        GetContentViewport().AddChild(currentLoadedScene);
        GetContentViewport().MoveChild(currentLoadedScene, 0);
    }

    private SubViewport GetContentViewport()
    {
        return GetTree().CurrentScene.GetNode<SubViewport>(
            "CenterContainer/SubViewportContainer/SubViewport");
    }
}