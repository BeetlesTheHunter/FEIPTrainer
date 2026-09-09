using Godot;
using System;

[GlobalClass]
public partial class QuestionData : Resource
{
    public int Id { get; set; } = 0;
    public string Category { get; set; } = string.Empty;
    public string Question { get; set; } = string.Empty;
    public string[] Options { get; set; } = Array.Empty<string>();
    public int AnswerIndex { get; set; } = 0;
    public string QuestionImage { get; set; } = string.Empty;
    public string[] OptionsImages { get; set; } = Array.Empty<string>();

    public QuestionData() { }

    public QuestionData(
        int id, 
        string category, 
        string question, 
        string[] options, 
        int answerIndex, 
        string questionImage, 
        string[] optionsImages)
    {
        Id = id;
        Category = category;
        Question = question;
        Options = options;
        AnswerIndex = answerIndex;
        QuestionImage = questionImage;
        OptionsImages = optionsImages;
    }
}