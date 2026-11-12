# BackgroundBlur.gdshader

## 説明
このシェーダーはキャンバスアイテムにぼかし効果を適用するために設計されています。ぼかしの強度は調整してぼかしの強度を制御できます。

## パラメータ
- `blur_amount`: ブラシの量を制御する浮動小数点数。範囲は0.0から10.0で、既定値は2.5です。

## 使用方法
このシェーダーを使用するには、シーンにキャンバスアイテムを適用します。シェーダーのプロパティで`blur_amount`パラメータを調整してぼかしの強度を制御します。

## 例
```gdscript
var material = ShaderMaterial.new()
material.shader = preload("res://Scripts/Shaders/BackgroundBlur.gdshader")
material.shader_params["blur_amount"] = 5.0

var sprite = Sprite2D.new()
sprite.modulate = Color(0.5, 0.5, 0.5)
sprite.material_override = material
```