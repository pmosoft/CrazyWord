using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;


public class GameControl : MonoBehaviour {

	//----------------------------------------------------------------------
	//                           GameControlZone
	//----------------------------------------------------------------------
	public GameBasic gameBasic;
	public GameTexture gameTexture;
	public GameShooting gameShooting;
	public GameRain gameRain;
	public GameMole gameMole;
	public GameBlank gameBlank;
	public GameHint gameHint;
	public GameBonus gameBonus;
	public GameState gameState;
	public GameComponent gameComponent;

	public LevelPass levelPass;
	public Account account;
	public DownTexture downTexture;

	public DBO dbo; public DBOW dbow; public Navi navi; public EventCommon ev;

	//--------------------------------------------------------- 
	// Control : time
	//---------------------------------------------------------
	float gameWordTweenDelayTime = 1.5f;     // 1.5f

	//---------------------------------------------------------
	// Control : count
	//---------------------------------------------------------
	int stageMaxSkip = 2; //2
	int bonusExeTurn5 = 5;    // 5

	//---------------------------------------------------------
	// 
	//---------------------------------------------------------
    public GameObject ScreenPopupBg14;


	//---------------------------------------------------------
	// sqlite
	//---------------------------------------------------------
	DataTable _data; DataRow dr;

    //---------------------------------------------------------
    // Sound
    //---------------------------------------------------------
    public AudioClip gamestartClip, menuClip, gameoverClip, clearClip, backspaceClip, startClip, timeoverClip;
    public AudioSource audioSource, timeoverAudioSource;

  	List<string> gameTips = new List<string>();

	//----------------------------------------------------------------------
	//                             ControlZone
	//----------------------------------------------------------------------
	void ControlZone____________() {}

    void Awake(){
        gameTips.Add("게임에서는 스킵을 두번만 사용할 수 있습니다. 아껴 써야 해요.");
        gameTips.Add("상점에서 페이스북 담벼락에 추천하시면 보너스 코인을 드립니다.");
        gameTips.Add("주제별 생활 활용 단어 순서로 외워보세요. 바로바로 생활 속에 활용해 보세요.");
        gameTips.Add("게임이 끝난 후 배경이미지를 떠올려 보세요. 단어 뜻이 바로 생각 날 거에요.");
        gameTips.Add("올팩을 구입하시면 모든 기능을 제한없이 사용할 수 있습니다.");
        gameTips.Add("예문(Word expression)과 워드북(Wordbook)으로 예습/복습을 하시면 게임이 더욱더 즐거워집니다.");
        gameTips.Add("[크레이지워드]에서 빠른 학습력과 학습효과의 성취를 위해서 음성지원을 제공해 드리고 있습니다.");
        gameTips.Add("기계음성이라 학습에 적합하지 않을 수 있고 귀에 거슬리실 수 있습니다. 정확한 음성/다양한 뜻은 [워드북]에서 [고급검색]아이콘을 탭해주세요.");
        gameTips.Add("단어를 콤보로 클리어하시면 별이 생기고 별을 10개 모일때마다 보너스 아이템 생깁니다.");   
        gameTips.Add("크레이지워드 타버젼 동시 수행시 TTS 설정이 변경될수 있습니다.");   
    }

	public void Start() {
        //string query;
        //test03();
        //query = "drop table imsi03;";
        //dbo.ExcuteWwwQuery(query);
        //query = "create table imsi03 ( c1 int, c2 int);";
        //dbo.ExcuteWwwQuery(query);

        //query = "create table imsi03 ( c1 int, c2 int);";
        //dbo.ExcuteWwwQuery(query);
        //imsi
       
        StartCoroutine( GameControlStart() );

	}

