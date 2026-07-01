using Godot;
using System;

public partial class ScoreManager : Node
{
	public int Score { get; private set; } = 0;
	public int NumbersCorrect { get; private set; } = 0;

    public int NumbersQuestion { get; private set; } = 0;


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
