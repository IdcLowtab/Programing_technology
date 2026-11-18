namespace GuessNumberGame.Interfaces;

// Данные для события при каждой попытке игрока
public class GuessEventArgs : EventArgs
{
    public int UserNumber { get; set; }
    public string Hint { get; set; } = "";
    public int Attempts { get; set; }
}

// Данные для события окончания игры
public class GameOverEventArgs : EventArgs
{
    public string LevelName { get; set; } = "";
    public int SecretNumber { get; set; }
    public int TotalAttempts { get; set; }
}
