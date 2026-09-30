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

        QuestionData[] array = new QuestionData[5];

        array[0] = QuestionDataManager.Instance.GetQuestionById(1);
        array[1] = QuestionDataManager.Instance.GetQuestionById(2);
        array[2] = QuestionDataManager.Instance.GetQuestionById(3);
        array[3] = QuestionDataManager.Instance.GetQuestionById(4);
        array[4] = QuestionDataManager.Instance.GetQuestionById(5);

        init(array);




    }

    // 仕様書通りのメソッド名（_init）
    public void init(QuestionData[] array) // 配列を受け取るためのメソッド
    {
        questionArray = array;
        currentIndex = 0;

        if (questionArray != null && questionArray.Length > 0)
        {
            currentQuestion = questionArray[currentIndex];
            UpdateQuestionUI();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
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
        // 配列が渡されている場合は配列から次の問題を取得
        if (questionArray != null && questionArray.Length > 0)
        {
            currentIndex++;

            if (currentIndex < questionArray.Length)// 
            {
                currentQuestion = questionArray[currentIndex];
                UpdateQuestionUI();
            }
            else
            {
                // 最後の問題を答え終わったら「ステージクリア」と出力
                GD.Print("ステージクリア");
            }
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