using UnityEngine;
using UnityEngine.UI;

public class AudioSingleton : MonoBehaviour {

    private static AudioClip openingTrack, loopTrack, creditTrack;
    private AudioSource audioSource;

    public static AudioSingleton instance = null;

    //Audio BS
    private Image volumeImage;
    private Sprite imgMuted, imgUnmuted;
    private AudioSource music;
    private bool muted = true;

    public static AudioSingleton Instance {
        get { return instance; }
    }

    void Awake() {
        //Keep class in existance
        if(instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        return;
    }

    void Start() {
        GameObject go = GameObject.Find("Music");
        audioSource = go.GetComponent<AudioSource>();

        //openingTrack = Resources.Load<AudioClip> ("Music/opening theme music");
        loopTrack = Resources.Load<AudioClip>("Music/card interaction 1");
        creditTrack = Resources.Load<AudioClip>("Music/credits guidance");

        /*//Check if muted (Unsuccessful)
        muted = (PlayerPrefs.GetInt("Mute") == 0);
        VolumeControl();*/

        //Get audio button images
        imgMuted = Resources.Load<Sprite>("Buttons/SoundOn");
        imgUnmuted = Resources.Load<Sprite>("Buttons/SoundOff");

        return;
    }

    void Update() {
        if(!audioSource.isPlaying) {
            audioSource.clip = loopTrack;
            audioSource.Play();
        }

        return;
    }

    public void AudioSetup() {
        //Get objects
        GameObject volumeBtn = GameObject.Find("VolumeBtn");
        volumeImage = volumeBtn.GetComponent<Image>();

        GameObject volumeObj = GameObject.Find("Music");
        music = volumeObj.GetComponent<AudioSource>();

        if(music.volume.Equals(0f)) {
            //Debug.Log("Ch-ch-ch-ch-changes");
            volumeImage.sprite = imgUnmuted;
            muted = false;
        }

        return;
    }

    public void VolumeControl() {
        //Set mute button values
        muted = !muted;
        if(muted) {
            volumeImage.sprite = imgMuted;
            music.volume = 1f;
        } else {
            volumeImage.sprite = imgUnmuted;
            music.volume = 0f;
        }

        /*//Save mute value (Unsuccessful)
        PlayerPrefs.SetInt("Mute", (muted ? 1 : 0));
        PlayerPrefs.Save();*/

        return;
    }
}
