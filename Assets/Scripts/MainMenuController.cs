using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    Button startGameButton;

    [SerializeField]
    Slider difficultySlider;
    [SerializeField]
    Slider maxPointsSlider;

    GameManager gameManager;

    private void Awake() {
        this.gameManager = GameManager.Instance;
        this.gameManager.RegisterMainMenu(this);

        this.startGameButton.onClick.AddListener(() => 
        {
            this.gameManager.StartGame(this.difficultySlider.value, this.maxPointsSlider.value);
        });
    }
}