using UnityEngine;
using Pathfinding;

public class FlyingEnemiAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform enemyGFX;
    [SerializeField] private float speed;
    [SerializeField] private float nextWaypointDistance;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D body;

    private void Start() {
        seeker = GetComponent<Seeker>();
        body = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    private void FixedUpdate() {
        if (path == null) {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count) {
            reachedEndOfPath = true;
            return;
        } else {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - body.position).normalized;
        Vector2 force = direction * speed;

        body.AddForce(force);

        float distance = Vector2.Distance(body.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }

        if (force.x >= 0.01f) {
            enemyGFX.localScale = new Vector3(-0.06f, 0.06f, 0.06f);
        } else if (force.x <= -0.01f) {
            enemyGFX.localScale = new Vector3(0.06f, 0.06f, 0.06f);
        }
    }

    private void UpdatePath() {
        if (seeker.IsDone()) {
            seeker.StartPath(body.position, target.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }
}
