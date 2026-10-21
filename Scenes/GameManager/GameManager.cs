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
        Instance = this;
        gameManager = this;

        if (questionDataManagerScene != null)
        {
            questionDataManager = questionDataManagerScene.Instantiate() as QuestionDataManager;
            AddChild(questionDataManager);
        }
        else GD.PushError("[GameManager] questionDataManagerScene が未設定です");

        if (userDataManagerScene != null)
        {
            userDataManager = userDataManagerScene.Instantiate() as UserDataManager;
            AddChild(userDataManager);
        }
        else GD.PushError("[GameManager] userDataManagerScene が未設定です");

        // Autoload の _Ready 時点ではまだメインシーン(CurrentScene)が無いので、
        // 1フレーム遅らせてから最初の画面(ダッシュボード)を開く。
        CallDeferred(nameof(LoadInitialScene));
    }

    private void LoadInitialScene()
    {
        ReplaceScene(dashBoardScreen);
    }

    public Node ReplaceScene(PackedScene scene)
    {
        if (currentLoadedScene != null)
        {
            currentLoadedScene.QueueFree();
        }

        currentLoadedScene = scene.Instantiate();

        Node parent = GetTree().CurrentScene ?? GetTree().Root;
        parent.AddChild(currentLoadedScene);
        return currentLoadedScene;
    }
}
