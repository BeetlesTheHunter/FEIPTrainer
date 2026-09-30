using Godot;
using System;
using System.Text.Json.Serialization;

[GlobalClass]
public partial class QuestionData : Resource
{
    [JsonPropertyName("id")]
    public int Id { get; set; } = 0;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("question")]
    public string Question { get; set; } = string.Empty;

    [JsonPropertyName("options")]
    public string[] Options { get; set; } = Array.Empty<string>();

    [JsonPropertyName("answerIndex")]
    public int AnswerIndex { get; set; } = 0;

    [JsonPropertyName("questionImage")]
    public string QuestionImage { get; set; } = string.Empty;

    [JsonPropertyName("optionsImages")]
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