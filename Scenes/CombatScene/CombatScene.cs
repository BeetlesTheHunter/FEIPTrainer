using Godot;
using System;
using System.Threading.Tasks;

public partial class CombatScene : Node2D
{
	[Export] private CharacterNode playerNode;
	private Character player;
	[Export] private CharacterNode enemyNode;
	private Character enemy;
	public event Action CombatOver; //TODO call this when one of the characters die

	public void Init(CharacterData playerData, CharacterData enemyData)
	{
		player = new Character(playerNode,playerData);
		enemy = new Character(enemyNode, enemyData);
	}

	public async Task<int> PlayerAttack()
	{
		return await DamageCharacter(player,enemy);
	}

	public async Task<int> EnemyAttack()
	{
		return await DamageCharacter(enemy,player);
	}

	private int minimumDamage = 1;
	private async Task<int> DamageCharacter(Character attacker, Character defender)
	{
		int value;

		int damageAmount = attacker.CharacterData.Strength - defender.CharacterData.Defense;
		if(damageAmount <= 0) damageAmount = minimumDamage; 

		defender.CharacterHealth -= damageAmount;
		value = damageAmount;

		attacker.CharacterNode.AttackAnimation();

		if(defender.CharacterHealth > 0) defender.CharacterNode.DamageAnimation();
		else defender.CharacterNode.DeathAnimation();

		await ToSignal(defender.CharacterNode, AnimatedSprite2D.SignalName.AnimationFinished);
		

		return value;
	}

	private sealed class Character
	{
		public CharacterNode CharacterNode {get; private set;}
		public CharacterData CharacterData {get; private set;}
		public int CharacterHealth {get; set;}

		public Character(CharacterNode character, CharacterData data)
		{
			CharacterNode = character;
			CharacterData = data;
			CharacterHealth = data.Health;

			CharacterNode.CharacterData = data;
		}
	}

	
}


