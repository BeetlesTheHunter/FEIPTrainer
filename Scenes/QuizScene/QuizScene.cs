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
    private int questionNumber = 0; //何問やったか管理するために使う
    private int randomQuestionNumberMax = 0;
    
    [Export] public ScoreManager scoreManager {get; private set;}
    private QuestionDataManager questionDataManager;

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

    public void Init(QuestionDataManager qdm, int numQuestions)
    {
        quizMode = QuizMode.Randomized;

        questionDataManager = qdm;

        questionNumber = 0;
        randomQuestionNumberMax = numQuestions;

        currentQuestion = questionDataManager.GetRandomQuestion();
        UpdateQuestionUI();
    }

    public void Init(QuestionDataManager qdm, QuestionData[] array)
    {
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
        GD.Print("end of quiz reached");
    }


    private const int ButtonAValue = 0;
    private const int ButtonBValue = 1;
    private const int ButtonCValue = 2;
    private const int ButtonDValue = 3;


    private void CheckAnswer(int answerIndex)
    {
        if (currentQuestion.AnswerIndex == answerIndex)
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

    private void DisableButtons(bool disabled)
    {
        ButtonA.Disabled = disabled;
        ButtonB.Disabled = disabled;
        ButtonC.Disabled = disabled;
        ButtonD.Disabled = disabled;

    }

    private void HandleButtonAPressed()
    {
        CheckAnswer(ButtonAValue);
        DisableButtons(true);
        NextQuestion();
    }

    private void HandleButtonBPressed()
    {
        CheckAnswer(ButtonBValue);
        DisableButtons(true);
        NextQuestion();
    }

    private void HandleButtonCPressed()
    {
        CheckAnswer(ButtonCValue);
        DisableButtons(true);
        NextQuestion();
    }

    private void HandleButtonDPressed()
    {
        CheckAnswer(ButtonDValue);
        DisableButtons(true);
        NextQuestion();
    }

    
}