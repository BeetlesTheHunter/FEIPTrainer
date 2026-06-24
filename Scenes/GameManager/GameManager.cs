using Godot;
using System;

public partial class GameManager : Node
{
    [Export] private PackedScene dashBoardScreen;
    [Export] private PackedScene quizScreen;
    [Export] private PackedScene resultsScreen;

    [Export] private PackedScene ESCMenuOverlay;

    [Export] private PackedScene initialScene;

    private CanvasLayer mainScene;
    public static GameManager instance;

    public override void _Ready()
    {
        ReplaceScene(initialScene);
        instance = this;
    }

    private void ReplaceScene(PackedScene scene)
    {
        Node target = scene.Instantiate();
        GetTree().CurrentScene.AddChild(target);
    }

}
