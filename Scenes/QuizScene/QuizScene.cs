using Godot;
using System;

public partial class QuizScene : Control
{
    // Called when the node enters the scene tree for the first time.
    [Export] private Button ButtonA;
    [Export] private Button ButtonB;
    [Export] private Button ButtonC;
    [Export] private Button ButtonD;
    [Export] private RichTextLabel QuestionLabel;

    private QuestionData currentQuestion;
    private QuestionData[] questionArray;
    private int questionNumber = 0; //何問やったか管理するために使う
    private int randomQuestionNumberMax = 0;
    
    [Export] public ScoreManager scoreManager {get; private set;}
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

    public void Init(GameManager gm, QuestionDataManager qdm, QuestionData[] array)
    {
        gameManager = gm;
        quizMode = QuizMode.ArrayQustion;

        questionDataManager = qdm;

        questionArray = array;
        questionNumber = 0;

        if(questionArray == null)
        {
            QuestionData[] testArray = {
            questionDataManager.GetQuestionById(1),
            questionDataManager.GetQuestionById(418),
            questionDataManager.GetQuestionById(993),
            questionDataManager.GetQuestionById(1240),
            questionDataManager.GetQuestionById(1505)};

            questionArray = testArray;
        }

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
            if(questionNumber <= randomQuestionNumberMax)
            {
                currentQuestion = questionDataManager.GetRandomQuestion();
                UpdateQuestionUI();
            }
            else
            {
                EndQuiz();
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
            }
        }
        
       
    }

    private void EndQuiz()
    {
        // この時点で questionNumber = 答えた問題数(Randomized / ArrayQustion どちらも同じ)
        int rate = questionNumber > 0
            ? Mathf.RoundToInt(scoreManager.NumbersCorrect * 100f / questionNumber)
            : 0;

        gameManager.ResultScreen(scoreManager.Score, rate);
    }

    private void HandleButtonAPressed()
    {
        if (currentQuestion.AnswerIndex == 0)
        {
            GD.Print("正解");
            RaiseCorrectAnswer();
        }
        else
        {
            GD.Print("不正解");
            RaiseWrongAnswer();
        }
        NextQuestion();
    }

    private void HandleButtonBPressed()
    {
        if (currentQuestion.AnswerIndex == 1)
        {
            GD.Print("正解");
            RaiseCorrectAnswer();
        }
        else
        {
            GD.Print("不正解");
            RaiseWrongAnswer();
        }
        NextQuestion();
    }

    private void HandleButtonCPressed()
    {
        if (currentQuestion.AnswerIndex == 2)
        {
            GD.Print("正解");
            RaiseCorrectAnswer();
        }
        else
        {
            GD.Print("不正解");
            RaiseWrongAnswer();
        }
        NextQuestion();
    }

    private void HandleButtonDPressed()
    {
        if (currentQuestion.AnswerIndex == 3)
        {
            GD.Print("正解");
            RaiseCorrectAnswer();
        }
        else
        {
            GD.Print("不正解");
            RaiseWrongAnswer();
        }
        NextQuestion();
    }
}