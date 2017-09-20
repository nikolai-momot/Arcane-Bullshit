using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour {

    public void Start() {
        AudioSingleton.instance.AudioSetup();

        return;
    }

    public void Update() {
        //Checking platform and scene *** Change when compiling to iOS ***
        if ((Application.platform == RuntimePlatform.Android) && (Application.loadedLevelName != "Menu")) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Application.LoadLevel("Menu");

                return;
            }
        } else if (Application.loadedLevelName == "Menu") {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Application.Quit();

                return;
            }
        }
    }

    public void getCards() {
        Application.LoadLevel("DrawCard");
        return;
    }
    public void getSpread() {
        Application.LoadLevel("Spread");

        return;
    }
    public void getDeck() {
        Application.LoadLevel("Explore");

        return;
    }
    public void getGuidance() {
        Application.LoadLevel("Guidance");

        return;
    }

    public void VolumeControl() {
        AudioSingleton.instance.VolumeControl();

        return;
    }

}
