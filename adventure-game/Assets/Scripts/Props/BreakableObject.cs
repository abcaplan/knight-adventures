using UnityEngine;
using UnityEngine.Events;
using Pathfinding;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] private UnityEvent hit;
    private SpriteRenderer spriteRend;
    private Color colorBrightRed;
    private Color colorDarkRed;
    private int strongHits = 0;
    private int weakHits = 0;

    private void Awake() {
        spriteRend = GetComponent<SpriteRenderer>();
        colorBrightRed = new Color(0.93f, 0.29f, 0.16f, 1f); // #EE4B2B
        colorDarkRed = new Color(0.48f, 0.066f, 0.066f, 1f); // #7B1111
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "PlayerFireball" || collider.tag == "SwordRange") {

            if (gameObject.tag == "Props") {
                // Update A* pathfinding
                UpdatePath(collider);

                // Break object
                hit?.Invoke();
            }

            if (gameObject.tag == "WeakBreakable") {
                if (weakHits == 0) {
                    spriteRend.color = Color.red;
                } else if(weakHits == 1) {
                    spriteRend.color = colorBrightRed;
                } else if(weakHits == 2) {
                    spriteRend.color = colorDarkRed;
                } else if (weakHits == 3) {
                    // Update A* pathfinding
                    UpdatePath(collider);

                    // Break object
                    hit?.Invoke();
                    weakHits = 0;
                }
                weakHits++;
            }

            if (gameObject.tag == "StrongBreakable") {
                if (strongHits == 0) {
                    spriteRend.color = Color.red;
                } else if (strongHits == 2) {
                    spriteRend.color = colorBrightRed;
                } else if (strongHits == 4) {
                    spriteRend.color = colorDarkRed;
                } else if (strongHits == 5) {
                    // Update A* pathfinding
                    UpdatePath(collider);

                    // Break object
                    hit?.Invoke();
                    strongHits = 0;
                }
                strongHits++;
            }
        }
    }

    private void UpdatePath(Collider2D collider) {
        // Update A* pathfinding
        var guo = new GraphUpdateObject(collider.bounds);
        guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs (guo);
    }
}
