using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float spaceBetween;


    private void Update() {
        if (Vector2.Distance(target.position, transform.position) >= spaceBetween) {
            Vector2 direction = target.position - transform.position;
            transform.Translate(direction * Time.deltaTime);
        }
    }
}