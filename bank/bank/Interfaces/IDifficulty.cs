namespace GuessNumberGame.Interfaces;
public interface IDifficulty
{
    string Title { get; }
    GameConfig CreateConfig();
}