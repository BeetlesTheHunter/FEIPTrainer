using Godot;
using Godot.Collections;

public partial class FirebaseConfig : Node
{
    public override void _Ready()
    {
        // firebase.env の読み込み
        string path = "res://firebase.env";
        if (!FileAccess.FileExists(path))
        {
            GD.PrintErr("エラー: firebase.env が見つかりません。設定ファイルを作成してください。");
            return;
        }

        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        string jsonText = file.GetAsText();

        var json = new Json();
        Error parseResult = json.Parse(jsonText);
        if (parseResult != Error.Ok)
        {
            GD.PrintErr($"JSONパースエラー: {json.GetErrorMessage()} (行: {json.GetErrorLine()})");
            return;
        }

        var data = (Dictionary)json.Data;

        // 1. Firebase基本設定の構築
        GodotObject firebase = GetNode<GodotObject>("/root/Firebase");
        if (firebase == null) return;

        var config = new Godot.Collections.Dictionary
        {
            { "apiKey", (string)data["api_key"] },
            { "authDomain", (string)data["auth_domain"] },
            { "projectId", (string)data["project_id"] },
            { "storageBucket", (string)data["storage_bucket"] },
            { "messagingSenderId", (string)data["messaging_sender_id"] },
            { "appId", (string)data["app_id"] }
        };

        firebase.Set("config", config);

        // 2. Google OAuthプロバイダの設定
        GodotObject auth = (GodotObject)firebase.Get("Auth");
        if (auth != null)
        {
            // 既存の _config を取得（消さないようにする）
            var authConfig = (Godot.Collections.Dictionary)auth.Get("_config");
            if (authConfig == null)
            {
                authConfig = new Godot.Collections.Dictionary();
            }

            // auth.gd が参照している変数名（キャメルケース）に合わせて更新
            authConfig["clientId"] = (string)data["client_id"];
            authConfig["clientSecret"] = (string)data["client_secret"];

            // auth_providers キーが存在しない場合は空の辞書をセットしておく
            if (!authConfig.ContainsKey("auth_providers"))
            {
                authConfig["auth_providers"] = new Godot.Collections.Dictionary();
            }

            // 反映
            auth.Set("_config", authConfig);
            auth.Set("_local_port", 8060);
        }
    }
}