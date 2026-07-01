using Godot;
using System;

public partial class ScoreManager : Node
{
	[Export] private QuizScene quizScene;
	
	public int Score { get; private set; } = 0;
	public int NumbersCorrect { get; private set; } = 0;

    public int NumbersQuestion { get; private set; } = 0;

    public override void _Ready()
    {
		quizScene.CorrectAnswer += HndleCorrectAnswer;
    }

	private void HndleCorrectAnswer()
	{
		addNumbersCorrect();
		addScore(10);
		GD.Print(Score);
	}

    public void addNumbersCorrect()
	{
		NumbersCorrect++;
		
	}

    public void addNumbersQuestion()
    {
        NumbersQuestion++;
    }

	public void addScore(int value)
	{
		Score+=value;
	}



}
