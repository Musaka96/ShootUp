using Lean.Common;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool canShoot;
    bool autoShoot;
    Camera camera;
    Canvas canvas;

    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    float Sensitivity = 1.0f;

    [SerializeField]
    float Inertia;
    [SerializeField]
    float Damping = -1.0f;
    [SerializeField]
    Direction direction = Direction.Left;
    Vector3 remainingTranslation;

    LevelManager levelManager;
    GameManager gameManager;
    AudioSource hitAudio;

    private void Awake() {
        this.canvas = this.transform.GetComponentInParent<Canvas>();
        if (this.canvas != null && this.canvas.renderMode != RenderMode.ScreenSpaceOverlay) {
            this.camera = this.canvas.worldCamera;
        }

        this.canShoot = true;
        this.gameManager = GameManager.Instance;
        this.hitAudio = this.GetComponent<AudioSource>();
    }

    private void Update() {
        Vector3 oldPosition = this.transform.localPosition;

        List<LeanFinger> allFingers = LeanTouch.Fingers;

        List<LeanFinger> fingersOnSide = allFingers.Where(finger => this.direction == Direction.Left ?
        finger.StartScreenPosition.x < Screen.width / 2f : finger.StartScreenPosition.x > Screen.width / 2f).ToList();

        Vector2 screenDelta = LeanGesture.GetScreenDelta(fingersOnSide);

        if (screenDelta != Vector2.zero) {
            this.TranslateUI(screenDelta);
        }

        // Increment
        this.remainingTranslation += this.transform.localPosition - oldPosition;

        // Get t value
        float factor = LeanHelper.GetDampenFactor(this.Damping, Time.deltaTime);

        // Dampen remainingDelta
        Vector3 newRemainingTranslation = Vector3.Lerp(this.remainingTranslation, Vector3.zero, factor);

        // Shift this transform by the change in delta
        this.transform.localPosition = oldPosition + this.remainingTranslation - newRemainingTranslation;

        if (fingersOnSide.Count == 0 && this.Inertia > 0.0f && this.Damping > 0.0f) {
            newRemainingTranslation = Vector3.Lerp(newRemainingTranslation, this.remainingTranslation, this.Inertia);
        }

        // Update remainingDelta with the dampened value
        this.remainingTranslation = newRemainingTranslation;


        if ((fingersOnSide.Count > 1 || this.autoShoot)
            && this.canShoot) {
            this.Shoot();
        }
    }

    private void Shoot() {
        this.canShoot = false;

        GameObject projectileObject = Instantiate(
            this.projectilePrefab,
            new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z),
            this.transform.rotation,
            this.canvas.transform);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Setup(this);
        projectileObject.GetComponent<Rigidbody2D>()
            .AddForce(this.transform.up * this.gameManager.Settings.ProjectileSpeed * projectile.speedMod * this.gameManager.Difficulty);

        this.StartCoroutine(this.ShootCooldown(this.gameManager.Settings.ShootCooldownTime));
    }

    private void TranslateUI(Vector2 screenDelta) {
        float previousX = this.transform.position.x;

        // Screen position of the transform
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(this.camera, this.transform.position);

        // Add the deltaPosition
        screenPoint += screenDelta * this.Sensitivity;

        // Convert back to world space
        Vector3 worldPoint = default(Vector3);

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.transform.parent as RectTransform, screenPoint, this.camera, out worldPoint) == true) {
            worldPoint.x = previousX;
            worldPoint.y = Mathf.Clamp(worldPoint.y, 0, Screen.height);
            this.transform.position = worldPoint;
        }
    }

    public IEnumerator ShootCooldown(float cooldownTime) {
        float countdown = 0;
        while (countdown <= cooldownTime) {
            countdown += .1f;
            if (countdown <= cooldownTime) {
                yield return new WaitForSeconds(.1f);
            } else break;
        }

        this.canShoot = true;
    }
    public void SetAutoShoot(bool value) {
        this.autoShoot = value;
    }

    public void OnProjectileHit(Projectile projectile) {
        this.levelManager.OnPlayerHit(this);
        this.hitAudio.Play();
    }

    public void SetLevelmanager(LevelManager levelManager) {
        this.levelManager = levelManager;
    }
}