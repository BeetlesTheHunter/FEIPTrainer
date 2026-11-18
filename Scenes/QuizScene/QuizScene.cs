
using Godot;
using System;

public partial class QuizScene : Control
{
    [Export] private Button ButtonA;
    [Export] private Button ButtonB;
    [Export] private Button ButtonC;
    [Export] private Button ButtonD;
    [Export] private RichTextLabel QuestionLabel;

    private QuestionData currentQuestion;
    private QuestionData[] questionArray;

    private int questionNumber = 0;
    private int randomQuestionNumberMax = 0;

    [Export] public ScoreManager scoreManager { get; private set; }

    private QuestionDataManager questionDataManager;
    private GameManager gameManager;

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

    public override void _Ready()
    {
        ButtonA.Pressed += HandleButtonAPressed;
        ButtonB.Pressed += HandleButtonBPressed;
        ButtonC.Pressed += HandleButtonCPressed;
        ButtonD.Pressed += HandleButtonDPressed;
    }

    // ランダム出題モード
    public void Init(GameManager gm, QuestionDataManager qdm, int numberOfQuestions)
    {
        gameManager = gm;
        quizMode = QuizMode.Randomized;

        questionDataManager = qdm;

        questionNumber = 0;
        randomQuestionNumberMax = numberOfQuestions;

        currentQuestion = questionDataManager.GetRandomQuestion();
        UpdateQuestionUI();
    }

    // 配列出題モード
    public void Init(GameManager gm, QuestionDataManager qdm, QuestionData[] array)
    {
        gameManager = gm;
        quizMode = QuizMode.ArrayQustion;
        questionNumber = 0;

        if (array == null)
        {
            questionDataManager = qdm;

            QuestionData[] testArray = {
                questionDataManager.GetQuestionById(1),
                questionDataManager.GetQuestionById(418),
                questionDataManager.GetQuestionById(993),
                questionDataManager.GetQuestionById(1240),
                questionDataManager.GetQuestionById(1505)
            };

            array = testArray;
        }

        questionArray = array;

        if (questionArray != null && questionArray.Length > 0)
        {
            currentQuestion = questionArray[questionNumber];
            UpdateQuestionUI();
        }
    }

    // 問題画面の更新
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

    // 問題画像の追加
    private void AddQuestionImage()
    {
        if (currentQuestion.QuestionImage != null)
        {
            Texture2D image = GD.Load<Texture2D>(currentQuestion.QuestionImage);
            QuestionLabel.AddImage(image, 400, 256);
        }
    }

    // 次の問題へ
    private void NextQuestion()
    {
        questionNumber++;

        if (quizMode == QuizMode.Randomized)
        {
            if (questionNumber < randomQuestionNumberMax)
            {
                currentQuestion = questionDataManager.GetRandomQuestion();
                UpdateQuestionUI();
            }
            else
            {
				ResetButtonColors();
                EndQuiz();
				return;
            }
        }
        else if (quizMode == QuizMode.ArrayQustion)
        {
            if (questionNumber < questionArray.Length)
            {
                currentQuestion = questionArray[questionNumber];
                UpdateQuestionUI();
            }
            else
            {
				ResetButtonColors();
                EndQuiz();
				return;
            }
        }
		ResetButtonColors();
		ToggleButtons(false);
    }

    // クイズ終了
    private void EndQuiz()
    {
        int rate = questionNumber > 0
            ? Mathf.RoundToInt(scoreManager.NumbersCorrect * 100f / questionNumber)
            : 0;

        gameManager.ResultScreen(scoreManager.Score, rate);
    }

    // ボタンの有効・無効切り替え
    private void ToggleButtons(bool disabled)
    {
        ButtonA.Disabled = disabled;
        ButtonB.Disabled = disabled;
        ButtonC.Disabled = disabled;
        ButtonD.Disabled = disabled;
    }

    private const int ButtonAValue = 0;
    private const int ButtonBValue = 1;
    private const int ButtonCValue = 2;
    private const int ButtonDValue = 3;

    // 回答判定・色変更
    private async void CheckAnswer(int answerIndex)
    {
        // ボタンを押せなくする
        ToggleButtons(true);

        // 正解のボタンを緑色にする
        Button correctButton = GetAnswerButton(currentQuestion.AnswerIndex);
        correctButton.Modulate = Colors.Green;

        if (currentQuestion.AnswerIndex == answerIndex)
        {
            GD.Print("正解");
            RaiseCorrectAnswer();
        }
        else
        {
            GD.Print("不正解");

            // 選択した間違いのボタンを赤色にする
            Button selectedButton = GetAnswerButton(answerIndex);
            selectedButton.Modulate = Colors.Red;

            RaiseWrongAnswer();
        }

        // 1.5秒待つ
        await ToSignal(
            GetTree().CreateTimer(2.0),
            SceneTreeTimer.SignalName.Timeout
        );

        // 次の問題へ
        NextQuestion();
    }

    // 番号から対応するボタンを取得
    private Button GetAnswerButton(int index)
    {
        switch (index)
        {
            case 0: return ButtonA;
            case 1: return ButtonB;
            case 2: return ButtonC;
            case 3: return ButtonD;
            default: return null;
        }
    }

	private void HandleResetQuiz()
	{
		CheckAnswer(ButtonAValue);
	}

    // ボタンの色をリセット
    private void ResetButtonColors()
    {
        ButtonA.Modulate = Colors.White;
        ButtonB.Modulate = Colors.White;
        ButtonC.Modulate = Colors.White;
        ButtonD.Modulate = Colors.White;
    }

    // Aボタン
    private void HandleButtonAPressed()
    {
        CheckAnswer(ButtonAValue);
    }

    // Bボタン
    private void HandleButtonBPressed()
    {
        CheckAnswer(ButtonBValue);
    }

    // Cボタン
    private void HandleButtonCPressed()
    {
        CheckAnswer(ButtonCValue);
    }

    // Dボタン
    private void HandleButtonDPressed()
    {
        CheckAnswer(ButtonDValue);
    }
}
