using UnityEngine;
using System.Collections;

public class GameBonus : MonoBehaviour {

	//----------------------------------------------------------------------
	//                           GameControlZone
	//----------------------------------------------------------------------
	public GameControl gameControl;
	public GameBasic gameBasic;
	public GameTexture gameTexture;
	public GameHint gameHint;

	public GameState gameState;
	public GameComponent gameComponent;

	public Account account;
	
	public Navi navi;
	public EventCommon ev;

	public GameObject LetterPrefab;

	//-----------
	// Bonus
	//-----------
	private int bonusTextMinLength = 1;       // 1
	private int bonusTextMaxLength = 1;       // 3
	private int bonusTouchCnt = 20;        // 20
	private int bonusMaxInCorrectCnt = 10; // 10
	
	private int bonusTextCnt;
	private string bonusWordText;
	private string bonusText;
	private int bonusInCorrectCnt;
		
	private string bonusAllWord;
	private string touchWord;
	
	private int wordCnt;
	private int wordCnt2;
	
	private float bonusSliderDistance;
	private float bonusSliderMove;

	float hue = 0.0f;
	Color rgbCol;
	HSBColor hsbCol;

	public IEnumerator ShowBonusGameRule(){

		Debug.Log("ShowBonusGameRule");
		
		//--------------------
		// Init
		//--------------------
		gameState.StateGameLevel("bonus");
		gameState.StateMemoryGamePanel();


		wordCnt2 = 0;
		gameComponent.gameSceneBgUITexture.color = Color.gray;

		gameComponent.gameIconUISprite.spriteName = "speedgameicons";
		//gameComponent.gameIconUISprite.MakePixelPerfect();


		//--------------------
		// State
		//--------------------
		g.isGameTexture = true;
		gameState.StateBonusGame();
		gameHint.HintLetterEffectAllStop();

		//--------------------
		// Modify Widget Size
		//--------------------
		Vector3 v3 = gameState.WordCnt.transform.localPosition;
		gameState.WordCnt.transform.localPosition = new Vector3(-237,v3.y,v3.z); //-224 --> -237
		gameComponent.wordCntUISprite.width = 130; // 90-->120 
		gameComponent.clickWordUILabel.fontSize = 45; // 60-->50
		
		//--------------------------------------------
		// BonusGameRule
		//--------------------------------------------
		int alphabat1;
		int alphabat2;
		int alphabat3;
		
		gameComponent.clickWordUILabel.text = "BonusTime";
		
		bonusTextCnt = Random.Range(bonusTextMinLength,bonusTextMaxLength);


        //---------------------------------------------
        // Instantiate Tile
        //---------------------------------------------
        gameBasic.InitTile(5,5,LetterPrefab);
        gameBasic.SetLetter();

        gameComponent.EnableGridBoxCollider(false);



        //Debug.Log(g.letterExcluded[0].ToString());
        //Debug.Log(g.letterExcluded[1].ToString());
        //Debug.Log(g.letterExcluded[2].ToString());
        //Debug.Log(g.letterExcluded[3].ToString());
        //Debug.Log(g.letterExcluded[4].ToString());
        //Debug.Log(g.letterExcluded[5].ToString());

        bonusWordText = g.letterExcluded[Random.Range(0,25)].ToString();	



        //alphabat1 = Random.Range(65,89);
        //alphabat2 = Random.Range(65,89);
        //while(alphabat1==alphabat2) alphabat2 = Random.Range(65,89);
        //alphabat3 = Random.Range(65,89);
        //while(alphabat1==alphabat3 || alphabat2==alphabat3) alphabat3 = Random.Range(65,89);
		
        //bonusWordText = System.Convert.ToChar(alphabat1).ToString();	
        //if(bonusTextCnt==2) {
        //    bonusWordText += System.Convert.ToChar(alphabat2).ToString();	
        //} else if (bonusTextCnt==3) {
        //    bonusWordText += System.Convert.ToChar(alphabat2).ToString();	
        //    bonusWordText += System.Convert.ToChar(alphabat3).ToString();	
        //}
		
		bonusAllWord = "";
		for(int i = 1; i <=  bonusTouchCnt; i++){
			bonusAllWord += bonusWordText;
		}
		
		//Debug.Log("bonusAllWord="+bonusAllWord);
		
		gameComponent.bonusRuleTouchLabel.text = "[ff0000]"+bonusWordText+"[-]";
		gameComponent.bonusRuleCountLabel.text = "[315bc1]" + bonusTouchCnt.ToString()+"[-]";
		gameComponent.bonusRuleTimeLabel.text = "[009900]" + g.bonusLimitTime.ToString() + "[-]";
		
		//--------------------
		// setting limitTime
		//--------------------
		g.limitTime = (int) (g.limitBaseTime * g.bonusLimitTime);
		gameComponent.limitUILabel.text = g.limitTime.ToString();
		gameComponent.limitLineUILabel.text = g.limitTime.ToString();
		
		gameComponent.wordCntUILabel.text = bonusTouchCnt.ToString();

        //Debug.Log("gameComponent.wordCntUILabel.text=" + gameComponent.wordCntUILabel.text);

        Color frameCol = g.GetRandomColor();
		gameComponent.gameTextureUISprite.color = frameCol;
        gameComponent.pictureFrameUISprite.color = frameCol;


		//--------------------
		// fade in
		//--------------------
		yield return StartCoroutine( navi.FadeIn() );

		//--------------------
		// CountDown
		//--------------------
		for(int i=53;i>47;i--){
			if(g.isGameTexture) {
				gameComponent.bonusRuleReadyLabel.text = "[ff0000]"+System.Convert.ToChar(i).ToString()+"[-]";
				yield return new WaitForSeconds(0.5f);
                BonusRuleShowTime++;
			}
		}

		if(g.isGameTexture) BonusGameStart();
		//StartCoroutine ( BonusGameStart() );
	}
    		
