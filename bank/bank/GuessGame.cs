using GuessNumberGame.Interfaces;

public class GuessGame
{
    private readonly INumberGenerator _generator;
    private readonly IInput _input;

    // События игры
    public event EventHandler<GameConfig>? OnGameStart;
    public event EventHandler<GuessEventArgs>? OnGuess;
    public event EventHandler<GameOverEventArgs>? OnGameOver;

    public GuessGame(INumberGenerator generator, IInput input)
    {
        _generator = generator;
        _input = input;
    }

    public void Start(GameConfig config)
    {
        int secretNumber = _generator.Next(config.Min, config.Max);
        int attempts = 0;

        // Уведомляем подписчиков о старте
        OnGameStart?.Invoke(this, config);

        while (true)
        {
            int guess = _input.ReadNumber($"Попытка #{attempts + 1}. Введи число: ", config.Min, config.Max);
            attempts++;

            if (guess == secretNumber)
            {
                OnGuess?.Invoke(this, new GuessEventArgs
                {
                    UserNumber = guess,
                    Hint = "Угадал",
                    Attempts = attempts
                });
                break;
            }

            string hint = guess < secretNumber ? "Загаданное число БОЛЬШЕ." : "Загаданное число МЕНЬШЕ.";
            OnGuess?.Invoke(this, new GuessEventArgs
            {
                UserNumber = guess,
                Hint = hint,
                Attempts = attempts
            });
        }

        // Уведомляем подписчиков о победе
        OnGameOver?.Invoke(this, new GameOverEventArgs
        {
            LevelName = config.LevelName,
            SecretNumber = secretNumber,
            TotalAttempts = attempts
        });
    }
}
