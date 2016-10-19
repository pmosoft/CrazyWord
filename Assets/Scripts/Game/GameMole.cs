using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMole : MonoBehaviour {

	public GameControl gameControl;
	public GameTexture gameTexture;
	public GameBasic gameBasic;

	public GameState gameState;
	public GameComponent gameComponent;

	public LevelPass levelPass;

	public EventCommon ev;

	public GameObject LetterPrefab;

	float moleTweenTime = 1.0f; //1.3f
	int moleShowTimeCnt = 2; // 2

	int aniCnt = 0;
	public void GameMoleStart(){
		
		Debug.Log("GameMoleStart");
		//---------------------------------------------
		// Setting State
		//---------------------------------------------
		gameState.StateGameLevel("mole");
		gameState.StateMoleGamePanel();

		//---------------------------------------------
		// Init variable
		//---------------------------------------------
		gameTexture.SetTurnTexture();
   		levelPass.LevelPassButtonStart();

		aniCnt = 0;
		g.correctLetterIdx = 0;
		//gameComponent.clickWordUILabel.text = g.texShowName;
		gameComponent.clickWordUILabel.text = "";
		//gameComponent.clickWordUILabel.text += "[ff0000]"+gameComponent.letterUILabel.text+"[-]";

		g.col = gameComponent.gameTextureUISprite.color;
		//gameComponent.gameUITexture.color = new Color(g.col.r,g.col.g,g.col.b,0.4f);
		
        Color frameCol = g.GetRandomColor();
		gameComponent.gameTextureUISprite.color = frameCol;
        gameComponent.pictureFrameUISprite.color = frameCol;

		gameComponent.gameIconUISprite.spriteName = "molegameicons";
		//gameComponent.gameIconUISprite.MakePixelPerfect();

		//---------------------------------------------
		// LimitTime
		//---------------------------------------------
		g.limitTime = g.limitBaseTime * g.texShowName.Length;
		gameComponent.limitUILabel.text = g.limitTime.ToString();
		gameComponent.limitLineUILabel.text = g.limitTime.ToString();
		
		gameControl.LimitTimeStart();

		//---------------------------------------------
		// Instantiate Tile
		//---------------------------------------------
		gameBasic.InitTile(2,2,LetterPrefab);

		//---------------------------------------------
		// Setting Letter
		//---------------------------------------------
		StartCoroutine ( RoutionAni() );

	}

	Animator letterCnt;
	int correctMaxCnt = 3; // 2
	int incorrectMaxCnt = 1; // 2
	int correctCnt = 0;
	int inCorrectCnt = 0;
	bool isShowCorrect = false;
	int showWidthNum , showHeightNum;
	public void SetMoleLetter(bool tf){

		showWidthNum  = Random.Range(0,g.tileWidthCnt);  // 0..9
		showHeightNum = Random.Range(0,g.tileHeightCnt); // 0..9

		for (int y = 0; y < g.tileHeightCnt; y++) {
			for (int x = 0; x < g.tileWidthCnt; x++) {

				//--------------
				// Init Prefab
				//--------------
				gameComponent.letterPrefab = gameState.Grid.transform.Find("Letter"+x+y);
				if(gameComponent.letterPrefab==null) break;
				gameComponent.SetLetterPrefab( gameComponent.letterPrefab );

				//--------------
				// Event key
				//--------------
				EventDelegate eventClick = new EventDelegate(this, "EventLetterOnPress");
				EventDelegate.Parameter param = new EventDelegate.Parameter();
				param.obj = gameComponent.letterPrefab;
				param.expectedType = typeof(Transform);
				eventClick.parameters[0] = param;
				EventDelegate.Add (gameComponent.letterUIEventTrigger.onRelease,eventClick);

				//--------------
				// letterAnimator
				//--------------
				//letterAnimator = gameComponent.letterLabel.GetComponent<letterAnimator>();
				//letterAnimator.SetBool("play",tf);

				//--------------
				// Letter
				//--------------

				// mole out
				if ( aniCnt == moleShowTimeCnt && x == showWidthNum && y == showHeightNum ) {
					MoleWordExcluded();
					// 1..3
					isShowCorrect = (int) Random.Range(1,8) == 1 ? false : true; // 0,1  4
					//Debug.Log("isShowCorrect1="+isShowCorrect);
//					if(correctCnt == correctMaxCnt) {
//						Debug.Log("correctCnt1="+correctCnt);
//
//						isShowCorrect = false;
//						correctCnt = 0; inCorrectCnt = 0;
//					}
					if(inCorrectCnt == incorrectMaxCnt) {
						//Debug.Log("correctCnt2="+correctCnt);
						isShowCorrect = true;
						correctCnt = 0; inCorrectCnt = 0;
					}
					//Debug.Log("isShowCorrect2="+isShowCorrect);

					// show correct letter
					if( isShowCorrect ) {
						correctCnt++;
						gameComponent.letterUILabel.text = g.texShowName[g.correctLetterIdx].ToString();
						//Debug.Log("isShowCorrect letter="+gameComponent.letterUILabel.text);
						gameComponent.letterUILabel.color = new Color32(80,84,255,255); //blue
						gameComponent.letterUILabel.effectColor = Color.black;
					// show incorrect letter
					} else {
						inCorrectCnt++;
						gameComponent.letterUILabel.text = g.letterExcluded[0].ToString();
						gameComponent.letterUILabel.color = new Color32(255,99,99,255); //red
						//Debug.Log("isInCorrect letter="+gameComponent.letterUILabel.text);
						gameComponent.letterUILabel.effectColor = Color.black;
					}
				// mole in
				} else {

					// when mole game start, show incorrect letter
					if( g.correctLetterIdx == 0 )
						gameComponent.letterUILabel.text = g.letterExcluded[0].ToString();
					// show previous letter
					else 
						gameComponent.letterUILabel.text = g.texShowName[g.correctLetterIdx-1].ToString();

					gameComponent.letterUILabel.color = g.letterColor;
					gameComponent.letterUILabel.effectColor = Color.white;
				}

				if(tf) {
					gameComponent.letterScale.from = Vector3.one * 0f;
					gameComponent.letterScale.to = Vector3.one * 7f;
					gameComponent.letterScale.duration = moleTweenTime;
					gameComponent.letterScale.enabled = true;
					//gameComponent.letterScale.ResetToBeginning();
				} else {
					gameComponent.letterScale.enabled = false;
					gameComponent.letterScale.ResetToBeginning();
				}

			}	
		}

	}

	IEnumerator RoutionAni(){

		aniCnt++;

		SetMoleLetter(true) ;
		yield return new WaitForSeconds(moleTweenTime);
		if(aniCnt == moleShowTimeCnt) aniCnt = 0;
		SetMoleLetter(false) ;

		//if(!g.isLevelPassing) StartCoroutine ( RoutionAni() );
		StartCoroutine ( RoutionAni() );
	}



	//----------------------------------------------------------
	//                       Event
	//----------------------------------------------------------
	public void EventLetterOnPress(Transform letterClone){
			
		//Debug.Log("EventLetterOnPress");
		gameComponent.SetLetterPrefab(letterClone);

		// Correct
		if(aniCnt == moleShowTimeCnt && gameComponent.letterUILabel.text == g.texShowName[g.correctLetterIdx].ToString() ) {

			// TTS
			gameBasic.PlayTTS(g.texShowName[g.correctLetterIdx].ToString());

			gameComponent.clickWordUILabel.text += g.texName[g.correctLetterIdx].ToString();
			//gameBasic.CorrectWordColor(g.correctLetterIdx);
			g.correctLetterIdx++;
		// InCorrect
		} else if(g.correctLetterIdx > 0 && aniCnt == moleShowTimeCnt && gameComponent.letterUILabel.text != g.texShowName[g.correctLetterIdx].ToString() ) {
			g.correctLetterIdx--;
			gameComponent.clickWordUILabel.text = gameComponent.clickWordUILabel.text.Remove(gameComponent.clickWordUILabel.text.Length-1);
			//gameBasic.CorrectWordColor(g.correctLetterIdx-1);
			StartCoroutine ( InCorrect() );
		} else {
			//Debug.Log("incorrect");
			StartCoroutine ( InCorrect() );
		}

		//------------------------
		// Checking TurnClear
		//------------------------
		if (g.correctLetterIdx == g.texShowName.Length) {
			//StopCoroutine("RoutionAni");
			gameControl.LevelPassStart("clear");
		}

	}

	IEnumerator InCorrect(){

		// Vibrate
		if(g.isVibrate) Handheld.Vibrate();

		gameState.MoleInCorrect.SetActive (true);
		gameComponent.EnableGridBoxCollider(false);

		gameComponent.moleInCorrectScale.enabled = true;

		yield return new WaitForSeconds(1.2f);

		gameState.MoleInCorrect.SetActive (false);
		gameComponent.EnableGridBoxCollider(true);
		gameComponent.moleInCorrectScale.enabled = false;
		gameComponent.moleInCorrectScale.ResetToBeginning();

	}

	public void MoleWordExcluded() {
		
		g.letterExcluded = g.letterAlphabet;
		
		string letterExcludedTemp1 = g.letterAlphabet;
		string letterExcludedTemp2 = g.letterAlphabet;
		int letterIndex;
		//Debug.Log("g.texShowName="+g.texShowName);
		for (int i = 0; i < g.texShowName.Length; i++) {
			
			letterIndex = letterExcludedTemp1.IndexOf(g.texShowName[i]);
			//Debug.Log("letterIndex="+letterIndex);
			if (letterIndex > -1) {
				letterExcludedTemp2 = letterExcludedTemp1.Remove(letterIndex,1);
				letterExcludedTemp1 = letterExcludedTemp2;
				
				//showingLetter += g.texShowName[i];
			}
		}
		g.letterExcluded = letterExcludedTemp1;
		
		//Debug.Log("g.letterExcluded="+g.letterExcluded);
		char[] charLetters = new char[g.letterExcluded.Length];
		for (int i = 0; i < g.letterExcluded.Length; i++) {
			charLetters[i] = g.letterExcluded[i];
			//Debug.Log(charLetters[i]);
		}

		char temp;
		for (int i = 0; i < g.letterExcluded.Length; i++) {
			int randomIndex = Random.Range(i, g.letterExcluded.Length);
			temp = charLetters[i];
			charLetters[i] = charLetters[randomIndex];
			charLetters[randomIndex] = temp;
		}
		g.letterExcluded = "";
		for (int i = 0; i < charLetters.Length; i++) g.letterExcluded += charLetters[i];
		//Debug.Log("g.letterExcluded="+g.letterExcluded);
	}
}