	int BonusRuleShowTime = 0;	
		
	public void BonusGameStart(){
		
		Debug.Log("BonusGameStart");
		//--------------------
		// State && Init
		//--------------------
		g.isGameTexture = false;
		g.isGameTurning = true;
		
		bonusInCorrectCnt = 0;
		touchWord = "";


        gameComponent.EnableGridBoxCollider(true);


		gameState.StateBonusGameStart();
		gameComponent.clickWordUILabel.text = "";

		StartCoroutine(BonusRandomBackgroundColor());

		//--------------------
		// Modify TimeSlider
		//-------------------- 

		gameComponent.EndTimeSliderSprite.spriteName = "runnigman4"; 
		gameComponent.StartTimeSliderSprite.spriteName = "runnigman2"; 
		
		
		//------------------------------------
		// LimitTime
		//------------------------------------
		StartCoroutine(gameControl.LimitTime());
		
		bonusSliderDistance = Mathf.Abs(gameState.EndTimeSlider.transform.localPosition.x)+Mathf.Abs(gameState.StartTimeSlider.transform.localPosition.x);
		bonusSliderMove = bonusSliderDistance/(bonusTextCnt * bonusTouchCnt);
		
		//Debug.Log("bonusSliderDistance="+bonusSliderDistance);
		//Debug.Log("bonusSliderMove="+bonusSliderMove);

        //---------------------------------------------
        // Instantiate Tile
        //---------------------------------------------
        //gameBasic.InitTile(5, 5, LetterPrefab);
        //gameBasic.SetLetter();

		//yield return new WaitForSeconds( 5f);

	}


