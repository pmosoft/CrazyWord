using UnityEngine;
using System.Collections;

public class GameHint : MonoBehaviour {

	public GameControl gameControl;
	public GameBasic gameBasic;
	public GameTexture gameTexture;

	public GameState gameState;
	public GameComponent gameComponent;

	public GameObject LetterPrefab;


	//------------------------------
	// Time & Count
	//------------------------------
	float hintMaxShowTime = 20f;

	float hintShowTime; int clickLetterCnt; 
	public void GameHintStart(){
		Debug.Log("GameHintStart");

		StartCoroutine ( "ProcessHintLetter" );

		//---------------------------------------------
		// Setting State
		//---------------------------------------------
		gameState.StateGameLevel("hint");
		gameState.StateHintGamePanel();
		//StartCoroutine ( gameTexture.SetTurnTexture( g.randomTurnTexNum[3] ) );
		
		//---------------------------------------------
		// Init variable
		//---------------------------------------------
		hintShowTime = 0.0f;
		clickLetterCnt = 0;
		g.letterCols.Clear();

		gameComponent.clickWordUILabel.text = "";

		gameComponent.gameIconUISprite.spriteName = "hintgameicons";
		//gameComponent.gameIconUISprite.MakePixelPerfect();

		gameComponent.wordCntUILabel.text = g.texName.Length.ToString();

        Color frameCol = g.GetRandomColor();
		gameComponent.gameTextureUISprite.color = frameCol;
        gameComponent.pictureFrameUISprite.color = frameCol;


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
		HintLetterEffectAllStop();

	}


	//----------------------------------------------------------
	//                       Hint
	//----------------------------------------------------------
    public void ProcessHintLetterStop() {
        StopCoroutine ( "ProcessHintLetter" );
    }

	public IEnumerator ProcessHintLetter(){

		while(!g.isGameHint) {
			yield return new WaitForSeconds(1f);
		}

		hintShowTime = 0.0f;

		//Debug.Log("start:"+System.DateTime.Now);
		//Debug.Log("hintShowTime="+hintShowTime);
		while(hintShowTime <= hintMaxShowTime) {
			//Debug.Log("Process hintShowTime="+hintShowTime+" "+g.isGameTexture);

			yield return new WaitForSeconds(0.01f * Time.deltaTime);
			hintShowTime += 0.1f;
			
		}

		//Debug.Log("end:"+System.DateTime.Now);

		HintLetterEffectAllStop();

        //Transform tr = FindHintLetter();
        //if (tr != null) HintLetterEffect(tr, true);
        HintLetterEffect(FindHintLetter(), true);

		while(hintShowTime >= hintMaxShowTime) {
			yield return new WaitForSeconds(0.01f);
		}


		StartCoroutine( "ProcessHintLetter" );

	}

	string hintLetter;
	int blackLetterPos,redLetterPos;
	public Transform FindHintLetter(){
		//void FindHintLetter(){

		//Debug.Log("g.letterCols.Count1="+g.letterCols.Count);
		g.texName = g.stageTextures[g.stageTurn-1][0].texName;
		//Debug.Log("g.texName="+g.texName);
		//Debug.Log("g.texName.Substring(0,1)="+g.texName.Substring(0,1));

		blackLetterPos = -1;
		redLetterPos = -1;
		for(int i=0;i<g.letterCols.Count;i++) {
			if(g.letterCols[i].col == Color.red) {
				redLetterPos = i;
				hintLetter = g.texName.Substring(i,1);
				break;
			} else if (g.letterCols[i].col == Color.black) {
				blackLetterPos = i;
			}
			//Debug.Log("g.letterCols["+i+"].col="+g.letterCols[i].col);
		}

		//Debug.Log("redLetterPos="+redLetterPos);
		//Debug.Log("blackLetterPos="+blackLetterPos);
		if      (blackLetterPos == -1 && redLetterPos == -1)
			hintLetter = g.texName.Substring(0,1);
		else if (blackLetterPos != -1 && redLetterPos == -1)
			hintLetter = g.texName.Substring(blackLetterPos+1,1);
		else if (blackLetterPos == -1 && redLetterPos != -1)
			hintLetter = g.texName.Substring(0,1);
		else if (blackLetterPos != -1 && redLetterPos != -1)
			hintLetter = g.texName.Substring(redLetterPos,1);

		//Debug.Log("g.letterCols.Count2="+g.letterCols.Count);
		//Debug.Log("hintLetter="+hintLetter);
		
		//Debug.Log("letterDictionary[hintLetter].name="+g.letterDictionary[hintLetter].name);
        
        //Transform ret = null;
        //if (g.letterCols.Count > 0) ret = g.letterDictionary[hintLetter];
        return g.letterDictionary[hintLetter];
	}
	
