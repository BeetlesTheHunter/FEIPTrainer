using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class UserDataManager : Node
{
	public static UserDataManager Instance { get; private set; }

	private UserData userData;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		userData= new UserData();
	}

	public void setUserId(int userId)
	{
		userData.userId = userId;
    }
	

	public int getUserId()
	{
		return userData.userId;
	}
	

	

}
