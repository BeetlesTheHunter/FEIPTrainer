using Godot;
using System;

public partial class UserDataManager : Node
{
	

	public UserData userData{ get; private set; }

	public void SetUserData(UserData data)
	{
		userData = data;
	}
	public enum LoginDateStatus
	{
		Today,
		Yesterday,
		Other
	}
	public int GetTodayDateInt()
	{
    	DateTime today = DateTime.Today;
    	return today.Year * 10000 + today.Month * 100 + today.Day;
	}

	public LoginDateStatus CheckLastLoginStatus()
	{
	    if (userData == null) return LoginDateStatus.Other;
    	int todayInt = GetTodayDateInt();
		
    	DateTime yesterday = DateTime.Today.AddDays(-1);
    	int yesterdayInt = yesterday.Year * 10000 + yesterday.Month * 100 + yesterday.Day;

    	if (userData.lastLoginDate == todayInt)
    	{
        	return LoginDateStatus.Today;
    	}
    	else if (userData.lastLoginDate == yesterdayInt)
    	{
        	return LoginDateStatus.Yesterday;
    	}
    	else
    	{
        	return LoginDateStatus.Other;
    	}
	}
	
	public void DoneDaily()
	{
    	userData.lastLoginDate = GetTodayDateInt();
		userData.consecutiveLoginNumber++;
	}
	public void UpdateConsecutiveLoginNumber()
	{
		LoginDateStatus status = CheckLastLoginStatus();
		if (status == LoginDateStatus.Other)
		{
			userData.consecutiveLoginNumber = 0;
		}
		
	}


	

	

}
