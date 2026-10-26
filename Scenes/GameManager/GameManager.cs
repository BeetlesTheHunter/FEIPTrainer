using Godot;

public partial class GameManager : Node
{
    [Export] private PackedScene dashBoardScene;
    [Export] private PackedScene quizScene;
    [Export] private PackedScene resultsScene;
    [Export] private PackedScene startScene;
    [Export] private PackedScene categorySelectScene;
    [Export] private PackedScene partQuizScene;

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

    public void QuizScreen()
    {
        QuizScene scene = quizScene.Instantiate() as QuizScene;
        scene.Init(gameManager, questionDataManager, null);
        ReplaceScene(scene);

    }

    public void CategorySelectScreen()
    {
        CategorySelect scene = categorySelectScene.Instantiate() as CategorySelect;
        scene.Init(gameManager, questionDataManager);
        ReplaceScene(scene);
    }

    public void PartQuizScreen(QuestionData[] questions)
    {
        PartQuizScene scene = partQuizScene.Instantiate() as PartQuizScene;
        scene.Init(gameManager, questions);
        ReplaceScene(scene);
    }

    public void ResultScreen(int score, int rate)
    {
        Results scene = resultsScene.Instantiate() as Results;
        scene.NextStagePressed += DashBoardScreen;
        ReplaceScene(scene);
        scene.lnit(score, rate);
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