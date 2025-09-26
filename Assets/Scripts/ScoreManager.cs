using UnityEngine;

public class ScoreManager : MonoBehaviour
{
   public static ScoreManager Instance { get; private set; }

    public int CurrentScore { get; private set; }

    void Awake()
    {
        // Enforce singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: Keeps score across scenes
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;
    }

    public void ResetScore()
    {
        CurrentScore = 0;
    }
}
