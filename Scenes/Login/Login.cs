using Godot;
using System;
using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;
using Supabase;
using Supabase.Gotrue;

public partial class Login : Control
{
    [Export] private Button loginBotan;

    private const string SUPABASE_URL = "https://fujfdfelirgbkbjccdim.supabase.co";
    private string SUPABASE_KEY = "sb_publishable_jzMN0sp1ay8Nq0qTk7UCPg_DbTAJQtz";
    private const string REDIRECT_URI = "http://localhost:54321/";

    private Supabase.Client _supabase;

    public override async void _Ready()
    {
        // 1. ボタンがインスペクターで割り当てられているか確認し、イベントを接続
        if (loginBotan != null)
        {
            loginBotan.Pressed += OnLoginBotanPressed;
            loginBotan.Disabled = true; // Supabaseの初期化が終わるまで押せないようにする
        }
        else
        {
            GD.PrintErr("エラー: loginBotan がインスペクターでアタッチされていません。");
        }

        // 2. Supabase クライアントの初期化
        try
        {
            var options = new SupabaseOptions { AutoConnectRealtime = false };
            _supabase = new Supabase.Client(SUPABASE_URL, SUPABASE_KEY, options);
            await _supabase.InitializeAsync();

            GD.Print("Supabaseの初期化が完了しました。");
            if (loginBotan != null)
            {
                loginBotan.Disabled = false; // 初期化完了後にボタンを押せるようにする
            }
        }
        catch (Exception ex)
        {
            OnLoginFailed($"Supabaseの初期化に失敗しました: {ex.Message}");
        }
    }

    // ボタンが押されたとき（Googleログイン起動）
    private async void OnLoginBotanPressed()
    {
        GD.Print("Google ログイン処理を開始します...");

        if (loginBotan != null)
        {
            loginBotan.Disabled = true; // 連打防止
        }

        if (_supabase == null)
        {
            OnLoginFailed("Supabase クライアントが初期化されていません。");
            return;
        }

        try
        {
            // 1. ローカルでブラウザからの戻りを待つ簡易サーバーを起動
            using var listener = new HttpListener();
            listener.Prefixes.Add(REDIRECT_URI);
            listener.Start();

            // 2. Googleログイン用のURLを取得してOSの標準ブラウザで開く
            var providerAuth = await _supabase.Auth.SignIn(Constants.Provider.Google, new SignInOptions
            {
                RedirectTo = REDIRECT_URI,
                FlowType = Constants.OAuthFlowType.PKCE
            });

            Process.Start(new ProcessStartInfo
            {
                FileName = providerAuth.Uri.ToString(),
                UseShellExecute = true
            });

            GD.Print("ブラウザでのGoogleログイン画面を開きました。操作を待機中...");

            // 3. ユーザーがブラウザでログインするのを待機
            var context = await listener.GetContextAsync();
            var authCode = context.Request.QueryString["code"];
            var errorDesc = context.Request.QueryString["error_description"];

            // 4. ブラウザ画面に結果を表示して閉じてもらう
            var response = context.Response;
            response.ContentType = "text/html; charset=utf-8"; // 文字化け・解釈ミス防止
            string html = string.IsNullOrEmpty(errorDesc)
                ? "<html><meta charset='utf-8'><body><h1>ログイン成功！</h1><p>このタブを閉じてアプリに戻ってください。</p></body></html>"
                : $"<html><meta charset='utf-8'><body><h1>ログインエラー</h1><p>{errorDesc}</p></body></html>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(html);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.OutputStream.Flush(); // データを確実に送り出す
            response.Close(); // レスポンスを正常終了させる

            await Task.Delay(500); // ブラウザが画面を描画しきるまで0.5秒だけ待つ
            listener.Stop();

            // 5. 認証コードをSupabaseに渡してログイン確定
            if (!string.IsNullOrEmpty(authCode))
            {
                var session = await _supabase.Auth.ExchangeCodeForSession(providerAuth.PKCEVerifier, authCode);
                if (session?.User != null)
                {
                    OnLoginSucceeded(session.User);
                }
                else
                {
                    OnLoginFailed("ユーザー情報の取得に失敗しました。");
                }
            }
            else
            {
                OnLoginFailed($"認証コードが取得できませんでした。詳細: {errorDesc}");
            }
        }
        catch (Exception ex)
        {
            OnLoginFailed(ex.Message);
        }
    }

    // ログイン成功時
    private void OnLoginSucceeded(User user)
    {
        GD.Print("====================================");
        GD.Print("★ Google ログインに成功しました！");

        // Googleアカウントの表示名（full_name または name）を取得
        string name = "名前なし";
        if (user.UserMetadata != null)
        {
            if (user.UserMetadata.TryGetValue("full_name", out var fullName) && fullName != null)
                name = fullName.ToString();
            else if (user.UserMetadata.TryGetValue("name", out var userName) && userName != null)
                name = userName.ToString();
        }

        string email = !string.IsNullOrEmpty(user.Email) ? user.Email : "メールなし";
        string uid = !string.IsNullOrEmpty(user.Id) ? user.Id : "";

        GD.Print($"表示名: {name}");
        GD.Print($"メール: {email}");
        GD.Print($"Supabase固有UID: {uid}");
        GD.Print("====================================");

        // TODO: ここで学習用メイン画面シーンに移動する処理を記述します
        // GetTree().ChangeSceneToFile("res://main_menu.tscn");
    }

    // ログイン失敗時
    private void OnLoginFailed(string message)
    {
        GD.PrintErr($"ログイン失敗: メッセージ={message}");

        // 失敗した場合はボタンを再度押せる状態に戻す
        if (loginBotan != null)
        {
            loginBotan.Disabled = false;
        }
    }
}