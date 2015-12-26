using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour {

	private Image volumeImage;
	private Sprite imgMuted, imgUnmuted;
	private AudioSource music;
	
	public void Start(){
		AudioSetup ();

		return;
	}

	public void Update(){
		//Checking platform and scene *** Change when compiling to iOS ***
		if ((Application.platform == RuntimePlatform.Android) && (Application.loadedLevelName != "Menu")) {
				if (Input.GetKeyDown (KeyCode.Escape)) {
					Application.LoadLevel ("Menu");

					return;
				}
		} else if (Application.loadedLevelName == "Menu") {
				if (Input.GetKeyDown (KeyCode.Escape)) {
					Application.Quit ();

					return;
				}
		}
	}

	public void getCards()
	{
		Application.LoadLevel("DrawCard");
		
		return;
	}
	public void getSpread()
	{
		Application.LoadLevel("Spread");
		
		return;
	}
	public void getDeck()
	{
		Application.LoadLevel("Explore");
		
		return;
	}
	public void getGuidance()
	{
		Application.LoadLevel("Guidance");
		
		return;
	}

	private void AudioSetup(){		
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
	
	public void VolumeControl(){
		if(music.volume.Equals(0f)){
			volumeImage.sprite = imgMuted;
			music.volume = 1f;
		} else {
			volumeImage.sprite = imgUnmuted;
			music.volume = 0f;
		}
		
		return;
	}

}
