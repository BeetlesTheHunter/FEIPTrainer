using Godot;
using System;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// 問題データの「組み合わせ」を表す種類。
/// QuizScene はこの種類を見て、左側ページ(左上・左下)の配置を決める。
/// </summary>
public enum QuestionLayoutType
{
    /// <summary>問題文(テキスト) + 選択肢(テキスト)</summary>
    TextOnly,
    /// <summary>問題文(テキスト) + 問題画像 + 選択肢(テキスト)</summary>
    TextWithImage,
    /// <summary>問題画像のみ(問題文なし) + 選択肢(テキスト)</summary>
    ImageOnly,
    /// <summary>問題画像の中に選択肢も含まれている(選択肢テキストなし)</summary>
    OptionsInImage,
    /// <summary>選択肢が画像(問題文・問題画像はあってもなくてもよい)</summary>
    OptionsAsImages
}

/// <summary>
/// QuestionData を描画しやすい形に整えたもの。QuestionData 自体は変更しない。
/// </summary>
public sealed class QuestionView
{
    public QuestionLayoutType Type;
    public string Text = "";

    /// <summary>JSON に書かれていた画像パス(生)。空なら画像なし。</summary>
    public string RawImagePath = "";
    /// <summary>res:// に解決できたパス。見つからなければ空。</summary>
    public string ResolvedImagePath = "";

    public string[] OptionTexts = new string[4];
    public string[] RawOptionImagePaths = new string[4];
    public string[] ResolvedOptionImagePaths = new string[4];

    public bool HasText => !string.IsNullOrWhiteSpace(Text);
    public bool HasImage => !string.IsNullOrWhiteSpace(RawImagePath);
    public bool HasOptionTexts => OptionTexts.Any(o => !string.IsNullOrWhiteSpace(o));
    public bool HasOptionImages => RawOptionImagePaths.Any(o => !string.IsNullOrWhiteSpace(o));
}

public static class QuestionLayout
{
    private static readonly Regex ImagePathRegex =
        new Regex(@"^\S+\.(png|jpe?g|webp|gif)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private const string ImageRoot = "res://Data/img/";

    public static QuestionView Build(QuestionData q)
    {
        var v = new QuestionView();
        if (q == null) return v;

        v.Text = (q.Question ?? "").Trim();
        v.RawImagePath = (q.QuestionImage ?? "").Trim();

        // データ不備対策: question に画像パスが入っている場合(例: id 1247)は画像として扱う
        if (v.RawImagePath == "" && ImagePathRegex.IsMatch(v.Text))
        {
            v.RawImagePath = v.Text;
            v.Text = "";
        }

        for (int i = 0; i < 4; i++)
        {
            v.OptionTexts[i] = (q.Options != null && i < q.Options.Length) ? (q.Options[i] ?? "").Trim() : "";
            v.RawOptionImagePaths[i] = (q.OptionsImages != null && i < q.OptionsImages.Length) ? (q.OptionsImages[i] ?? "").Trim() : "";
        }

        v.ResolvedImagePath = ResolveImagePath(v.RawImagePath);
        for (int i = 0; i < 4; i++)
            v.ResolvedOptionImagePaths[i] = ResolveImagePath(v.RawOptionImagePaths[i]);

        v.Type = Classify(v);
        return v;
    }

    private static QuestionLayoutType Classify(QuestionView v)
    {
        if (v.HasOptionImages) return QuestionLayoutType.OptionsAsImages;
        if (!v.HasOptionTexts && v.HasImage) return QuestionLayoutType.OptionsInImage;
        if (v.HasImage && v.HasText) return QuestionLayoutType.TextWithImage;
        if (v.HasImage) return QuestionLayoutType.ImageOnly;
        return QuestionLayoutType.TextOnly;
    }

    /// <summary>
    /// JSON のパス("../kakomon/img/202001S/25.png" など表記ゆれあり)を res://Data/img/... に解決する。
    /// 見つからなければ空文字を返す。
    /// </summary>
    public static string ResolveImagePath(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return "";

        string p = raw.Replace('\\', '/').Trim();
        string[] parts = p.Split('/', StringSplitOptions.RemoveEmptyEntries);

        // 候補1: 既に res:// 形式
        if (p.StartsWith("res://") && ResourceLoader.Exists(p)) return p;

        // 候補2: 末尾2要素(フォルダ名/ファイル名)を Data/img 配下に当てる
        //        "../kakomon/img/202306S/57.png" や "../kakomon/202306S/57.png"、
        //        "../kakommon/img/..."(typo) をまとめて吸収できる。
        //        ファイル名は 0埋めの有無("202001Q2.png" ⇔ "202001Q02.png")も試す
        if (parts.Length >= 2)
        {
            foreach (string name in FileNameVariants(parts[^1]))
            {
                string c = ImageRoot + parts[^2] + "/" + name;
                if (ResourceLoader.Exists(c)) return c;
            }
        }

        // 候補3: ファイル名だけ(Data/img 直下)
        foreach (string name in FileNameVariants(parts[^1]))
        {
            string c2 = ImageRoot + name;
            if (ResourceLoader.Exists(c2)) return c2;
        }

        GD.PushWarning($"[QuestionLayout] 画像が見つかりません: {raw}");
        return "";
    }

    // "202001Q2.png" -> ["202001Q2.png", "202001Q02.png"] / "202001Q02.png" -> ["202001Q02.png", "202001Q2.png"]
    private static readonly Regex QNumberRegex =
        new Regex(@"^(?<pre>.*Q)(?<num>\d+)(?<ext>\.[A-Za-z0-9]+)$", RegexOptions.Compiled);

    private static string[] FileNameVariants(string fileName)
    {
        var m = QNumberRegex.Match(fileName);
        if (!m.Success) return new[] { fileName };

        string pre = m.Groups["pre"].Value, num = m.Groups["num"].Value, ext = m.Groups["ext"].Value;
        string unpadded = pre + num.TrimStart('0').PadLeft(1, '0') + ext;
        string padded = pre + num.TrimStart('0').PadLeft(2, '0') + ext;
        return new[] { fileName, padded, unpadded };
    }
}
