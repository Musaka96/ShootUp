using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    LevelManager levelManager;
    MainMenuController mainMenuManager;

    float difficulty;
    float maxPoints;

    [SerializeField]
    GameSettings settings;

    public GameSettings Settings { get => this.settings; }
    public float Difficulty { get => this.difficulty; }

    public void RegisterMainMenu(MainMenuController mainMenu) {
        this.mainMenuManager = mainMenu;
    }

    public void RegisterLevelManager(LevelManager levelManager) {
        this.levelManager = levelManager;
        this.levelManager.SetMaxScore(this.maxPoints);
    }

    public void StartGame(float difficulty, float maxPoints) {
        this.difficulty = difficulty;
        this.maxPoints = maxPoints;

        DontDestroyOnLoad(this);
        SceneManager.LoadScene(this.settings.mainLevelSceneName, LoadSceneMode.Single);
    }
}
