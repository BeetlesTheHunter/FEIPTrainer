using Godot;
using System;

public partial class UserDataManager : Node
{
	public static UserDataManager Instance { get; private set; }

	private const string ProfilePath = "user://profile.cfg";
	private const string ProfileSection = "profile";
	private const string TotalScoreKey = "total_score";
	private UserData userData;

	public override void _Ready()
	{
		Instance = this;
		userData = new UserData();

		ConfigFile profile = new ConfigFile();
		Error loadError = profile.Load(ProfilePath);
		if (loadError != Error.Ok && loadError != Error.FileNotFound)
		{
			GD.PushError($"Could not load player profile before resetting score: {loadError}");
			return;
		}

		profile.SetValue(ProfileSection, TotalScoreKey, 0);
		Error saveError = profile.Save(ProfilePath);
		if (saveError != Error.Ok)
		{
			GD.PushError($"Could not reset player profile score: {saveError}");
		}
	}

	public bool AddScore(int score)
	{
		if (score < 0)
		{
			GD.PushError("Cannot add a negative score to the player profile.");
			return false;
		}

		int updatedTotal;
		try
		{
			updatedTotal = checked(userData.totalScore + score);
		}
		catch (OverflowException)
		{
			GD.PushError("Could not save player profile: total score exceeds the supported range.");
			return false;
		}

		ConfigFile profile = new ConfigFile();
		Error loadError = profile.Load(ProfilePath);
		if (loadError != Error.Ok && loadError != Error.FileNotFound)
		{
			GD.PushError($"Could not load player profile before saving: {loadError}");
			return false;
		}

		profile.SetValue(ProfileSection, TotalScoreKey, updatedTotal);
		Error saveError = profile.Save(ProfilePath);
		if (saveError != Error.Ok)
		{
			GD.PushError($"Could not save player profile: {saveError}");
			return false;
		}

		userData.totalScore = updatedTotal;
		return true;
	}

	public void setUserId(int userId)
	{
		userData.userId = userId;
    }
	public void setSolvedCount(int solvedCount)
	{
		userData.solvedCount = solvedCount;
	}
	public void setCorrectCount(int correctCount)
	{
		userData.correctCount = correctCount; 
	}

	public int getUserId()
	{
		return userData.userId;
	}
	public int getSolvedCount()
	{
		return userData.solvedCount;
	}
	public int getCorrectCount()
	{
		return userData.correctCount;
	}

	public int getTotalScore()
	{
		return userData.totalScore;
	}

}
