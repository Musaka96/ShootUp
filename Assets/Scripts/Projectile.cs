using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    PlayerController shootingPlayer;

    protected static string LevelTag = "Level";
    protected static string PlayerTag = "Player";

    public float speedMod = 1f;

    public void Setup(PlayerController player) {
        this.shootingPlayer = player;
    }

    private void OnTriggerEnter2D(Collider2D trigger) {
        if (trigger.tag == LevelTag) {
            this.OnLevelTrigger(trigger);
        } else if (trigger.tag == PlayerTag) {
            PlayerController player = trigger.GetComponent<PlayerController>();

            if (player.gameObject.name != this.shootingPlayer.gameObject.name) {
                this.OnAnotherPlayerTrigger(player);
            }

        }
    }

    protected abstract void OnLevelTrigger(Collider2D trigger);
    protected abstract void OnAnotherPlayerTrigger(PlayerController player);
}
