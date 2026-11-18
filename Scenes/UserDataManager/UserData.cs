using Godot;
using System;

public partial class UserData : Resource
{
    public int userId{get; private set;}//ユーザーID
    public string userName{get; private set;}//ユーザー名
    public long totalScore{get; set;}//総スコア
    public int consecutiveLoginNumber{get; set;}//連続ログイン数
    public int lastLoginDate{get; set;}//最終ログイン日
    public int totalAnswerCount{get; set;}//解いた問題数
    public int totalCorrectCount{get; set;}//正解数

    public UserData(int id, string name, long score, int loginNumber, int lastLogin, int answerCount, int correctCount)
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
        userId = 0;
        userName = "";
        totalScore = 0;
        consecutiveLoginNumber = 0;
        lastLoginDate = 0;
        totalAnswerCount = 0;
        totalCorrectCount = 0;
    }
    
}
