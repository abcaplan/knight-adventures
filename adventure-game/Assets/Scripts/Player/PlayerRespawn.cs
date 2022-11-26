using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private Health playerHealth;

    private void Awake() {
        playerHealth = GetComponent<Health>();
    }

    private void Respawn() {
        transform.position = currentCheckpoint.position;

        // Restore player normal attributes
        playerHealth.Respawn();

        // Camera?
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.transform.tag == "Checkpoint") {
            currentCheckpoint = collider.transform;
            SoundManager.instance.PlaySound(checkpointSound);
            collider.GetComponent<Collider2D>().enabled = false;
            collider.GetComponent<Animator>().SetTrigger("appear");
        }
    }
}
