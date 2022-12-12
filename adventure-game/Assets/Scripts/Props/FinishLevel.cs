using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    [SerializeField] private AudioClip finishSound;
    [SerializeField] private int indexLevelToLoad;
    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player") {
            SoundManager.instance.PlaySound(finishSound);
            anim.SetTrigger("finish");
            Invoke("LoadLevel", 2f);
        }
    }

    private void LoadLevel() {
        SceneManager.LoadScene(indexLevelToLoad);
    }
}
