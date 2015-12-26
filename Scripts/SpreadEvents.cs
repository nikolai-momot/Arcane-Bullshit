using UnityEngine;
using UnityEngine.UI;
using System.Collections; 
using System.Collections.Generic;
using System.IO;
using System;

public class SpreadEvents : MonoBehaviour {
	//Text Components
	public Text CardName, Name, Description;
	private Text[] FortuneTexts;
	
	//Text Asset
	public TextAsset CardText, Fortunes;
	
	private int[] CardIndex;	//Card Index Array

	//Sprites
	private Sprite[] AllCards;	//Card Sprite Array

	//GameObjects
	private GameObject IntroCard, MainCard, BackCard, InfoBtn, BackBtn, HomeBtn;
	private GameObject[] CardsInSpread;
	
	//Image Components
	private Image MainCardImage;
	private Image[] CardImages;
	
	//Rotation Bools
	private bool RotatingRight, RotatingLeft, FrontFacing;
	
	//Rotation Smoothness
	public float smooth = 40;
	
	//CSV Arrays
	private string[] ImageNames, FullNames, Descriptions;

	//Button Positions
	private const int LEFT = -226, CENTER = 13, RIGHT = 245, Y = -613;

	//RectTranform Components
	private RectTransform MainRect, BackRect, InfoBtnRect;
	private List<RectTransform> CardsInRotation;

	//Audio BS
	private Image volumeImage;
	private Sprite imgMuted, imgUnmuted;
	private AudioSource music;
	
	void Start(){
		//Finding Objects
		IntroCard	= GameObject.Find ("IntroCard");
		MainCard	= GameObject.Find ("MainCard");
		BackCard	= GameObject.Find ("BackCard");
		InfoBtn		= GameObject.Find ("Info");
		BackBtn		= GameObject.Find ("Back");
		HomeBtn		= GameObject.Find ("Home");
		
		CardsInSpread = GameObject.FindGameObjectsWithTag("Card");

		GameObject NameObj			= GameObject.Find ( "Name" );
		GameObject DescriptionObj	= GameObject.Find ( "Description" );
		GameObject[] FortuneObjects = GameObject.FindGameObjectsWithTag("Fortune");

		AllCards = Resources.LoadAll<Sprite>("Cards");	//Getting cards from deck
		
		MainCardImage = MainCard.GetComponent<Image> ();

		MainRect	= MainCard.GetComponent<RectTransform> ();
		BackRect	= BackCard.GetComponent<RectTransform> ();
		InfoBtnRect = InfoBtn.GetComponent<RectTransform> ();

		Name		= NameObj.GetComponent<Text>();
		Description = DescriptionObj.GetComponent<Text>();

		RotatingRight	= false;
		RotatingLeft	= false;
		FrontFacing		= true;

		//Initializing arrays
		CardIndex	= new int[CardsInSpread.Length];
		CardImages	= new Image[CardsInSpread.Length];
		
		ImageNames	= new string[AllCards.Length];
		FullNames	= new string[AllCards.Length];
		Descriptions= new string[AllCards.Length];

		FortuneTexts	= new Text[CardsInSpread.Length];	
		CardsInRotation = new List<RectTransform>(CardsInSpread.Length);
		
		int i = 0;
		
		foreach (GameObject FortuneObject in FortuneObjects)
			FortuneTexts [i++] = FortuneObject.GetComponent<Text> ();//Fortune

		readFortunes ();//Filling csv arrays
		showSpread (false);//Hiding small cards
		
		//Hiding main card and showing intro card 
		MainCard.SetActive (false);
		IntroCard.SetActive (true);
		InfoBtn.SetActive (false);
		
		readTSV ();

		AudioSetup ();
	}
	
	void Update(){
		if ( Input.GetKeyDown( KeyCode.Escape ) && MainCard.activeSelf )
			Back();
		else if ( Input.GetKeyDown( KeyCode.Escape ) )
			Home();
			
		if(RotatingLeft)
			TurnToBack();
		else if(RotatingRight)
			TurnToFront();
		
		if(CardsInRotation.Count > 0)
			rotateCards();
		
		return;
	}
	
