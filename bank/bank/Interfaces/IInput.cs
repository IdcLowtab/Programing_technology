namespace GuessNumberGame.Interfaces;

internal interface IInput
{
    int ReadNumber(string message, int min, int max);
}
