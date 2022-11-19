using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    private float currentHealth;
    private Animator anim;
    private bool dead;

    private void Awake() {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    // Currently the game is made to oneshot the player (it can be updated for health if needed)
    public void TakeDamage(float _damage) {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (!dead) {
            anim.SetTrigger("dead");
            GetComponent<PlayerMovement>().enabled = false;
            dead = true;
        }        
    }
}