	public IEnumerator BonusGameEnded() {
		
		Debug.Log("BonusGameEnded");
		
		//--------------------
		// State && Init
		//--------------------
		g.isGameBonus = false;
		g.isGameTurning = false;

		gameComponent.DestroyGridChildrens();

		gameState.StateBonusGameEnded();
		
		g.turnDelayTime = 2.5f;
		
		
		//------------------------
		// Effect Letter
		//------------------------
		if (int.Parse (gameComponent.wordCntUILabel.text) > 0) {
			gameComponent.bonusEndUILabel.text = "Mission Fail!!";
		}

		gameComponent.bonusEndScale.from = Vector3.one * 6f;
		gameComponent.bonusEndScale.to = Vector3.one * 1.2f;
		gameComponent.bonusEndScale.duration = 1.2f;
		gameComponent.bonusEndScale.enabled = true;
		gameComponent.bonusEndScale.ResetToBeginning();

		if (int.Parse (gameComponent.wordCntUILabel.text) > 0) {
			gameBasic.PlayTTS("Mission");
			yield return new WaitForSeconds( 0.7f );
			gameBasic.PlayTTS("Fail");
			yield return new WaitForSeconds( 0.5f );

			gameComponent.bonusEndUILabel.text = "Mission Fail!!";

		} else {
			gameBasic.PlayTTS("Mission");
			yield return new WaitForSeconds( 0.7f );
			gameBasic.PlayTTS("Complete");
			yield return new WaitForSeconds( 0.5f );
			g.bonusCnt++;

            gameComponent.rankUIScrollBar.barSize = (float)(g.curStageScore+g.bonusScore)/(float)g.curStageVitualMaxScore;

		}


		//FadeInOutPanel.SetActive(false); 

		//------------------------
		// Modify Panel
		//------------------------
		// Modify Widget Size
		Vector3 v3 = gameState.WordCnt.transform.localPosition;
		v3 = new Vector3(-224,v3.y,v3.z); //-224 --> -237
		gameComponent.wordCntUISprite.width = 90; // 90-->130 
		gameComponent.clickWordUILabel.fontSize = 60; // 60-->45
		// Modify TimeSlider
		gameComponent.EndTimeSliderSprite.spriteName = "bomb";
		gameComponent.StartTimeSliderSprite.spriteName = "bombflame";

		//------------------------
		// Next Panel
    	//------------------------
		gameState.BonusGameEnd.SetActive(false);
		yield return new WaitForSeconds(1.0f);
		StartCoroutine( gameControl.StageMaster() );
		
	}

	IEnumerator BonusRandomBackgroundColor(){
		
		float colorChangeTime = 0.01f;
		float offset = 0.001f;
		
		hsbCol = new HSBColor(hue,1.0f,1.0f);
		rgbCol = hsbCol.ToColor();
		gameComponent.gameSceneBgUITexture.color = rgbCol;
		
		hue = hue + offset;
		if(hue > 1.0f) hue = 0.0f;
		
		if(g.isGameBonus) {
			yield return new WaitForSeconds(colorChangeTime);		
			StartCoroutine ( BonusRandomBackgroundColor() );
		}
		
	}	

	//----------------------------------------------------------
	//                        Event
	//----------------------------------------------------------
	public void EventBonusGameOnRelease(){
		Debug.Log("EventLessonOnRelease");
		ev.EventOnRelease();
		if (ev.isCurPrevDist && BonusRuleShowTime >= 1) BonusGameStart();
	}