	public IEnumerator GameControlStart() {

        // imsi tester
        //if(g.userId == "110217636682633959576") dbo.UptSkipImsi(); // psh
        //if(g.userId == "108496762632519304787") dbo.UptSkipImsi(); // kds

		//gameComponent.EnableGameSceneButtonBoxCollider(false);
		//---------------------------------------------
		// for test, temporaray
		//---------------------------------------------
        #if     UNITY_EDITOR
                //dbo.UptSkipImsi(); // crazy word
                
                // 맵의 curStage 정보 클리어 오류발생
                dbo.SelUserInfo();
                yield return StartCoroutine(dbow.SelUserInfo());
                g.wwwDate = System.DateTime.Now.ToString("yyyyMMdd");
        #elif    UNITY_ANDROID

        #endif


        //yield return new WaitForSeconds(10f);

		//---------------------------------------------
		// Setting State
		//---------------------------------------------
		g.stageTurn = 1;
		g.comboCnt = 0;
		g.bonusCnt = 0;
		g.stageSkip = stageMaxSkip;

		gameComponent.comboGameBoxCollider.enabled = false;
        gameComponent.skipBoxCollider.enabled = true;
        gameComponent.mapBottomBoxCollider.enabled = true;

		g.easyGameCnt = 0;
		SetRandomEasyGame();

        gameState.StateShowTexture();
        //ScreenPopupBg14.SetActive(false);

        //---------------------------------------------
        // Banner
        //---------------------------------------------
        if (g.noBannerYn == "Y") {
            gameComponent.bannerUISprite.spriteName = "buttonbg1x22";
            #if UNITY_ANDROID && !UNITY_EDITOR
                AdMobAndroid.hideBanner(true);
            # endif

        } else {
            gameComponent.bannerUISprite.spriteName = "baner";
            #if UNITY_ANDROID && !UNITY_EDITOR
                AdMobAndroid.hideBanner(false);
            # endif
        }

		//--------------------
		// Sound
		//--------------------
        if(g.isSound == true) {
            audioSource.enabled = true;
            audioSource.volume = g.buttonVolume; 
            timeoverAudioSource.volume = 1f;
            gameComponent.soundUISprite.spriteName = "soundnyes";
        } else {
            audioSource.volume = 0f; timeoverAudioSource.volume = 0f;
            gameComponent.soundUISprite.spriteName = "soundno";
        }

		//---------------------------------------------
		// download texture
		//---------------------------------------------
        if (g.curStage > 0)
        {
            gameComponent.tipTextUILael.text = gameTips[Random.Range(0, gameTips.Count)];
            gameState.StateDownLoadTexture(true);
            yield return StartCoroutine( downTexture.StartDownload("real") );
            gameState.StateDownLoadTexture(false);
        }

        //---------------------------------------------
        // Setting Backgroud Image
        //---------------------------------------------
        gameBasic.SetBackgroudColor();
        gameComponent.gameSceneImgUITexture.mainTexture = g.userBgImgTextures[g.userBgImg];

		//---------------------------------------------
		// Setting random Backgroud
		//---------------------------------------------
		gameBasic.SetBackgroudColor();
		
		//------------------------------------------------
		// Setting stage all-turn textures from local DB
		//------------------------------------------------
		SetStageTurnTextures();
         
		//---------------------------------------------
		// ==> Setting random turn texture in advance
		//---------------------------------------------
		gameTexture.SetTurnTexture();

		//---------------------------------------------
		// Fade in
		//---------------------------------------------
		yield return StartCoroutine (navi.FadeIn ());
        
		//---------------------------------------------
		// Set HP info
		//---------------------------------------------
		account.HpInfo();

		//---------------------------------------------
		// Setting GamePanel UILabel
		//---------------------------------------------
		SetGamePanelUILabels();

		//---------------------------------------------
		// Rank ProgressBar
		//---------------------------------------------
        g.col = g.GetRandomColor();
        gameComponent.rankBarForeBg0UISprite.color = g.col;
        gameComponent.rankBarForeBgUISprite.color = g.col;
        account.SetCurStageVitualMaxScore();

		//---------------------------------------------
		// Navigation
		//---------------------------------------------
		StartCoroutine( gameTexture.ShowTexture() );
	}

    
    string kind2;
	public void GameStart(string kind){
		
		Debug.Log("GameStart kind="+kind);

		RandomTurnTexNum();

		//-------------------------------
		// BoxCollider
		//-------------------------------
		gameComponent.EnableGameSceneCommonBoxCollider(true);
        timeoverAudioSource.Stop();

        //kind = "hint";


        if (kind == "memory")
        {

			gameBasic.GameMemoryStart(); 

			//gameRain.GameRainStart();
			//gameMole.gameObject.SetActive(true);
			//gameMole.GameMoleStart();

			//gameBlank.gameObject.SetActive(true);
			//gameBlank.GameBlankStart();
			//gameHint.GameHintStart();
			//gameShooting.GameShootingStart();

		} else if (kind=="easyRandomGame") {

			//gameComponent.levelSkipBoxCollider.enabled = true;
			gameComponent.levelSkipUISprite.spriteName = "levelskip";
			gameComponent.levelSkipUISprite.MakePixelPerfect();
			gameComponent.levelSkipAnimator.SetBool("isButtonWiggle",true);
			g.comboCnt = 0;

			//gameRain.GameRainStart();
			kind2 = g.easyRandomGame[g.easyGameCnt];

			if        (kind2=="shooting") {
				//Debug.Log("GameShootingStart");
				gameShooting.gameObject.SetActive(true);
				gameShooting.GameShootingStart();
			} else if (kind2=="rain") {
				//Debug.Log("GameRainStart");
				gameRain.gameObject.SetActive(true);
				gameRain.GameRainStart();
			} else if (kind2=="mole") {
				//Debug.Log("GameMoleStart");
				gameMole.gameObject.SetActive(true);
				gameMole.GameMoleStart();
			} else if (kind2=="blank") {
				//Debug.Log("GameBlankStart");
				gameBlank.gameObject.SetActive(true);
				gameBlank.GameBlankStart();
			}

		} else if (kind=="hint") {
			//Debug.Log("GameHintStart");
			gameHint.GameHintStart();

		} else if (kind=="bonus") {
			//Debug.Log("GameBonusStart");
			//gameHint.gameObject.SetActive(true);
			//gameHint.GameHintStart();
		}

	}

