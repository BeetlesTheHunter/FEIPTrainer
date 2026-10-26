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
    private int currentIndex = 0; // 配列のインデックス管理用
    
    [Export] public ScoreManager scoreManager {get; private set;}
    private QuestionDataManager questionDataManager;

    

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

    public void Init(QuestionDataManager qdm, QuestionData[] array)
    {
        questionDataManager = qdm;

        questionArray = array;
        currentIndex = 0;

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
            currentQuestion = questionArray[currentIndex];
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