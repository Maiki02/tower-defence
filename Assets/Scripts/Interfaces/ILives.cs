public interface ILives
{
    int MaxLives { get; }
    int CurrentLives { get; }

    void LoseLife();
}