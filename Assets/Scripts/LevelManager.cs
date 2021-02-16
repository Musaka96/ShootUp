using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    PlayerController player1Controller;
    [SerializeField]
    PlayerController player2Controller;

    [SerializeField]
    TextMeshProUGUI player1Text;
    [SerializeField]
    TextMeshProUGUI player2Text;

    [SerializeField]
    Toggle player1Toggle;
    [SerializeField]
    Toggle player2Toggle;

    int player1Score = 0;
    int player2Score = 0;
    float maxScore;

    [SerializeField]
    Button pauseMenuButton;

    [SerializeField]
    PauseMenu pauseMenu;

    private void Awake() {
        GameManager.Instance.RegisterLevelManager(this);
        this.player1Controller.SetLevelmanager(this);
        this.player2Controller.SetLevelmanager(this);
        this.pauseMenu.SetLevelmanager(this);

        this.player1Toggle.onValueChanged.AddListener((value) =>
        {
            this.player1Controller.SetAutoShoot(value);
        });

        this.player2Toggle.onValueChanged.AddListener((value) =>
        {
            this.player2Controller.SetAutoShoot(value);
        });

        this.pauseMenuButton.onClick.AddListener(this.pauseMenu.ShowPauseMenu);

        this.UpdateScoreUI();
    }

    public void SetMaxScore(float maxScore) {
        this.maxScore = maxScore;
    }

    public void OnPlayerHit(PlayerController playerController) {
        if (playerController == this.player1Controller) {
            this.player2Score++;
        } else {
            this.player1Score++;
        }

        this.UpdateScoreUI();
        this.CheckVictory();
    }

    public void RestartLevel() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameManager.Instance.Settings.mainLevelSceneName, LoadSceneMode.Single);
    }

    private void CheckVictory() {
        if (this.player1Score >= this.maxScore || this.player2Score >= this.maxScore) {
            this.pauseMenu.ShowVictoryMenu(this.player1Score >= this.maxScore ? "Left Player" : "Right Player");
        }
    }

    private void UpdateScoreUI() {
        this.player1Text.text = this.player1Score.ToString();
        this.player2Text.text = this.player2Score.ToString();
    }

    public void GoToMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameManager.Instance.Settings.mainMenuSceneName, LoadSceneMode.Single);
    }

}