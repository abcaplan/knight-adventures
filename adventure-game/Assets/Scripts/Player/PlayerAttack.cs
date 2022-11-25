using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header ("Attributes")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    private Animator anim;
    private PlayerMovement playerMovement;
    public float cooldownTimer = Mathf.Infinity;

    [Header ("Audio")]
    [SerializeField] private AudioClip fireballSound;
    // [SerializeField] private AudioClip swordSound;

    private void Awake() {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update() {
        if (Input.GetKey(KeyCode.L) && cooldownTimer > attackCooldown && playerMovement.canAttack()) {
            FireballAttack();
        }
        cooldownTimer += Time.deltaTime;
    }

    private void FireballAttack() {
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
