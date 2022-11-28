using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class ButtonController : MonoBehaviour
{
    [SerializeField] GameObject moveToUI;
    [SerializeField] GameObject currentUI;

    // Use for onButtonClicked, activates new UI
    public void OpenUI() {
        if (currentUI != null) {
            currentUI.SetActive(false);
        }

        if (moveToUI != null) {
            moveToUI.SetActive(true);
        }

    }

    public void ResetScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}