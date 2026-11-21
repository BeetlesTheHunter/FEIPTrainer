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
		
		playerCharacter.OnCharacterDeath += RaiseCombatOver;
		enemyCharacter.OnCharacterDeath +=  RaiseCombatOver;

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
		//EnemyCharacter.Damage(PlayerCharacter.Strength);

		return value;
	}

	public int EnemyAttack()
	{
		int value;

		value = playerCharacterData.Defense - enemyCharacterData.Strength;
		if(value >= 0) value = 0;
		

		enemyCharacter.AttackAnimation();
		playerCharacter.DamageAnimation();
		//PlayerCharacter.Damage(EnemyCharacter.Strength);

		return value;
	}



	
}
