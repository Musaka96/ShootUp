using UnityEngine;

public class Bullet : Projectile
{
    protected override void OnAnotherPlayerTrigger(PlayerController player) {
        player.OnProjectileHit(this);
        Destroy(this.gameObject);
    }

    protected override void OnLevelTrigger(Collider2D trigger) {
        Destroy(this.gameObject);
    }
}
