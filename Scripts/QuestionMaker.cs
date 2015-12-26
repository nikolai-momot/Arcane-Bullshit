using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;

public class QuestionMaker : MonoBehaviour {
	//Text Component
	public Text exampleQuestion;
	
	//Text Asset
	public TextAsset questionText;

	private string[] QuestionArray;
	private int QuestionCount;

	void Start() {
		readTSV ();
		exampleQuestion.text = QuestionArray[ UnityEngine.Random.Range (0, QuestionCount-1) ];
	}

	private void readTSV(){
		Debug.Log ("Reading Question TSV");

		//Reading TSV File
		string[] lines = questionText.text.Split('\n');

		QuestionArray = new string[lines.Length];
			
		QuestionCount = 0;	//Line Counter

		foreach(string line in lines){
			if ((line != null) && (line.Length > 0))
				QuestionArray[QuestionCount++] = "e.g. "+line;
		}

		return;

	}

}

