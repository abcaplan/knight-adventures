using UnityEngine;

public class SwordAttackArea : MonoBehaviour
{
    [Header ("Sword Attributes")]
    [SerializeField] private int damage;

    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.GetComponent<Health>() != null) {
            Health health = collider.GetComponent<Health>();
            health.TakeDamage(damage);
        }
    }
}
