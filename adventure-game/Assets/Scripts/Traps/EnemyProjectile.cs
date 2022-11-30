using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [Header ("Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float resetTravelDuration;
    private float lifetime;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private bool hit;

    private void Awake() {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile() {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        boxCollider.enabled = true;
    }

    private void Update() {
        if (hit) {
            return;
        }

        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTravelDuration) {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        hit = true;
        boxCollider.enabled = false;

        // Goes to Enemy Damage OnTriggerEnter2D, executes then returns here
        base.OnTriggerEnter2D(collision);
        gameObject.SetActive(false);

        if (anim != null) {
            anim.SetTrigger("explode");
        } else {
            gameObject.SetActive(false);
        }
    }

    private void Deactivate() {
        gameObject.SetActive(false);
    }
}
