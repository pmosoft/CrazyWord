using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;


public class GameShooting : MonoBehaviour {

	public GameControl gameControl;
	public GameBasic gameBasic;
	public GameTexture gameTexture;
	public GameHint gameHint;
	public GameMole gameMole;

	public GameState gameState;
	public GameComponent gameComponent;

	public LevelPass levelPass;

	public EventCommon ev;
	
	//private bool isSetLetter = false;
	Transform gridTile;
	Animator animator;
	string TileName;
	
	UILabel letterUILabel;
	public GameObject LetterPrefab;
	public GameObject ArrowGunPrefab;
	public GameObject BowGunPrefab;

	int tileWidthCnt = 5;
	int tileHeightCnt = 5;

	int bowCnt = 3;


	Transform arrowGunClone;

	TweenPosition tileTweenPos;
	Vector3 localPos;

	public void GameShootingStart(){

		Debug.Log("GameShooting");
		//---------------------------------------------
		// Setting State
		//---------------------------------------------
		gameState.StateGameLevel("shooting");
		gameState.StateShootingGamePanel();
		
		g.isGameTexture = false;
		g.isGameTurning = true;

		//gameComponent.gameTextureBoxCollider.enabled = false;

		//---------------------------------------------
		// Init variable
		//---------------------------------------------
		gameTexture.SetTurnTexture();

		g.col = gameComponent.gameTextureUISprite.color;

        Color frameCol = g.GetRandomColor();
		gameComponent.gameTextureUISprite.color = frameCol;
        gameComponent.pictureFrameUISprite.color = frameCol;
		
		gameComponent.gameIconUISprite.spriteName = "shootinggameicons";
		//gameComponent.gameIconUISprite.MakePixelPerfect();

		gameComponent.clickWordUILabel.text = g.texName;

		g.correctLetterIdx = 0;

		g.tileWidthCnt = tileWidthCnt;
		g.tileHeightCnt = tileHeightCnt;

		g.arrowShootCnt = 0;

		//---------------------------------------------
		// LimitTime
		//---------------------------------------------
		g.limitTime = g.limitBaseTime * g.texName.Length;
		gameComponent.limitUILabel.text = g.limitTime.ToString();
		gameComponent.limitLineUILabel.text = g.limitTime.ToString();
		
		StartCoroutine(gameControl.LimitTime());



		SetArrowGun();

		g.letterColorR = Random.Range(0.0f,1.0f);
		g.letterColorG = Random.Range(0.0f,1.0f);
		g.letterColorB = Random.Range(0.0f,1.0f);



		//---------------------------------------------
		// Setting Letter
		//---------------------------------------------
		for(int i=0;i<3;i++) StartCoroutine ( RandomLetter(g.tileWidthCnt,g.tileHeightCnt,i,false) );

		levelPass.LevelPassButtonStart();

	}

	public void SetArrowGun(){

		g.TileRatio();

		for (int x = 0; x < bowCnt; x++) {
			SetArrowGunPreFab(x,g.tileHeightCnt);
			SetBowGunPreFab(x,g.tileHeightCnt);
		}
	}

	public void SetBowGunPreFab(int x,int y){

		//------------------------
		// Instantiate Prefab
		//------------------------
		GameObject bowGunPrefab = Instantiate(BowGunPrefab) as GameObject;
		bowGunPrefab.name = "Bow" + x + y;
		
		//arrowGunPrefab.tag = x == 0 ? "ArrowLeft" : "ArrowRight";
		bowGunPrefab.tag = "Bow";
		bowGunPrefab.transform.parent = gameState.Grid.transform;
		bowGunPrefab.transform.localPosition = new Vector3( (x * g.tileWidth * 1.3f) + 82
		                                                     ,(y * -g.tileHeight) + 70
		                                                     ,0f);
		bowGunPrefab.transform.localScale = Vector3.one;
		gameComponent.SetBowPrefab(bowGunPrefab.transform);

		//------------------------
		// Event key
		//------------------------
		EventDelegate eventClick = new EventDelegate(this, "EventBowOnPress");
		EventDelegate.Parameter param = new EventDelegate.Parameter();
		param.obj = bowGunPrefab.transform;
		param.expectedType = typeof(Transform);
		eventClick.parameters[0] = param;
		EventDelegate.Add (gameComponent.bowUIEventTrigger.onPress,eventClick);

		//------------------------
		// array arrowSprite
		//------------------------
		gameComponent.bowUISprite.spriteName = "arrow4";
		//gameComponent.bowUISprite.MakePixelPerfect();

		g.bowSprite[x] = gameComponent.bowUISprite;
		g.bowBox[x] = gameComponent.bowBoxcollider;
		//Debug.Log("g.arrowBox.Length="+g.bowBox.Length);
	}