	public void FrontTap(){
		RotatingLeft = true;
		
		return;
	}
	
	public void BackTap(){
		RotatingRight = true;
		
		return;
	}
	
	public void Home(){
		Application.LoadLevel("Menu");
		
		return;
	}
	
	public void Info(){
		if(this.FrontFacing)
			FrontTap();
		else
			BackTap();
		 
		return;
	}
	
	public void Back(){
		
		if (IntroCard.activeSelf) {
			setSpread();
		} else {
			showSpread(true);
			
			RotatingLeft = false;
			RotatingRight = false;
		}
		
		return;
	}
	
	private void showSpread( bool show){
		//Show or hide small cards
		foreach (GameObject Card in CardsInSpread){
			Card.SetActive (show);
			Card.transform.parent.gameObject.SetActive(show);
		}
		
		MainRect.eulerAngles = new Vector3(0f, 0f, 0f);
		BackRect.eulerAngles = new Vector3(0f, 90f, 0f);
		
		MainCard.SetActive( !show );
		InfoBtn.SetActive( !show );	
		BackBtn.SetActive( !show );
		
		return;
	}
	
	private void TurnToBack(){
			
		if( ( this.MainRect.eulerAngles.y < 87 /*|| this.MainRect.eulerAngles.y == 0*/ ) /*&& this.RotatingLeft*/ ){
			MainRect.Rotate(Vector3.up * Time.deltaTime * smooth);
			//Debug.Log("MainRect.eulerAngles.y: "+this.MainRect.eulerAngles.y);
		}
		else if ( ( BackRect.eulerAngles.y < 180 /*|| this.MainRect.eulerAngles.y <= 90*/ ) /*&& this.RotatingLeft*/ ){
			//Debug.Log("BackRect.eulerAngles.y: "+this.BackRect.eulerAngles.y);
			MainRect.eulerAngles = new Vector3(0f, 90f, 0f);
			BackRect.Rotate(Vector3.up * Time.deltaTime * smooth);
		}
		else{
			//Debug.Log("TurnToBack() Complete");
			RotatingLeft = false;
			FrontFacing = false;
			BackRect.eulerAngles = new Vector3(0f, 180f, 0f);
		}
		
		return;
	}

	private void TurnToFront(){
			
		if( ( BackRect.eulerAngles.y > 93 /*|| this.MainRect.eulerAngles.y == 0*/ ) /*&& this.RotatingRight*/ ){
			BackRect.Rotate( Vector3.down * Time.deltaTime * smooth );
			//Debug.Log("BackRect.eulerAngles.y: "+BackRect.eulerAngles.y);
		}
		else if ( ( /*this.MainRect.eulerAngles.y > 0 ||*/ MainRect.eulerAngles.y < 91 ) /*&& this.RotatingRight*/ ){
			//Debug.Log("MainRect.eulerAngles.y: "+MainRect.eulerAngles.y);
			BackRect.eulerAngles = new Vector3(0f, 90f, 0f);
			MainRect.Rotate( Vector3.down * Time.deltaTime * smooth );
		}
		else{
			//Debug.Log("TurnToFront() Complete");
			RotatingRight = false;
			FrontFacing = true;
			MainRect.eulerAngles = new Vector3(0, 0, 0);
		}
		
		return;
	}
	
	private void rotateCards(){
		
		for(int i = 0; i < CardsInRotation.Count; i++){
			//Debug.Log("Bip["+i+"] = "+CardsInRotation[i].eulerAngles.y);
			
			CardsInRotation[i].Rotate(Vector3.down * Time.deltaTime*smooth);
			
			if( CardsInRotation[i].eulerAngles.y > 180 || CardsInRotation[i].eulerAngles.y == 0){
				//Debug.Log("Beep");
				
				CardsInRotation.Remove(CardsInRotation[i]);				
			} else if( CardsInRotation[i].eulerAngles.y >= 88 && CardsInRotation[i].eulerAngles.y <= 92 ){
				//Debug.Log("Boop");
				
				removeFromRotation(CardsInRotation[i]);
			}
		}
		
		return;
	}
	
