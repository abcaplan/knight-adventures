using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private AudioClip collectItem;
    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            SoundManager.instance.PlaySound(collectItem);
            anim.SetTrigger("destroy");
        }
    }

    private void DestroyCollectable() {
        Destroy(gameObject);
    }
}