	public GameObject SetArrowGunPreFab(int x,int y){

		//------------------------
		// Instantiate Prefab
		//------------------------
		GameObject arrowGunPrefab = Instantiate(ArrowGunPrefab) as GameObject;
		arrowGunPrefab.name = "Arrow" + x + y;

		//arrowGunPrefab.tag = x == 0 ? "ArrowLeft" : "ArrowRight";
		arrowGunPrefab.tag = "Arrow";
		arrowGunPrefab.transform.parent = gameState.Grid.transform;

		arrowGunPrefab.transform.localPosition = new Vector3( (x * g.tileWidth * 1.3f) + 82
		                                                     ,(y * -g.tileHeight) + 70
		                                                     ,0f);
		arrowGunPrefab.transform.localScale = Vector3.one;
		gameComponent.SetArrowPrefab(arrowGunPrefab.transform);

		arrowGunPrefab.GetComponent<GameShootingTrigger>().gameShooting = gameState.gameShooting;
		arrowGunPrefab.GetComponent<GameShootingTrigger>().gameBasic = gameState.gameBasic;
		arrowGunPrefab.GetComponent<GameShootingTrigger>().gameComponent = gameState.gameComponent;
		arrowGunPrefab.GetComponent<GameShootingTrigger>().gameControl = gameState.gameControl;

		//------------------------
		// array arrowSprite
		//------------------------
		gameComponent.arrowUISprite.spriteName = "arrow1";
		//gameComponent.arrowUISprite.MakePixelPerfect();
		//gameComponent.arrowUISprite.renderer.enabled = false;

		g.arrowTransform[x] = arrowGunPrefab.transform;


		g.arrowSprite[x]  = gameComponent.arrowUISprite;

		g.arrowSprite[x].enabled = false;
		return arrowGunPrefab;
	}


	int x; int showLetterMaxIdx;
	float letterScaleX;
	public IEnumerator RandomLetter (int widthCnt,int heightCnt,int y,bool isFirst ){
		string showingLetter = "";

		//Debug.Log("RandomLetter");


		UILabel letterUILabel;
		int letterIdx;

		float pRatio = 1.0f;
		float sRatio = 1.0f;
		
		g.tileWidthCnt = widthCnt;
		g.tileHeightCnt= heightCnt;
		
		if     (g.tileWidthCnt == 5)  { pRatio = 1.0f;  sRatio = 1.0f; }
		//if     (g.tileWidthCnt == 5)  { pRatio = 0.45f; sRatio = 0.5f; }
		else if(g.tileWidthCnt == 10) { pRatio = 0.45f; sRatio = 0.5f; }
		

		//x = Random.Range(1,g.tileWidthCnt);
				
		//------------------------
		// Instantiate Object
		//------------------------
		GameObject tileButtonPrefab = Instantiate(LetterPrefab) as GameObject;
		GameObject LetterTile = new GameObject();

		//Debug.Log("x="+x);


		//Debug.Log("Tile="+LetterTile.name);

		LetterTile.tag = "Tile";
		LetterTile.AddComponent<TweenPosition>();
		//------------------------
		// Hierarchy
		//------------------------
		LetterTile.transform.parent = gameState.Grid.transform;
		tileButtonPrefab.transform.parent = LetterTile.transform;
		
		//------------------------
		// Position,Rotation,Scale
		//------------------------
		//LetterTile.transform.localPosition = new Vector3( x * tileWidth, y * -tileHeight , 0f);
		LetterTile.transform.localPosition = new Vector3(-60
		                                                ,(y * -g.tileWidth * pRatio) -30 
		                                                ,0f);
		LetterTile.transform.localScale = Vector3.one * sRatio;

		gameComponent.SetLetterPrefab(tileButtonPrefab.transform);
		gameComponent.letterEffectLabel.gameObject.SetActive(false);
		letterUILabel = gameComponent.letterUILabel;
		letterUILabel.fontSize = Random.Range(80,180);

		// letter outline
		//letterUILabel.effectStyle = UILabel.Effect.Outline;
		//letterUILabel.effectColor = Color.white;
		gameComponent.letterScale.style = UITweener.Style.Loop;
		gameComponent.letterScale.duration = 1f;
//		letterScaleX = 1.2f;
//		gameComponent.letterScale.to  = Vector3.one * letterScaleX;
		//gameComponent.letterScale.animationCurve
		//gameComponent.letterAnimator.runtimeAnimatorController = "MoleLetter";

//		AnimationCurve animationCurve = new AnimationCurve(
//			new Keyframe(0.0f,1.0f,1.0f,1f),
//			new Keyframe(0.5f,letterScaleX,letterScaleX,1f),
//			new Keyframe(1.0f,1.0f,1.0f,1f)
//			);
//		gameComponent.letterScale.animationCurve = animationCurve;
		animator = gameComponent.letterLabel.GetComponent<Animator>();
		animator.SetBool("isLetterWiggle",true);



		showLetterMaxIdx = g.texName.Length - g.correctLetterIdx > 2 ? 2 : g.texName.Length - g.correctLetterIdx;
		showingLetter = g.texName.Substring(g.correctLetterIdx,showLetterMaxIdx);
		letterIdx = Random.Range(0,showingLetter.Length);
		LetterTile.name = showingLetter[ letterIdx ].ToString() + y;

		letterUILabel.text = showingLetter[ letterIdx ].ToString();
		//letterUILabel.color = new Color(g.letterColorR,g.letterColorG,g.letterColorB,1.0f);
		letterUILabel.color = g.letterColors[Random.Range(0, 7)];

		//------------------------
		// Move Letter
		//------------------------
		tileTweenPos = LetterTile.GetComponent<TweenPosition>();

		localPos = LetterTile.transform.localPosition;

		tileTweenPos.from = localPos;
		tileTweenPos.to = new Vector3(g.tileWidthCnt * g.tileWidth * pRatio +200
		                              ,localPos.y,localPos.z);
		
		float speed1 = Random.Range(letterMinSpeed,letterMaxSpeed);
		tileTweenPos.duration = speed1;
		//tileTweenPos.delay = isFirst ? Random.Range(0.01f,2.99f) : 0f;
		tileTweenPos.enabled = true;

		// if Collision, break.
		// LetterTile's position must be low than Grid's bottom position.
		// Only GameTurning must be executed.
		while(g.isGameTurning)  
		{
			// Collision or turnSkip,turnClear
			if(tileButtonPrefab==null) break;
			// Excess Range
			if(LetterTile.transform.localPosition.x >= (g.tileWidthCnt-0.5) * g.tileWidth * pRatio - 40) break;
					
			if( LetterTile.transform.localPosition.x < -10 ) {
				letterUILabel.text = "";
			} else {
				letterUILabel.text = showingLetter[ letterIdx ].ToString();
			}	
			yield return new WaitForSeconds(0.1f);
		}

		if(LetterTile != null) Destroy(LetterTile);

		if(g.isGameTurning) StartCoroutine ( RandomLetter(g.tileWidthCnt,g.tileHeightCnt,y,true) );
	}


