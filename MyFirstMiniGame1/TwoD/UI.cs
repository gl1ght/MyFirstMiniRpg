using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Raylib_cs;

// Вспомогательные функции отрисовки: шрифт с кириллицей, кнопки, полосы, текст.
static class UI
{
    public static Font Font;

    public static Color Bg      = Rgb(25, 25, 35);
    public static Color Panel   = Rgb(35, 35, 50);
    public static Color PanelHi = Rgb(45, 45, 65);
    public static Color Border  = Rgb(90, 90, 120);
    public static Color BtnIdle = Rgb(55, 55, 80);
    public static Color BtnHover= Rgb(80, 80, 115);
    public static Color TextMain= Rgb(235, 235, 245);
    public static Color TextDim = Rgb(160, 160, 180);
    public static Color Hp      = Rgb(200, 70, 70);
    public static Color Food    = Rgb(220, 170, 60);
    public static Color Exp     = Rgb(90, 160, 220);

    public static Color Rgb(int r, int g, int b) => new Color((byte)r, (byte)g, (byte)b, (byte)255);

    public static void LoadFont()
    {
        var codepoints = new List<int>();
        for (int c = 32; c <= 126; c++) codepoints.Add(c);     // ASCII
        for (int c = 0x0400; c <= 0x04FF; c++) codepoints.Add(c); // кириллица
        codepoints.Add(0x2116); // №

        string[] candidates =
        {
            @"C:\Windows\Fonts\segoeui.ttf",
            @"C:\Windows\Fonts\arial.ttf",
            @"C:\Windows\Fonts\calibri.ttf",
            @"C:\Windows\Fonts\tahoma.ttf",
        };

        foreach (string path in candidates)
        {
            if (File.Exists(path))
            {
                int[] cps = codepoints.ToArray();
                Font = Raylib.LoadFontEx(path, 40, cps, cps.Length);
                Raylib.SetTextureFilter(Font.Texture, TextureFilter.Bilinear);
                return;
            }
        }

        Font = Raylib.GetFontDefault();
    }

    public static void Text(string text, float x, float y, int size, Color color)
        => Raylib.DrawTextEx(Font, text, new Vector2(x, y), size, 1f, color);

    public static Vector2 Measure(string text, int size)
        => Raylib.MeasureTextEx(Font, text, size, 1f);

    public static void TextCentered(string text, float centerX, float y, int size, Color color)
    {
        Vector2 s = Measure(text, size);
        Text(text, centerX - s.X / 2f, y, size, color);
    }

    public static bool Button(string label, Rectangle rect)
    {
        Vector2 mouse = Raylib.GetMousePosition();
        bool hover = Raylib.CheckCollisionPointRec(mouse, rect);

        Raylib.DrawRectangleRec(rect, hover ? BtnHover : BtnIdle);
        Raylib.DrawRectangleLinesEx(rect, 2, Border);

        Vector2 ts = Measure(label, 20);
        Text(label,
            rect.X + (rect.Width - ts.X) / 2f,
            rect.Y + (rect.Height - ts.Y) / 2f,
            20, TextMain);

        return hover && Raylib.IsMouseButtonPressed(MouseButton.Left);
    }

    public static void Bar(float x, float y, float w, float h, float ratio, Color fill, string label)
    {
        ratio = Math.Clamp(ratio, 0f, 1f);
        Raylib.DrawRectangleRec(new Rectangle(x, y, w, h), Rgb(30, 30, 42));
        Raylib.DrawRectangleRec(new Rectangle(x, y, w * ratio, h), fill);
        Raylib.DrawRectangleLinesEx(new Rectangle(x, y, w, h), 2, Border);
        Text(label, x + 8, y + (h - 18) / 2f, 18, TextMain);
    }

    public static void PanelBox(Rectangle rect)
    {
        Raylib.DrawRectangleRec(rect, Panel);
        Raylib.DrawRectangleLinesEx(rect, 2, Border);
    }
}
