using Godot;
using System;

[Tool]
[GlobalClass]
public partial class CharacterData : Resource
{
    //Stats
    [Export] public StringName Name {get; private set;}
    [Export] public int Health {get; private set;}
    [Export] public int Strength {get; private set;}
    [Export] public int Defense {get; private set;}

    //Visuals and Animations
    [Export] public SpriteFrames Animations {get; private set;}



}
