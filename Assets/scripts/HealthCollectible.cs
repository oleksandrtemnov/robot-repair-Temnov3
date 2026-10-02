using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public int healAmount = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        // Перевіряємо, чи це гравець і чи його здоров'я менше за максимум
        if (controller != null && controller.health < controller.maxHealth)
        {
            controller.ChangeHealth(healAmount);
            Destroy(gameObject); // Знищуємо аптечку
        }
    }
}