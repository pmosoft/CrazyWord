using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBlank : MonoBehaviour {
	
	public GameControl gameControl;
	public GameBasic gameBasic;
	public GameTexture gameTexture;
	
	public GameState gameState;
	public GameComponent gameComponent;


	public LevelPass levelPass;

	public GameObject LetterPrefab;

	//------------------------------
	// Time & Count
	//------------------------------
	float inCorrectPeneltyTime = 2f;
	int consonantMaxLetterLen = 7;



	int correctPos = 0;
	public void GameBlankStart(){
		
		Debug.Log("GameBlank"); 
		//---------------------------------------------
		// Setting State
		//---------------------------------------------
		gameState.StateGameLevel("blank");
		gameState.StateBlankGamePanel();
		
		//---------------------------------------------
		// Init variable
		//---------------------------------------------
		gameTexture.SetTurnTexture();
        levelPass.LevelPassButtonStart();

		correctPos = 0;
		blankPos.Clear();
		
		//g.col = gameComponent.gameTextureUISprite.color;
		//gameComponent.gameTextureUISprite.color = new Color(g.col.r,g.col.g,g.col.b,0.4f);
		
		gameComponent.gameIconUISprite.spriteName = "blankgameicons";
		//gameComponent.gameIconUISprite.MakePixelPerfect();

		gameComponent.wordCntUILabel.text = g.texName.Length.ToString();

        Color frameCol = g.GetRandomColor();
		gameComponent.gameTextureUISprite.color = frameCol;
        gameComponent.pictureFrameUISprite.color = frameCol;


		//---------------------------------------------
		// Setting BlankWord in clickWordUILabel
		//---------------------------------------------
		SetBlankWord();
		
		//---------------------------------------------
		// LimitTime
		//---------------------------------------------
		g.limitTime = g.limitBaseTime * g.texName.Length;
		gameComponent.limitUILabel.text = g.limitTime.ToString();
		gameComponent.limitLineUILabel.text = g.limitTime.ToString();
		
		StartCoroutine(gameControl.LimitTime());

		
		//---------------------------------------------
		// Instantiate Tile
		//---------------------------------------------
		gameBasic.InitTile(5,5,LetterPrefab);
		
		//---------------------------------------------
		// Setting Letter
		//---------------------------------------------
		gameBasic.SetLetter();
	}
	
	//----------------------------------------------------------
	//                          Core
	//----------------------------------------------------------
	string blankAlpha = "";
	string blankTexName = "";
	string tempTexName = "";
	List<int> blankPos = new List<int>(); 
	public void SetBlankWord() {

		//Debug.Log("SetBlankWord="+g.CrazyWordLanguage);
		if     (g.CrazyWordLanguage=="English"){ SetBlankWordEnglish(); }
		else if(g.CrazyWordLanguage=="Japanese"){ SetBlankWordJapan();   }
	}

	int blankCnt,blankIdx=0;
	void SetBlankWordJapan(){

		// blankCnt : len 123=1,456=2,789=3,101112=4
		blankCnt = ((g.texName.Length-1)/3)+1;

        //Debug.Log("blankCnt=" + blankCnt);
        //Debug.Log("g.texName=" + g.texName);

		// add blankPos while blankPos.Count = blankCnt
		while(blankPos.Count<blankCnt) {
			blankIdx = Random.Range(0,g.texName.Length);
            if (!blankPos.Contains(blankIdx)) { blankPos.Add(blankIdx); Debug.Log("blankIdx=" + blankIdx); }
		}

        blankPos.Sort();

		blankTexName = g.texName;

		// replace g.texName
		for(int i=0;i<blankPos.Count;i++) {
			//Debug.Log( blankPos[i] );
			//Debug.Log( "blankTexName1="+blankTexName );
            //blankTexName = blankTexName.Replace(g.texName[blankPos[i]], '□');
            blankTexName  = blankTexName.Substring(0,blankPos[i]) + "□" + blankTexName.Substring(blankPos[i]+1,blankTexName.Length-blankPos[i]-1 );
			//Debug.Log( "blankTexName2="+blankTexName );

		}

		gameComponent.clickWordUILabel.text = blankTexName;
		gameComponent.wordCntUILabel.text = (blankPos.Count).ToString();
		
	}

	string vowelAlpha = "AEIOUYW";
	string consonantAlpha = "BCDFGHJKLMNPQRSTVWXZ";
	void SetBlankWordEnglish(){
		//Debug.Log("g.texName="+g.texName);
		
		// Random.Range : 0,1
		blankAlpha = Random.Range(0,2) == 0 ? consonantAlpha : vowelAlpha; 
		if(g.texName.Length <= consonantMaxLetterLen) blankAlpha = consonantAlpha;
		
		for(int i=0;i<blankAlpha.Length;i++) {
			for(int j=0;j<g.texName.Length;j++) {
				if( blankAlpha[i] == g.texName[j] )
					blankPos.Add(j);
			}
		}
		
		blankPos.Sort();
		
		blankTexName = g.texName;
		
		for(int i=0;i<blankPos.Count;i++) {
			//Debug.Log( blankPos[i] );
			blankTexName = blankTexName.Replace( g.texName[ blankPos[i] ],'♡');
		}
		//Debug.Log("blankTexName="+blankTexName);
		
		//Debug.Log( FillBlank('A') );
		//Debug.Log( FillBlank('C') );
		gameComponent.clickWordUILabel.text = blankTexName;
		gameComponent.wordCntUILabel.text = (blankPos.Count).ToString();

	}


	private char touchLetter;
	public IEnumerator FillBlank() {

        //Debug.Log("touchLetter=" + touchLetter);
        //Debug.Log("gameComponent.letterUILabel.text=" + gameComponent.letterUILabel.text);
        //Debug.Log("g.texName=" + g.texName);
        //Debug.Log("blankPos[correctPos]=" + blankPos[correctPos] + " correctPos=" + correctPos);

		touchLetter = gameComponent.letterUILabel.text[0];
		// correct
		if( touchLetter == g.texName [ blankPos[correctPos] ] ) {
			
			tempTexName = g.texName.Substring(0,blankPos[correctPos]+1);
			blankTexName = tempTexName + blankTexName.Substring(blankPos[correctPos]+1,blankTexName.Length-blankPos[correctPos]-1);
			correctPos++;
			
			gameComponent.clickWordUILabel.text = blankTexName;
			
			// incorrect
		} else {

			// Vibrate
			if(g.isVibrate) Handheld.Vibrate();

			gameState.BlankInCorrect.SetActive (true);
			gameComponent.EnableGridBoxCollider(false);

			yield return new WaitForSeconds( inCorrectPeneltyTime );
			
			gameState.BlankInCorrect.SetActive (false);
			gameComponent.EnableGridBoxCollider(true);

			//Debug.Log( g.texName [ blankPos[correctPos] ]  + " : " + touchLetter );
		}
		
	}

	//----------------------------------------------------------
	//                       Event
	//----------------------------------------------------------
	public void EventLetterOnRelease(){
		gameBasic.TouchLetterEffect ();
		
		//------------------------
		// Fill Blank
		//------------------------
		StartCoroutine ( FillBlank() );
		
		//------------------------
		// wordCnt
		//------------------------ 
		if(int.Parse(gameComponent.wordCntUILabel.text) >= 1) 
			gameComponent.wordCntUILabel.text = ( blankPos.Count - correctPos ).ToString();
		
		//------------------------
		// Checking TurnClear
		//------------------------
		if (blankPos.Count == correctPos) {
			StartCoroutine ( EventDelayStart() );
		}
	}
	
	IEnumerator EventDelayStart(){
        g.isGameTurning = false;
		yield return new WaitForSeconds(0.5f);
		gameControl.LevelPassStart("clear");
	}
	
	
}
