using Godot;
using System;

public partial class CombatScene : Node2D
{
	[Export] private Character playerCharacter;
	private CharacterData playerCharacterData;
	private int playerHealth;

	[Export] private Character enemyCharacter;
	private CharacterData enemyCharacterData;
	private int enemyHealth;

	public event Action CombatOver;
	private void RaiseCombatOver() => CombatOver?.Invoke();

	public event Action ProcessOver;
	private void RaiseProcessOver() => ProcessOver?.Invoke();

	public void Init(CharacterData player, CharacterData enemy)
	{
		playerCharacter.CharacterData = player;
		playerHealth = player.Health;

		enemyCharacter.CharacterData = enemy;
		enemyHealth = enemy.Health;
	}
	public override void _Ready()
	{
		playerCharacter.OnDamageOver += RaiseProcessOver;
		enemyCharacter.OnDamageOver +=  RaiseProcessOver;
		
	}

	public int PlayerAttack()
	{
		int value;

		value = playerCharacterData.Strength - enemyCharacterData.Defense;
		if(value <= 0) value = 1;

		playerCharacter.AttackAnimation();
		enemyCharacter.DamageAnimation();

		return value;
	}

	public int EnemyAttack()
	{
		int value;

		value = playerCharacterData.Defense - enemyCharacterData.Strength;
		if(value >= 0) value = 0;
		

		enemyCharacter.AttackAnimation();
		playerCharacter.DamageAnimation();

		return value;
	}

	//Code that has been taken out of the Character Scene that should be moved to this class
	//Allowing the Character class to only manage visuals while this class can control the game logic

	//This should be moved to the combatscene
	// public void Damage(int damageValue)
	// {
	// 	damageValue -= Defense;
	// 	if(damageValue <= 0) damageValue = 1;

	// 	Health -= damageValue;
	// 	DamagedEffect();

	// 	if(Health <= 0)
	// 	{
	// 		Play(DeathAnimation);
			
	// 	}
	// 	GD.Print(Health);
	// }



	
}
