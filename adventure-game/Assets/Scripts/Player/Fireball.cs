using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header ("Fireball Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private int resetTravelDuration;
    private float direction;
    private bool hit;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;


    private void Awake() {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        if (hit) {
            return;
        }

        float travelSpeed = speed * Time.deltaTime * direction;
        transform.Translate(travelSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTravelDuration) {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");

        if (collision.tag == "Enemy") {
            collision.GetComponent<Health>().TakeDamage(1);
        }
    }

    public void SetDirection(float _direction) {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction) {
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate() {
        gameObject.SetActive(false);
    }
}
