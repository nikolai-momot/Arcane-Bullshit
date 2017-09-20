using UnityEngine;
using UnityEngine.UI;
using System.Collections; 
using System.IO;
using System;

public class ExploreCards : MonoBehaviour {
	
	//Text Components
	private Text Name, Description;
	
	//Text Asset
	public TextAsset CardText;

	//GameObjects
	private GameObject MainCard, BackCard, InfoBtn, BackBtn;
	private GameObject CardPrefab;

	//Image Component
	private Image MainCardImage;

	//Sprites
	private Sprite[] AllCards;

	//Rotation Bools
	private bool RotatingRight, RotatingLeft, FrontFacing;
	
	//Rotation Smoothness
	public float smooth = 40, aX = 0, aY = 0, bX = 0, bY = 0;
	
	//CSV Arrays
	private string[] ImageNames;
	private string[] FullNames;
	private string[] Descriptions;
	
	//RectTranform Components
	private RectTransform MainRect, BackRect;

	//Audio BS
	private Image volumeImage;
	private Sprite imgMuted, imgUnmuted;
	private AudioSource music;
	
	public void Start(){
		AllCards = Resources.LoadAll<Sprite>("Cards");
		
		CardPrefab = GameObject.Find ("ACard");
		MainCard = GameObject.Find ("MainCard");
		BackCard = GameObject.Find ("BackCard");
		BackBtn =  GameObject.Find ("Back");
		InfoBtn = GameObject.Find ("Info");
		
		GameObject NameObj = GameObject.Find ( "Name" );
		GameObject DescriptionObj = GameObject.Find ( "Description" );
		
		Name = NameObj.GetComponent<Text>();
		Description = DescriptionObj.GetComponent<Text>();
		
		MainRect = MainCard.GetComponent<RectTransform>();
		BackRect = BackCard.GetComponent<RectTransform>();
		
		MainCardImage = MainCard.GetComponent<Image>();
		
		this.RotatingLeft =  false;
		this.RotatingRight = false;
		this.FrontFacing = true;
	
		ImageNames = new string[AllCards.Length];
		FullNames = new string[AllCards.Length];
		Descriptions = new string[AllCards.Length];

		generateThumbnails ();
		
		readTSV ();
	
		//Hiding small cards
		showCards (true);
		
		

		return;
	}

	public void Update(){
		if ( Input.GetKeyDown( KeyCode.Escape ) && MainCard.activeSelf )
			Back();
		else if ( Input.GetKeyDown( KeyCode.Escape ) )
			Home();
			
		if(this.RotatingLeft)
			TurnToBack();
			
		if(this.RotatingRight)
			TurnToFront();
	}
	
	public void Back(){
		
		showCards(true);
		
		this.RotatingLeft = false;
		
		
		this.RotatingRight = false;
		
		return;
	}
	
	private void TurnToBack(){
			
		if( ( this.MainRect.eulerAngles.y < 87 /*|| this.MainRect.eulerAngles.y == 0*/ ) /*&& this.RotatingLeft*/ ){
			MainRect.Rotate(Vector3.up * Time.deltaTime * this.smooth);
			Debug.Log("MainRect.eulerAngles.y: "+this.MainRect.eulerAngles.y);
		}
		else if ( ( this.BackRect.eulerAngles.y < 180 /*|| this.MainRect.eulerAngles.y <= 90*/ ) /*&& this.RotatingLeft*/ ){
			Debug.Log("BackRect.eulerAngles.y: "+this.BackRect.eulerAngles.y);
			this.MainRect.eulerAngles = new Vector3(0f, 90f, 0f);
			this.BackRect.Rotate(Vector3.up * Time.deltaTime * this.smooth);
		}
		else{
			Debug.Log("TurnToBack() Complete");
			this.RotatingLeft = false;
			this.FrontFacing = false;
			this.BackRect.eulerAngles = new Vector3(0f, 180f, 0f);
		}
		
		return;
	}

