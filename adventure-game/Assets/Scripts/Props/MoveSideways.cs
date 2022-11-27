using UnityEngine;

public class MoveSideways : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;

    [Header ("Attributes")]
    [SerializeField] private float speed;

    private int currentWaypoint = 0;

    private void Update() {
        if (Vector2.Distance(waypoints[currentWaypoint].transform.position, transform.position) < 0.1f) {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length) {
                currentWaypoint = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, Time.deltaTime * speed);
    }
}
