using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header ("Attributes")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    private bool attacking = false;
    public bool blocking = false;
    private Animator anim;
    private PlayerMovement playerMovement;
    public float cooldownTimer = Mathf.Infinity;

    [Header ("Audio")]
    [SerializeField] private AudioClip fireballSound;
    [SerializeField] private AudioClip swordSound;

    private void Awake() {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update() {
        if (cooldownTimer > attackCooldown && playerMovement.canAttack() && !playerMovement.isCrouching) {
            if (Input.GetKey(KeyCode.J)) {
                SwordAttack();
            } else if (Input.GetKey(KeyCode.K)) {
                FireballAttack();
            } else if (Input.GetKey(KeyCode.L)) {
                BlockAttack();
            }
        }

        cooldownTimer += Time.deltaTime;

        // Adjust cooldown for ranged attacks, melee attacks and sword blocks
        if (attacking && cooldownTimer >= attackCooldown) {
            cooldownTimer = 0;
            attacking = false;

            // Deactivate sword damage range
            attackArea.SetActive(attacking);
        }
    }

    private void SwordAttack() {
        attacking = true;
        SoundManager.instance.PlaySound(swordSound);
        anim.SetTrigger("swordAttack");
        cooldownTimer = 0;

        attackArea.SetActive(attacking);
    }

    private void BlockAttack() {
        // Incomplete
        attacking = true;
        blocking = true;
        anim.SetTrigger("blockAttack");
        cooldownTimer = 0;
    }

    private void FireballAttack() {
        attacking = true;
        SoundManager.instance.PlaySound(fireballSound);
        anim.SetTrigger("fireballAttack");
        cooldownTimer = 0;
        
        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Fireball>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindFireball() {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
