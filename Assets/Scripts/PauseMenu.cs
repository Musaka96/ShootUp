using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI menuText;

    [SerializeField]
    Button restartButton;
    [SerializeField]
    Button mainMenuButton;

    LevelManager levelManager;

    bool canClose = true;

    private void Awake() {
        this.mainMenuButton.onClick.AddListener(() =>
        {
            this.levelManager.GoToMainMenu();
        });

        this.restartButton.onClick.AddListener(() =>
        {
            this.levelManager.RestartLevel();
        });
    }

    public void SetLevelmanager(LevelManager levelManager) {
        this.levelManager = levelManager;
    }

    public void ShowPauseMenu() {
        this.menuText.text = "PAUSE";
        this.gameObject.SetActive(true);
        this.transform.SetAsLastSibling();
        Time.timeScale = 0;
    }

    public void ShowVictoryMenu(string whoWon) {
        this.canClose = false;

        this.menuText.text = whoWon + " WON";
        this.gameObject.SetActive(true);
        this.transform.SetAsLastSibling();
        Time.timeScale = 0;
    }

    public void ClosePauseMenu() {
        if (!this.canClose) return;

        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
