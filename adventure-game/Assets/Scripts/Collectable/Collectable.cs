using UnityEngine;

public class Collectable : MonoBehaviour
{    
    [SerializeField] private int score;
    [SerializeField] private AudioClip collectItem;
    [SerializeField] private BoxCollider2D boxCollider;
    private PlayerMovement playerMovement;
    private Animator anim;
  
    private void Awake() {
        anim = GetComponent<Animator>();
    }    

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            playerMovement.AddScore(score);
            boxCollider.enabled = false;
            SoundManager.instance.PlaySound(collectItem);
            anim.SetTrigger("destroy");
        }
    }

    private void DestroyCollectable() {
        Destroy(gameObject);
    }
}
