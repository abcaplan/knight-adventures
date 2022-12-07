using UnityEngine;

public class MoveEnemyUpDown : MonoBehaviour
{
    [Header ("Enemy Attributes")]
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    private bool movingDown;
    private float downLocation;
    private float upLocation;

    private void Awake() {
        downLocation = transform.position.y - movementDistance;
        upLocation = transform.position.y + movementDistance;
    }

    private void Update() {
        if (movingDown) {
            if (transform.position.y > downLocation) {
                transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
            } else {
                movingDown = false;
            }
        } else {
            if (transform.position.y < upLocation) {
                transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
            } else {
                movingDown = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
