using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header ("iFrames")]
    [SerializeField] private float immuneDuration;
    [SerializeField] private int flashes;
    private SpriteRenderer spriteRend;

    [Header ("Components")]
    [SerializeField] private Behaviour[] components;
    private bool immunity;

    [Header ("Audio")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    private void Awake() {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage) {
        if (immunity) {
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0) {
            anim.SetTrigger("hurt");
            StartCoroutine(Immunity());
            SoundManager.instance.PlaySound(hurtSound);
        } else {        
            if (!dead) {
                anim.SetTrigger("dead");

                foreach (Behaviour component in components){
                    component.enabled = false;
                }

                dead = true;
                SoundManager.instance.PlaySound(deathSound);
            }
        }
    }
    private IEnumerator Immunity() {
        immunity = true;
        Physics2D.IgnoreLayerCollision(8, 9, true);
        for (int i = 0; i < flashes; i++) {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(immuneDuration / (flashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(immuneDuration / (flashes * 2));
        }
        Physics2D.IgnoreLayerCollision(8, 9, false);
        immunity = false;
    }

    private void Deactivate() {
        gameObject.SetActive(false);
    }
}
