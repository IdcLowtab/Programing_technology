using GuessNumberGame.Interfaces;

public class ConsoleGameWatcher
{
    private readonly IOutput _output;

    public ConsoleGameWatcher(IOutput output)
    {
        _output = output;
    }

    public void Attach(GuessGame game)
    {
        game.OnGameStart += (sender, cfg) =>
        {
            _output.Print($"\nУровень: {cfg.LevelName}. Я загадал число от {cfg.Min} до {cfg.Max}. Поехали!");
        };

        game.OnGuess += (sender, args) =>
        {
            if (args.Hint != "Угадал")
            {
                _output.Print(args.Hint);
            }
        };

        game.OnGameOver += (sender, args) =>
        {
            _output.Print($"Поздравляю! Ты угадал число {args.SecretNumber} за {args.TotalAttempts} попыток!\n");
        };
    }
}