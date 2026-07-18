using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

// Один враг на карте мира (вид сверху).
class WorldEnemy
{
    public Enemy Enemy;
    public Vector2 Pos;
    public Color Color;
    public float Radius = 22;
    public float WanderTimer;
    public Vector2 WanderDir;

    // случайное поведение: агрессивность, радиус аггро и скорость
    public bool Aggressive;
    public float AggroRange;
    public float ChaseSpeed;

    public WorldEnemy(Enemy enemy, Vector2 pos, Color color)
    {
        Enemy = enemy;
        Pos = pos;
        Color = color;
    }
}

// Состояние мира: игрок ходит (вид сверху), враги блуждают и преследуют.
class WorldState
{
    public Vector2 Player;
    public float PlayerRadius = 20;
    public float Speed = 230f;
    public readonly List<WorldEnemy> Enemies = new List<WorldEnemy>();
    public Rectangle Area;

    private readonly Random rng = new Random();
    private float spawnTimer;
    private float dustTimer;

    // недоторканність після бою/втечі: вороги не переслідують і не чіпляються
    public float Grace { get; private set; }
    public void SetGrace(float seconds) => Grace = seconds;

    private static readonly Func<Enemy>[] Factories =
    {
        () => new Wolf(),
        () => new Bear(),
        () => new Bandit(),
        () => new Goblin(),
        () => new Slime(),
        () => new GiantSpider(),
    };

    public WorldState(Rectangle area)
    {
        Area = area;
        Player = new Vector2(area.X + area.Width / 2f, area.Y + area.Height / 2f);
        for (int i = 0; i < 4; i++)
            SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        Enemy enemy = Factories[rng.Next(Factories.Length)]();
        Vector2 pos;
        int tries = 0;
        do
        {
            pos = new Vector2(
                Area.X + 30 + (float)rng.NextDouble() * (Area.Width - 60),
                Area.Y + 30 + (float)rng.NextDouble() * (Area.Height - 60));
            tries++;
        }
        while (Vector2.Distance(pos, Player) < 170 && tries < 20);

        var we = new WorldEnemy(enemy, pos, Game2D.EnemyColor(enemy.name))
        {
            Aggressive = rng.NextDouble() < 0.5,               // лише частина ворогів агресивні
            AggroRange = 110f + (float)rng.NextDouble() * 130f, // випадковий радіус аггро (110..240)
            ChaseSpeed = 70f + (float)rng.NextDouble() * 45f,   // випадкова швидкість (70..115)
        };
        Enemies.Add(we);
    }

    // Повертає ворога, якщо гравець зіткнувся з ним (ворог зникає з карти), інакше null.
    public WorldEnemy? Update(float dt)
    {
        if (Grace > 0)
            Grace = Math.Max(0, Grace - dt);

        // рух гравця
        Vector2 move = Vector2.Zero;
        if (Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.Up)) move.Y -= 1;
        if (Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Down)) move.Y += 1;
        if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left)) move.X -= 1;
        if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right)) move.X += 1;

        if (move != Vector2.Zero)
        {
            move = Vector2.Normalize(move);
            Player += move * Speed * dt;
            dustTimer -= dt;
            if (dustTimer <= 0)
            {
                dustTimer = 0.05f;
                Fx.Burst(new Vector2(Player.X, Player.Y + PlayerRadius * 0.6f), UI.Rgb(95, 85, 65), 2, 60f);
            }
        }

        Player.X = Math.Clamp(Player.X, Area.X + PlayerRadius, Area.X + Area.Width - PlayerRadius);
        Player.Y = Math.Clamp(Player.Y, Area.Y + PlayerRadius, Area.Y + Area.Height - PlayerRadius);

        foreach (WorldEnemy e in Enemies)
        {
            float dist = Vector2.Distance(e.Pos, Player);

            // переслідують лише агресивні вороги в межах свого радіуса аггро і не під час недоторканності
            if (Grace <= 0 && e.Aggressive && dist < e.AggroRange)
            {
                Vector2 dir = Vector2.Normalize(Player - e.Pos);
                e.Pos += dir * e.ChaseSpeed * dt;
            }
            else
            {
                // блукання
                e.WanderTimer -= dt;
                if (e.WanderTimer <= 0)
                {
                    e.WanderTimer = 1f + (float)rng.NextDouble() * 2.5f;
                    double a = rng.NextDouble() * Math.PI * 2;
                    e.WanderDir = new Vector2((float)Math.Cos(a), (float)Math.Sin(a));
                }
                e.Pos += e.WanderDir * 45f * dt;
            }

            e.Pos.X = Math.Clamp(e.Pos.X, Area.X + e.Radius, Area.X + Area.Width - e.Radius);
            e.Pos.Y = Math.Clamp(e.Pos.Y, Area.Y + e.Radius, Area.Y + Area.Height - e.Radius);

            // зіткнення викликає бій лише поза недоторканністю
            if (Grace <= 0 && dist < e.Radius + PlayerRadius)
            {
                Enemies.Remove(e);
                return e;
            }
        }

        spawnTimer -= dt;
        if (spawnTimer <= 0 && Enemies.Count < 5)
        {
            spawnTimer = 6f;
            SpawnEnemy();
        }

        return null;
    }
}
