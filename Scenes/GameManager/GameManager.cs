using Godot;
using System;


public partial class GameManager : Node
{
    [Export] public PackedScene dashBoardScreen;
    [Export] public PackedScene quizScreen;
    [Export] public PackedScene resultsScreen;

    [Export] public PackedScene ESCMenuOverlay;
    
    [Export] private PackedScene questionDataManagerScene;
    [Export] private PackedScene userDataManagerScene;

    private QuestionDataManager questionDataManager;
    private UserDataManager userDataManager;
    private GameManager gameManager;


    private CanvasLayer mainScene;
    public static GameManager Instance;
    
    private Node currentLoadedScene;

    public override void _Ready()
    {
        questionDataManager = questionDataManagerScene.Instantiate() as QuestionDataManager;
        AddChild(questionDataManager);

        userDataManager = userDataManagerScene.Instantiate() as UserDataManager;
        AddChild(userDataManager);

        gameManager = this;

        Instance = this;
    }

    public void ReplaceScene(PackedScene scene)
    {
        if (currentLoadedScene != null)
        {
            currentLoadedScene.QueueFree();
        }

        currentLoadedScene = scene.Instantiate();
        
        GetTree().CurrentScene.AddChild(currentLoadedScene);
    }
}