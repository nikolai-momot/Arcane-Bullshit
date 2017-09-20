using UnityEngine;
using System.Collections;

public class AudioSingleton : MonoBehaviour {

    private static AudioClip openingTrack, loopTrack, creditTrack;
    private AudioSource audioSource;
    private static AudioSingleton instance = null;

    //Audio BS
    private Image volumeImage;
    private Sprite imgMuted, imgUnmuted;
    private AudioSource music;

    public static AudioSingleton Instance {
        get { return instance; }
    }

    void Awake() {
        if (instance != null && instance != this) {
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

        AudioSetup();

        return;
    }

    void Update() {
        if (!audioSource.isPlaying) {
            audioSource.clip = loopTrack;
            audioSource.Play();
        }

        return;
    }

    public void AudioSetup() {
        GameObject volumeBtn = GameObject.Find("VolumeBtn");
        volumeImage = volumeBtn.GetComponent<Image>();

        imgMuted = Resources.Load<Sprite>("Buttons/SoundOn");
        imgUnmuted = Resources.Load<Sprite>("Buttons/SoundOff");

        GameObject volumeObj = GameObject.Find("Music");
        music = volumeObj.GetComponent<AudioSource>();

        if (music.volume.Equals(0f)) {
            //Debug.Log("Ch-ch-ch-ch-changes");
            volumeImage.sprite = imgUnmuted;
        }

        return;
    }

    public void VolumeControl() {
        if (music.volume.Equals(0f)) {
            volumeImage.sprite = imgMuted;
            music.volume = 1f;
        } else {
            volumeImage.sprite = imgUnmuted;
            music.volume = 0f;
        }

        return;
    }
}
