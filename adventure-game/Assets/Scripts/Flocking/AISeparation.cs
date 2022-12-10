using UnityEngine;

public class AISeparation : MonoBehaviour
{
    GameObject[] AI;
    [SerializeField] private float spaceBetween;

    private void Start() {
        AI = GameObject.FindGameObjectsWithTag("AI Bird");
    }

    private void Update() {
        foreach(GameObject go in AI) {
            if (go != gameObject) {
                float distance = Vector2.Distance(go.transform.position, this.transform.position);
                if (distance <= spaceBetween) {
                    Vector2 direction = transform.position - go.transform.position;
                    transform.Translate(direction * Time.deltaTime);
                }
            }
        }
    }
}
