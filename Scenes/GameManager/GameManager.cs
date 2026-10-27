using Godot;

public partial class GameManager : Node
{
    [Export] private PackedScene dashBoardScene;
    [Export] private PackedScene quizScene;
    [Export] private PackedScene resultsScene;
    [Export] private PackedScene startScene;
    [Export] private PackedScene categorySelectScene;

    [Export] private PackedScene ESCMenuOverlay;
    
    [Export] private PackedScene questionDataManagerScene;
    [Export] private PackedScene userDataManagerScene;

    private QuestionDataManager questionDataManager;
    private UserDataManager userDataManager;
    private GameManager gameManager;
    
    private Node currentLoadedScene;

    public override void _Ready()
    {
        questionDataManager = questionDataManagerScene.Instantiate() as QuestionDataManager;
        AddChild(questionDataManager);

        userDataManager = userDataManagerScene.Instantiate() as UserDataManager;
        AddChild(userDataManager);

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

    // questions を渡すとその問題で開始する(null のときは従来どおりテスト用の問題)
    public void QuizScreen(QuestionData[] questions = null)
    {
        QuizScene scene = quizScene.Instantiate() as QuizScene;
        scene.Init(gameManager, questions, questionDataManager);
        ReplaceScene(scene);

    }

    public void CategorySelectScreen()
    {
        CategorySelect scene = categorySelectScene.Instantiate() as CategorySelect;
        scene.Init(gameManager, questionDataManager);
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
        GetTree().CurrentScene.AddChild(scene);
    }

    private void ReplaceScene(Node scene)
    {
        if (currentLoadedScene != null)
        {
            currentLoadedScene.QueueFree();
        }

        currentLoadedScene = scene;
        
        GetTree().CurrentScene.AddChild(currentLoadedScene);
        GetTree().CurrentScene.MoveChild(currentLoadedScene,0);
    }
}