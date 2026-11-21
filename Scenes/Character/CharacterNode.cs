using Godot;
using System;



[Tool]
public partial class CharacterNode : AnimatedSprite2D
{
	private const string IDLEANIMATION = "idle";
	private const string ATKANIMATION = "attack";
	private const string DEATHANIMATION = "death";
	private const string WALKANIMATION = "walk";
	private const string DAMAGEDANIMATION = "damaged";

	private const string DAMAGESHADERUID = "uid://q327ahvrhn3o";
	private ShaderMaterial DamageShader;

	public event Action DamageOver;
	public void RaiseDamageOver() => DamageOver?.Invoke();


	[Export] public CharacterData CharacterData{ 
		get => _characterData;
		set
		{
			_characterData = value;
			UpdateCharacter();
		}  
	}
	private CharacterData _characterData;

	public void Init(CharacterData cd)
	{
		CharacterData = cd;
	}

	public override void _Ready()
	{
		UpdateCharacter();
		if(SpriteFrames != null) Play(IDLEANIMATION);
		AnimationFinished += HandleAnimaitionFinished;
		if(!Engine.IsEditorHint()) DamageShader = GD.Load<ShaderMaterial>(DAMAGESHADERUID).Duplicate() as ShaderMaterial;
		if(!Engine.IsEditorHint()) Material = DamageShader;
	}

	private void UpdateCharacter()
	{
		if(CharacterData is null)
		{
	
			SpriteFrames = null;
			return;
		} 


		SpriteFrames = CharacterData.Animations;

		if(SpriteFrames != null)Play(IDLEANIMATION);
	}

	public void AttackAnimation()
	{
		Play(ATKANIMATION);
	}

	public void DamageAnimation()
	{
		Play(DAMAGEDANIMATION);
		DamageFlashEffect();
	}

	public void DeathAnimation()
	{
		Play(DEATHANIMATION);
		DamageFlashEffect();
	}


	private const int FLASHTIMES = 2;
	private const float FLASHMAX = 1.0f;
	private const float FLASHSPEED = 0.25f;
	private void DamageFlashEffect()
	{
		Tween tween = GetTree().CreateTween();
		tween.BindNode(this);
		tween.SetLoops(FLASHTIMES);
		tween.TweenProperty(Material, "shader_parameter/flash_amount", FLASHMAX, FLASHSPEED);
		tween.TweenProperty(Material, "shader_parameter/flash_amount", 0.0f, FLASHSPEED);
		
	}

	public void HandleAnimaitionFinished()
	{
		// This guard prevents the sprite from going back to the idle animation after the death animation. 
		if (Animation == DEATHANIMATION) return;
		if (Animation == DAMAGEDANIMATION)
		{
			if(Material is ShaderMaterial mat)mat.SetShaderParameter("flash_amount",0f);
			RaiseDamageOver();
		}
		
		Play(IDLEANIMATION);

		
	}

}
