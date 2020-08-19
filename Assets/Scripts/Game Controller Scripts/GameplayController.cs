using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField] 
    private GameObject gameOverPanel;

    private GameObject background;
    private GameObject ground;

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text highScoreText;

    private int score;

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        StartCoroutine(IncreaseDifficulty());
        score = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        background = GameObject.Find("Background");
        ground = GameObject.Find("Ground");

        scoreText.text = score + "M";
        CheckAndSetHighScore();
        StartCoroutine(CountScore());
    }

    /// This function is called when the object becomes enabled and active.
    void OnEnable()
    {
        Player.endGame += PlayerDiedEndTheGame;
    }

    /// This function is called when the behaviour becomes disabled or inactive.
    void OnDisable()
    {
        Player.endGame -= PlayerDiedEndTheGame;
    }

    private IEnumerator CountScore() {
        yield return new WaitForSeconds(0.6f);
        score++;
        scoreText.text = score + "M";

        CheckAndSetHighScore();

        StartCoroutine(CountScore());
    }

    private IEnumerator IncreaseDifficulty() {
        yield return new WaitForSecondsRealtime(30f);

        background.GetComponent<BackgroundLooper> ().SetSpeed(0.1f);
        ground.GetComponent<BackgroundLooper> ().SetSpeed(0.1f);
        ObstacleSpawner.instance.SetNewObstacleSpeed(-2f);
        ObstacleSpawner.instance.SetSpawningTime(1f);

        StartCoroutine(IncreaseDifficulty());
    }

    private void PlayerDiedEndTheGame() {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void CheckAndSetHighScore() {
        if(!PlayerPrefs.HasKey("HighScore")) {
            PlayerPrefs.SetInt("HighScore", 0);
        } else {
            int highScore = PlayerPrefs.GetInt("HighScore");

            if(highScore < score) {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }
        highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString() + "M";
    }

    public void PauseGame() {
        if(!gameOverPanel.activeInHierarchy){
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
    }

    public void GoToMenu() {
        AdsManager.instance.ShowAd();
        Time.timeScale = 1f;
        //AdsManager.instance.DestroyAd();
        SceneManager.LoadScene("1_MainMenu");
    }

    public void ResumeGame() {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void RestartGame() {
        Time.timeScale = 1f;
        //gameOverPanel.SetActive(false);
        SceneManager.LoadScene("2_Gameplay");
    }
}
