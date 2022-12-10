using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour
{
    private PlatformEffector2D effector;
    private float buttonCooler = 0.5f;
    private int buttonCount = 0;

    private void Awake() {
        effector = GetComponent<PlatformEffector2D>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            if (buttonCooler > 0 && buttonCount == 1) {
                effector.rotationalOffset = 180f;
            } else {
                buttonCooler = 0.5f;
                buttonCount++;
            }
        }

        if (buttonCooler > 0) {
            buttonCooler -= 1 * Time.deltaTime;
        } else {
            effector.rotationalOffset = 0f;
            buttonCount = 0;
        }
    }
}