	private void TurnToFront(){
			
		if( ( this.BackRect.eulerAngles.y > 93 /*|| this.MainRect.eulerAngles.y == 0*/ ) /*&& this.RotatingRight*/ ){
			BackRect.Rotate( Vector3.down * Time.deltaTime * this.smooth );
			Debug.Log("BackRect.eulerAngles.y: "+this.BackRect.eulerAngles.y);
		}
		else if ( ( /*this.MainRect.eulerAngles.y > 0 ||*/ this.MainRect.eulerAngles.y < 91 ) /*&& this.RotatingRight*/ ){
			Debug.Log("MainRect.eulerAngles.y: "+this.MainRect.eulerAngles.y);
			this.BackRect.eulerAngles = new Vector3(0f, 90f, 0f);
			this.MainRect.Rotate( Vector3.down * Time.deltaTime * this.smooth );
		}
		else{
			Debug.Log("TurnToFront() Complete");
			this.RotatingRight = false;
			this.FrontFacing = true;
			this.MainRect.eulerAngles = new Vector3(0, 0, 0);
		}
		
		return;
	}


	public void FrontTap(){
		this.RotatingLeft = true;
		
		return;
	}
	
	public void BackTap(){
		this.RotatingRight = true;
		
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
	
	//Set show to false to hide cards, true to show cards
	private void showCards( bool show ){
		foreach( Sprite aCard in AllCards ){
			GameObject someCard = GameObject.Find( aCard.name );
			Image someCardImage = someCard.GetComponent<Image>();
			someCardImage.enabled = show;
		}
		
		this.MainRect.eulerAngles = new Vector3(0f, 0f, 0f);
		this.BackRect.eulerAngles = new Vector3(0f, 90f, 0f);
		
		BackBtn.SetActive(!show);
		InfoBtn.SetActive(!show);
		MainCard.SetActive (!show);
		
		return;
	}

	public void enlargeCard( GameObject someCard ){
		Debug.Log (someCard.name);

		showCards ( false );

		Image someCardImage = someCard.GetComponent<Image> ();
		MainCardImage.sprite = someCardImage.sprite;
		
		Name.text = FullNames[ Array.IndexOf( ImageNames, someCard.name ) ];
		Description.text = Descriptions[ Array.IndexOf( ImageNames, someCard.name ) ];

		return;
	}

	//Switch between card image and description
	public void flipMainCard(){
		Debug.Log ("Main Card Clicked");
	
		MainCard.SetActive(false);	//Hide main card
		showCards( true );			//Show tiny cards
		
		return;
	}
	
	private void generateThumbnails(){
		int ItemCount = AllCards.Length;//Count how many cards there are
		
		GameObject container = GameObject.Find ("CardContainer");
		RectTransform containerRectTransform = container.GetComponent<RectTransform>();
		RectTransform rowRectTransform = CardPrefab.GetComponent<RectTransform>();
		
		int ColumnCount = 3;
		
		//calculate the width and height of each child item.
		float width = (containerRectTransform.rect.width / ColumnCount);
		float ratio = width / rowRectTransform.rect.width;
		float height = rowRectTransform.rect.height * ratio;
		int rowCount = ItemCount / ColumnCount;
		
		if (ItemCount % rowCount > 0)
			rowCount++;
		
		//adjust the height of the container so that it will just barely fit all its children
		float scrollHeight = height * rowCount * 1.74f;
		containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);//****************
		containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);
		
		//containerRectTransform.offsetMin = 14000f;
		
		int j = 0;
		
		for (int i = 0; i < ItemCount; i++)
		{
			//this is used instead of a double for loop because itemCount may not fit perfectly into the rows/columns
			if (i % ColumnCount == 0)
				j++;
			
			//create a new item, name it, and set the parent
			GameObject newItem = Instantiate(CardPrefab) as GameObject;
			newItem.name = AllCards[i].name;
			newItem.transform.SetParent(container.transform);
			
			//move and size the new item
			RectTransform rectTransform = newItem.GetComponent<RectTransform>();
			
			float x = -containerRectTransform.rect.width / 2 + width * (i % ColumnCount) * bX;
			float y = containerRectTransform.rect.height / 2 - height * j * bY;
			rectTransform.offsetMin = new Vector2(x, y);
			
			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y + height;
			
			rectTransform.offsetMax = new Vector2(x, y);
			
			rectTransform.localScale = new Vector3( 1f, 1f, 1f );	//Setting scale to 1
			
			rectTransform.sizeDelta = new Vector2(397f/aX, 698f/aY);
			
			//Setting newItem image to card image from deck
			Image newImage = newItem.GetComponent<Image>(); 
			newImage.sprite = AllCards[ i ];
		}
		
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