	public void HintLetterEffect(Transform letterLabel, bool tf){
		if(letterLabel != null) {
			//letterLabel.GetComponent<Animator>().SetBool("isLetterBegging",tf);

			letterLabel.GetComponent<TweenScale>().enabled = tf; 
			letterLabel.GetComponent<TweenScale>().ResetToBeginning();
            //Debug.Log(letterLabel.name);
            //Debug.Log(letterLabel.gameObject.name);
            //Debug.Log(letterLabel.gameObject.name);

            //letterLabel.gameObject.
            //letterEffectLabel

		}
	}

	public void HintLetterEffectAllStop(){
		if (g.gridLetterTrans.Count > 10) {
			foreach(Transform tr in g.gridLetterTrans) {
				//Debug.Log("tr.name="+tr.name);
				HintLetterEffect( tr, false );
			}
		}	
	}

	//----------------------------------------------------------
	//                       Event
	//----------------------------------------------------------
	int wordCnt;
	public void EventLetterOnRelease(){

		gameBasic.TouchLetterEffect ();

		//------------------------
		// wordCnt
		//------------------------ 
		if(int.Parse(gameComponent.wordCntUILabel.text) >= 1) {
			gameComponent.wordCntUILabel.text = ( int.Parse(gameComponent.wordCntUILabel.text) - 1).ToString();
		}
		wordCnt = int.Parse(gameComponent.wordCntUILabel.text);
		
		//------------------------
		// Setting clickWord
		//------------------------
		if ( wordCnt >= 0 && clickLetterCnt < g.texName.Length) {
			//Debug.Log("g.texName.Substring(clickLetterCnt,1)="+g.texName.Substring(clickLetterCnt,1));
			//Debug.Log("letterUILabel.text="+letterUILabel.text);
			if (g.texName.Substring(clickLetterCnt,1) != gameComponent.letterUILabel.text) {
				gameComponent.clickWordUILabel.text += "[ff0000]"+gameComponent.letterUILabel.text+"[-]";
				g.letterCols.Add(new g.LetterCol( gameComponent.letterUILabel.text, Color.red ));
			} else {
				gameComponent.clickWordUILabel.text += gameComponent.letterUILabel.text;
				g.letterCols.Add(new g.LetterCol( gameComponent.letterUILabel.text, Color.black ));
			}

			if (hintLetter == gameComponent.letterUILabel.text) {
				HintLetterEffect(gameComponent.letterLabel, false);
			}
			
			hintShowTime = 0.0f;
			
			clickLetterCnt++;
		}
		
		//------------------------
		// Checking TurnClear
		//------------------------
		if (gameComponent.clickWordUILabel.text == g.texName) {
			g.letterCols.Clear();
			StartCoroutine ( gameControl.WordClear ());
		}
	}

	string clickWord;
	public void EventBackSpaceOnRelease(){

		if(gameComponent.clickWordUILabel.text.Length >= 1) {
			//-----------------
			// clickWord
			//-----------------
			clickWord = gameComponent.clickWordUILabel.text;
			
			// color letter must delete 11 letters.
			if(clickWord.LastIndexOf("]") == clickWord.Length-1) {
				gameComponent.clickWordUILabel.text = clickWord.Substring(0,clickWord.LastIndexOf("]")-11);
			} else 
				gameComponent.clickWordUILabel.text = clickWord.Remove(clickWord.Length-1);
			
			clickLetterCnt--;
			
			//-----------------
			// letterCols
			//-----------------
			g.letterCols.RemoveAt(clickLetterCnt);
			
			//-------------------------
			// HintLetterEffectAllStop
			//-------------------------
			HintLetterEffectAllStop();
			
			//-------------------------
			// coroutine stop
			//-------------------------
			hintShowTime = 0.0f;
			
			//-------------------------
			// wordCnt
			//-------------------------
			gameComponent.wordCntUILabel.text = ( int.Parse(gameComponent.wordCntUILabel.text) + 1).ToString();
		}
	}


}
