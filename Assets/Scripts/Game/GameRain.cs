using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public class GameRain : MonoBehaviour {

	public GameControl gameControl;
	public GameTexture gameTexture;
	public GameBasic gameBasic;

	public GameState gameState;
	public GameComponent gameComponent;

	public LevelPass levelPass;

	public EventCommon ev;


	public GameObject LetterPrefab;
	public GameObject FlowerPrefab;
	public GameObject CloudPrefab;

	UISprite[] flowerSprite = new UISprite[5];
	Transform[] flowerTransform = new Transform[5];
	UISprite[] flowerSweatSprite = new UISprite[5];
	Animator[] flowerSweatAnimator = new Animator[5];
	Animator[] flowerAnimator = new Animator[5];

	string[] flowerAni = new string[18];
	string[] flowerEatAni = new string[15];

	bool[] isAni = new bool[5];

	void Awake(){

		flowerAni[0]  = "flower_ani002";
		flowerAni[1]  = "flower_ani003";
		flowerAni[2]  = "flower_ani004";
		flowerAni[3]  = "flower_ani005";
		flowerAni[4]  = "flower_ani006";
		flowerAni[5]  = "flower_ani007";
		flowerAni[6]  = "flower_ani008";
		flowerAni[7]  = "flower_ani007";
		flowerAni[8]  = "flower_ani008";
		flowerAni[9]  = "flower_ani007";
		flowerAni[10] = "flower_ani008";
		flowerAni[11] = "flower_ani007";
		flowerAni[12] = "flower_ani006";
		flowerAni[13] = "flower_ani005"; 
		flowerAni[14] = "flower_ani004";
		flowerAni[15] = "flower_ani003";
		flowerAni[16] = "flower_ani002";
		flowerAni[17] = "flower_ani001";

		flowerEatAni[0]  = "flowerbleak_001";
		flowerEatAni[1]  = "flowerbleak_002";
		flowerEatAni[2]  = "flowerbleak_003";
		flowerEatAni[3]  = "flowerbleak_004";
		flowerEatAni[4]  = "flowerbleak_005";
		flowerEatAni[5]  = "flowerbleak_006";
		flowerEatAni[6]  = "flowerbleak_007";
		flowerEatAni[7]  = "flowerbleak_008";
		flowerEatAni[8]  = "flowerbleak_009";
		flowerEatAni[9]  = "flowerbleak_010";
		flowerEatAni[10] = "flowerbleak_011";
		flowerEatAni[11] = "flowerbleak_012";
		flowerEatAni[12] = "flowerbleak_013";
		flowerEatAni[13] = "flowerbleak_014";
		flowerEatAni[14] = "flowerbleak_015";

	}


	public void GameRainStart(){
		
		Debug.Log("GameRainStart");

		//---------------------------------------------
		// Set State
		//---------------------------------------------
		gameState.StateGameLevel("rain");
		gameState.StateRainGamePanel();

		
		//---------------------------------------------
		// Initiate Variable
		//---------------------------------------------
		gameTexture.SetTurnTexture();
   		levelPass.LevelPassButtonStart();

        Color frameCol = g.GetRandomColor();
		gameComponent.gameTextureUISprite.color = frameCol;
        gameComponent.pictureFrameUISprite.color = frameCol;



		gameComponent.gameIconUISprite.spriteName = "flowergameicons";
		//gameComponent.gameIconUISprite.MakePixelPerfect();

		gameComponent.clickWordUILabel.text = g.texName;
		
		g.tileWidthCnt = 5;
		g.tileHeightCnt = 5;
		g.TileRatio();

		g.correctLetterIdx = 0;

		flowerPosY = FlowerPosY();

		for(int i=0;i<isAni.Length;i++) isAni[i]=false;

		//---------------------------------------------
		// Execute LimitTime
		//--------------------------------------------- 
		g.limitTime = g.limitBaseTime * g.texName.Length;
		gameComponent.limitUILabel.text = g.limitTime.ToString();
		gameComponent.limitLineUILabel.text = g.limitTime.ToString();
		
		StartCoroutine(gameControl.LimitTime());


		//---------------------------------------------
		// Daemon Cloud
		//--------------------------------------------- 
		StartCoroutine ( InitCloud() );

		//---------------------------------------------
		// Set FlowerPreFab
		//--------------------------------------------- 
		for (int x=0; x<g.tileWidthCnt; x++) SetFlowerPreFab(x);

		//---------------------------------------------
		// Execute RandomLetter
		//--------------------------------------------- 
		for (int x=0; x<g.tileWidthCnt; x++) StartCoroutine ( RandomLetter(x) );

	}


	IEnumerator InitCloud(){
		CloudClone1 = Instantiate(CloudPrefab) as GameObject;
		CloudClone1.transform.parent = gameState.Grid.transform;
		CloudClone1.transform.localPosition = cloudFrom;
		CloudClone1.transform.localScale = Vector3.one;
		CloudClone1.GetComponent<UISprite>().spriteName = "cloud1";
		cloudMove1 = CloudClone1.GetComponent<TweenPosition>();

		StartCoroutine(RespwanCloud(1));
		yield return new WaitForSeconds(4.0f);

		CloudClone2 = Instantiate(CloudPrefab) as GameObject;
		CloudClone2.transform.parent = gameState.Grid.transform;
		CloudClone2.transform.localPosition = cloudFrom;
		CloudClone2.transform.localScale = Vector3.one;
		CloudClone2.GetComponent<UISprite>().spriteName = "cloud2";
		cloudMove2 = CloudClone2.GetComponent<TweenPosition>();
		StartCoroutine(RespwanCloud(2));
	}

	GameObject CloudClone1,CloudClone2;
	TweenPosition cloudMove1,cloudMove2;

	Vector3 cloudFrom = new Vector3( 750f,-60f,0f);
	Vector3 cloudTo   = new Vector3(-300f,-60f,0f);

	float cloudMinScale1 = 1.51f; float cloudMaxScale1 = 2.01f;
	float cloudMinScale2 = 0.40f; float cloudMaxScale2 = 1.01f;

	float cloudFastSpeed1 =  7f; float cloudSlowSpeed1 = 12f;
	float cloudFastSpeed2 = 18f; float cloudSlowSpeed2 = 22f;

	IEnumerator RespwanCloud(int cloudNum){
		float cloudDuration;
		float cloudScale;

		if(cloudNum==1) {
			cloudScale = Random.Range(cloudMinScale1,cloudMaxScale1);
			cloudDuration = Random.Range(cloudFastSpeed1,cloudSlowSpeed1);

			CloudClone1.transform.localScale = Vector3.one * cloudScale;
			cloudMove1.from = cloudFrom;
			cloudMove1.to = cloudTo;
			cloudMove1.duration = cloudDuration;
			cloudMove1.enabled = true;
			cloudMove1.ResetToBeginning();
			yield return new WaitForSeconds(cloudDuration);
			if(cloudMove1!=null) cloudMove1.enabled = false;
		} else {
			cloudScale = Random.Range(cloudMinScale2,cloudMaxScale2);
			cloudDuration = Random.Range(cloudFastSpeed2,cloudSlowSpeed2);

			CloudClone2.transform.localScale = Vector3.one * cloudScale;
			cloudMove2.from = cloudFrom;
			cloudMove2.to = cloudTo;
			cloudMove2.duration = cloudDuration;
			cloudMove2.enabled = true;
			cloudMove2.ResetToBeginning();
			yield return new WaitForSeconds(cloudDuration);
			if(cloudMove2!=null) cloudMove2.enabled = false;
		}

		if(cloudMove1!=null) StartCoroutine ( RespwanCloud(cloudNum) );
	}


	float FlowerPosX(int x) { return (x *  g.tileWidth  * g.posRatio); }
	float FlowerPosY()      { return (g.tileHeightCnt * -g.tileHeight * g.posRatio) + 110; }
	float flowerPosY;
	public void SetFlowerPreFab(int x){
		//------------------------
		// Instantiate Prefab
		//------------------------
		GameObject FlowerClone = Instantiate(FlowerPrefab) as GameObject;
		FlowerClone.name = "Flower" + x;		
		FlowerClone.tag = "Flower";
		FlowerClone.transform.parent = gameState.Grid.transform;
		FlowerClone.transform.localPosition = new Vector3(FlowerPosX(x),flowerPosY,0f);
		FlowerClone.transform.localScale = Vector3.one * g.scaleRatio;
		gameComponent.SetFlowerPrefab(FlowerClone.transform);

		//------------------------
		// Event key
		//------------------------
		EventDelegate eventClick = new EventDelegate(this, "EventFlowerOnPress");
		EventDelegate.Parameter param = new EventDelegate.Parameter();
		param.obj = FlowerClone.transform;
		param.expectedType = typeof(Transform);
		eventClick.parameters[0] = param;
		EventDelegate.Add (gameComponent.flowerUIEventTrigger.onPress,eventClick);
		
		//------------------------
		// Initiate spriteName
		//------------------------
		gameComponent.flowerUISprite.spriteName = "flower_ani001";
		//gameComponent.flowerUISprite.MakePixelPerfect();

		//------------------------
		// set flower's array
		//------------------------
		flowerSprite[x] = gameComponent.flowerUISprite;
		flowerTransform[x] = FlowerClone.transform;

		flowerSweatSprite[x] = gameComponent.flowerSweatUISprite;

		flowerSweatAnimator[x] = gameComponent.flowerSweatAnimator;
		flowerAnimator[x] = gameComponent.flowerAnimator;

	}

	float letterFastSpeed = 2.01f; // fast
	float letterSlowSpeed = 7.01f; // slow
	int letterFontMinSize = 60;
	int letterFontMaxSize = 120;
	int showLetterMaxIdx;
	public IEnumerator RandomLetter (int x){

		//Debug.Log( "RandomLetter");
		//------------------------
		// Instantiate Object
		//------------------------
		GameObject LetterClone = Instantiate(LetterPrefab) as GameObject;

		g.LetterClone[x] = LetterClone;

		//------------------------
		// Hierarchy
		//------------------------
		LetterClone.transform.parent = gameState.Grid.transform;

		//------------------------
		// Position,Scale
		//------------------------
		g.letterStartPos[x] = new Vector3(FlowerPosX(x), 200f, 0f);
		LetterClone.transform.localPosition = g.letterStartPos[x];
		LetterClone.transform.localScale = Vector3.one * g.scaleRatio;

		//------------------------
		// Set Letter 
		//------------------------
		// call letterUILabel
		gameComponent.SetLetterPrefab(LetterClone.transform);
		gameComponent.letterEffectLabel.gameObject.SetActive(false);
		UILabel letterUILabel = gameComponent.letterUILabel;

		// fontSize
		letterUILabel.fontSize = Random.Range(letterFontMinSize,letterFontMaxSize);

		// Outline
		letterUILabel.effectStyle = UILabel.Effect.Outline;
		letterUILabel.effectColor = Color.white;

		// Animation letter's scale
		Animator letterAnimator = letterUILabel.GetComponent<Animator>();
		//letterAnimator.runtimeAnimatorController = Resources.Load("Animators/RainGame") as RuntimeAnimatorController;
		letterAnimator.runtimeAnimatorController = Resources.Load("Animators/RainGame") as RuntimeAnimatorController;
		letterAnimator.SetBool("isLetterWiggle",true);

		// set showing letter
		showLetterMaxIdx = g.texName.Length - g.correctLetterIdx > 2 ? 2 : g.texName.Length - g.correctLetterIdx;
		string showingLetter = g.texName.Substring(g.correctLetterIdx,showLetterMaxIdx);
		if (g.correctLetterIdx == g.texName.Length) 
			showingLetter = g.texName.Substring(g.texName.Length-1,1);
		int letterIdx = Random.Range(0,showingLetter.Length);

		// text
		letterUILabel.text = showingLetter[ letterIdx ].ToString();

		// color
		letterUILabel.color = g.letterColors[Random.Range(0, 7)];

		// LetterTile.name
		LetterClone.name = showingLetter[letterIdx].ToString() + x;

		//------------------------
		// Move Letter
		//------------------------
		letterUILabel.text = showingLetter[letterIdx].ToString();

		TweenPosition letterTweenPos = LetterClone.GetComponent<TweenPosition>();
		float letterTweenPosSpeed = Random.Range(letterFastSpeed,letterSlowSpeed);

		letterTweenPos.from = LetterClone.transform.localPosition;
		letterTweenPos.to = new Vector3(letterTweenPos.from.x, flowerPosY + 20f , letterTweenPos.from.z);
		letterTweenPos.duration = letterTweenPosSpeed;
		letterTweenPos.enabled = true;
		letterTweenPos.ResetToBeginning();

		//letterTweenPos.onFinished
		yield return new WaitForSeconds(letterTweenPosSpeed);

		if(LetterClone != null) {
			//--------------------------
			// letter break ani
			//--------------------------
			//letterTweenPos.enabled = false;
			letterTweenPos.enabled = false;

			//letterAnimator.SetBool("isLetterWiggle",false);

			//Debug.Log("Excess["+x+"]="+LetterClone.transform.localPosition.y+" flowerPosY="+flowerPosY);

			if ( Random.Range(1,3) == 1 )
				letterAnimator.SetBool("isLetterBrokenLeft",true);
			else letterAnimator.SetBool("isLetterBrokenRight",true);
			yield return new WaitForSeconds(0.5f);

			//--------------------------
			// Destroy
			//--------------------------
			Destroy(LetterClone);

			if(g.isGameTurning) StartCoroutine ( RandomLetter(x) );
		}


	}

	//----------------------------------------------------------
	//                       Event
	//----------------------------------------------------------
	public void EventFlowerOnPress(Transform flowerClone){

		int flowerIdx = int.Parse(flowerClone.gameObject.name.Substring(6,1));
		//Debug.Log("EventFlowerOnPress["+flowerIdx+"]");

		if(!isAni[flowerIdx]) StartCoroutine(FlowerAni(flowerIdx));
	}

	float flowerAniFrameTime = 0.06f;
	float eatingAniFrameTime = 0.02f;
	int minDistMouthLetter = 60;
	int maxDistMouthLetter = 90;
	int eatingAniCnt = 3;
	IEnumerator FlowerAni(int flowerIdx){
		float dist;
		bool isEatSuccess = false;
		isAni[flowerIdx] = true;

		for(int i=0;i<flowerAni.Length;i++) {

			//-----------------------------
			// Play flower animation
			//-----------------------------
			flowerSprite[flowerIdx].spriteName = flowerAni[i];

			//-----------------------------------
			// Check1 flower's open-mouth Zone
			//-----------------------------------
			if( i > 5 && i < 13 ) {

				//-----------------------------
				// if EatSuccess, Loop Skip
				//-----------------------------
				if(isEatSuccess) continue;

				//-----------------------------------------
				// Check2 distance open-mouth and letter
				//-----------------------------------------
				// if letter'pos is lower than flower'pos
				dist = distLetterFlower(flowerIdx);
				if( dist > minDistMouthLetter && dist< maxDistMouthLetter ) {
					//Debug.Log("eat["+flowerIdx+"] sucess="+dist);

					//-----------------------------------------
					// Check3 eating letter is correct 
					//-----------------------------------------
					CheckCorrectLetter(flowerIdx);
                    // limit time quit
                    if (g.correctLetterIdx == g.texName.Length) { g.isGameTurning = false; }
					//-----------------------------------------
					// Destroy letter
					//-----------------------------------------
					Destroy( g.LetterClone[flowerIdx]);

					//-----------------------------------------
					// Play eating Animation
					//-----------------------------------------
					flowerSprite[flowerIdx].spriteName = flowerEatAni[0];
					yield return new WaitForSeconds(eatingAniFrameTime);

					for(int k=1;k<=eatingAniCnt;k++){
						for(int j=1;j<flowerEatAni.Length;j++) {
							flowerSprite[flowerIdx].spriteName = flowerEatAni[j];
							yield return new WaitForSeconds(eatingAniFrameTime);
						}					
					}
					isEatSuccess = true;

					StartCoroutine ( RandomLetter(flowerIdx) );

					//-----------------------------------------
					// Navigation to TurnClear
					//-----------------------------------------
					if (g.correctLetterIdx == g.texName.Length) {
						gameControl.LevelPassStart("clear");
					}
				}
			}

			yield return new WaitForSeconds(flowerAniFrameTime);
	    }
		isAni[flowerIdx] = false;

		//-------------------------
		// Stop flower animation
		//-------------------------
		if( flowerTransform[flowerIdx] != null ) {
			flowerSweatAnimator[flowerIdx].SetBool("isSweatFlicker",false);
			flowerAnimator[flowerIdx].SetBool("isFlowerColor",false);
			flowerSweatSprite[flowerIdx].enabled = false;
			flowerSprite[flowerIdx].color = Color.white;
		}
	}

	void CheckCorrectLetter(int flowerIdx){

		//Debug.Log("CheckCorrect["+flowerIdx+"]");

		string hitLetter = g.LetterClone[flowerIdx].transform.FindChild("LetterLabel").GetComponent<UILabel>().text;
		if( hitLetter == g.texName[g.correctLetterIdx].ToString() ) {

			// TTS
			gameBasic.PlayTTS(g.texShowName[g.correctLetterIdx].ToString());

			gameBasic.CorrectWordColor(g.correctLetterIdx);
			g.correctLetterIdx++;
		} else if(g.correctLetterIdx > 0 && hitLetter != g.texName[g.correctLetterIdx].ToString() ) {
			g.correctLetterIdx--;
			gameBasic.CorrectWordColor(g.correctLetterIdx-1);
			PlayFlowerSweatAni(flowerIdx);
		} else if(g.correctLetterIdx == 0) {
			PlayFlowerSweatAni(flowerIdx);
		}
	}


	void PlayFlowerSweatAni(int flowerIdx) {

		// Vibrate
		if(g.isVibrate) Handheld.Vibrate();

		flowerSweatSprite[flowerIdx].enabled = true;
		flowerSweatAnimator[flowerIdx].SetBool("isSweatFlicker",true);
		//flowerSprite[flowerIdx].color = Color.red;
		flowerAnimator[flowerIdx].SetBool("isFlowerColor",true);
	}

	float distLetterFlower(int flowerIdx) {
		float dist = Mathf.Abs(flowerTransform [flowerIdx].localPosition.y) - Mathf.Abs(g.LetterClone [flowerIdx].transform.localPosition.y);
		//Debug.Log("dist f "+flowerIdx+"="+Mathf.Abs(flowerTransform [flowerIdx].localPosition.y));
		//Debug.Log("dist l "+flowerIdx+"="+Mathf.Abs(g.LetterClone [flowerIdx].transform.localPosition.y));
		//Debug.Log("dist["+flowerIdx+"]="+dist);
		return dist;
	}


}
