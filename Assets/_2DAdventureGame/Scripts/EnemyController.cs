using UnityEngine;
public class EnemyController : MonoBehaviour
{
    public float speed = 1f;
    public bool vertical;
    public float changeTime = 3f;
    public int damage = 1;
    public bool isBroken { get; private set; } = true;
    Rigidbody2D body;
    Animator animator;
    float timer;
    int direction = 1;
    void Awake() { body = GetComponent<Rigidbody2D>(); animator = GetComponent<Animator>(); timer = changeTime; }
    void Update()
    {
        if (!isBroken || (GameManager.instance != null && GameManager.instance.ended)) return;
        timer -= Time.deltaTime;
        if (timer <= 0) { direction = -direction; timer += changeTime; }
        if (animator != null) { animator.SetFloat("Move X", vertical ? 0 : direction); animator.SetFloat("Move Y", vertical ? direction : 0); }
    }
    void FixedUpdate()
    {
        if (isBroken && (GameManager.instance == null || !GameManager.instance.ended))
            body.MovePosition(body.position + (vertical ? Vector2.up : Vector2.right) * direction * speed * Time.fixedDeltaTime);
    }
    void OnTriggerStay2D(Collider2D other) { if (isBroken) other.GetComponent<PlayerController>()?.ChangeHealth(-damage); }
    public void Fix()
    {
        if (!isBroken) return;
        isBroken = false;
        body.linearVelocity = Vector2.zero;
        body.angularVelocity = 0f;
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        body.bodyType = RigidbodyType2D.Static;
        if (animator != null) animator.SetTrigger("Fixed");
        foreach(var c in GetComponents<Collider2D>()) if(c.isTrigger) c.enabled = false;
    }
}
