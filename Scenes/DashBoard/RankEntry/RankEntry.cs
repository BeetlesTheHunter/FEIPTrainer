using Godot;
using System;

public partial class RankEntry : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export] private Label rankLabel;
	[Export] private Label nameLabel;
	[Export] private Label scoreLabel;

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void Init(int rank, string name, int score)
	{
		rankLabel.Text = rank.ToString();
		nameLabel.Text = name;
		scoreLabel.Text = score.ToString();
	}

	public void SetData(int rank, string userName, long totalScore)
    {
        rankLabel.Text = $"{rank}位";
        nameLabel.Text = userName;
        scoreLabel.Text = totalScore.ToString();
    }

    // ユーザーが5人未満のときの空枠表示用
    public void SetEmpty(int rank)
    {
        rankLabel.Text = $"{rank}位";
        nameLabel.Text = "---";
        scoreLabel.Text = "-";
    }
	
}
