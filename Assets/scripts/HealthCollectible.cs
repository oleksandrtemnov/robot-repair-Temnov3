using UnityEngine;
public class HealthCollectible : MonoBehaviour
{
    public int healAmount = 1;
    bool collected;
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerController>();
        if (!collected && player != null && player.health > 0 && player.health < player.maxHealth)
        { collected = true; player.ChangeHealth(healAmount); Destroy(gameObject); }
    }
}
