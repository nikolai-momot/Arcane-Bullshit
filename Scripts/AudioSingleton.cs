using UnityEngine;
using System.Collections;

public class AudioSingleton : MonoBehaviour {
    
    private static AudioClip openingTrack, loopTrack, creditTrack;
	private AudioSource audioSource;
    private static AudioSingleton instance = null;
	 
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
     
     void Start(){
		GameObject go = GameObject.Find ("Music");
		audioSource = go.GetComponent<AudioSource>();

		//openingTrack = Resources.Load<AudioClip> ("Music/opening theme music");
		loopTrack = Resources.Load<AudioClip> ("Music/card interaction 1");
		creditTrack = Resources.Load<AudioClip>("Music/credits guidance");

		return;
     }
     
	void Update() {
		if (!audioSource.isPlaying) {
			audioSource.clip = loopTrack;
			audioSource.Play();
		}

		return;
	}
} 
