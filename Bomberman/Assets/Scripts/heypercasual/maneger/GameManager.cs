using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI liveGemsText; 
    [SerializeField] private TextMeshProUGUI totalGemsText; 
    private int collectedGems = 0;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        UpdateLiveGemsUI();
        Time.timeScale = 1f;
    }
    public void AddGems(int amount)
    {
        collectedGems += amount;
        UpdateLiveGemsUI();
    }
    private void UpdateLiveGemsUI()
    {
        if (liveGemsText != null)
        {
            liveGemsText.text = collectedGems.ToString();
        }
    }
    public void LevelCompleted()
    {
        if (winPanel != null) winPanel.SetActive(true); 
        if (totalGemsText != null)
        {
            totalGemsText.text = "Gems: " + collectedGems.ToString();
        }
        Time.timeScale = 0f;
    }
    public void GameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    } 
}