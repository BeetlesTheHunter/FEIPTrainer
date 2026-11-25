using Godot;
using System;
using System.Threading.Tasks;

public partial class CombatScene : Node2D
{
	[Export] private CharacterNode playerNode;
	private Character player;
	[Export] private CharacterNode enemyNode;
	private Character enemy;

	public enum CombatResult{
		Continue,
		CombatOver,
		// Bottom two can be used to tell quiz scene who won the battle if necessary in the future
		PlayerVictory,
		EnemyVictory
	}
	public event Action<CombatResult,int> CombatOver; 
	private void RaiseCombatOver(CombatResult result, int score) => CombatOver?.Invoke(result,score);

	public void Init(CharacterData playerData, CharacterData enemyData)
	{
		player = new Character(playerNode,playerData);
		enemy = new Character(enemyNode, enemyData);
	}

	public async void PlayerAttack()
	{
		if(!IsInstanceValid(this) || !IsInsideTree()) return;
		
		try
		{
			await DamageCharacter(player,enemy);
		}
		catch(OperationCanceledException e)
		{
			GD.PrintErr(e);
		}
		catch(ObjectDisposedException e)
		{
			GD.PrintErr(e);
		}
		catch (Exception ex)
		{
			GD.PrintErr($"PlayerAttack failed: {ex}");
		}
	}

	public async void EnemyAttack()
	{
		if(!IsInstanceValid(this) || !IsInsideTree()) return;
		
		try
		{
			await DamageCharacter(enemy,player);
		}
		catch(OperationCanceledException e)
		{
			GD.PrintErr(e);
		}
		catch(ObjectDisposedException e)
		{
			GD.PrintErr(e);
		}
		catch (Exception ex)
		{
			GD.PrintErr($"EnemyAttack failed: {ex}");
		}
	}

	private int minimumDamage = 1;
	private const int MAXFALLBACKSECONDS = 3;
	private static async Task WaitSignal(SignalAwaiter awaiter) => await awaiter;
	private async Task DamageCharacter(Character attacker, Character defender)
	{
		int value;

		int damageAmount = attacker.CharacterData.Strength - defender.CharacterData.Defense;
		if(damageAmount <= 0) damageAmount = minimumDamage; 
		
		defender.CharacterHealth -= damageAmount;
		value = damageAmount;

		attacker.CharacterNode.AttackAnimation();

		if(defender.CharacterHealth > 0){ defender.CharacterNode.DamageAnimation();}
		else defender.CharacterNode.DeathAnimation();
		
		var animTask = ToSignal(defender.CharacterNode, AnimatedSprite2D.SignalName.AnimationFinished);
		var timeOutTask = ToSignal(GetTree().CreateTimer(MAXFALLBACKSECONDS), SceneTreeTimer.SignalName.Timeout);

		await Task.WhenAny(
			WaitSignal(animTask),
			WaitSignal(timeOutTask)
		);

		if(!IsInstanceValid(this) || !IsInsideTree()) throw new OperationCanceledException();
		
		if(defender.CharacterHealth <= 0)
		{
			RaiseCombatOver(CombatResult.CombatOver, value);
			return;
		} 

		RaiseCombatOver(CombatResult.Continue, value);
		
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


