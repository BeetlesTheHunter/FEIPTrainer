using Godot;
using System;
using System.Text.Json.Serialization;

public class UserData //: Resource
{
    [JsonPropertyName("user_id")]
    public string userId{get; set;}="";//ユーザーID
    
    [JsonPropertyName("user_name")]
    public string userName{get; set;}="";//ユーザー名
    
    [JsonPropertyName("total_score")]
    public long totalScore{get; set;}//総スコア
    
    [JsonPropertyName("consecutive_login_number")]
    public int consecutiveLoginNumber{get; set;}//連続ログイン数
    
    [JsonPropertyName("last_login_date")]
    public int lastLoginDate{get; set;}//最終ログイン日
    
    [JsonPropertyName("total_answer_count")]
    public int totalAnswerCount{get; set;}//解いた問題数
    
    [JsonPropertyName("total_correct_count")]
    public int totalCorrectCount{get; set;}//正解数

    public UserData(string id, string name, long score, int loginNumber, int lastLogin, int answerCount, int correctCount)
    {
        userId = id;
        userName = name;
        totalScore = score;
        consecutiveLoginNumber = loginNumber;
        lastLoginDate = lastLogin;
        totalAnswerCount = answerCount;
        totalCorrectCount = correctCount;
    }
    public UserData()
    {
        userId = "";
        userName = "";
        totalScore = 0;
        consecutiveLoginNumber = 0;
        lastLoginDate = 0;
        totalAnswerCount = 0;
        totalCorrectCount = 0;
    }
    public UserData(string id)
    {
        userId = id;
        userName = "";
        totalScore = 0;
        consecutiveLoginNumber = 0;
        lastLoginDate = 0;
        totalAnswerCount = 0;
        totalCorrectCount = 0;
    }
    
}
