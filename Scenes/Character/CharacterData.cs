using Godot;
using System;

[Tool]
[GlobalClass]
public partial class CharacterData : Resource
{
    //Stats
    [Export] public StringName Name { get; set; }
    [Export] public int Health { get; set; }
    [Export] public int Strength { get; set; }
    [Export] public int Defense { get; set; }
    [Export] public SpriteFrames Animations { get; set; }
    //Visuals and Animations
    
}