	void SetRandomEasyGame(){
		
		List<string> easyGameList  = new List<string>();
		for(int i=0;i<g.easyGameList.Length;i++) {
			//Debug.Log("g.easyGameList["+i+"]="+g.easyGameList[i]);
			easyGameList.Add(g.easyGameList[i]);
		}
		
		for (int i = 0; i < g.easyRandomGame.Length; i++) {
			int randomIndex = Random.Range(0, easyGameList.Count-1); // except mole
			//Debug.Log("easyGameList.Count="+easyGameList.Count);
			//Debug.Log("randomIndex="+randomIndex);
			//Debug.Log("easyGameList["+randomIndex+"]="+easyGameList[randomIndex]);
			
			g.easyRandomGame[i] = easyGameList[randomIndex];
			easyGameList.RemoveAt(randomIndex);
			
			//Debug.Log("g.easyRandomGame["+i+"]="+g.easyRandomGame[i]);
		}

        // mole game only 1 times 10
        if (Random.Range(0, 5) == 0) {
            int moleIndex = Random.Range(0, easyGameList.Count - 1);
            //g.easyRandomGame[moleIndex] = g.easyGameList[3];
            g.easyRandomGame[2] = g.easyGameList[3];
        }

        // imsi test "shooting","rain","blank","mole"
		//g.easyRandomGame[0] = g.easyGameList[2];
		//g.easyRandomGame[1] = g.easyGameList[2];
		//g.easyRandomGame[2] = g.easyGameList[2];
		
	}


	//----------------------------------------------------------
	//                        Event
	//----------------------------------------------------------
	public void EventComboGameOnRelease(){	
		Debug.Log("EventGameOnRelease");
		ev.EventOnRelease();
		if (ev.isCurPrevDist) {
			//gameComponent.EnableGameSceneButtonBoxCollider(false);
            audioSource.clip = gamestartClip; audioSource.Play();

			gameComponent.comboGameBoxCollider.enabled = false;
			gameComponent.EnableGameSceneCommonBoxCollider(false);
			//StartCoroutine ( EventDelayStart("memory"));
			GameStart("memory");
		}
	}
	
	public void EventHardGameOnRelease(){	
		Debug.Log("EventGameOnRelease");
		ev.EventOnRelease();
		if (ev.isCurPrevDist) {
            audioSource.clip = gamestartClip; audioSource.Play();
            gameComponent.hardGameBoxCollider.enabled = false;
			gameComponent.EnableGameSceneCommonBoxCollider(false);
			StartCoroutine ( EventDelayStart("memory"));
		}
	}
	
	public void EventEasyOnRelease(){	
		Debug.Log("EventGameOnRelease");
		ev.EventOnRelease();
		if (ev.isCurPrevDist) {
            audioSource.clip = gamestartClip; audioSource.Play();
			gameComponent.easyGameBoxCollider.enabled = false;
			gameComponent.EnableGameSceneCommonBoxCollider(false);
			StartCoroutine ( EventDelayStart("easyRandomGame"));
			//GameStart("easyRandomGame");
		}
	}

	IEnumerator EventDelayStart(string gameName){
		g.isShowLetterEffect = false;
		yield return new WaitForSeconds(1f);
		GameStart(gameName);
	}


	public void EventPassOnRelease(){
		Debug.Log("EventPassOnRelease");
		ev.EventOnRelease();
		if (ev.isCurPrevDist) {
            
			gameComponent.levelSkipBoxCollider.enabled = false;
			gameComponent.EnableGameSceneCommonBoxCollider(false);
            LevelPassStart("pass");
		}
	}

	public void EventBackSpaceOnRelease(){
		ev.EventOnRelease();
		if (ev.isCurPrevDist) {
            audioSource.clip = backspaceClip; audioSource.Play();

			if(g.isGameHint)  gameHint.EventBackSpaceOnRelease();
			else {
				if(gameComponent.clickWordUILabel.text.Length >= 1) {
					gameComponent.clickWordUILabel.text = gameComponent.clickWordUILabel.text.Remove(gameComponent.clickWordUILabel.text.Length-1);
					if( int.Parse(gameComponent.wordCntUILabel.text) < g.texName.Length )
						gameComponent.wordCntUILabel.text = ( int.Parse(gameComponent.wordCntUILabel.text) + 1).ToString();
				}
			}
		}
	}

	public void EventSkipOnRelease(){

		ev.EventOnRelease();
		if (ev.isCurPrevDist) {
            
			gameComponent.skipBoxCollider.enabled = false;
			gameComponent.EnableGameSceneCommonBoxCollider(false);
			if (int.Parse(gameComponent.skipNumUILabel.text) > 0 && gameState.Grid.activeSelf && !g.isGameBonus)
				StartCoroutine(WordSkip());
		}
	}


