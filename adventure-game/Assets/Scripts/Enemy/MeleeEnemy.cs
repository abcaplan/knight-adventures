using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header ("Attack Attributes")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float range;

    [Header ("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header ("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    private Health playerHealth;

    [Header ("Audio")]
    [SerializeField] private AudioClip swordSound;

    private Animator anim;
    private EnemyPatrol enemyPatrol;

    private void Awake() {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update() {
        cooldownTimer += Time.deltaTime;

        if (CanSeePlayer()) {
            if (cooldownTimer >= attackCooldown && playerHealth.currentHealth > 0) {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
                SoundManager.instance.PlaySound(swordSound);
            }
        }
        
        if (enemyPatrol != null) {
            enemyPatrol.enabled = !CanSeePlayer();
        }
    }

    private bool CanSeePlayer() {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
         new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        
        if (hit.collider != null) {
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
         new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer() {
        if (CanSeePlayer()) {
            playerHealth.TakeDamage(damage);
        }
    }
}
