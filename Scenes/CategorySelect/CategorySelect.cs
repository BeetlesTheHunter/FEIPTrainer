using Godot;
using System;

/// <summary>
/// カテゴリ選択 → パート選択(1パート=10問) → QuizScene 開始。
/// UI はコードで組み立てている(デザインはあとでチームが差し替えてよい)。
/// </summary>
public partial class CategorySelect : Control
{
    private Label titleLabel;
    private Button backButton;
    private Control content;          // ScrollContainer の中身を入れ替える
    private string selectedCategory;  // null ならカテゴリ一覧を表示中

    private GameManager gameManager;
    private QuestionDataManager questionDataManager;

    // GameManager.CategorySelectScreen() から呼ばれる(既存の画面と同じ Init パターン)
    public void Init(GameManager gm, QuestionDataManager qdm)
    {
        gameManager = gm;
        questionDataManager = qdm;
    }

    public override void _Ready()
    {
        var bg = new ColorRect { Color = new Color(0.12f, 0.13f, 0.18f) };
        bg.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(bg);

        var margin = new MarginContainer();
        margin.SetAnchorsPreset(LayoutPreset.FullRect);
        foreach (var side in new[] { "left", "top", "right", "bottom" })
            margin.AddThemeConstantOverride($"margin_{side}", 32);
        AddChild(margin);

        var root = new VBoxContainer();
        root.AddThemeConstantOverride("separation", 16);
        margin.AddChild(root);

        var header = new HBoxContainer();
        header.AddThemeConstantOverride("separation", 16);
        root.AddChild(header);

        backButton = new Button { Text = "← 戻る", CustomMinimumSize = new Vector2(140, 48) };
        backButton.Pressed += OnBackPressed;
        header.AddChild(backButton);

        titleLabel = new Label();
        titleLabel.AddThemeFontSizeOverride("font_size", 32);
        header.AddChild(titleLabel);

        var scroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
        scroll.HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled;
        root.AddChild(scroll);

        content = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
        content.AddThemeConstantOverride("separation", 12);
        scroll.AddChild(content);

        ShowCategories();
    }

    private void ClearContent()
    {
        foreach (Node child in content.GetChildren())
            child.QueueFree();
    }

    // ---------- カテゴリ一覧 ----------
    private void ShowCategories()
    {
        selectedCategory = null;
        titleLabel.Text = "カテゴリを選択";
        ClearContent();

        var mgr = questionDataManager;
        if (mgr == null)
        {
            content.AddChild(new Label { Text = "問題データを読み込めませんでした" });
            return;
        }

        foreach (var (category, count) in mgr.GetCategories())
        {
            int parts = mgr.GetPartCount(category);
            var btn = new Button
            {
                Text = $"{category}   ({count}問 / {parts}パート)",
                CustomMinimumSize = new Vector2(0, 64),
                Alignment = HorizontalAlignment.Left
            };
            btn.AddThemeFontSizeOverride("font_size", 24);
            string c = category;
            btn.Pressed += () => ShowParts(c);
            content.AddChild(btn);
        }
    }

    // ---------- パート一覧 ----------
    private void ShowParts(string category)
    {
        selectedCategory = category;
        titleLabel.Text = category;
        ClearContent();

        var mgr = questionDataManager;
        int partCount = mgr.GetPartCount(category);

        var grid = new GridContainer { Columns = 4, SizeFlagsHorizontal = SizeFlags.ExpandFill };
        grid.AddThemeConstantOverride("h_separation", 12);
        grid.AddThemeConstantOverride("v_separation", 12);
        content.AddChild(grid);

        for (int i = 0; i < partCount; i++)
        {
            int n = mgr.GetPart(category, i).Length;
            var btn = new Button
            {
                Text = $"パート{i + 1}\n({n}問)",
                CustomMinimumSize = new Vector2(0, 80),
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            btn.AddThemeFontSizeOverride("font_size", 22);
            int partIndex = i;
            btn.Pressed += () => StartPart(category, partIndex);
            grid.AddChild(btn);
        }
    }

    private void OnBackPressed()
    {
        if (selectedCategory != null) ShowCategories();
        else gameManager.DashBoardScreen(); // ダッシュボードへ戻る
    }

    // ---------- クイズ開始 ----------
    private void StartPart(string category, int partIndex)
    {
        QuestionData[] questions = questionDataManager.GetPart(category, partIndex);
        if (questions.Length == 0) return;

        gameManager.PartQuizScreen(questions);
    }
}