	public void EventHintOnRelease(){
		ev.EventOnRelease();
		if (ev.isCurPrevDist) {
			//gameComponent.bannerHintUILabel.text = g.texName;

            gameTexture.SetBanner();
		}
	}

    public void EventSoundOnRelease()
    {
        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            if (g.isSound)
            {
                g.isSound = false;
                audioSource.volume = 0f; timeoverAudioSource.volume = 0f;
                gameComponent.soundUISprite.spriteName = "soundno";
            }
            else {
                g.isSound = true;
                audioSource.enabled = true;
                audioSource.volume = g.buttonVolume; 
                timeoverAudioSource.volume = 1f;
                gameComponent.soundUISprite.spriteName = "soundnyes";
            }
        }
    }

    //---------------------
    // MapScene
    //---------------------
    public void EventMapOnRelease()
    {
        Debug.Log("EventMapOnRelease");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            gameState.BlankSceneBg.SetActive(true);
            gameState.GameTipBg.SetActive(true);
            gameComponent.tipTextNaviUILael.text = gameTips[Random.Range(0, gameTips.Count)];
            StartCoroutine(navi.GoMapScene());
        }
    }


	//----------------------------------------------------------------------
	//                           GamePanelZone
	//----------------------------------------------------------------------

//	public IEnumerator LevelClear(){
//		Debug.Log("LevelClear");
//		StartCoroutine(StageMaster());
//	}

	float interludeEffectTime = 1.0f;

	public void LevelPassStart(string kind) { StartCoroutine ( "LevelPass",kind ); }
    
    public IEnumerator LevelPass(string kind) {

        Debug.Log("LevelPass");
        //--------------------
        // State && Init
        //--------------------
        g.isGameTurning = false;
        g.isLevelPassing = true;
        gameComponent.gameInterludeUISprite.spriteName = "";
		//------------------------------------------------------
		// StopCoroutine
		//------------------------------------------------------
        LimitTimeStop();
        timeoverAudioSource.Stop();
        levelPass.LevelPassButtonStop();
        gameHint.ProcessHintLetterStop();

        gameState.BlankInCorrect.SetActive(false);

		//--------------------
		// BoxCollider
		//--------------------
		gameComponent.levelSkipBoxCollider.enabled = false;
		gameComponent.skipBoxCollider.enabled = false;
		//--------------------
		// Start delay
		//--------------------
		yield return new WaitForSeconds(g.touchProludeTime); // 0.2f 0.7f
	
		gameComponent.DestroyGridChildrens();
		gameState.StateGameInterlude();
		
		gameComponent.levelSkipAnimator.SetBool("isButtonWiggle",false);

		//------------------------
		// Effect Letter
		//------------------------
		// TTS

        audioSource.clip = startClip; audioSource.Play();
        yield return new WaitForSeconds(0.2f);

		gameBasic.PlayTTS(kind);
		
		gameComponent.gameInterludeUISprite.spriteName = kind;
		gameComponent.gameInterludeUISprite.MakePixelPerfect();
		
		gameState.GameInterlude.SetActive(true);
		gameComponent.gameInterludeAnimator.SetBool("isMiddleMoveRightHide",true);
		yield return new WaitForSeconds(interludeEffectTime);
		gameComponent.gameInterludeAnimator.SetBool("isMiddleMoveRightHide",false);
		gameState.GameInterlude.SetActive(false);
		

		//------------------------
		// Post Effect
		//------------------------
		gameState.ShowWord.SetActive(true);
		yield return StartCoroutine ( gameTexture.CorrectWordEffect() );
		yield return StartCoroutine ( gameTexture.CorrectWordEffect() );
		gameState.ShowWord.SetActive(false);
		
		//------------------------
		// Post State && Init
		//------------------------
		g.isLevelPassing = false;
		g.easyGameCnt++;


		//------------------------------------------------------
		// Score
		//------------------------------------------------------
		if(kind=="clear") account.ModifyScore();
		else dbo.InsUserStageTurnHis(account.GetGameKind(),0);

		//------------------------------------------------------
		// Rank Progress bar
        //------------------------------------------------------
        RankBarProgress();

		//------------------------------------------------------
		// Navigation
		//------------------------------------------------------
		if( g.easyGameCnt < g.easyRandomGame.Length )
			GameStart("easyRandomGame");
		else 
			GameStart("hint");
	}

    void RankBarProgress(){
        float rankBarRatio = (float)g.curStageScore/(float)g.curStageVitualMaxScore;
        if(rankBarRatio > 0.1f && rankBarRatio < 0.2f) gameComponent.rankBarStar1.color = g.GetRandomColor();
        else if (rankBarRatio >= 0.2f && rankBarRatio < 0.4f) gameComponent.rankBarStar2.color = g.GetRandomColor();
        else if (rankBarRatio >= 0.4f && rankBarRatio < 0.6f) gameComponent.rankBarStar3.color = g.GetRandomColor();
        else if (rankBarRatio >= 0.6f && rankBarRatio < 0.8f) gameComponent.rankBarStar4.color = g.GetRandomColor();
        else if (rankBarRatio >= 0.8f) gameComponent.rankBarStar5.color = g.GetRandomColor();

        if(rankBarRatio > 0.87f) rankBarRatio = 0.87f;
        
        gameComponent.rankUIScrollBar.barSize = rankBarRatio;
    }

	public IEnumerator WordClear(){

		Debug.Log("WordClear");

        //--------------------
        // State && Init
        //--------------------
        g.isGameTurning = false;
        gameComponent.gameInterludeUISprite.spriteName = "";

		//--------------------
		// stopcoroutine
		//--------------------
        LimitTimeStop();
        timeoverAudioSource.Stop();
        levelPass.LevelPassButtonStop();
        gameHint.ProcessHintLetterStop();

		//--------------------
		// BoxCollider
		//--------------------
		gameComponent.levelSkipBoxCollider.enabled = false;
		gameComponent.skipBoxCollider.enabled = false;
		
		//--------------------
		// Start delay
		//--------------------
		yield return new WaitForSeconds(g.autoProludeTime); // 0.2f 0.7f

		//--------------------
		// State && Init
		//--------------------
		g.isGameTurning = false;
		gameComponent.DestroyGridChildrens();
		gameState.StateGameInterlude();

		g.easyGameCnt = 0;
		SetRandomEasyGame();
		dbo.UptCorrectHis(1); // black (map note letter color)

		//--------------------
		// Effect Letter
		//--------------------
		// TTS

        audioSource.clip = clearClip; audioSource.Play();
        yield return new WaitForSeconds(0.3f);

		gameBasic.PlayTTS("word clear");
		
		gameComponent.gameInterludeUISprite.spriteName = "wordclear";
		gameComponent.gameInterludeUISprite.MakePixelPerfect();

		gameState.GameInterlude.SetActive(true);
		gameComponent.gameInterludeAnimator.SetBool("isLargeSmallMiddle",true);
		yield return new WaitForSeconds(2.0f);
		gameComponent.gameInterludeAnimator.SetBool("isLargeSmallMiddle",false);
		gameState.GameInterlude.SetActive(false);

		//------------------------------------------------------
		// Score
		//------------------------------------------------------
		account.ModifyScore();

		//------------------------------------------------------
		// ComboYn
		//------------------------------------------------------
        if(g.isCombo) dbo.UptCorrectHisComboYn();
 
		//------------------------------------------------------
		// Rank Progress bar
        //------------------------------------------------------
        RankBarProgress();

		//------------------------------------------------------
		// Navigation
		//------------------------------------------------------
		StartCoroutine( WordClearNavi() );
	}

	IEnumerator WordClearNavi(){
		//StartCoroutine(gameTexture.ShowTexture());
		yield return null;

		//--------------------
		// Post State && Init
		//--------------------
        if( g.stageTurn == g.turnTotalCnt && g.turnTotalCnt == bonusExeTurn5 && g.curStage > 0 ) {
            yield return StartCoroutine( navi.FadeOut(50f) );
            StartCoroutine ( gameBonus.ShowBonusGameRule() );
        } else if(g.stageTurn < g.turnTotalCnt) {
        //} else if(g.stageTurn <= 1) {
//		if(g.stageTurn <= 1) {
            g.stageTurn++;
            StartCoroutine(gameTexture.ShowTexture());
        } else {
            StartCoroutine(StageMaster());
        }

	}

	public IEnumerator WordSkip() {

		Debug.Log("WordSkip");

		//--------------------
		// stopcoroutine
		//--------------------
        LimitTimeStop();
        timeoverAudioSource.Stop();
        levelPass.LevelPassButtonStop();
        gameHint.ProcessHintLetterStop();

        gameState.BlankInCorrect.SetActive(false);

		//--------------------
		// BoxCollider
		//--------------------
		gameComponent.levelSkipBoxCollider.enabled = false;
		gameComponent.skipBoxCollider.enabled = false;
		

		//--------------------
		// Start delay
		//--------------------
		yield return new WaitForSeconds(g.touchProludeTime); // 0.2f 0.7f

		//--------------------
		// State && Init
		//--------------------
		g.isGameTurning = false;
        gameComponent.gameInterludeUISprite.spriteName = "";

		gameComponent.DestroyGridChildrens();
		gameState.StateGameInterlude();

		account.SubtractionSkip();

		g.comboCnt = 0;

		account.SkipInfo();

		g.easyGameCnt = 0;
		SetRandomEasyGame();

		dbo.InsUserStageTurnHis(account.GetGameKind(),0);
		dbo.UptUserStageTurnHisScore();

		dbo.UptCorrectHis(2); // red(map note letter color)


		//--------------------
		// Effect Letter
		//--------------------
		// TTS

        audioSource.clip = gameoverClip; audioSource.Play();
        yield return new WaitForSeconds(0.3f);


		gameBasic.PlayTTS("skip");
		
		gameComponent.gameInterludeUISprite.spriteName = "skipclear";
		gameComponent.gameInterludeUISprite.MakePixelPerfect();
		
		gameState.GameInterlude.SetActive(true);
		gameComponent.gameInterludeAnimator.SetBool("isLargeSmallMiddle",true);
		yield return new WaitForSeconds(1.0f);
		gameComponent.gameInterludeAnimator.SetBool("isLargeSmallMiddle",false);
		gameState.GameInterlude.SetActive(false);
        gameComponent.gameInterludeUISprite.spriteName = "";
		//------------------------------------------------------
		// Navigation
		//------------------------------------------------------
		StartCoroutine( WordClearNavi() );
		//StartCoroutine( StageMaster() );
	}

	public IEnumerator GameOver(){
		
		Debug.Log("GameOver");

		//--------------------
		// stopcoroutine
		//--------------------
        LimitTimeStop();
        timeoverAudioSource.Stop();
        levelPass.LevelPassButtonStop();
        gameHint.ProcessHintLetterStop();

        //--------------------
        // State && Init
        //--------------------
        g.isGameTurning = false;
        gameComponent.gameInterludeUISprite.spriteName = "";
        gameComponent.DestroyGridChildrens();
        gameState.StateGameInterlude();

		//--------------------
		// BoxCollider
		//--------------------
		gameComponent.levelSkipBoxCollider.enabled = false;
		gameComponent.skipBoxCollider.enabled = false;

		//--------------------
		// Start delay
		//--------------------
		yield return new WaitForSeconds(g.autoProludeTime); // 0.2f 0.7f

		
		//hpNumUILabel.text = (int.Parse(hpNumUILabel.text) - 1).ToString();
		//hpNumLineUILabel.text = (int.Parse(hpNumUILabel.text) - 1).ToString();
		g.comboCnt = 0;

		dbo.DelUserStageTurnHis();
		dbo.UptCorrectHis(2); // red(map note letter color)

		//--------------------
		// Effect Letter
		//--------------------
		// TTS

        audioSource.clip = gameoverClip; audioSource.Play();
        yield return new WaitForSeconds(0.5f);

		gameBasic.PlayTTS("gameover");
		
		gameComponent.gameInterludeUISprite.spriteName = "gameover";
		gameComponent.gameInterludeUISprite.MakePixelPerfect();

        gameComponent.gameInterludeAnimator.speed = 0.4f;
		gameState.GameInterlude.SetActive(true);
		gameComponent.gameInterludeAnimator.SetBool("isLargeSmallMiddle",true);
		yield return new WaitForSeconds(2.0f);
		gameComponent.gameInterludeAnimator.SetBool("isLargeSmallMiddle",false);
		gameState.GameInterlude.SetActive(false);
        gameComponent.gameInterludeAnimator.speed = 1f;
		
		//------------------------
		// Next
		//------------------------
		Application.LoadLevel("Map");
	
	}


	public IEnumerator StageMaster(){
		
		Debug.Log("StageMaster");

		//------------------------
		// Effect Letter
		//------------------------
		// TTS
		gameBasic.PlayTTS("lesson master");

		gameState.GameInterlude.SetActive(true);
		gameComponent.gameInterludeUISprite.spriteName = "lessonmaster";
		gameComponent.gameInterludeUISprite.MakePixelPerfect();

        gameComponent.gameInterludeAnimator.speed = 0.4f;
		gameState.GameInterlude.SetActive(true);
		gameComponent.gameInterludeAnimator.SetBool("isLargeSmallMiddle",true);
		yield return new WaitForSeconds(2.0f);
		gameComponent.gameInterludeAnimator.SetBool("isLargeSmallMiddle",false);
		gameState.GameInterlude.SetActive(false);
        gameComponent.gameInterludeAnimator.speed = 1f;

		//--------------------------------------------
		// Show PopUpAccount
		//--------------------------------------------
        if(g.curStage > 0) {
            StartCoroutine ( "ShowPopUpAccountBefore" );
        } else Application.LoadLevel("Map");
	}

    public IEnumerator ShowPopUpAccountBefore() {
        WWW www = new WWW(g.internetConnectTestUrl);
        yield return www;
        if (www.isDone && www.error == null) {
            Debug.Log("CheckInternet www.text=" + www.text);
            g.isInternetReachability = true;
        } else { 
            Debug.Log("CheckInternet www.error=" + www.error); 
            g.isInternetReachability = false;
        }
        account.ShowPopUpAccount();
    }	
		
	//-----------------------------------------------------
	// LimitTime
	//-----------------------------------------------------

    public void LimitTimeStart(){ StartCoroutine ( "LimitTime" ); }
    public void LimitTimeStop(){ StopCoroutine ( "LimitTime" ); }
	public IEnumerator LimitTime(){

		Debug.Log("LimitTime");

        float addTime = account.GetGameLimitAddTime();
        //Debug.Log("addTime1111111111111="+addTime);
        //Debug.Log("g.limitTime11111111="+g.limitTime);
        if (g.isGameBonus) g.limitTime = 350;
        else if(g.curStage < 1) g.limitTime *= 2;
        else {
            if(!g.isGameHint) g.limitTime = (int)(g.limitTime * account.GetGameLimitAddTime());
        }
        //Debug.Log("g.limitTime22222222222="+g.limitTime);

        int iCnt = 1;



		
		float limitDelayTime = (1f/60f) * Time.deltaTime;
		
		gameState.StartTimeSlider.transform.localPosition = new Vector3(246,0,0);
		
		float TimeSliderDistance = Mathf.Abs(gameState.EndTimeSlider.transform.localPosition.x)+Mathf.Abs(gameState.StartTimeSlider.transform.localPosition.x);
		//Debug.Log("TimeSliderDistance="+TimeSliderDistance);
		float xMove = (TimeSliderDistance-40)/g.limitTime;
		
		float slideMove = (1.0f/g.limitTime);
		float limitTime30 = (float)g.limitTime * 0.3f;
		float limitTime05 = (float)g.limitTime * 0.05f;
		//Debug.Log("g.limitTime="+g.limitTime);
		//Debug.Log("limitTime05="+limitTime05);
		gameComponent.TimeUISlider.value = 1;

		if (g.isGameBonus) {
			gameComponent.TimeUISlider.transform.localPosition = new Vector3 (0, -8, 0);
			//gameState.StartTimeSlider.GetComponent<UISprite> ().color = new Color(1/255,1/255,255/255,170/255);
		} else { 
			gameComponent.TimeUISlider.transform.localPosition = new Vector3 (0, -5, 0);
			//gameState.StartTimeSlider.GetComponent<UISprite> ().color = new Color(1/255,1/255,255/255,255/255);
		}

		while(!g.isLevelPassing && g.limitTime > 0 && g.isGameTurning) {
			g.limitTime--;

			gameComponent.limitUILabel.text = g.limitTime.ToString();
			gameComponent.limitLineUILabel.text = g.limitTime.ToString();

			if(!g.isGameBonus){
				gameState.StartTimeSlider.transform.localPosition = new Vector3(gameState.StartTimeSlider.transform.localPosition.x - xMove,0,0);
				gameComponent.TimeUISlider.value -= slideMove;
                if (g.limitTime <= limitTime30) {
                    if (iCnt == 1) {
                        timeoverAudioSource.clip = timeoverClip; timeoverAudioSource.Play(); 
                        iCnt++;
                    }
                    gameComponent.EndTimeSliderTweenScale.enabled = true;
                } 
                if (g.limitTime <= limitTime05) {
                    gameComponent.skipBoxCollider.enabled = false;
                    //gameComponent.mapBottomBoxCollider.enabled = false;
                    //ScreenPopupBg14.SetActive(true);
                }
			} else {
				if(gameComponent.TimeUISlider.value < 0.2) {
					gameState.StartTimeSlider.GetComponent<UISprite>().spriteName = "runnigman3"; 
				} 
			}

			yield return new WaitForSeconds( limitDelayTime );
		}


        timeoverAudioSource.Stop();

		//Debug.Log("g.isGameTurning="+g.isGameTurning);

		if(g.isLevelPassing) Debug.Log("");
		else if(g.isGameTurning && !g.isGameBonus) {
			StartCoroutine ( GameOver() );
			//Application.LoadLevel("MainMenu");
		}
		else if(g.isGameBonus) {
			StartCoroutine ( gameBonus.BonusGameEnded() );
		}

	}

	public IEnumerator GameInterludeEffect(float to, float from, float duration){
		
		gameState.GameInterlude.SetActive(true);
		
		gameComponent.gameInterludeTweenAlpha.from = from;
		gameComponent.gameInterludeTweenAlpha.to = to;
		gameComponent.gameInterludeTweenAlpha.duration = duration;
		gameComponent.gameInterludeTweenAlpha.enabled = true;
		yield return new WaitForSeconds( duration );
		gameComponent.gameInterludeTweenAlpha.ResetToBeginning();
		gameComponent.gameInterludeTweenAlpha.enabled = false;
		
		gameState.GameInterlude.SetActive(false);
		
	}





	//----------------------------------------------------------------------
	//                      Update_TouchEvent_Zone
	//----------------------------------------------------------------------
	void Update () {
        //if (Input.GetKeyDown(KeyCode.Escape)) GameObject.Find("AdMobManager").GetComponent<AdMobManager>().SetFrontAdmob();
		//if(Input.GetKeyDown(KeyCode.Escape)) EventQuitWarning();
   		//if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
		//if(Input.GetKeyDown(KeyCode.S))	if (gameState.Grid.activeSelf && !g.isGameBonus) StartCoroutine(WordSkip());
		//if(Input.GetKeyDown(KeyCode.M))	ev.EventMapSceneOnRelease();
	}

    //-------------------------------------------
    //           Quit Warning Popup
    //-------------------------------------------
	//public AudioClip[] audioClip = new AudioClip[1];
    public AudioSource buttonAudioSource;

    public GameObject PopupBg8;
    public GameObject QuitWarningPopup;
    public Animator quitWarningPopupAnimator;
    public void EventQuitWarning() {
        Debug.Log("EventQuitWarning");
        ev.EventOnPress();

        buttonAudioSource.clip = startClip; buttonAudioSource.Play();
        QuitWarningPopup.SetActive(true);
        PopupBg8.SetActive(true);
        quitWarningPopupAnimator.speed = 2;
        quitWarningPopupAnimator.SetBool("isSmallLargeScale2", true);
    }
    public void EventQuitWarningOK() {
        Debug.Log("EventQuitWarningOK");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) Application.Quit();
    }
    public void EventQuitWarningClose() {
        Debug.Log("EventQuitWarningClose");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            buttonAudioSource.clip = startClip; buttonAudioSource.Play();
    
            QuitWarningPopup.SetActive(false);
            PopupBg8.SetActive(false);
            quitWarningPopupAnimator.SetBool("isSmallLargeScale2", false);
        }
    }


	//--------------------------------------------------------
	//                     Start Function
	//--------------------------------------------------------

	/*
	 *  Setting Turn Information
	 */
	void SetStageTurnTextures() {
		
		//------------------------------------
		// stageTexBasics & Cnt
		//------------------------------------
		g.stageTextures = new List<g.TexInfo>[g.turnTotalCnt];

        for (int i = 0; i < g.turnTotalCnt; i++) {
			g.stageTextures [i] = new List<g.TexInfo> ();

			_data = dbo.SelTextureInfoTurn(g.curStage,i+1);

			for (int j = 0; j < _data.Rows.Count; j++) {
				dr = _data.Rows [j];
                for(int k=1; k <= g.wordTexCnt; k++) {
				    g.stageTextures [i].Add (new g.TexInfo (dr ["texName"].ToString (), 
				                                            dr ["texShowName"].ToString (),
                                                            k));
                }
                Debug.Log("texName[" + i + "]=" + dr["texName"].ToString());
			}
		}
		
		//------------------------------
		// random basic turn
		//------------------------------

		//Debug.Log("turnTotalCnt="+g.turnTotalCnt);
		int randomIndex;
		for (int i=0; i<g.turnTotalCnt; i++) {

			for(int j=0; j<g.stageTextures[i].Count; j++){
				randomIndex = Random.Range(j,g.stageTextures[i].Count);
				g.TexInfo temp1 = g.stageTextures[i][j]; 
				g.stageTextures[i][j] = g.stageTextures[i][randomIndex];
				g.stageTextures[i][randomIndex] = temp1;
			}

			randomIndex = Random.Range(i, g.turnTotalCnt);
			List<g.TexInfo> temp = g.stageTextures[i];
			g.stageTextures[i] = g.stageTextures[randomIndex];
			g.stageTextures[randomIndex] = temp;
		}
		
        //for (int i = 0; i < g.turnTotalCnt; i++) {
        //    Debug.Log("stageTexBasics["+i+"]="+g.stageTextures[i][0].texName);
        //}
	}


	public void RandomTurnTexNum() {
		int temp; 

		g.turnTexTotalNum = g.stageTextures[g.stageTurn-1].Count;

		for (int i=0;i<g.randomTurnTexNum.Count;i++) g.randomTurnTexNum.RemoveAt(i);

		for (int i = 0; i < g.turnTexTotalNum; i++) {
			g.randomTurnTexNum.Add(i);
			//Debug.Log(g.randomTurnTexNum[i]);
		}
		for (int i = 0; i < g.turnTexTotalNum; i++) {
			int randomIndex = Random.Range(i, g.turnTexTotalNum);
			temp = g.randomTurnTexNum[i];
			g.randomTurnTexNum[i] = g.randomTurnTexNum[randomIndex];
			g.randomTurnTexNum[randomIndex] = temp;
		}
	}

	void SetGamePanelUILabels() {

		//---------------------------------------------
		// Setting Skip inpormation
		//---------------------------------------------
		g.stageSkip = g.allSkip()> stageMaxSkip ? stageMaxSkip : g.allSkip();
		gameComponent.skipNumUILabel.text = g.stageSkip.ToString();
		gameComponent.totalSkipNumUILabel.text = g.allSkip().ToString();
		//---------------------------------------------
		// Setting Progressbar Rank inpormation
		//---------------------------------------------		
		//---------------------------------------------
		// Setting BoardPanel infomation
		//---------------------------------------------
        _data = dbo.SelUserStageHis(g.curStage); dr = _data.Rows[0];
        g.curStageMaxScore = int.Parse(dr["stageMaxScore"].ToString());

        gameComponent.bestScoreUILabel.text = g.curStageMaxScore.ToString();
        gameComponent.bestScoreLineUILabel.text = g.curStageMaxScore.ToString();

	}
	


}