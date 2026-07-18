using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

// Простые визуальные эффекты: всплывающий текст (урон), частицы, тряска экрана.
static class Fx
{
    class FloatText
    {
        public string Text = "";
        public Vector2 Pos;
        public Vector2 Vel;
        public float Life;
        public float Max;
        public int Size;
        public Color Color;
    }

    class Particle
    {
        public Vector2 Pos;
        public Vector2 Vel;
        public float Life;
        public float Max;
        public float Size;
        public Color Color;
    }

    static readonly List<FloatText> texts = new List<FloatText>();
    static readonly List<Particle> parts = new List<Particle>();
    static readonly Random rng = new Random();
    static float shake;

    public static Vector2 ShakeOffset { get; private set; } = Vector2.Zero;

    public static void Reset()
    {
        texts.Clear();
        parts.Clear();
        shake = 0;
        ShakeOffset = Vector2.Zero;
    }

    public static void AddText(string text, Vector2 pos, Color color, int size = 28)
        => texts.Add(new FloatText { Text = text, Pos = pos, Vel = new Vector2(0, -55), Life = 1.1f, Max = 1.1f, Size = size, Color = color });

    public static void AddShake(float amount) => shake = MathF.Max(shake, amount);

    public static void Burst(Vector2 pos, Color color, int count = 16, float speed = 200f)
    {
        for (int i = 0; i < count; i++)
        {
            double ang = rng.NextDouble() * Math.PI * 2;
            float sp = speed * (0.3f + (float)rng.NextDouble());
            parts.Add(new Particle
            {
                Pos = pos,
                Vel = new Vector2((float)Math.Cos(ang) * sp, (float)Math.Sin(ang) * sp),
                Life = 0.55f,
                Max = 0.55f,
                Size = 2 + (float)rng.NextDouble() * 4,
                Color = color
            });
        }
    }

    public static void Update(float dt)
    {
        for (int i = texts.Count - 1; i >= 0; i--)
        {
            FloatText f = texts[i];
            f.Pos += f.Vel * dt;
            f.Life -= dt;
            if (f.Life <= 0) texts.RemoveAt(i);
        }

        for (int i = parts.Count - 1; i >= 0; i--)
        {
            Particle p = parts[i];
            p.Pos += p.Vel * dt;
            p.Vel *= 0.90f;
            p.Life -= dt;
            if (p.Life <= 0) parts.RemoveAt(i);
        }

        if (shake > 0.05f)
        {
            shake = MathF.Max(0, shake - dt * 45f);
            ShakeOffset = new Vector2(
                (float)(rng.NextDouble() * 2 - 1) * shake,
                (float)(rng.NextDouble() * 2 - 1) * shake);
        }
        else
        {
            ShakeOffset = Vector2.Zero;
        }
    }

    public static void Draw()
    {
        foreach (Particle p in parts)
        {
            byte a = (byte)(255 * Math.Clamp(p.Life / p.Max, 0f, 1f));
            Raylib.DrawCircleV(p.Pos, p.Size, new Color(p.Color.R, p.Color.G, p.Color.B, a));
        }

        foreach (FloatText f in texts)
        {
            float k = Math.Clamp(f.Life / f.Max, 0f, 1f);
            byte a = (byte)(255 * k);
            UI.Text(f.Text, f.Pos.X, f.Pos.Y, f.Size, new Color(f.Color.R, f.Color.G, f.Color.B, a));
        }
    }
}
