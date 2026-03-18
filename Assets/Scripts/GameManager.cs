using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Configuración del juego")]
    public int startLives = 3;
    public int maxLevels = 5;

    [Header("Estado actual")]
    public int currentLevel = 1;
    public int lives;
    public bool gameEnded = false;

    [Header("UI")]
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public GameObject winPanel;
    public GameObject losePanel;

    private Button buttonNextLevel;
    private Button buttonRestart;
    private Button buttonRestartLose;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            lives = startLives;

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;

        RelinkUIReferences();
        RelinkButtons();

        ShowHUD();
        HidePanels();
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        gameEnded = false;

        RelinkUIReferences();
        RelinkButtons();

        ShowHUD();
        HidePanels();
        UpdateUI();

        Debug.Log("Escena recargada. Nivel actual: " + currentLevel);

        if (MazeGenerator.Instance != null)
        {
            MazeGenerator.Instance.ApplyDifficulty(currentLevel);
        }
    }

    private void RelinkUIReferences()
    {
        livesText = FindTMPInScene("LivesText");
        levelText = FindTMPInScene("LevelText");
        scoreText = FindTMPInScene("ScoreText");

        winPanel = FindInactiveObjectInScene("WinPanel");
        losePanel = FindInactiveObjectInScene("LosePanel");
    }

    private void RelinkButtons()
    {
        buttonNextLevel = FindButtonInScene("ButtonNextLevel");
        buttonRestart = FindButtonInScene("ButtonRestart");
        buttonRestartLose = FindButtonInScene("ButtonRestartLose");

        if (buttonNextLevel != null)
        {
            buttonNextLevel.onClick.RemoveAllListeners();
            buttonNextLevel.onClick.AddListener(NextLevel);
        }

        if (buttonRestart != null)
        {
            buttonRestart.onClick.RemoveAllListeners();
            buttonRestart.onClick.AddListener(RestartGame);
        }

        if (buttonRestartLose != null)
        {
            buttonRestartLose.onClick.RemoveAllListeners();
            buttonRestartLose.onClick.AddListener(RestartGame);
        }
    }

    private TextMeshProUGUI FindTMPInScene(string objectName)
    {
        TextMeshProUGUI[] texts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();

        foreach (TextMeshProUGUI txt in texts)
        {
            if (txt.gameObject.name == objectName && txt.gameObject.scene.IsValid())
            {
                return txt;
            }
        }

        return null;
    }

    private GameObject FindInactiveObjectInScene(string objectName)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject go in allObjects)
        {
            if (go.name == objectName && go.scene.IsValid())
            {
                return go;
            }
        }

        return null;
    }

    private Button FindButtonInScene(string objectName)
    {
        Button[] buttons = Resources.FindObjectsOfTypeAll<Button>();

        foreach (Button btn in buttons)
        {
            if (btn.gameObject.name == objectName && btn.gameObject.scene.IsValid())
            {
                return btn;
            }
        }

        return null;
    }

    private void HidePanels()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    private void ShowHUD()
    {
        if (livesText != null) livesText.gameObject.SetActive(true);
        if (levelText != null) levelText.gameObject.SetActive(true);
        if (scoreText != null) scoreText.gameObject.SetActive(true);
    }

    private void HideHUD()
    {
        if (livesText != null) livesText.gameObject.SetActive(false);
        if (levelText != null) levelText.gameObject.SetActive(false);
        if (scoreText != null) scoreText.gameObject.SetActive(false);
    }

    public void LoseLife()
    {
        if (gameEnded) return;

        lives--;
        UpdateUI();

        Debug.Log("Vida perdida. Vidas restantes: " + lives);

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            RestartLevel();
        }
    }

    public void WinLevel()
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("Nivel superado: " + currentLevel);

        HideHUD();

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("WinPanel no encontrado");
        }

        Time.timeScale = 0f;
    }

    public void NextLevel()
    {
        if (!gameEnded)
            return;

        if (currentLevel < maxLevels)
        {
            currentLevel++;
            Debug.Log("Pasando al nivel: " + currentLevel);

            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.Log("JUEGO COMPLETADO. Último nivel alcanzado: " + currentLevel);

            Time.timeScale = 0f;

            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }
        }
    }

    public void GameOver()
    {
        gameEnded = true;
        Debug.Log("GAME OVER");

        HideHUD();

        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("LosePanel no encontrado");
        }

        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Debug.Log("Reiniciando nivel actual");

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartGame()
    {
        Debug.Log("Reiniciando juego completo");

        currentLevel = 1;
        lives = startLives;
        gameEnded = false;

        if (ScoreSystem.Instance != null)
        {
            ScoreSystem.Instance.ResetScore();
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateUI()
    {
        if (livesText != null)
            livesText.text = "Vidas: " + lives;

        if (levelText != null)
            levelText.text = "Nivel: " + currentLevel;

        if (scoreText != null)
        {
            int currentScore = 0;

            if (ScoreSystem.Instance != null)
                currentScore = ScoreSystem.Instance.score;

            scoreText.text = "Puntos: " + currentScore;
        }
    }
}