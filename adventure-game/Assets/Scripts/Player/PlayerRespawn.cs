using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private Health playerHealth;

    private void Awake() {
        playerHealth = GetComponent<Health>();
    }

    private void CheckRespawn() {
        if (currentCheckpoint != null) {
            transform.position = currentCheckpoint.position;
            // Restore player normal attributes
            playerHealth.Respawn();
            // CURRENTLY IT DOESN'T RESPAWN ENEMIES
        } else {
            // When there s no respawn point, reset everything
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }   
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.transform.tag == "Checkpoint") {
            // Assign new checkpoint to the latest one
            currentCheckpoint = collider.transform;
            
            // Play sound and animation
            SoundManager.instance.PlaySound(checkpointSound);
            collider.GetComponent<Collider2D>().enabled = false;
            collider.GetComponent<Animator>().SetTrigger("appear");
        }
    }
}
