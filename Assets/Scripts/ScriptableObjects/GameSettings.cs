using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptables/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public string mainLevelSceneName;
    public string mainMenuSceneName;

    public float ShootCooldownTime;
    public float ProjectileSpeed;
}
