using UnityEngine;
public class DamageZone : MonoBehaviour
{
    public int damageAmount = -1;
    void OnTriggerStay2D(Collider2D other) { other.GetComponent<PlayerController>()?.ChangeHealth(-Mathf.Abs(damageAmount)); }
}
