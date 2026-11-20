# DamageEffect.gdshader

## Description
This shader is designed to apply a flash effect to a canvas item when damage occurs. The flash color and intensity can be adjusted to customize the effect.

## Parameters
- `flash_color`: A vector4 value representing the color of the flash. The default value is white (vec4(1.0, 1.0, 1.0, 1.0)).
- `flash_amount`: A float value that controls the intensity of the flash. The range is from 0.0 to 1.0, with a default value of 0.0.

## Usage
To use this shader, apply it to a canvas item in your scene. Adjust the `flash_color` and `flash_amount` parameters in the shader's properties to customize the flash effect.

## Example
```gdscript
var material = ShaderMaterial.new()
material.shader = preload("res://Scripts/Shaders/DamageEffect.gdshader")
material.shader_params["flash_color"] = Color(1.0, 0.0, 0.0, 1.0)  # Red flash
material.shader_params["flash_amount"] = 0.5

var sprite = Sprite2D.new()
sprite.modulate = Color(1.0, 1.0, 1.0)
sprite.material_override = material
```