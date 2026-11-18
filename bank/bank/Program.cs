using GuessNumberGame.Interfaces;
using GuessNumberGame;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var console = new ConsoleService();
var generator = new DefaultRandomGenerator();

// Создаем игру и подключаем подписчика на события
var game = new GuessGame(generator, console);
var watcher = new ConsoleGameWatcher(console);
watcher.Attach(game);

var levels = new List<IDifficulty>
{
    new EasyLevel(),
    new NormalLevel(),
    new HardLevel(),
    new CustomLevel(console)
};

console.Print("=== ИГРА: УГАДАЙ ЧИСЛО ===");
console.Print("Выбери уровень сложности:");

for (int i = 0; i < levels.Count; i++)
{
    console.Print($"{i + 1}. {levels[i].Title}");
}

int choice = console.ReadNumber("Твой выбор: ", 1, levels.Count) - 1;
GameConfig config = levels[choice].CreateConfig();

game.Start(config);
