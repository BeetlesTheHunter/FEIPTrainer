using Godot;
using System;
using System.Text.Json.Serialization.Metadata;

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
    private int currentIndex = 0; // 配列のインデックス管理用
    
    [Export] public ScoreManager scoreManager {get; private set;}

    

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
        int numberOfQuestions = 11;
        QuestionData[] array = new QuestionData[numberOfQuestions];

    }

    public void Init(QuestionData[] array)
    {
        questionArray = array;
        currentIndex = 0;

        if (questionArray != null && questionArray.Length > 0)
        {
            currentQuestion = questionArray[currentIndex];
            UpdateQuestionUI();
        }
    }

   

    private void UpdateQuestionUI()
    {
        if (currentQuestion != null)
        {
            QuestionLabel.Text = currentQuestion.Question;
            QuestionLabel.Text += "\n";
            QuestionLabel.Text += "\n A: " + currentQuestion.Options[0];
            QuestionLabel.Text += "\n B: " + currentQuestion.Options[1];
            QuestionLabel.Text += "\n C: " + currentQuestion.Options[2];
            QuestionLabel.Text += "\n D: " + currentQuestion.Options[3];
        }
    }

    int questionCount = 0; //仮

    private void NextQuestion()
    {
       
        currentIndex++;

        if (currentIndex < questionArray.Length)// 
        {
            currentQuestion = questionArray[currentIndex];
            UpdateQuestionUI();
        }
        else
        {
            GD.Print("ステージクリア");
        }
       
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