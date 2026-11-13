using System.Collections;
using UnityEngine;
public class TimedSwitch : MonoBehaviour
{
    public GameObject door;
    public float openTime = 4f;
    public SpriteRenderer indicator;
    public TimedSwitch controller;
    public SpriteRenderer[] linkedIndicators;
    public float remaining { get; private set; }
    Color initialColor;
    void Awake() { if(indicator != null) initialColor = indicator.color; }
    void OnTriggerEnter2D(Collider2D other) { if(other.GetComponent<PlayerController>() != null) (controller != null ? controller : this).Activate(); }
    public void Activate() { if(door == null) return; StopAllCoroutines(); StartCoroutine(OpenDoor()); }
    IEnumerator OpenDoor()
    {
        // Collider.bounds is empty while the door is inactive after a timer refresh.
        var box = door.GetComponent<BoxCollider2D>();
        var bounds = new Bounds(door.transform.TransformPoint(box.offset), Vector3.Scale((Vector3)box.size, door.transform.lossyScale));
        door.SetActive(false); SetColor(new Color(.3f, 1f, .6f)); remaining = openTime;
        while(remaining > 0) { UIHandler.instance?.ShowHint("Двері відкрито на " + openTime.ToString("0.#") + " с\nЗалишилось: " + remaining.ToString("0.0") + " с"); remaining -= Time.deltaTime; yield return null; }
        remaining = 0;
        while(PlayerInside(bounds)) { UIHandler.instance?.ShowHint("Звільни дверний прохід"); yield return null; }
        door.SetActive(true); SetColor(initialColor); UIHandler.instance?.ShowHint("Двері зачинено. Плита запускає таймер знову.");
    }
    bool PlayerInside(Bounds bounds) { foreach(var hit in Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0)) if(hit.GetComponent<PlayerController>() != null) return true; return false; }
    void SetColor(Color color) { if(indicator != null) indicator.color = color; if(linkedIndicators != null) foreach(var item in linkedIndicators) if(item != null) item.color = color; }
    void OnDisable() { StopAllCoroutines(); remaining = 0; if(controller == null && door != null) door.SetActive(false); }
}
