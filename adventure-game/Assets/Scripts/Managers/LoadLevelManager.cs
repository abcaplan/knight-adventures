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

    
    void OnGUI() {
        if (true) {
            GUI.Label(new Rect(0, 0, 256, 32), "1: " + level1.ToString());
            GUI.Label(new Rect(0, 16, 256, 32), "2: " + level2.ToString());
            GUI.Label(new Rect(0, 32, 256, 32), "3: " + level3.ToString());
        }
    }
}
