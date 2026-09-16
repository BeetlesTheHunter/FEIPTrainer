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


    private QuestionData currentQuestion ;

    public event Action CorrectAnswer;
    private void RaiseCorrectAnswer() => CorrectAnswer?.Invoke();
    public event Action WrongAnswer;
    private void RaiseWrongAnswer() => WrongAnswer  ?.Invoke();
    public override void _Ready()
    {
        ButtonA.Pressed += HandleButtonAPressed;
        ButtonB.Pressed += HandleButtonBPressed;
        ButtonC.Pressed += HandleButtonCPressed;
        ButtonD.Pressed += HandleButtonDPressed;

        currentQuestion = QuestionDataManager.Instance.GetQuestionById(4);
        UpdateQuestionUI();
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

        //仮
        //RaiseCorrectAnswer();
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
            RaiseWrongAnswer() ;
        }
        //仮
        //RaiseWrongAnswer();
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
    }


}
