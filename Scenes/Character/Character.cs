using Godot;
using System;
using System.Threading.Tasks;


[Tool]
public partial class Character : AnimatedSprite2D
{
	public const string IdleAnimation = "idle";
	public const string ATKAnimation = "attack";
	public const string DeathAnimation = "death";
	public const string WalkAnimation = "walk";
	public const string DamagedAnimation = "damaged";

	private const string DamageShaderPath = "uid://q327ahvrhn3o";
	private ShaderMaterial DamageShader;


	public event Action OnCharacterDeath;
	public void RaiseCharacterDeath() => OnCharacterDeath?.Invoke();

	public event Action OnDamageOver;
	public void RaiseDamageOver() => OnDamageOver?.Invoke();


	[Export] public CharacterData CharacterData{ 
		get => _characterData;
		set
		{
			_characterData = value;
			UpdateCharacter();
		}  
	}
	private CharacterData _characterData;

	[Export] public string CharacterName {get; private set;}
	[Export] public int Health {get; private set;}
	[Export] public int Strength {get; private set;}
	[Export] public int Defense {get; private set;}

	public void Init(CharacterData cd)
	{
		CharacterData = cd;
	}

	public override void _Ready()
	{
		UpdateCharacter();
		if(SpriteFrames != null) Play(IdleAnimation);
		AnimationFinished += HandleAnimaitionFinished;
		if(!Engine.IsEditorHint()) DamageShader = GD.Load<ShaderMaterial>(DamageShaderPath).Duplicate() as ShaderMaterial;
	}

	private void UpdateCharacter()
	{
		if(CharacterData is null)
		{
			CharacterName = null;
			Health = 0;
			Strength = 0;
			Defense = 0;
			SpriteFrames = null;
			return;
		} 

		CharacterName = CharacterData.Name;
		Health = CharacterData.Health;
		Strength = CharacterData.Strength;
		Defense = CharacterData.Defense;
		SpriteFrames = CharacterData.Animations;

		if(SpriteFrames != null)Play(IdleAnimation);
	}

	public void Damage(int damageValue)
	{
		damageValue -= Defense;
		if(damageValue <= 0) damageValue = 1;

		Health -= damageValue;
		DamagedEffect();

		if(Health <= 0)
		{
			Play(DeathAnimation);
			
		}
		GD.Print(Health);
	}

	private async void DamagedEffect()
	{
		Material = DamageShader;

		Play(DamagedAnimation);

		Tween tween = GetTree().CreateTween();
		tween.BindNode(this);
		tween.SetLoops(0);
		tween.TweenProperty(Material, "shader_parameter/flash_amount", 1.0f, 0.25f);
		tween.TweenProperty(Material, "shader_parameter/flash_amount", 0.0f, 0.25f);

		await ToSignal(this, AnimatedSprite2D.SignalName.AnimationFinished);
		if(!IsInstanceValid(instance:this)) return;

		tween.Kill();
		Material = null;
		RaiseDamageOver();
	}

	public void HandleAnimaitionFinished()
	{
		if (Animation == DeathAnimation)
		{
			RaiseCharacterDeath();
			return;
		}
		Play(IdleAnimation);

		
	}

	

	
}