	public void EventLetterOnRelease(Transform letterClone){
		//Debug.Log("EventBonusLetterOnRelease");
		ev.EventOnRelease();

		gameBasic.TouchLetterEffect ();

		gameComponent.SetLetterPrefab(letterClone);

		gameBasic.TouchLetterEffect ();

		//------------------------
		// wordCnt
		//------------------------ 
		if(int.Parse(gameComponent.wordCntUILabel.text) >= 1) {
			gameComponent.wordCntUILabel.text = ( int.Parse(gameComponent.wordCntUILabel.text) - 1).ToString();
		}

		//-------------------------------------
		// clickWord 10 length
		//-------------------------------------
		UILabel letterUILabel = letterClone.FindChild("LetterLabel").transform.GetComponent<UILabel>();

		if ( int.Parse(gameComponent.wordCntUILabel.text) > 0) {
			gameComponent.clickWordUILabel.text += letterUILabel.text;
			touchWord += letterUILabel.text;
			
			//Debug.Log(gameComponent.clickWordUILabel.text.Length);
			
			if( gameComponent.clickWordUILabel.text.Length > 10) { 
				gameComponent.clickWordUILabel.text = "";
				gameComponent.clickWordUILabel.text = letterUILabel.text;
			}
		}

		//-------------------------------------
		// Checking Correct
		//-------------------------------------
		int wordCnt = bonusTouchCnt - int.Parse(gameComponent.wordCntUILabel.text);

        //Debug.Log("wordCnt=" + wordCnt);
        //Debug.Log("bonusTouchCnt=" + bonusTouchCnt);
        //Debug.Log("bonusAllWord=" + bonusAllWord);
        //Debug.Log("touchWord=" + touchWord);

		if (wordCnt < bonusTouchCnt) {
			if (touchWord.Substring (0,wordCnt) != bonusAllWord.Substring (0,wordCnt)) {
				gameComponent.clickWordUILabel.text = gameComponent.clickWordUILabel.text.Remove(gameComponent.clickWordUILabel.text.Length-1);
				touchWord = touchWord.Remove(touchWord.Length-1);
				gameComponent.wordCntUILabel.text = ( int.Parse(gameComponent.wordCntUILabel.text) + 1).ToString();
				gameState.GameBackgroud.SetActive (false);
				//gameState.Grid.SetActive(false);
				//gameState.Grid.transform.renderer.enabled = false;
				gameState.BonusInCorrect.SetActive (true);
				if(g.isVibrate) Handheld.Vibrate();

				if( bonusInCorrectCnt >= bonusMaxInCorrectCnt) StartCoroutine(BonusGameEnded());
				bonusInCorrectCnt++;

			} else  {
				
				//			if( bonusTextCnt == 1) {
				//				gameState.StartTimeSlider.transform.localPosition = new Vector3(gameState.StartTimeSlider.transform.localPosition.x - bonusSliderMove,0,0);
				//				if(	gameComponent.TimeUISlider.value < 0.7f) gameComponent.TimeUISlider.value -= (1/((float)bonusTouchCnt-2f));
				//				else gameComponent.TimeUISlider.value -= (1/(float)bonusTouchCnt);
				//			} else if( bonusTextCnt > 1 && touchWord.Substring(0,wordCnt).Length % bonusTextCnt == 0) {
				//				gameState.StartTimeSlider.transform.localPosition = new Vector3(gameState.StartTimeSlider.transform.localPosition.x - bonusSliderMove,0,0);
				//				if(	gameComponent.TimeUISlider.value < 0.7f) gameComponent.TimeUISlider.value -= (1/((float)bonusTouchCnt-2f));
				//				else gameComponent.TimeUISlider.value -= (1/(float)bonusTouchCnt);
				//			}
				
				gameState.StartTimeSlider.transform.localPosition = new Vector3(gameState.StartTimeSlider.transform.localPosition.x - (bonusSliderMove*bonusTextCnt) ,0,0);
				if(	gameComponent.TimeUISlider.value < 0.7f) gameComponent.TimeUISlider.value -= (1/( (float)bonusTouchCnt-2f) );
				else gameComponent.TimeUISlider.value -= ( 1/ (float)bonusTouchCnt );
			}
		}

	}


	public void EventBonusInCorrectOnPress(){
		gameState.BonusInCorrect.SetActive(false);
		gameState.GameBackgroud.SetActive(true);
		gameState.Grid.SetActive(true);
	}



	void Update () {
		
		if (g.isGameBonus) {
			if (int.Parse (gameComponent.wordCntUILabel.text) == 0) {
				StartCoroutine ( BonusGameEnded() );
			}
		}
	}



}
