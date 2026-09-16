using Godot;
using System;

public partial class GameManager : Node
{
    [Export] public PackedScene dashBoardScreen;
    [Export] public PackedScene quizScreen;
    [Export] public PackedScene resultsScreen;

    [Export] public PackedScene ESCMenuOverlay;

    [Export] private PackedScene initialScene;

    private CanvasLayer mainScene;
    public static GameManager Instance;

    public override void _Ready()
    {
        ReplaceScene(initialScene);
        Instance = this;
    }

    public void ReplaceScene(PackedScene scene)
    {
        Node target = scene.Instantiate();
        GetTree().CurrentScene.AddChild(target);
    }

}
