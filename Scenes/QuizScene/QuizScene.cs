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

        currentQuestion = QuestionDataManager.Instance.GetQuestionById(23);
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
        GD.Print("A");
        //仮
        //RaiseCorrectAnswer();
    }
    private void HandleButtonBPressed()
    {
        GD.Print("B");
        //仮
        //RaiseWrongAnswer();
    }
    private void HandleButtonCPressed()
    {
        GD.Print("C");
    }
    private void HandleButtonDPressed()
    {
        GD.Print("D");
    }


}
