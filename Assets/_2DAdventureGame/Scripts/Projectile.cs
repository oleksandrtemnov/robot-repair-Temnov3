using UnityEngine;
public class Projectile : MonoBehaviour
{
    public float maxDistance = 12f;
    public float lifetime = 3f;
    Vector2 origin;
    void Awake() { origin = transform.position; Destroy(gameObject, lifetime); }
    public void Launch(Vector2 direction, float speed) { GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * speed; }
    void Update() { if (Vector2.Distance(origin, transform.position) > maxDistance) Destroy(gameObject); }
    void Hit(Collider2D other)
    {
        if (other.GetComponentInParent<PlayerController>() != null || other.gameObject.layer == LayerMask.NameToLayer("CameraBounds")) return;
        if (other.isTrigger && other.GetComponentInParent<EnemyController>() == null) return;
        other.GetComponentInParent<EnemyController>()?.Fix(); Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other) { Hit(other); }
    void OnCollisionEnter2D(Collision2D collision) { Hit(collision.collider); }
}
