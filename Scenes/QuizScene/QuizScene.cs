using Godot;
using System;

/*
	TO-DO
	sub to CombatOver signal 
	The signal can emit a enum if its CombatResult.Continue Continue the quiz 
	else end it.

*/
public partial class QuizScene : Control
{
	[Export] private Button ButtonA;
	[Export] private Button ButtonB;
	[Export] private Button ButtonC;
	[Export] private Button ButtonD;
	[Export] private RichTextLabel QuestionLabel;

	private QuestionData currentQuestion;
	private QuestionData[] questionArray;
	private int questionNumber = 0; //何問やったか管理するために使う
	private int randomQuestionNumberMax = 0;
	
	[Export] private ScoreManager scoreManager;
    [Export] private CombatScene combatScene;
	private QuestionDataManager questionDataManager;
	private GameManager gameManager;
	private UserDataManager userDataManager;
	private DatabaseManager databaseManager;

	public enum QuizMode
	{
		Randomized,
		ArrayQustion,
	}

	private QuizMode quizMode;

	public event Action CorrectAnswer;
	private void RaiseCorrectAnswer() => CorrectAnswer?.Invoke();
	public event Action WrongAnswer;
	private void RaiseWrongAnswer() => WrongAnswer?.Invoke();

	[Export] private CharacterData testCharacterData;
	public override void _Ready()
	{
		ButtonA.Pressed += HandleButtonAPressed;
		ButtonB.Pressed += HandleButtonBPressed;
		ButtonC.Pressed += HandleButtonCPressed;
		ButtonD.Pressed += HandleButtonDPressed;

        //combatScene.CombatOver += HandleCombatOver;

		combatScene.Init(testCharacterData,testCharacterData);
		combatScene.CombatOver += HandleCombatOver;

	}

	public void Init(GameManager gm, QuestionDataManager qdm, int numQuestions, UserDataManager udm, DatabaseManager dbm)
	{
		gameManager = gm;
		userDataManager = udm;
		databaseManager = dbm;
		quizMode = QuizMode.Randomized;

		questionDataManager = qdm;

		questionNumber = 0;
		randomQuestionNumberMax = numQuestions;

		currentQuestion = questionDataManager.GetRandomQuestion();
		UpdateQuestionUI();
	}

	public void Init(GameManager gm, QuestionDataManager qdm, UserDataManager udm, DatabaseManager dbm, QuestionData[] array)
	{
		gameManager = gm;
		userDataManager = udm;
		databaseManager = dbm;
		quizMode = QuizMode.ArrayQustion;
		questionNumber = 0;
		

		if(array == null)
		{
			questionDataManager = qdm;
			
			QuestionData[] testArray = {
			questionDataManager.GetQuestionById(1),
			questionDataManager.GetQuestionById(418),
			questionDataManager.GetQuestionById(993),
			questionDataManager.GetQuestionById(1240),
			questionDataManager.GetQuestionById(1505)};

			array = testArray;
		}

		questionArray = array;

		if (questionArray != null && questionArray.Length > 0)
		{
			currentQuestion = questionArray[questionNumber];
			UpdateQuestionUI();
		}
	}

	private void UpdateQuestionUI()
	{
		if (currentQuestion != null)
		{
			QuestionLabel.Clear();

			QuestionLabel.AppendText(currentQuestion.Question);
			QuestionLabel.AppendText("\n");

			AddQuestionImage();
			QuestionLabel.AppendText("\n");

			QuestionLabel.AppendText("\n A: " + currentQuestion.Options[0]);
			QuestionLabel.AppendText("\n B: " + currentQuestion.Options[1]);
			QuestionLabel.AppendText("\n C: " + currentQuestion.Options[2]);
			QuestionLabel.AppendText("\n D: " + currentQuestion.Options[3]);
		   
		}
	}

	private void AddQuestionImage()
	{
		if(currentQuestion.QuestionImage != null)
		{
			Texture2D image = GD.Load<Texture2D>(currentQuestion.QuestionImage);
			QuestionLabel.AddImage(image,400,256);
		}
	}

