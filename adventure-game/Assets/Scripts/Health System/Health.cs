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

    private void Awake() {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage) {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0) {
            anim.SetTrigger("hurt");
            StartCoroutine(Immunity());
        } else {        
            if (!dead) {
                anim.SetTrigger("dead");

                foreach (Behaviour component in components){
                    component.enabled = false;
                }

                dead = true;
            }
        }
    }
    private IEnumerator Immunity() {
        Physics2D.IgnoreLayerCollision(8, 9, true);
        for (int i = 0; i < flashes; i++) {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(immuneDuration / (flashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(immuneDuration / (flashes * 2));
        }
        Physics2D.IgnoreLayerCollision(8, 9, false);
    }

    private void Deactivate() {
        gameObject.SetActive(false);
    }
}
