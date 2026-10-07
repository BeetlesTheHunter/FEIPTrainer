using Godot;
using Godot.Collections;
using System;

public partial class Login : Control
{
    [Export] private Button loginBotan;

    private GodotObject _firebase;
    private GodotObject _auth;

    public override void _Ready()
    {
        // 1. ボタンがインスペクターで割り当てられているか確認し、イベントを接続
        if (loginBotan != null)
        {
            loginBotan.Pressed += OnLoginBotanPressed;
        }
        else
        {
            GD.PrintErr("エラー: loginBotan がインスペクターでアタッチされていません。");
        }

        // 2. Autoload の Firebase シングルトンと Auth オブジェクトを取得
        _firebase = GetNode<GodotObject>("/root/Firebase");
        if (_firebase != null)
        {
            _auth = (GodotObject)_firebase.Get("Auth");
            
            if (_auth != null)
            {
                // GDScript 側のシグナル (login_succeeded / login_failed) を C# のメソッドに接続
                _auth.Connect("login_succeeded", Callable.From<Dictionary>(OnLoginSucceeded));
                _auth.Connect("login_failed", Callable.From<int, string>(OnLoginFailed));
            }
            else
            {
                GD.PrintErr("エラー: Firebase Auth モジュールの取得に失敗しました。");
            }
        }
        else
        {
            GD.PrintErr("エラー: /root/Firebase シングルトンが見つかりません。Autoload の設定を確認してください。");
        }
    }

    // ボタンが押されたとき（Googleログイン起動）
    private void OnLoginBotanPressed()
    {
        GD.Print("Google ログイン処理を開始します...");

        if (loginBotan != null)
        {
            loginBotan.Disabled = true; // 連打防止
        }

        // ==========================================================
        // 【追加修正】ここで clientId を強制的に設定してエラーを回避します！
        if (_firebase != null)
        {
            Godot.Collections.Dictionary config = _firebase.Get("config").As<Godot.Collections.Dictionary>();
            if (config != null)
            {
                // ↓★★★★★ 注意：ここをご自身のウェブ クライアントIDに書き換えてください ★★★★★
                config["clientId"] = "431530766200-jpklf767m243akgtc7ugv8ikrthfot7n.apps.googleusercontent.com"; 
                // ↑★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★

                // 書き換えた設定を GDScript (Firebase) 側にセットして反映させる
                _firebase.Set("config", config); 
                GD.Print("Firebaseのconfigに clientId をセットしました。");
            }
        }
        // ==========================================================

        if (_auth != null)
        {
            Variant providerVariant;
            providerVariant = _auth.Call("get_GoogleProvider");

            

            GodotObject googleProvider = providerVariant.As<GodotObject>();

            if (googleProvider != null)
            {
                // プラグインの仕様に合わせてログイン関数を呼び出す
                if (_auth.HasMethod("get_auth_with_popup"))
                {
                    _auth.Call("get_auth_with_popup", googleProvider);
                    GD.Print("ブラウザでのGoogleログイン画面を開きます(popup)...");
                }
                else if (_auth.HasMethod("get_auth_with_redirect"))
                {
                    _auth.Call("get_auth_with_redirect", googleProvider);
                    GD.Print("ブラウザでのGoogleログイン画面を開きます(redirect)...");
                }
                else if (_auth.HasMethod("login_with_oauth"))
                {
                    _auth.Call("login_with_oauth", googleProvider);
                    GD.Print("OAuthログイン処理を実行します...");
                }
                else
                {
                    GD.PrintErr("エラー: ログイン実行用のメソッドが見つかりません。");
                    if (loginBotan != null) loginBotan.Disabled = false;
                }
            }
        }
    }

    // ログイン成功時
    private void OnLoginSucceeded(Dictionary authResult)
    {
        GD.Print("====================================");
        GD.Print("★ Google ログインに成功しました！");
        
        string name = authResult.ContainsKey("displayName") ? authResult["displayName"].ToString() : "名前なし";
        string email = authResult.ContainsKey("email") ? authResult["email"].ToString() : "メールなし";
        string uid = authResult.ContainsKey("localid") ? authResult["localid"].ToString() : "";

        GD.Print($"表示名: {name}");
        GD.Print($"メール: {email}");
        GD.Print($"Firebase固有UID: {uid}");
        GD.Print("====================================");

        // TODO: ここで学習用メイン画面シーンに移動する処理を記述します
        // GetTree().ChangeSceneToFile("res://main_menu.tscn");
    }

    // ログイン失敗時
    private void OnLoginFailed(int errorCode, string message)
    {
        GD.PrintErr($"ログイン失敗: エラーコード={errorCode}, メッセージ={message}");
        
        // 失敗した場合はボタンを再度押せる状態に戻す
        if (loginBotan != null)
        {
            loginBotan.Disabled = false;
        }
    }
}