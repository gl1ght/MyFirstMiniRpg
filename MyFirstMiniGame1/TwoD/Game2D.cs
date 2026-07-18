using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

// 2D-версия игры на Raylib. Держит игровое состояние и рисует экраны.
static class Game2D
{
    enum Screen { MainMenu, Playing, Combat, World, Inventory, Stats, LoadMenu, GameOver }

    const int W = 1000;
    const int H = 600;

    static Screen screen = Screen.MainMenu;
    static GameSession? session;
    static List<string> saveFiles = new List<string>();
    static bool quit;

    // мир (вид сверху)
    static WorldState? world;
    static Screen combatReturn = Screen.Playing;

    // эффекты боя
    static float playerFlash, enemyFlash, playerLunge, enemyLunge;
    static readonly Vector2 CombatPlayerPos = new Vector2(300, 250);
    static readonly Vector2 CombatEnemyPos = new Vector2(700, 250);
    const float CombatCreatureR = 55f;

    public static void Run()
    {
        Raylib.InitWindow(W, H, "MyFirstMiniRpg 2D");
        Raylib.SetTargetFPS(60);
        Raylib.SetExitKey(KeyboardKey.Null); // ESC не закриває гру — використовується для навігації
        UI.LoadFont();

        while (!Raylib.WindowShouldClose() && !quit)
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(UI.Bg);

            switch (screen)
            {
                case Screen.MainMenu: DrawMainMenu(); break;
                case Screen.Playing:  DrawPlaying();  break;
                case Screen.Combat:   DrawCombat();   break;
                case Screen.World:    DrawWorld();    break;
                case Screen.Inventory:DrawInventory();break;
                case Screen.Stats:    DrawStats();    break;
                case Screen.LoadMenu: DrawLoadMenu(); break;
                case Screen.GameOver: DrawGameOver(); break;
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    // ===================== Экраны =====================

    static void DrawMainMenu()
    {
        UI.TextCentered("Симулятор виживання", W / 2f, 110, 44, UI.TextMain);
        UI.TextCentered("2D видання", W / 2f, 165, 24, UI.TextDim);

        float bw = 300, bh = 56, x = W / 2f - bw / 2f;

        if (UI.Button("Нова гра", new Rectangle(x, 250, bw, bh)))
        {
            GameLog.Clear();
            session = new GameSession(Player.CreateNew());
            world = null;
            GameLog.Add("Нова гра почалася. Щасти!");
            screen = Screen.Playing;
        }

        if (UI.Button("Завантажити", new Rectangle(x, 320, bw, bh)))
        {
            saveFiles = SaveManager.ListSaves();
            screen = Screen.LoadMenu;
        }

        if (UI.Button("Вихід", new Rectangle(x, 390, bw, bh)))
            quit = true;
    }

    static void DrawPlaying()
    {
        if (session == null) { screen = Screen.MainMenu; return; }
        Player p = session.Player;

        // ---- Панель статуса игрока ----
        var statusRect = new Rectangle(20, 20, 440, 290);
        UI.PanelBox(statusRect);

        UI.Text($"День: {p.statDayAlive}", 40, 34, 24, UI.TextMain);
        DrawCreature(95, 120, 38, UI.Rgb(90, 150, 220), null);
        UI.Text("Герой", 60, 165, 18, UI.TextDim);

        UI.Text($"Рівень: {p.level}", 200, 70, 22, UI.TextMain);
        UI.Text($"Гроші: {p.money}", 200, 105, 22, UI.Food);
        UI.Text($"Шкода: {p.Damage}", 200, 140, 22, UI.TextMain);

        UI.Bar(40, 200, 400, 26, p.MaxHealth == 0 ? 0 : (float)p.Health / p.MaxHealth,
            UI.Hp, $"Здоров'я: {p.Health}/{p.MaxHealth}");
        UI.Bar(40, 234, 400, 26, p.Hunger / 100f, UI.Food, $"Їжа: {p.Hunger}");
        UI.Bar(40, 268, 400, 26, (float)p.Exp / (p.level * 100),
            UI.Exp, $"Досвід: {p.Exp}/{p.level * 100}");

        // ---- Кнопка дослідження світу ----
        if (UI.Button("Дослідити світ (ходити)", new Rectangle(20, 316, 440, 44)))
            screen = Screen.World;

        // ---- Кнопки дій ----
        float bx = 20, by = 368, bw = 215, bh = 44, gx = 10, gy = 7;

        if (UI.Button("Шукати їжу", new Rectangle(bx, by, bw, bh)))
            DoSearchFood();
        if (UI.Button("Працювати", new Rectangle(bx + bw + gx, by, bw, bh)))
        { session.Work(); CheckAlive(); }

        if (UI.Button("Спати", new Rectangle(bx, by + bh + gy, bw, bh)))
        { session.Sleep(); CheckAlive(); }
        if (UI.Button("Інвентар", new Rectangle(bx + bw + gx, by + bh + gy, bw, bh)))
            screen = Screen.Inventory;

        if (UI.Button("Статистика", new Rectangle(bx, by + 2 * (bh + gy), bw, bh)))
            screen = Screen.Stats;
        if (UI.Button("Зберегти", new Rectangle(bx + bw + gx, by + 2 * (bh + gy), bw, bh)))
            DoSave();

        if (UI.Button("Завантажити", new Rectangle(bx, by + 3 * (bh + gy), bw, bh)))
        { saveFiles = SaveManager.ListSaves(); screen = Screen.LoadMenu; }
        if (UI.Button("У меню", new Rectangle(bx + bw + gx, by + 3 * (bh + gy), bw, bh)))
            screen = Screen.MainMenu;

        // ---- Панель-лог ----
        DrawLogPanel(new Rectangle(480, 20, 500, 560));
    }

    static void DrawCombat()
    {
        if (session?.Combat == null) { screen = Screen.Playing; return; }
        CombatSession c = session.Combat;
        Player p = session.Player;
        Enemy e = c.Enemy;

        float dt = Raylib.GetFrameTime();
        playerFlash = MathF.Max(0, playerFlash - dt);
        enemyFlash = MathF.Max(0, enemyFlash - dt);
        playerLunge = MathF.Max(0, playerLunge - dt * 4f);
        enemyLunge = MathF.Max(0, enemyLunge - dt * 4f);
        Fx.Update(dt);

        Vector2 shake = Fx.ShakeOffset;
        UI.TextCentered($"БІЙ: {e.name} {e.level}-го рівня", W / 2f, 30, 30, UI.TextMain);

        // позиції з випадом назустріч одне одному
        Vector2 pPos = CombatPlayerPos + shake + new Vector2(playerLunge * 34f, 0);
        Vector2 ePos = CombatEnemyPos + shake + new Vector2(-enemyLunge * 34f, 0);

        DrawCreature(pPos.X, pPos.Y, CombatCreatureR, UI.Rgb(90, 150, 220), "Герой");
        if (playerFlash > 0) DrawFlash(pPos, CombatCreatureR, playerFlash / 0.28f);

        DrawCreature(ePos.X, ePos.Y, CombatCreatureR, EnemyColor(e.name), e.name);
        if (enemyFlash > 0) DrawFlash(ePos, CombatCreatureR, enemyFlash / 0.28f);

        UI.Bar(120 + shake.X, 360 + shake.Y, 320, 28, p.MaxHealth == 0 ? 0 : (float)p.Health / p.MaxHealth,
            UI.Hp, $"Ти: {p.Health}/{p.MaxHealth}");
        UI.Bar(560 + shake.X, 360 + shake.Y, 320, 28, e.MaxHealth == 0 ? 0 : (float)e.Health / e.MaxHealth,
            UI.Hp, $"{e.name}: {e.Health}/{e.MaxHealth}");

        Fx.Draw();

        // міні-лог
        DrawLogPanel(new Rectangle(120, 410, 760, 120));

        float bw = 240, bh = 54;
        if (!c.Finished)
        {
            if (UI.Button("Атакувати!", new Rectangle(W / 2f - bw - 10, 540, bw, bh)))
                DoAttack(c);
            if (UI.Button("Втекти", new Rectangle(W / 2f + 10, 540, bw, bh)))
                DoFlee();
        }
        else
        {
            if (UI.Button("Продовжити", new Rectangle(W / 2f - bw / 2f, 540, bw, bh)))
                ExitCombat();
        }
    }

    static void ExitCombat()
    {
        if (session == null) return;
        session.EndCombat();
        if (!session.PlayerAlive)
        {
            screen = Screen.GameOver;
        }
        else
        {
            if (combatReturn == Screen.World)
                world?.SetGrace(1.8f);
            screen = combatReturn;
        }
    }

    static void DoFlee()
    {
        if (session == null) return;
        session.FleeCombat();
        GameLog.Add("Ти втік з бою і вцілів!");
        if (combatReturn == Screen.World)
        {
            world?.SetGrace(1.8f);
            screen = Screen.World;
        }
        else
        {
            screen = Screen.Playing;
        }
    }

    static void DoAttack(CombatSession c)
    {
        CombatSession.RoundResult r = c.PlayerAttack();

        // враг ударил игрока
        enemyLunge = 1f;
        playerFlash = 0.28f;
        Fx.AddText($"-{r.EnemyDamage}", CombatPlayerPos + new Vector2(-14, -80), UI.Hp);
        Fx.Burst(CombatPlayerPos, UI.Hp, 12, 170);
        Fx.AddShake(7);

        // игрок ответил
        if (r.PlayerAttacked)
        {
            playerLunge = 1f;
            enemyFlash = 0.28f;
            Fx.AddText($"-{r.PlayerDamage}", CombatEnemyPos + new Vector2(-14, -80), UI.Food);
            Fx.Burst(CombatEnemyPos, UI.Food, 16, 230);
            Fx.AddShake(4);
        }
    }

    static void DrawWorld()
    {
        if (session == null) { screen = Screen.MainMenu; return; }
        if (world == null)
        {
            world = new WorldState(new Rectangle(20, 100, 960, 420));
            world.SetGrace(1.2f);
        }

        float dt = Raylib.GetFrameTime();

        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            screen = Screen.Playing;
            return;
        }

        WorldEnemy? hit = world.Update(dt);
        Fx.Update(dt);

        // земля
        Raylib.DrawRectangleRec(world.Area, UI.Rgb(30, 42, 33));
        Raylib.DrawRectangleLinesEx(world.Area, 2, UI.Border);

        // вороги
        foreach (WorldEnemy we in world.Enemies)
            DrawCreature(we.Pos.X, we.Pos.Y, we.Radius, we.Color, we.Enemy.name);

        // герой (блимає під час недоторканності)
        bool blink = world.Grace > 0 && ((int)(world.Grace * 10) % 2 == 0);
        if (!blink)
            DrawCreature(world.Player.X, world.Player.Y, world.PlayerRadius, UI.Rgb(90, 150, 220), null);

        Fx.Draw();

        // HUD
        Player p = session.Player;
        UI.Bar(20, 20, 280, 24, p.MaxHealth == 0 ? 0 : (float)p.Health / p.MaxHealth, UI.Hp, $"HP: {p.Health}/{p.MaxHealth}");
        UI.Bar(20, 52, 280, 24, p.Hunger / 100f, UI.Food, $"Їжа: {p.Hunger}");
        UI.Text("WASD / стрілки — рух", 320, 20, 20, UI.TextMain);
        UI.Text($"Рівень: {p.level}    Ворогів на карті: {world.Enemies.Count}", 320, 48, 18, UI.TextDim);

        if (UI.Button("Вийти зі світу", new Rectangle(W - 210, 20, 190, 44)))
        {
            screen = Screen.Playing;
            return;
        }

        if (hit != null)
        {
            hit.Enemy.GenerateLevel(p.level, new Random());
            combatReturn = Screen.World;
            session.StartCombat(hit.Enemy);
            StartCombatFx();
            screen = Screen.Combat;
        }
    }

    static void DrawInventory()
    {
        if (session == null) { screen = Screen.MainMenu; return; }
        Inventory inv = session.Player.Inventory;

        UI.TextCentered("Інвентар", W / 2f, 24, 32, UI.TextMain);

        var panel = new Rectangle(60, 80, 880, 440);
        UI.PanelBox(panel);

        if (inv.slots.Count == 0)
        {
            UI.Text("Інвентар порожній.", 90, 110, 22, UI.TextDim);
        }
        else
        {
            // копія, щоб безпечно змінювати список по кліку
            var slots = new List<InventorySlot>(inv.slots);
            for (int i = 0; i < slots.Count; i++)
            {
                InventorySlot slot = slots[i];
                float y = 100 + i * 52;
                if (y > 470) break;

                UI.Text($"{slot.Item.Name}  x{slot.Count}   ({slot.TotalWeight:0.#} кг)", 90, y + 8, 22, UI.TextMain);

                if (UI.Button("Використати", new Rectangle(560, y, 170, 40)))
                {
                    inv.UseItem(slot.Item, session.Player);
                    GameLog.Add($"Використано: {slot.Item.Name}");
                    break;
                }
                if (UI.Button("Викинути", new Rectangle(745, y, 150, 40)))
                {
                    inv.RemoveItem(slot, 1);
                    GameLog.Add($"Викинуто: {slot.Item.Name}");
                    break;
                }
            }
        }

        if (UI.Button("Назад", new Rectangle(60, 535, 200, 48)))
            screen = Screen.Playing;
    }

    static void DrawStats()
    {
        if (session == null) { screen = Screen.MainMenu; return; }
        Player p = session.Player;

        UI.TextCentered("Статистика", W / 2f, 24, 32, UI.TextMain);

        var panel = new Rectangle(60, 80, 880, 440);
        UI.PanelBox(panel);

        string[] lines =
        {
            $"Рівень: {p.level}",
            $"Днів прожито: {p.statDayAlive}",
            $"Днів відпрацьовано: {p.statDayWork}",
            $"Днів у пошуках їжі: {p.statDayEat}",
            $"Днів відпочинку: {p.statDaySleep}",
            $"Гроші: {p.money}",
        };
        for (int i = 0; i < lines.Length; i++)
            UI.Text(lines[i], 100, 120 + i * 46, 24, UI.TextMain);

        if (UI.Button("Назад", new Rectangle(60, 535, 200, 48)))
            screen = Screen.Playing;
    }

    static void DrawLoadMenu()
    {
        UI.TextCentered("Завантаження гри", W / 2f, 24, 32, UI.TextMain);

        var panel = new Rectangle(60, 80, 880, 440);
        UI.PanelBox(panel);

        if (saveFiles.Count == 0)
        {
            UI.Text("Немає збережень.", 90, 110, 22, UI.TextDim);
        }
        else
        {
            for (int i = 0; i < saveFiles.Count; i++)
            {
                float y = 100 + i * 54;
                if (y > 470) break;

                if (UI.Button(saveFiles[i], new Rectangle(90, y, 820, 44)))
                {
                    Player? loaded = SaveManager.Load(saveFiles[i]);
                    if (loaded != null)
                    {
                        session = new GameSession(loaded);
                        world = null;
                        GameLog.Add($"Загружено: {saveFiles[i]}");
                        screen = Screen.Playing;
                    }
                    else
                    {
                        GameLog.Add("Не вдалося завантажити файл.");
                    }
                }
            }
        }

        if (UI.Button("Назад", new Rectangle(60, 535, 200, 48)))
            screen = session == null ? Screen.MainMenu : Screen.Playing;
    }

    static void DrawGameOver()
    {
        UI.TextCentered("Ти програв", W / 2f, 200, 52, UI.Hp);
        if (session != null)
            UI.TextCentered($"Днів прожито: {session.Player.statDayAlive}", W / 2f, 280, 26, UI.TextDim);

        float bw = 300, bh = 56;
        if (UI.Button("У меню", new Rectangle(W / 2f - bw / 2f, 360, bw, bh)))
        {
            session = null;
            world = null;
            screen = Screen.MainMenu;
        }
    }

    // ===================== Действия / помощники =====================

    static void DoSearchFood()
    {
        if (session == null) return;
        session.SearchFood();
        if (session.Combat != null)
        {
            combatReturn = Screen.Playing;
            StartCombatFx();
            screen = Screen.Combat;
        }
        else
        {
            CheckAlive();
        }
    }

    static void StartCombatFx()
    {
        Fx.Reset();
        playerFlash = enemyFlash = playerLunge = enemyLunge = 0f;
        Fx.AddShake(10);
    }

    static void DoSave()
    {
        if (session == null) return;
        string name = $"Save-{DateTime.Now:yyyyMMdd-HHmmss}";
        bool ok = SaveManager.Save(session.Player, name);
        GameLog.Add(ok ? $"Гру збережено: {name}" : "Помилка збереження!");
    }

    static void CheckAlive()
    {
        if (session != null && !session.PlayerAlive)
            screen = Screen.GameOver;
    }

    static void DrawLogPanel(Rectangle rect)
    {
        UI.PanelBox(rect);
        UI.Text("Журнал:", rect.X + 12, rect.Y + 8, 18, UI.TextDim);


        var lines = GameLog.Lines;
        int lineHeight = 22;
        int top = (int)rect.Y + 34;
        int maxLines = (int)((rect.Height - 44) / lineHeight);

        int start = Math.Max(0, lines.Count - maxLines);
        for (int i = start; i < lines.Count; i++)
        {
            float y = top + (i - start) * lineHeight;
            UI.Text(lines[i], rect.X + 12, y, 18, UI.TextMain);
        }
    }

    static void DrawCreature(float cx, float cy, float radius, Color body, string? label)
    {
        var center = new Vector2(cx, cy);
        Raylib.DrawCircleV(center, radius, body);
        Raylib.DrawCircleV(center, radius, body);
        // глаза
        Raylib.DrawCircleV(new Vector2(cx - radius / 3f, cy - radius / 5f), radius / 6f, UI.Rgb(20, 20, 30));
        Raylib.DrawCircleV(new Vector2(cx + radius / 3f, cy - radius / 5f), radius / 6f, UI.Rgb(20, 20, 30));

        if (label != null)
            UI.TextCentered(label, cx, cy + radius + 8, 20, UI.TextMain);
    }

    static void DrawFlash(Vector2 center, float radius, float strength)
    {
        byte a = (byte)(180 * Math.Clamp(strength, 0f, 1f));
        Raylib.DrawCircleV(center, radius, new Color((byte)255, (byte)255, (byte)255, a));
    }

    public static Color EnemyColor(string name)
    {
        switch (name)
        {
            case "Вовк":             return UI.Rgb(130, 130, 140);
            case "Ведмідь":          return UI.Rgb(120, 80, 50);
            case "Слизень":          return UI.Rgb(90, 190, 120);
            case "Гоблін":           return UI.Rgb(110, 160, 90);
            case "Бандит":           return UI.Rgb(180, 120, 70);
            case "Гігантський павук":return UI.Rgb(70, 60, 90);
            default:                  return UI.Rgb(200, 90, 90);
        }
    }
}
