using System.Collections.Generic;

// Простой журнал сообщений: заменяет Console.WriteLine в игровой логике.
// UI (Game2D) читает эти строки и рисует их в панели-логе.
static class GameLog
{
    static readonly List<string> _lines = new List<string>();

    public static IReadOnlyList<string> Lines => _lines;

    public static void Add(string message)
    {
        if (message == null)
            return;

        foreach (string part in message.Split('\n'))
            _lines.Add(part.TrimEnd('\r'));

        // не даём логу расти бесконечно
        while (_lines.Count > 300)
            _lines.RemoveAt(0);
    }

    public static void Clear() => _lines.Clear();
}
