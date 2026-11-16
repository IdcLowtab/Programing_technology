Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== ИГРА: УГАДАЙ ЧИСЛО ===");

var random = new Random();
int secretNumber = random.Next(1, 101);
int attempts = 0;

Console.WriteLine("Я загадал число от 1 до 100. Попробуй угадать!");

while (true)
{
    Console.Write($"Попытка #{attempts + 1}. Введи число: ");
    string? input = Console.ReadLine();

    if (!int.TryParse(input, out int guess))
    {
        Console.WriteLine("Нужно ввести целое число!");
        continue;
    }

    attempts++;

    if (guess == secretNumber)
    {
        Console.WriteLine($"Поздравляю! Ты угадал число {secretNumber} за {attempts} попыток!");
        break;
    }

    if (guess < secretNumber)
    {
        Console.WriteLine("Загаданное число БОЛЬШЕ.");
    }
    else
    {
        Console.WriteLine("Загаданное число МЕНЬШЕ.");
    }
}