	public void removeFromRotation(RectTransform someCard){
		
		foreach( Transform child in someCard.gameObject.transform){
			if(child.name == "Image"){
				Image childImage = child.GetComponent<Image>();
				childImage.enabled = false;
				break;
			}
		}
		
		return;
	}
	
	public void addToRotation(GameObject someCard){
		//Debug.Log("Adding card to rotation");
		
		GameObject someParent = someCard.transform.parent.gameObject;
		
		RectTransform rectTransform = someParent.GetComponent<RectTransform>();
		rectTransform.Rotate(Vector3.right * Time.deltaTime);
		
		if( !CardsInRotation.Contains(rectTransform) )
			CardsInRotation.Add(rectTransform);
		
		return;
	}

	public void setSpread(){

		int cardCount = 0;

		while (cardCount < CardsInSpread.Length) {	//Loop until all 4 cards have an image
			int someNum = UnityEngine.Random.Range (0, AllCards.Length - 1);	//get random number with range of deck size
			//Loop if the card generated is already drawn
			while( Array.IndexOf(CardIndex, someNum) > -1 ) someNum = UnityEngine.Random.Range (0, AllCards.Length - 1); 

			CardIndex[ cardCount++ ] = someNum;	//Increment drawn card count

			//Debug.Log( someNum );
		}


		IntroCard.SetActive (false);	//Hiding intro card
		InfoBtn.SetActive (true);

		showSpread(true);


		int i = 0;
		foreach (GameObject Card in CardsInSpread){
			Card.name = AllCards[CardIndex [i]].name;
			CardImages[i] = Card.GetComponent<Image> ();
			CardImages[i].sprite = AllCards[ CardIndex[i] ];
			i++;
		}

		return;
	}

	public void enlargeCard( GameObject someCard ){
		GameObject someParent = someCard.transform.parent.gameObject;
		CardName.text = "Your "+( someParent.GetComponentInChildren<Text>() ).text;
		 
		Image someImage = someCard.GetComponent<Image> ();
		MainCardImage.sprite = someImage.sprite;	//Change mainCard to selected sprite

		showSpread (false);			//Hide small cards
		
		Name.text = FullNames[ Array.IndexOf( ImageNames, someCard.name ) ];
		Description.text = Descriptions[ Array.IndexOf( ImageNames, someCard.name ) ];

		return;
	}

	public void shrinkCard(){
		Debug.Log ("Main Card Shrinked");

		CardName.text = "";
		showSpread(true);	//show small cards

		return;
	}
	
	private void readTSV(){
		Debug.Log ("Reading TSV");
		//Reading TSV File
		
		int i = 0;	//Line Counter
		
		string[] lines = CardText.text.Split("\n"[0]);
		
		foreach(string line in lines)
		{	
			//Read Line
			if ((line != null) && (line.Length > 0))
			{
				//Split line at tab spaces
				string[] values = line.Split( '\t' );
				//Assigning line values to their respective arrays
				ImageNames[i] 	= values[0];
				FullNames[i]	= values[1];
				Descriptions[i] = values[2];
				
				i++;
			}
		}
		
	}
	
	private void readFortunes(){
		Debug.Log ("Reading Fortunes");
		//Reading TSV File

		string[] lines = Fortunes.text.Split("\n"[0]);

		for(int i = 0; i < FortuneTexts.Length; i++)
		{	
			//Read Line
			if ( (lines[i] != null) && (lines[i].Length > 0) )
			{
				if( i == ( FortuneTexts.Length - 1 ) )
					FortuneTexts[i].text = lines[ lines.Length - 1 ];
				else
					FortuneTexts[i].text = lines[i];
			}
		}
		
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