	private void NextQuestion()
	{
	
		questionNumber++;
 
		if(quizMode == QuizMode.Randomized)
		{
			if(questionNumber < randomQuestionNumberMax)
			{
				currentQuestion = questionDataManager.GetRandomQuestion();
				UpdateQuestionUI();
			}
			else
			{
				EndQuiz();
				return;
			}
		}
		else if(quizMode == QuizMode.ArrayQustion)
		{
			if(questionNumber < questionArray.Length)
			{
				currentQuestion = questionArray[questionNumber];
				UpdateQuestionUI();
			}
			else
			{
				EndQuiz();
				return;
			}
		}

		DisableAnswerButtons(false);
		ResetButtonColors();
		
	}

	private void EndQuiz()
	{
		// この時点で questionNumber = 答えた問題数(Randomized / ArrayQustion どちらも同じ)
		int rate = questionNumber > 0
			? Mathf.RoundToInt(scoreManager.NumbersCorrect * 100f / questionNumber)
			: 0;

		userDataManager.AddScore(scoreManager.Score);
		try
		{
			databaseManager.SaveUserData(userDataManager.userData);
		}
		catch (Exception ex)
		{
			GD.PrintErr($"ユーザーデータの保存に失敗しました: {ex.Message}");
		}
		
		gameManager.ResultScreen(scoreManager.Score, rate);
	}

    public void HandleCombatOver()
    {
        EndQuiz();
    }

	private void DisableAnswerButtons(bool disabled)
	{
		//true turns the button off and false turns them on
		ButtonA.Disabled = disabled;
		ButtonB.Disabled = disabled;
		ButtonC.Disabled = disabled;
		ButtonD.Disabled = disabled;

	}

	private const int ButtonAValue = 0;
	private const int ButtonBValue = 1;
	private const int ButtonCValue = 2;
	private const int ButtonDValue = 3;


	private async void CheckAnswer(int answerIndex)
	{
		DisableAnswerButtons(true);

		Button correctButton = GetAnswerButton(currentQuestion.AnswerIndex);
		correctButton.Modulate = Colors.Green;

		if (currentQuestion.AnswerIndex == answerIndex)
		{
			GD.Print("正解");
			RaiseCorrectAnswer();
			combatScene.PlayerAttack();
		}
		else
		{
			GD.Print("不正解");

			Button wrongButton = GetAnswerButton(answerIndex);
			wrongButton.Modulate = Colors.Red;

			RaiseWrongAnswer();
			combatScene.EnemyAttack();
			
		}
		
	}
	private Button GetAnswerButton(int index)
	{
		switch (index)
		{
			case ButtonAValue:
				return ButtonA;
			case ButtonBValue:
				return ButtonB;
			case ButtonCValue:
				return ButtonC;
			case ButtonDValue:
				return ButtonD;
			default:
            throw new ArgumentOutOfRangeException(nameof(index));
				
		};
	}

	private void ResetButtonColors()
{
    ButtonA.Modulate = Colors.White;
    ButtonB.Modulate = Colors.White;
    ButtonC.Modulate = Colors.White;
    ButtonD.Modulate = Colors.White;
}

	private void HandleButtonAPressed()
	{
		CheckAnswer(ButtonAValue);
	}

	private void HandleButtonBPressed()
	{
		CheckAnswer(ButtonBValue);
	}

	private void HandleButtonCPressed()
	{
		CheckAnswer(ButtonCValue);
	}

	private void HandleButtonDPressed()
	{
		CheckAnswer(ButtonDValue);
	}

	private void HandleCombatOver(CombatScene.CombatResult result, int score)
	{
		scoreManager.addScore(score);

		if(result == CombatScene.CombatResult.Continue)
		{
			NextQuestion();
		}

		if(result == CombatScene.CombatResult.CombatOver)
		{
			EndQuiz();
			return;
		}
	}

	
}
