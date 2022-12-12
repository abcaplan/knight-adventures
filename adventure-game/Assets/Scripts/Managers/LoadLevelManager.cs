using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelManager : MonoBehaviour
{
    [SerializeField] private int indexLevelToLoad;
    public bool level1 = false;
    public bool level2 = false;
    public bool level3 = false;

    private void OnTriggerEnter2D(Collider2D collider) {
        GameObject collision = collider.gameObject;

        if(collision.tag == "Player") {
            if (indexLevelToLoad == 2) {
                level1 = true;
            } else if (indexLevelToLoad == 3) {
                level2 = true;
            } else if (indexLevelToLoad == 4) {
                level3 = true;
            }

            LoadLevel();
        }
    }

    private void LoadLevel() {
        SceneManager.LoadScene(indexLevelToLoad);
    }
}