	float letterMinSpeed = 2f;
	float letterMaxSpeed = 10f;
	float arrowSpeed = 1f; //0.5f

	//----------------------------------------------------------
	//                       Event
	//----------------------------------------------------------
	float arrowEndPosY;
	Transform arrowClone;
	//float prevButtonTime = 0;
	public bool isTouched = false;
	public void EventBowOnPress(Transform bowClone){

		gameComponent.EnableBowBoxCollider(false);


		//Debug.Log("start:"+System.DateTime.Now);

		g.arrowShootCnt++;

		//Debug.Log("EventArrowOnPress");
		gameComponent.SetArrowPrefab(bowClone);

		//gameComponent.bowUISprite.spriteName = "arrow1";
		//gameComponent.bowUISprite.MakePixelPerfect();
		//bowClone.name.Substring(4,1);
		//gameComponent.arrowUISprite.renderer.enabled = true;
		arrowClone = g.arrowTransform[int.Parse(bowClone.name.Substring(3,1))];
		gameComponent.SetArrowPrefab(arrowClone);


		localPos = arrowClone.localPosition;
		//Debug.Log("localPos.x="+localPos.x);

		g.arrowIdx = int.Parse(arrowClone.name.Substring(5,1));
		//Debug.Log("arrowIdx="+g.arrowIdx);
		gameComponent.arrowUISprite.enabled = true;

		arrowGunClone = arrowClone;
		arrowEndPosY = (g.tileWidth * g.posRatio) - 100;
		//g.bowBox[g.arrowIdx].enabled = true;

		HOTween.To(arrowClone, arrowSpeed, new TweenParms()
		           .Prop("localPosition", new Vector3(localPos.x,arrowEndPosY,localPos.z)) 
		           .Ease(EaseType.Linear)
		           .OnComplete( AfterTween ) // Ease
		);

		StartCoroutine( BowEffect (g.bowSprite[ g.arrowIdx ]) );


	}

	IEnumerator TouchDelay(){
		isTouched = true;
		yield return new WaitForSeconds(3.0f);
		isTouched = false;
	}

	float bowShake = 0.02f;
	IEnumerator BowEffect( UISprite sp) {
		for(int i=0;i<5;i++) {
			sp.spriteName = "arrow3"; yield return new WaitForSeconds(bowShake);
			sp.spriteName = "arrow2"; yield return new WaitForSeconds(bowShake);
		}
	}

	void AfterTween() {
		//Debug.Log("hotween g.arrowDir="+g.arrowDir);

		Destroy(arrowGunClone.gameObject);
		SetArrowGunPreFab(g.arrowIdx,g.tileHeightCnt);
		gameComponent.EnableBowBoxCollider(true);

		//g.arrowDir = g.arrowDir==0 ? g.tileWidthCnt-1 : 0;
		g.bowSprite[ g.arrowIdx ].spriteName = "arrow4";

	}

}
