using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public PlayerController player;
    public UIHandler uiHandler;
    public float restartDelay = 5f;
    public bool ended { get; private set; }
    public float elapsed { get; private set; }
    public int repaired { get; private set; }
    EnemyController[] enemies;
    void Awake() { instance = this; }
    void OnEnable() { instance = this; }
    void Start() { enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None); }
    void Update()
    {
        if(ended) return; elapsed += Time.deltaTime;
        repaired = 0; foreach(var enemy in enemies) if(enemy != null && !enemy.isBroken) repaired++;
        uiHandler?.SetProgress(repaired, enemies.Length);
        if(player.health <= 0) End(false);
        else if(enemies.Length > 0 && repaired == enemies.Length) End(true);
    }
    void End(bool win) { ended = true; if(win) uiHandler?.DisplayWinScreen(); else uiHandler?.DisplayLoseScreen(); Invoke(nameof(ReloadScene), restartDelay); }
    public void ReloadScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    void OnDestroy() { if(instance == this) instance = null; }
}
