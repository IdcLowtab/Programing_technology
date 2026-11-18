namespace GuessNumberGame.Interfaces;

public interface IInput
{
    int ReadNumber(string message, int min, int max);
}
