using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    private int score = 0;

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Collectable") {
            score += 100;
            scoreText.text = "Score: " + score;
        }
    }
}
