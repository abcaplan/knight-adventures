using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelManager : MonoBehaviour
{
    [SerializeField] private int indexLevelToLoad;

    private void OnTriggerEnter2D(Collider2D collider) {
        GameObject collision = collider.gameObject;

        if(collision.tag == "Player") {
            LoadLevel();
        }
    }

    private void LoadLevel() {
        SceneManager.LoadScene(indexLevelToLoad);
    }
}
