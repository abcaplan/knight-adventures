using UnityEngine;
using System.Collections;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered;
    private bool activated;

    private Health player;

    private void Awake() {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (!triggered) {
                StartCoroutine(ActivateFiretrap());
            }

            player = collision.GetComponent<Health>();

            if (activated) {
                player.TakeDamage(damage);
            }  
        }
    }

    private IEnumerator ActivateFiretrap() {
        triggered = true;
        spriteRend.color = Color.red;

        yield return new WaitForSeconds(activationDelay);
        spriteRend.color = Color.white;
        activated = true;
        anim.SetBool("activated", true);

        yield return new WaitForSeconds(activeTime);
        activated = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}
