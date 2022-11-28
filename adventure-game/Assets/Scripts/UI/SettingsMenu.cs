using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public TMPro.TMP_Dropdown difficultyDropdown;
    Resolution[] resolutions;
    private string destination => Path.Combine(Application.persistentDataPath, "save.dat");

    void Start() {

        resolutions = Screen.resolutions;

        // Remove all options in dropdown and add the correct resolutions
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        // Create formatted string for each resolution
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height) {
                    currentResolutionIndex = i;
            }
        }

        // Add correct resolutions to the resolution drop down
        resolutionDropdown.AddOptions(options);

        // Set the initial correct resolution to the recommended one
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    /*
    public void SetDifficulty(int difficultyIndex){
        if (difficultyIndex == 0) {
            difficulty = 0;
        } else if (difficultyIndex == 1) {
            difficulty = 1;
        } else {
            difficulty = 2;
        }
    }
    */

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume) {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetFullscreen (bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

}
