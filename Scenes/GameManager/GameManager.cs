using Godot;
using System;

public partial class GameManager : Node
{
    [Export] public PackedScene dashBoardScreen;
    [Export] public PackedScene quizScreen;
    [Export] public PackedScene resultsScreen;

    [Export] public PackedScene ESCMenuOverlay;


    private CanvasLayer mainScene;
    public static GameManager Instance;
    
    private Node currentLoadedScene;

    public override void _Ready()
    {
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