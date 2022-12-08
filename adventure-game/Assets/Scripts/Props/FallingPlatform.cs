using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float fallDelay;
    [SerializeField] private float destroyDelay;
    private SpriteRenderer spriteRend;

    private void Awake() {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall(){
        spriteRend.color = Color.red;
        yield return new WaitForSeconds(fallDelay);
        spriteRend.color = Color.white;
        body.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyDelay);
    }
}
