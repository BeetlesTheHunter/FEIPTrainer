using System.Collections.Generic;
using Godot;
using System.Text.Json;
using System.Linq;

public partial class QuestionDataManager : Node
{
	private const string questionDataPath = "res://Data/all_questions.json";
	private Dictionary<int, QuestionData> questionDictionary = new Dictionary<int, QuestionData>();
	private List<QuestionData> allQuestions = new List<QuestionData>();

	/// <summary>1パートあたりの問題数</summary>
	public const int PartSize = 10;
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
		allQuestions = questionList;
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

	// ---------- カテゴリ / パート ----------

	/// <summary>カテゴリ名と問題数の一覧(JSON内の登場順)</summary>
	public List<(string Category, int Count)> GetCategories()
	{
		return allQuestions
			.GroupBy(q => q.Category)
			.Select(g => (g.Key, g.Count()))
			.ToList();
	}

	public int GetPartCount(string category)
	{
		int count = allQuestions.Count(q => q.Category == category);
		return (count + PartSize - 1) / PartSize;
	}

	/// <summary>カテゴリ内の問題をID順に並べ、partIndex(0始まり)番目の PartSize 問を返す</summary>
	public QuestionData[] GetPart(string category, int partIndex)
	{
		return allQuestions
			.Where(q => q.Category == category)
			.OrderBy(q => q.Id)
			.Skip(partIndex * PartSize)
			.Take(PartSize)
			.ToArray();
	}
}
