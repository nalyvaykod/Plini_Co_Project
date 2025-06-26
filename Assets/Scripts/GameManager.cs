using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Progression")]
    [SerializeField] private int baseCoinsToWin = 10;
    [SerializeField] private int coinsIncreasePerLevel = 2;
    private int coinsToWin;
    private int currentCoins = 0;

    private int currentLevel = 1;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI coinCountText; 
    [SerializeField] private TextMeshProUGUI levelText; 


    private bool gameEnded = false;

    private const string CurrentLevelKey = "CurrentLevel";


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        currentLevel = PlayerPrefs.GetInt(CurrentLevelKey, 1);

        coinsToWin = baseCoinsToWin + (currentLevel - 1) * coinsIncreasePerLevel;

        currentCoins = 0;
        UpdateCoinUI();
        UpdateLevelUI();

        // Переконайтеся, що панелі приховані на старті, якщо ви їх використовуєте
        // if (winPanel != null) winPanel.SetActive(false);
        // if (losePanel != null) losePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (gameEnded) return;
    }

    public void CollectCoin()
    {
        if (gameEnded) return;

        currentCoins++;
        Debug.Log("Coin collected! Total: " + currentCoins);
        UpdateCoinUI();

        if (currentCoins >= coinsToWin)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        if (gameEnded) return;
        gameEnded = true;
        Debug.Log("You Win!");
        Time.timeScale = 0f;
        // if (winPanel != null) winPanel.SetActive(true);
        // Буде виклик сцени перемоги (зробити пізніше)

        currentLevel++;
        PlayerPrefs.SetInt(CurrentLevelKey, currentLevel);
        PlayerPrefs.Save();
    }

    public void LoseGame()
    {
        if (gameEnded) return;
        gameEnded = true;
        Debug.Log("You Lose!");
        Time.timeScale = 0f;
        // if (losePanel != null) losePanel.SetActive(true);
        // Буде виклик сцени поразки (зробити пізніше)
    }

    private void UpdateCoinUI()
    {
        // Перевіряємо, чи призначено TextMeshProUGUI об'єкт
        if (coinCountText != null)
        {
            coinCountText.text = "Coins: " + currentCoins + " / " + coinsToWin;
        }
    }

    private void UpdateLevelUI()
    {
        // Перевіряємо, чи призначено TextMeshProUGUI об'єкт
        if (levelText != null)
        {
            levelText.text = "Level: " + currentLevel;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToNextLevel()
    {
        Time.timeScale = 1f;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // некст рівень
    }

    public bool IsGameEnded()
    {
        return gameEnded;
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }
}