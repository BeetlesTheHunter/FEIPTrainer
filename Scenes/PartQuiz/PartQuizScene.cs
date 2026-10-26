using Godot;
using System;

/// <summary>
/// パート別クイズ画面(カテゴリ選択から開始する 10 問セット用。既存の QuizScene は変更していない)
///   右 1/3 : 上=GIF(正解/不正解演出。今は空)  下=A/B/C/D ボタン(回答後は同じ場所に「次へ」)
///   左 2/3 : 上=問題(文章/画像)  下=選択肢カード(テキスト or 画像)
/// 問題データの組み合わせは QuestionLayout が判定する。文字サイズは枠に合わせて自動調整(スクロールなし)。
/// </summary>
public partial class PartQuizScene : Control
{
    // ---- 右下: 回答ボタン(固定) ----
    [Export] private Button ButtonA;
    [Export] private Button ButtonB;
    [Export] private Button ButtonC;
    [Export] private Button ButtonD;
    [Export] private Control ButtonGrid;

    // ---- 左: 問題 / 選択肢 ----
    [Export] private Control LeftColumn;
    [Export] private Control QuestionZone;
    [Export] private Control QuestionBox;
    [Export] private RichTextLabel QuestionText;
    [Export] private TextureRect QuestionImage;
    [Export] private Label MissingImageLabel;
    [Export] private Control OptionsZone;
    [Export] private GridContainer OptionsGrid;

    // ---- 右上: 将来 GIF を置く場所(今は何も表示しない) ----
    [Export] private Control FeedbackSlot;

    private const int ScorePerCorrect = 10; // ScoreManager と同じ配点

    private GameManager gameManager;

    private QuestionData currentQuestion;
    private QuestionData[] questionArray;
    private int currentIndex = 0;
    private bool answered = false;
    private int correctCount = 0;
    private int wrongCount = 0;
    private Timer advanceTimer;
    private QuestionView currentView;
    private bool refitQueued = false;

    // 選択肢カード(A〜D)
    private readonly PanelContainer[] optionCards = new PanelContainer[4];
    private readonly RichTextLabel[] optionTexts = new RichTextLabel[4];
    private readonly TextureRect[] optionImages = new TextureRect[4];
    private static readonly string[] Letters = { "A", "B", "C", "D" };
    private StyleBoxFlat cardNormalBox, cardCorrectBox, cardWrongBox;

    // 文字サイズ自動調整の範囲(px)
    private const int QuestionMaxFont = 36;
    private const int OptionsMaxFont = 28;
    private const int MinFont = 12;
    private const int ComfortFont = 24;
    private const int OptionMeasureFont = 20;
    private const double AutoNextDelaySec = 1.2; // 回答してから次の問題へ自動で進むまでの時間

    public override void _Ready()
    {
        ButtonA.Pressed += () => HandleAnswer(0);
        ButtonB.Pressed += () => HandleAnswer(1);
        ButtonC.Pressed += () => HandleAnswer(2);
        ButtonD.Pressed += () => HandleAnswer(3);
        advanceTimer = new Timer { OneShot = true, WaitTime = AutoNextDelaySec };
        advanceTimer.Timeout += NextQuestion;
        AddChild(advanceTimer);

        // スクロールは使わず、文字サイズを自動調整して枠に収める
        QuestionText.ScrollActive = false;
        LeftColumn.Resized += QueueRefit;
        QuestionBox.Resized += QueueRefit;
        OptionsGrid.Resized += QueueRefit;

        BuildOptionCards();
        QuestionZone.Visible = false;
        OptionsZone.Visible = false;

        // Init() は _Ready の前に呼ばれる(既存画面と同じ流れ)ので、ここで1問目を表示する
        if (questionArray != null && questionArray.Length > 0)
        {
            currentQuestion = questionArray[currentIndex];
            UpdateQuestionUI();
        }
    }

    public void Init(GameManager gm, QuestionData[] array)
    {
        gameManager = gm;
        questionArray = array;
        currentIndex = 0;
        correctCount = 0;
        wrongCount = 0;
    }

    // ---------- 見た目(コードで作るカード) ----------

    private static StyleBoxFlat MakeBox(Color bg, Color border, int radius, int borderWidth, int marginX, int marginY)
    {
        var sb = new StyleBoxFlat { BgColor = bg, BorderColor = border };
        sb.SetBorderWidthAll(borderWidth);
        sb.SetCornerRadiusAll(radius);
        sb.ContentMarginLeft = marginX;
        sb.ContentMarginRight = marginX;
        sb.ContentMarginTop = marginY;
        sb.ContentMarginBottom = marginY;
        return sb;
    }

