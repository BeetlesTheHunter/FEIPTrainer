using System.Collections.Generic;
using Godot;
using System.Text.Json;
using System.Linq;

public partial class QuestionDataManager : Node
{
	private const string questionDataPath = "res://Data/all_questions.json";
	private Dictionary<int, QuestionData> questionDictionary = new Dictionary<int, QuestionData>();
	public static QuestionDataManager Instance { get; private set; }


	public override void _EnterTree()
    {
        if (Instance != null && Instance != this)
        {
            QueueFree();
            return;
        }
        Instance = this;
    }

	public override void _Ready()
	{
		LoadQuestionData();
	}

	private void LoadQuestionData()
	{
		if(!FileAccess.FileExists(questionDataPath))
		{
			GD.PrintErr($"Question data file not found at path: {questionDataPath}");
			return;
		}

		using var file = FileAccess.Open(questionDataPath, FileAccess.ModeFlags.Read);
		string jsonData = file.GetAsText();

		List<QuestionData> questionList = JsonSerializer.Deserialize<List<QuestionData>>(jsonData);
		
		if(questionList == null)
		{
			GD.PrintErr("Failed to deserialize question data.");
			return;
		}

		questionDictionary = questionList.ToDictionary(question => question.Id, question => question);
	}

	public QuestionData GetQuestionById(int id)
	{
		if (questionDictionary.TryGetValue(id, out var questionData))
		{
			return questionData;
		}
		else
		{
			GD.PrintErr($"Question with ID {id} not found.");
			return null;
		}
	}
}
