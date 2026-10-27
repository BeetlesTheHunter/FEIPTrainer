using Godot;
using System;

public partial class CombatScene : Node2D
{
	[Export] public Character PlayerCharacter { get; private set; }
	[Export] public Character EnemyCharacter { get; private set; }

	public event Action CombatOver;
	private void RaiseCombatOver() => CombatOver?.Invoke();

	public event Action ProcessOver;
	private void RaiseProcessOver() => ProcessOver?.Invoke();

	public override void _Ready()
	{
		PlayerCharacter.OnCharacterDeath += RaiseCombatOver;
		EnemyCharacter.OnCharacterDeath +=  RaiseCombatOver;

		PlayerCharacter.OnDamageOver += RaiseProcessOver;
		EnemyCharacter.OnDamageOver +=  RaiseProcessOver;
		
	}

	public int PlayerAttack()
	{
		int value;

		value = PlayerCharacter.Strength - EnemyCharacter.Defense;
		if(value <= 0) value = 1;

		PlayerCharacter.Play(Character.ATKAnimation);
		EnemyCharacter.Damage(PlayerCharacter.Strength);

		return value;
	}

	public int EnemyAttack()
	{
		int value;

		value = PlayerCharacter.Defense - EnemyCharacter.Strength;
		if(value >= 0) value = 0;
		

		EnemyCharacter.Play(Character.ATKAnimation);
		PlayerCharacter.Damage(EnemyCharacter.Strength);

		return value;
	}



	
}
