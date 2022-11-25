using UnityEngine;
using System.Collections;

public class FireTrap : MonoBehaviour
{
    [Header ("Attributes")]
    [SerializeField] private float damage;
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;

    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered;
    private bool activated;

    private Health playerHealth;

    [Header ("Audio")]
    [SerializeField] private AudioClip fireTrapSound;

    private void Awake() {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (playerHealth != null && activated) {
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            playerHealth = collision.GetComponent<Health>();

            if (!triggered) {
                StartCoroutine(ActivateFiretrap());
            }

            if (activated) {
                playerHealth.TakeDamage(damage);
            }  
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            playerHealth = null;
        }
    }

    private IEnumerator ActivateFiretrap() {
        triggered = true;
        spriteRend.color = Color.red;

        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(fireTrapSound);
        spriteRend.color = Color.white;
        activated = true;
        anim.SetBool("activated", true);

        yield return new WaitForSeconds(activeTime);
        activated = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}
