using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardFlipper : MonoBehaviour {

    //Text Documents
    public TextAsset CardText;

    //Card Index
    private int CardIndex;

    //Card Sprites
    private Sprite[] AllCards;

    //GameObjects
    private GameObject MainCard, BackCard;

    //RectTranform Components
    private RectTransform MainRect, BackRect;

    //Image Components
    private Image MainCardImage;

    //Text Components
    private Text CardName, CardDescription;

    //CSV Arrays
    private string[] ImageNames, FullNames, Descriptions;

    //Rotation Bools
    private bool RotatingRight, RotatingLeft, FrontFacing;

    //Rotation Smoothness
    public float smooth = 40;

    void Start() {
        this.AllCards = Resources.LoadAll<Sprite>("Cards");
        this.MainCard = GameObject.Find("MainCard");
        this.BackCard = GameObject.Find("BackCard");
        //this.InfoBtn = GameObject.Find ("Info");

        this.CardName = (GameObject.Find("Name")).GetComponent<Text>();
        this.CardDescription = (GameObject.Find("Description")).GetComponent<Text>();

        this.MainCardImage = this.MainCard.GetComponent<Image>();

        this.MainRect = this.MainCard.GetComponent<RectTransform>();
        this.BackRect = this.BackCard.GetComponent<RectTransform>();

        this.RotatingRight = false;
        this.RotatingLeft = false;
        this.FrontFacing = true;

        this.CardIndex = UnityEngine.Random.Range(0, this.AllCards.Length - 1);

        this.MainCardImage.sprite = this.AllCards[this.CardIndex];

        ImageNames = new string[AllCards.Length];
        FullNames = new string[AllCards.Length];
        Descriptions = new string[AllCards.Length];

        //Filling TSV arrays
        readTSV();

        CardName.text = FullNames[CardIndex];
        CardDescription.text = Descriptions[CardIndex];

        AudioSingleton.instance.AudioSetup();

        return;
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape))
            Home();

        if(this.RotatingLeft)
            TurnToBack();

        if(this.RotatingRight)
            TurnToFront();

        //For testing
        if(Input.GetKeyDown(KeyCode.RightArrow))
            Next();

        return;
    }

    private void TurnToBack() {

        if((this.MainRect.eulerAngles.y < 87 /*|| this.MainRect.eulerAngles.y == 0*/ ) /*&& this.RotatingLeft*/ ) {
            MainRect.Rotate(Vector3.up * Time.deltaTime * this.smooth);
            Debug.Log("MainRect.eulerAngles.y: " + this.MainRect.eulerAngles.y);
        } else if((this.BackRect.eulerAngles.y < 180 /*|| this.MainRect.eulerAngles.y <= 90*/ ) /*&& this.RotatingLeft*/ ) {
            Debug.Log("BackRect.eulerAngles.y: " + this.BackRect.eulerAngles.y);
            this.MainRect.eulerAngles = new Vector3(0f, 90f, 0f);
            this.BackRect.Rotate(Vector3.up * Time.deltaTime * this.smooth);
        } else {
            Debug.Log("TurnToBack() Complete");
            this.RotatingLeft = false;
            this.FrontFacing = false;
            this.BackRect.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        return;
    }

    private void TurnToFront() {

        if((this.BackRect.eulerAngles.y > 93 /*|| this.MainRect.eulerAngles.y == 0*/ ) /*&& this.RotatingRight*/ ) {
            BackRect.Rotate(Vector3.down * Time.deltaTime * this.smooth);
            Debug.Log("BackRect.eulerAngles.y: " + this.BackRect.eulerAngles.y);
        } else if(( /*this.MainRect.eulerAngles.y > 0 ||*/ this.MainRect.eulerAngles.y < 91) /*&& this.RotatingRight*/ ) {
            Debug.Log("MainRect.eulerAngles.y: " + this.MainRect.eulerAngles.y);
            this.BackRect.eulerAngles = new Vector3(0f, 90f, 0f);
            this.MainRect.Rotate(Vector3.down * Time.deltaTime * this.smooth);
        } else {
            Debug.Log("TurnToFront() Complete");
            this.RotatingRight = false;
            this.FrontFacing = true;
            this.MainRect.eulerAngles = new Vector3(0, 0, 0);
        }

        return;
    }


    public void FrontTap() {
        this.RotatingLeft = true;

        return;
    }

    public void BackTap() {
        this.RotatingRight = true;

        return;
    }

    public void Home() {
        SceneManager.LoadScene("Menu");

        return;
    }

    public void Info() {
        if(this.FrontFacing)
            FrontTap();
        else
            BackTap();

        return;
    }

    public void Next() {
        this.CardIndex = UnityEngine.Random.Range(0, this.AllCards.Length - 1);

        this.MainCardImage.sprite = this.AllCards[CardIndex];

        CardName.text = FullNames[CardIndex];
        CardDescription.text = Descriptions[CardIndex];

        if(!this.FrontFacing) {
            this.FrontFacing = true;
            this.MainRect.eulerAngles = new Vector3(0, 0, 0);
            this.BackRect.eulerAngles = new Vector3(0f, 90f, 0f);
        }

        return;
    }

    private void readTSV() {
        Debug.Log("Reading TSV");

        int i = 0;  //Line Counter

        string[] lines = CardText.text.Split("\n"[0]);

        foreach(string line in lines) {
            //Read Line
            if((line != null) && (line.Length > 0)) {
                //Split line at tab spaces
                string[] values = line.Split('\t');

                //Assigning line values to their respective arrays
                ImageNames[i] = values[0];
                FullNames[i] = values[1];
                Descriptions[i] = values[2];
                i++;
            }
        }

        return;
    }

    public void VolumeControl() {
        AudioSingleton.instance.VolumeControl();

        return;
    }
}
