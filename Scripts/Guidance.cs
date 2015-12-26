using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Guidance : MonoBehaviour {
	private Image volumeImage;
	private Sprite imgMuted, imgUnmuted;
	private AudioSource music;

	private Text CardText;
	private string MainText;

	// Use this for initialization
	void Start () {
		AudioSetup ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape))
			Home ();

		return;
	}
	
	public void BuyBtn()
	{
		Application.OpenURL("https://www.thegamecrafter.com/games/arcane-bullshit");
		return;
	}
	public void FacebookBtn()
	{
		Application.OpenURL("https://www.facebook.com/ArcaneBullshit");
		return;
	}

	public void Home(){
		Application.LoadLevel("Menu");

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
