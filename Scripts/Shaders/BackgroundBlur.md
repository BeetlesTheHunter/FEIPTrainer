# BackgroundBlur.gdshader

## Description
This shader is designed to apply a blur effect to a canvas item. The blur amount can be adjusted to control the intensity of the blur.

## Parameters
- `blur_amount`: A float value that controls the amount of blur. The range is from 0.0 to 10.0, with a default value of 2.5.

## Usage
To use this shader, apply it to a canvas item in your scene. Adjust the `blur_amount` parameter in the shader's properties to control the blur intensity.

## Example
```gdscript
var material = ShaderMaterial.new()
material.shader = preload("res://Scripts/Shaders/BackgroundBlur.gdshader")
material.shader_params["blur_amount"] = 5.0

var sprite = Sprite2D.new()
sprite.modulate = Color(0.5, 0.5, 0.5)
sprite.material_override = material
```