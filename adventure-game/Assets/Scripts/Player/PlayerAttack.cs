using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header ("Attributes")]
    [SerializeField] private float actionCooldown;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private GameObject blockArea;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private float playerSpeedBlocking;

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
        if (cooldownTimer > actionCooldown && playerMovement.canAttack() && !playerMovement.isCrouching && !blocking) {
            if (Input.GetKey(KeyCode.J)) {
                SwordAttack();
            } else if (Input.GetKey(KeyCode.K)) {
                FireballAttack();
            }
        }

        // Block Action
        if (Input.GetKey(KeyCode.L) && cooldownTimer > actionCooldown && playerMovement.canAttack() && !playerMovement.isCrouching) {
            blocking = true;
            anim.SetBool("blockAttack", blocking);
            cooldownTimer = 0;
            blockArea.SetActive(true);

            // Slow player movement while blocking is active
            playerMovement.currentSpeed = playerSpeedBlocking;
        }

        // Disable Block Animation
        if (Input.GetKeyUp(KeyCode.L)){
            blocking = false;
            anim.SetBool("blockAttack", blocking);
            blockArea.SetActive(false);

            // Re-enable full speed when no longer blocking
            playerMovement.currentSpeed = playerMovement.baseSpeed;
        }

        cooldownTimer += Time.deltaTime;

        // Adjust cooldown for ranged attacks, melee attacks and sword blocks
        if (attacking && cooldownTimer >= actionCooldown) {
            cooldownTimer = 0;
            attacking = false;
            blocking = false;

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

    // For testing
    /*
    void OnGUI() {
        if (true) {
            GUI.Label(new Rect(0, 0, 256, 32), "Is Blocking: " + blocking.ToString());
            GUI.Label(new Rect(0, 16, 256, 32), "Is Crouching: " + playerMovement.isCrouching.ToString());
        }
    }
    */
}
