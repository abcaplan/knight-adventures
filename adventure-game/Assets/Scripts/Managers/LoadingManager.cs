using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    // TESTING

    private void Update() {
        if(Input.GetKeyDown(KeyCode.F)) {
            // Build level 2
            SceneManager.LoadScene(1);
        }
    }
}
