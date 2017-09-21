using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Guidance : MonoBehaviour {
    private Text CardText;
    private string MainText;

    void Start() {
        AudioSingleton.instance.AudioSetup();

        return;
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape))
            Home();

        return;
    }

    public void BuyBtn() {
        Application.OpenURL("https://www.thegamecrafter.com/games/arcane-bullshit");
        return;
    }
    public void FacebookBtn() {
        Application.OpenURL("https://www.facebook.com/ArcaneBullshit");
        return;
    }

    public void Home() {
        SceneManager.LoadScene("Menu");
        return;
    }

    public void VolumeControl() {
        AudioSingleton.instance.VolumeControl();

        return;
    }
}
