using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set;}
    private AudioSource source;

    private void Awake() {
        source = GetComponent<AudioSource>();

        if (instance == null) {
            // Keep audio for other scenes
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != null && instance != this) {
            // Destroy duplicate audios
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip _sound) {
        source.PlayOneShot(_sound);
    }
}