    // 選択肢カード A〜D: [文字バッジ][テキスト or 画像] を左下の GridContainer に並べる
    private void BuildOptionCards()
    {
        var textColor = new Color(0.05f, 0.05f, 0.1f);
        cardNormalBox = MakeBox(new Color(0.98f, 0.98f, 1f), new Color(0.25f, 0.3f, 0.5f), 12, 2, 10, 6);
        cardCorrectBox = MakeBox(new Color(0.80f, 0.95f, 0.82f), new Color(0.10f, 0.60f, 0.20f), 12, 4, 10, 6);
        cardWrongBox = MakeBox(new Color(0.99f, 0.82f, 0.82f), new Color(0.80f, 0.15f, 0.15f), 12, 4, 10, 6);
        for (int i = 0; i < 4; i++)
        {
            var card = new PanelContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            card.AddThemeStyleboxOverride("panel", cardNormalBox);

            var row = new HBoxContainer();
            row.AddThemeConstantOverride("separation", 12);
            card.AddChild(row);

            var badge = new PanelContainer
            {
                CustomMinimumSize = new Vector2(44, 44),
                SizeFlagsVertical = SizeFlags.ShrinkCenter
            };
            badge.AddThemeStyleboxOverride("panel",
                MakeBox(new Color(0.31f, 0.42f, 0.93f), new Color(0.31f, 0.42f, 0.93f), 10, 0, 0, 0));
            var letter = new Label
            {
                Text = Letters[i],
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            letter.AddThemeColorOverride("font_color", Colors.White);
            letter.AddThemeFontSizeOverride("font_size", 26);
            badge.AddChild(letter);
            row.AddChild(badge);

            var text = new RichTextLabel
            {
                BbcodeEnabled = false,
                ScrollActive = false,
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            text.AddThemeColorOverride("default_color", textColor);
            row.AddChild(text);

            var img = new TextureRect
            {
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill,
                Visible = false
            };
            row.AddChild(img);

            OptionsGrid.AddChild(card);
            optionCards[i] = card;
            optionTexts[i] = text;
            optionImages[i] = img;
        }
    }

    // ---------- 問題の表示 ----------

    private void UpdateQuestionUI()
    {
        if (currentQuestion == null) return;

        QuestionView v = QuestionLayout.Build(currentQuestion);
        currentView = v;
        ResetCardHighlight();

        // 問題(テキスト → 画像 の順に縦に並べる)
        QuestionZone.Visible = v.HasText || v.HasImage;

        QuestionText.Visible = v.HasText;
        QuestionText.Text = v.Text;

        bool imageLoaded = v.HasImage && v.ResolvedImagePath != "";
        QuestionImage.Visible = imageLoaded;
        QuestionImage.Texture = imageLoaded ? ResourceLoader.Load<Texture2D>(v.ResolvedImagePath) : null;

        MissingImageLabel.Visible = v.HasImage && !imageLoaded;
        MissingImageLabel.Text = $"(画像が見つかりません: {v.RawImagePath})";

        // 選択肢(問題画像に選択肢が含まれる場合は非表示)
        bool useOptionImages = v.HasOptionImages;
        bool useOptionTexts = v.HasOptionTexts && !useOptionImages;
        OptionsZone.Visible = useOptionTexts || useOptionImages;

        // 選択肢が短ければ 2x2、長ければ縦 4 行
        OptionsGrid.Columns = (useOptionImages || AreOptionsShort(v.OptionTexts)) ? 2 : 1;

        for (int i = 0; i < 4; i++)
        {
            optionTexts[i].Visible = useOptionTexts;
            optionImages[i].Visible = useOptionImages;

            if (useOptionTexts)
                optionTexts[i].Text = v.OptionTexts[i];

            if (useOptionImages)
            {
                string path = v.ResolvedOptionImagePaths[i];
                optionImages[i].Texture = path != "" ? ResourceLoader.Load<Texture2D>(path) : null;
            }
        }

        GD.Print($"[QuizScene] id={currentQuestion.Id} layout={v.Type}");
        QueueRefit();
    }

    // 全角=1、半角=0.5 として数え、短い選択肢だけなら 2 列にする
    private static bool AreOptionsShort(string[] options)
    {
        foreach (string o in options)
        {
            float w = 0;
            foreach (char c in o ?? "") w += c < 0x80 ? 0.5f : 1f;
            if (w > 12f) return false;
        }
        return true;
    }

    // ---------- 文字サイズ・領域配分の自動調整(スクロールなしで枠に収める) ----------

    // レイアウト確定を1フレーム待ってから調整する(コンテナのサイズが決まってから測るため)
    private async void QueueRefit()
    {
        if (refitQueued) return;
        refitQueued = true;
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        refitQueued = false;
        if (!IsInstanceValid(this) || !IsInsideTree()) return;
        Refit();
    }

    private void Refit()
    {
        if (currentView == null) return;
        QuestionView v = currentView;
        bool showQ = QuestionZone.Visible;
        bool showO = OptionsZone.Visible;

        // 1) 問題ゾーンと選択肢ゾーンの高さ配分
        if (showQ && showO)
        {
            float share;
            if (v.HasOptionImages) share = v.HasImage ? 0.5f : 0.35f;
            else if (v.HasImage) share = 0.62f;
            else
            {
                // 文章のみ: 文章と選択肢それぞれに必要な高さの比率で配分する
                if (QuestionText.Size.X < 10) return;
                SetFont(QuestionText, ComfortFont);
                float hq = QuestionText.GetContentHeight() + 36;
                float ho = MeasureOptionsNeed();
                share = Mathf.Clamp(hq / (hq + ho), 0.28f, 0.65f);
            }
            QuestionZone.SizeFlagsStretchRatio = share;
            OptionsZone.SizeFlagsStretchRatio = 1f - share;
        }

        // 2) 問題文のフォント
        if (showQ && v.HasText && QuestionText.Size.X > 10)
        {
            float boxH = QuestionBox.Size.Y;
            if (v.HasImage && QuestionImage.Visible)
            {
                // 画像と同居: 文章に必要な高さ(上限は枠の半分)だけ確保し、残りを画像に回す
                SetFont(QuestionText, ComfortFont);
                float needed = QuestionText.GetContentHeight() + 6;
                float h = Mathf.Clamp(needed, boxH * 0.15f, boxH * 0.5f);
                QuestionText.SizeFlagsVertical = SizeFlags.ShrinkBegin;
                QuestionText.CustomMinimumSize = new Vector2(0, h);
                FitFontSize(QuestionText, h - 4, QuestionMaxFont);
            }
            else
            {
                QuestionText.SizeFlagsVertical = SizeFlags.ExpandFill;
                QuestionText.CustomMinimumSize = Vector2.Zero;
                FitFontSize(QuestionText, boxH - 4, QuestionMaxFont);
            }
        }

        // 3) 選択肢のフォント(4つとも同じサイズに揃える)
        if (showO && !v.HasOptionImages)
        {
            int common = OptionsMaxFont;
            bool measured = false;
            for (int i = 0; i < 4; i++)
            {
                RichTextLabel l = optionTexts[i];
                if (l.Size.X < 10 || l.Size.Y < 10) continue;
                common = Math.Min(common, FitFontSize(l, l.Size.Y - 2, OptionsMaxFont));
                measured = true;
            }
            if (measured)
                for (int i = 0; i < 4; i++) SetFont(optionTexts[i], common);
        }
    }

    // 選択肢カード1行ぶんに必要な高さ × 行数
    private float MeasureOptionsNeed()
    {
        float rowNeed = 0;
        for (int i = 0; i < 4; i++)
        {
            SetFont(optionTexts[i], OptionMeasureFont);
            rowNeed = Mathf.Max(rowNeed, optionTexts[i].GetContentHeight());
        }
        rowNeed += 20; // カードの余白
        int rows = OptionsGrid.Columns == 2 ? 2 : 4;
        return rows * rowNeed + (rows - 1) * 12 + 4;
    }

    private static void SetFont(RichTextLabel label, int size)
        => label.AddThemeFontSizeOverride("normal_font_size", size);

    // 内容の高さが availH 以下になる最大のフォントサイズを二分探索で求め、適用して返す
    private static int FitFontSize(RichTextLabel label, float availH, int maxSize)
    {
        int lo = MinFont, hi = maxSize, best = MinFont;
        while (lo <= hi)
        {
            int mid = (lo + hi) / 2;
            SetFont(label, mid);
            if (label.GetContentHeight() <= availH) { best = mid; lo = mid + 1; }
            else hi = mid - 1;
        }
        SetFont(label, best);
        return best;
    }

    // ---------- 回答の流れ ----------

    private void SetButtonsDisabled(bool value)
    {
        ButtonA.Disabled = value;
        ButtonB.Disabled = value;
        ButtonC.Disabled = value;
        ButtonD.Disabled = value;
    }

    private void SetAnsweredState(bool value)
    {
        answered = value;
    }

    private void ResetCardHighlight()
    {
        for (int i = 0; i < 4; i++)
            optionCards[i]?.AddThemeStyleboxOverride("panel", cardNormalBox);
    }

    private void NextQuestion()
    {
        if (questionArray == null) return;
        currentIndex++;

        if (currentIndex < questionArray.Length)
        {
            currentQuestion = questionArray[currentIndex];
            SetAnsweredState(false);
            UpdateQuestionUI();
        }
        else
        {
            GD.Print("ステージクリア");
            ShowResults();
        }
    }

    private void ShowResults()
    {
        SetButtonsDisabled(true);
        int total = correctCount + wrongCount;
        int rate = total > 0 ? Mathf.RoundToInt(correctCount * 100f / total) : 0;
        gameManager.ResultScreen(correctCount * ScorePerCorrect, rate);
    }

    private void HandleAnswer(int selectedIndex)
    {
        if (currentQuestion == null || answered) return;

        int correctIndex = currentQuestion.AnswerIndex;
        if (correctIndex == selectedIndex)
        {
            GD.Print("正解");
            correctCount++;
        }
        else
        {
            GD.Print("不正解");
            wrongCount++;
        }

        // 正解カードを緑、間違えて選んだカードを赤にして、少し待ってから自動で次へ。
        // 正解/不正解の演出(GIF)は右上の FeedbackSlot に後で追加する。
        if (correctIndex >= 0 && correctIndex < 4)
            optionCards[correctIndex]?.AddThemeStyleboxOverride("panel", cardCorrectBox);
        if (selectedIndex != correctIndex)
            optionCards[selectedIndex]?.AddThemeStyleboxOverride("panel", cardWrongBox);

        SetAnsweredState(true);
        advanceTimer.Start();
    }
}
