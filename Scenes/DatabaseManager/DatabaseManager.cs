using Godot;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public partial class DatabaseManager : Node
{
    // Project Settings -> API からコピーして貼り付ける
    private const string SupabaseUrl = "https://fujfdfelirgbkbjccdim.supabase.co";
    private const string SupabasePublishableKey = "sb_publishable_jzMN0sp1ay8Nq0qTk7UCPg_DbTAJQtz";

    // 通信に使い回すHttpClient（郵便屋さん）
    private static readonly System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();

    public override void _Ready()
    {
       
        _client.DefaultRequestHeaders.Add("apikey", SupabasePublishableKey);
     
    }

    /// <summary>
    /// Googleログイン成功後に呼び出し、本人のアクセストークン（JWT）をセットする
    /// これがないとRLS（authenticated）を通過できない！
    /// </summary>
    public void SetAccessToken(string accessToken)
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
        GD.Print("アクセストークンをセットしました");
    }

    /// <summary>
    /// データを保存する（新規作成 or 上書き更新）
    /// </summary>
    public async void SaveUserData(UserData data)
    {
        GD.Print("UserDataのデータベースへの保存をを開始します...");
		string url = $"{SupabaseUrl}/rest/v1/user_data";

        string json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = content;
        // 同じuser_idがあれば上書き（UPDATE）、無ければ新規作成（INSERT）する設定
        request.Headers.Add("Prefer", "resolution=merge-duplicates");

        try
        {
            HttpResponseMessage response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                GD.Print("保存成功！");
                return ;
            }

            string error = await response.Content.ReadAsStringAsync();
            GD.PrintErr($"保存失敗: {response.StatusCode} / {error}");
            return ;
        }
        catch (Exception e)
        {
            GD.PrintErr($"通信エラー: {e.Message}");
            return ;
        }
    }

    /// <summary>
    /// 指定したuserId（UUID）のデータを読み込む
    /// </summary>
    public async Task<UserData> LoadUserDataAsync(string userId)
    {
        GD.Print("UserDataをデータベースから読み込みます... ");
		string url = $"{SupabaseUrl}/rest/v1/user_data?user_id=eq.{userId}&select=*";

        try
        {
            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var list = JsonSerializer.Deserialize<List<UserData>>(json);

                if (list != null && list.Count > 0)
                {
                    GD.Print("読み込み成功！");
                    return list[0];
                }

                GD.Print("データが見つかりません（初回ログイン）");
                return null;
            }

            string error = await response.Content.ReadAsStringAsync();
            GD.PrintErr($"読み込み失敗: {response.StatusCode} / {error}");
            return null;
        }
        catch (Exception e)
        {
            GD.PrintErr($"通信エラー: {e.Message}");
            return null;
        }
    }
}