using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance;

    [Header("Puntuación")]
    public int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Añadir puntos
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score actualizado: " + score);

        UpdateUI();
    }

    // Resetear puntos (nuevo juego)
    public void ResetScore()
    {
        score = 0;
        Debug.Log("Score reiniciado");

        UpdateUI();
    }

    // Método centralizado para actualizar UI
    private void UpdateUI()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateUI();
        }
        else
        {
            Debug.LogWarning("GameManager.Instance es null al actualizar score");
        }
    }
}