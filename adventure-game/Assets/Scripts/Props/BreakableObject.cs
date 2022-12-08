using UnityEngine;
using UnityEngine.Events;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] private UnityEvent hit;

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "PlayerFireball" || collider.tag == "SwordRange") {
            hit?.Invoke();
        }
    }
}
