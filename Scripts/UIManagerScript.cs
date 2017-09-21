using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerScript : MonoBehaviour {

    public void Start() {
        AudioSingleton.instance.AudioSetup();

        return;
    }

    public void Update() {
        //Checking platform and scene *** Change when compiling to iOS ***
        if((Application.platform == RuntimePlatform.Android) && (SceneManager.GetActiveScene().name != "Menu")) {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                SceneManager.LoadScene("Menu");

                return;
            }
        } else if(SceneManager.GetActiveScene().name == "Menu") {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                Application.Quit();

                return;
            }
        }
    }

    public void getCards() {
        SceneManager.LoadScene("DrawCard");

        return;
    }
    public void getSpread() {
        SceneManager.LoadScene("Spread");

        return;
    }
    public void getDeck() {
        SceneManager.LoadScene("Explore");

        return;
    }
    public void getGuidance() {
        SceneManager.LoadScene("Guidance");

        return;
    }

    public void VolumeControl() {
        AudioSingleton.instance.VolumeControl();

        return;
    }

}
