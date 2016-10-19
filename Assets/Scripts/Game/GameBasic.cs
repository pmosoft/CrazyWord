using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBasic : MonoBehaviour {

	public GameControl gameControl;
	public GameTexture gameTexture;

	public GameBonus gameBonus;
	public GameHint gameHint;
	public GameBlank gameBlank;

	public GameState gameState;
	public GameComponent gameComponent;

	public LevelPass levelPass;

	public DBO dbo;
	public Navi navi;
	public EventCommon ev;
	
	public GameObject LetterPrefab;
	
	//---------------------------------------------------------
	// Count & Score
	//---------------------------------------------------------

	//----------------------------------------------------------
	//                      GameControl
	//----------------------------------------------------------
	
	public void GameMemoryStart(){
		
		Debug.Log("GameMemoryStart");
		
		//---------------------------------------------
		// Setting State
		//---------------------------------------------
		gameState.StateGameLevel("memory");
		gameState.StateMemoryGamePanel();
		
		//---------------------------------------------
		// Init variable
		//---------------------------------------------
		gameComponent.clickWordUILabel.text = "";


		if(g.isCombo) g.comboCnt++; else g.comboCnt = 0;
		gameComponent.comboNumUILabel.text = g.comboCnt.ToString();
		gameComponent.comboNumLineUILabel.text = g.comboCnt.ToString();			
		
		gameComponent.gameIconUISprite.spriteName = "memorygameicons";
		//gameComponent.gameIconUISprite.MakePixelPerfect();

		gameComponent.wordCntUILabel.text = g.texShowName.Length.ToString();

		//---------------------------------------------
		// LimitTime
		//---------------------------------------------
		g.limitTime = g.limitBaseTime * g.texShowName.Length;
		gameComponent.limitUILabel.text = g.limitTime.ToString();
		gameComponent.limitLineUILabel.text = g.limitTime.ToString();

		StartCoroutine(gameControl.LimitTime());
		
		//---------------------------------------------
		// Instantiate Tile
		//---------------------------------------------
		InitTile(5,5,LetterPrefab);

		//---------------------------------------------
		// Setting Letter
		//---------------------------------------------
		SetLetter();

		//---------------------------------------------
		// BoxCollider
		//---------------------------------------------
		gameComponent.backSpaceBoxCollider.enabled = true;

	}

	public void InitTile( int widthCnt, int heightCnt, GameObject LetterPrefab){
		
		float pRatio = 1.0f;
		float sRatio = 1.0f;
		Vector3 localPos;

		g.tileWidthCnt = widthCnt;
		g.tileHeightCnt= heightCnt;
		
		if     (g.tileWidthCnt == 2)  { pRatio = 2.5f;  sRatio = 2.2f; }
		else if(g.tileWidthCnt == 5)  { pRatio = 1.0f;  sRatio = 1.0f; }
		else if(g.tileWidthCnt == 6)  { pRatio = 0.77f; sRatio = 0.7f; }
		else if(g.tileWidthCnt == 7)  { pRatio = 0.65f; sRatio = 0.7f; }
		else if(g.tileWidthCnt == 10) { pRatio = 0.45f; sRatio = 0.5f; }
		
		Transform[] trans = gameState.Grid.GetComponentsInChildren<Transform>(); 
		foreach (Transform tr in trans) {
			if(tr.gameObject.tag == "Letter") Destroy(tr.gameObject); 
		}
		
		for (int y = 0; y < g.tileHeightCnt; y++) {
			for (int x = 0; x < g.tileWidthCnt; x++) {
				
				//------------------------
				// Instantiate Object
				//------------------------
				GameObject LetterClone = Instantiate(LetterPrefab) as GameObject;
				LetterClone.name = "Letter" + x + y;
				
				//------------------------
				// Hierarchy
				//------------------------
				LetterClone.transform.parent = gameState.Grid.transform;
				
				//------------------------
				// Position,Rotation,Scale
				//------------------------
				if(g.tileWidthCnt == 2) // molegame
					localPos = new Vector3( x *   g.tileWidth  * pRatio + 90
					                       ,y * - g.tileHeight * pRatio - 75 , 0f);
				else
					localPos = new Vector3( x * g.tileWidth * pRatio, y * - g.tileHeight * pRatio , 0f);
				LetterClone.transform.localPosition = localPos;
				LetterClone.transform.localScale = Vector3.one;
				gameComponent.SetLetterPrefab(LetterClone.transform);
				gameComponent.letterUILabel.fontSize = (int) (gameComponent.letterUILabel.fontSize * sRatio);

				g.gridLetterTrans.Add(gameComponent.letterLabel);
			}
		}
		
		//SetLetter();
	}


    public void SetLetter(){
		
		//---------------------------------------------
		// Exclude 1 Alphabat in 26 Alphabat
		//---------------------------------------------
		WordExcluded();
		
		//---------------------------------------------
		// Setting pattern letter
		//---------------------------------------------
        int noComboLargeCnt = dbo.SelUserCorrectHisComboLargeCnt();
		int patternKind = 0;

        if(noComboLargeCnt >= 7 && noComboLargeCnt <= 9) patternKind = 0;
        else if(noComboLargeCnt == 6) patternKind = Random.Range(0,2); // 0..1
        else if(noComboLargeCnt == 5) patternKind = Random.Range(0,3); // 0..2
        else if(noComboLargeCnt == 4) patternKind = Random.Range(0,5); // 0..3
        else if(noComboLargeCnt == 3) patternKind = Random.Range(0,5); // 0..4
        else if(noComboLargeCnt == 2) patternKind = Random.Range(0,6); // 0..5
        else if(noComboLargeCnt == 1) patternKind = Random.Range(0,7); // 0..6
        else if(noComboLargeCnt == 0) patternKind = Random.Range(0,8); // 0..7
	
		//int patternKind = 0;
		
		//---------------------------------------------
		// Setting tile to initialize letter prefab
		//---------------------------------------------
		int letterCnt = 0;
		
		g.letterDictionary.Clear();
		for (int y = 0; y < g.tileHeightCnt; y++) {
			for (int x = 0; x < g.tileWidthCnt; x++) {
				//Debug.Log("letterCnt1="+letterCnt);

				//--------------
				// Init Prefab
				//--------------
				gameComponent.letterPrefab = gameState.Grid.transform.Find("Letter"+x+y);
				gameComponent.SetLetterPrefab( gameComponent.letterPrefab );
				
				//-------------- 
				// Event key
				//--------------
				EventDelegate.Add (gameComponent.letterUIEventTrigger.onPress,ev.EventOnPress);
				EventDelegate eventClick = new EventDelegate(this, "EventLetterOnRelease");
				EventDelegate.Parameter param = new EventDelegate.Parameter();
				param.obj = gameComponent.letterPrefab;
				param.expectedType = typeof(Transform);
				eventClick.parameters[0] = param;
				EventDelegate.Add (gameComponent.letterUIEventTrigger.onRelease,eventClick);

				//--------------
				// Letter
				//--------------
				gameComponent.letterUILabel.text = g.letterExcluded[letterCnt].ToString();
				//gameComponent.letterUILabel.text = g.letterExcluded[g.letterPattern[patternKind,letterCnt,1]].ToString();

				//Debug.Log("gameComponent.letterUILabel.text="+gameComponent.letterUILabel.text);
                
                g.SetLetterColor();
				gameComponent.letterUILabel.color = g.letterColor;
				//gameComponent.letterUILabel.color = g.letterColors[Random.Range(0, 7)];
				//gameComponent.letterUILabel.color = Color.yellow;
                //gameComponent.letterUILabel.color = new Color( Random.Range(0f,256f),Random.Range(0f,256f),Random.Range(0f,256f));

				//Debug.Log("letterCnt2="+letterCnt);

				//--------------
				// Effect
				//--------------
				gameComponent.letterEffectScale.to = Vector3.one;
				
				//gameComponent.letterEffectScale.from = new Vector3(1,1,1);
				gameComponent.letterEffectScale.ResetToBeginning();
				gameComponent.letterEffectScale.enabled = false; 
				
				gameComponent.letterEffectAlpha.ResetToBeginning();
				gameComponent.letterEffectAlpha.enabled = false; 				
				
				//gameComponent.letterEffectUILabel.text = g.letterExcluded[letterCnt].ToString();
				gameComponent.letterEffectUILabel.text = g.letterExcluded[g.letterPattern[patternKind,letterCnt,1]].ToString();

				gameComponent.letterEffectUILabel.color = g.letterColor;

				
				letterCnt++;
				
				g.letterDictionary.Add(gameComponent.letterUILabel.text.ToString(),gameComponent.letterLabel);
				//Debug.Log("gameComponent.letterUILabel.text="+gameComponent.letterUILabel.text);

			}	
		}
		
		//Debug.Log("letterDictionary.Count="+g.letterDictionary.Count);
		
	}


	//----------------------------------------------------------
	//                        Event
	//----------------------------------------------------------
	public void EventLetterOnRelease(Transform letterClone){
		//Debug.Log("EventLetterOnRelease");
		ev.EventOnRelease();
		if (ev.isCurPrevDist) {
			
			gameComponent.SetLetterPrefab(letterClone);

			if      (g.isGameMemory)    TouchLetter();
			else if (g.isGameMole)      TouchLetter();
			//else if (g.isGameShooting) TouchLetter();
			else if (g.isGameBlank)    gameBlank.EventLetterOnRelease();
			else if (g.isGameHint)     gameHint.EventLetterOnRelease();
			else if (g.isGameBonus)    gameBonus.EventLetterOnRelease(letterClone);

		}
	}

	string blankLetterReplace(string s1) {
		return s1.Replace(" ","▒");
	} 



	void TouchLetter(){
		
		TouchLetterEffect ();
        Debug.Log("gameComponent.clickWordUILabel.text="+gameComponent.clickWordUILabel.text);
        Debug.Log("gameComponent.letterUILabel.text="+gameComponent.letterUILabel.text);
		//------------------------
		// wordCnt
		//------------------------ 
		if(int.Parse(gameComponent.wordCntUILabel.text) >= 1) {
			gameComponent.wordCntUILabel.text = ( int.Parse(gameComponent.wordCntUILabel.text) - 1).ToString();
		}
		
		//------------------------
		// Setting clickWord
		//------------------------
		if ( int.Parse(gameComponent.wordCntUILabel.text) >= 0) {
			gameComponent.clickWordUILabel.text += gameComponent.blankLetterReplace(gameComponent.letterUILabel.text);
		}
		
		//------------------------
		// Checking TurnClear
		//------------------------
		if (gameComponent.clickWordUILabel.text == g.texName) {
			StartCoroutine ( gameControl.WordClear ());
		}
	}
	
	//----------------------------------------------------------
	//                  Event Function
	//----------------------------------------------------------
	
	public void TouchLetterEffect(){
		//Debug.Log("TouchLetterCommon");

		PlayTTS(gameComponent.letterUILabel.text);

		//------------------------
		// effect
		//------------------------ 
		//gameComponent.letterEffectLabel.gameObject.SetActive(true);

        gameComponent.letterEffectScale.gameObject.SetActive(true);
		

		gameComponent.letterEffectScale.to = Vector3.one * 5;  
		gameComponent.letterEffectScale.enabled = true; 
		gameComponent.letterEffectScale.ResetToBeginning();
		
		gameComponent.letterEffectAlpha.enabled = true; 
		gameComponent.letterEffectAlpha.ResetToBeginning();

		//gameComponent.letterEffectLabel.gameObject.SetActive(false);

	}

	void WordExcluded(){
		
		Debug.Log("WordExcluded");

		g.letterExcluded = "";
		
		string letterExcludedTemp1 = g.letterCanShowing;
		string letterExcludedTemp2 = "";
		string letterExcludedTemp3 = ""; 
		int letterIndex;

		string letterDistinct1 = ""; 
		string letterDistinct2 = ""; 

		//Debug.Log("letterExcludedTemp1="+letterExcludedTemp1);

		// letterExcludedTemp2 = g.letterCanShowing - g.texName
		for (int i = 0; i < g.letterCanShowing.Length; i++) {

			letterIndex = g.texName.IndexOf(g.letterCanShowing[i]);

			//Debug.Log(g.texName.IndexOf(g.letterCanShowing[i])+" "+g.letterCanShowing[i]);

			if (letterIndex <= -1) {
				letterExcludedTemp2 += g.letterCanShowing[i];
			}
		}
		//Debug.Log("letterExcludedTemp2="+letterExcludedTemp2);

		// distinct g.texName
		for (int i = 0; i < g.texName.Length; i++) {
			if( letterDistinct1.IndexOf(g.texName[i]) == -1 ) {
				letterDistinct1 += g.texName[i];
			}
		}

		//Debug.Log("g.texName="+g.texName+" "+"letterDistinct1="+letterDistinct1);

		letterExcludedTemp3 = letterDistinct1 + letterExcludedTemp2.Substring(0,25-letterDistinct1.Length);
		//Debug.Log("letterExcludedTemp3="+letterExcludedTemp3);

		List<char> c1 = new List<char>();
		for(int i=0;i<letterExcludedTemp3.Length;i++){
			c1.Add(letterExcludedTemp3[i]);
       	}
		c1.Sort();

		for(int i=0;i<c1.Count;i++){
			g.letterExcluded += c1[i];
		}
		g.letterExcluded = g.letterExcluded.Trim();
		//Debug.Log("g.letterExcluded="+g.letterExcluded);


	}
	

	public void CorrectWordColor(int idx){
		string correctWord = "";
		
		for(int i=0;i<g.texName.Length;i++){
			if( i <= idx )
				correctWord += "[ff0000]"+g.texName[i]+"[-]";
			else 
				correctWord += g.texName[i].ToString();
		}
		gameComponent.clickWordUILabel.text = correctWord;
	}

	public void SetBackgroudColor(){
        g.SetBgColor();
        g.col = g.bgColOptions[g.userBgCol];
        //Debug.Log("g.userBgCol11111=" + g.userBgCol);
        //Debug.Log("g.col=" + g.col);
        gameComponent.gameSceneBgUITexture.color = g.col;
	}

	float _pitch = 1f;
	float _speechRate = 0f;
	int blankIdx;
	string context1,context2;
	public void PlayTTS(string context){

        if(g.isSound) _pitch = 1f; else _pitch = 0f;

		blankIdx = context.IndexOf("_");

		if(blankIdx < 0) {
            gameTexture.showWordTime = 1.3f;
			TTSManager.Speak(context+".", false, TTSManager.STREAM.Music, _pitch, _speechRate, transform.name, null, null);
		} else {
            gameTexture.showWordTime = 2f;
			context1 = context.Substring(0,blankIdx);
			context2 = context.Substring(blankIdx+1,context.Length-(blankIdx+1));
			StartCoroutine ("MultyPlayTTS");
		}
	}

	public IEnumerator MultyPlayTTS(){

        if(g.isSound) _pitch = 1f; else _pitch = 0f;


		TTSManager.Speak(context1+".", false, TTSManager.STREAM.Music, _pitch, _speechRate, transform.name, null, null);
		yield return new WaitForSeconds(1f);
		TTSManager.Speak(context2+".", false, TTSManager.STREAM.Music, _pitch, _speechRate, transform.name, null, null);

	}
	
}
