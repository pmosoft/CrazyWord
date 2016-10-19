using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class MapControl : MonoBehaviour {

    public Account account;
    //---------------------------------------------------------
    // State
    //---------------------------------------------------------
    private int minute1 = 60;

	//---------------------------------------------------------
	// Camera
	//---------------------------------------------------------
	public UISprite mapSceneBg;

    public UITexture mapSceneImg;
	public UISprite blurSceneBg;

	public GameObject BlankSceneBg;
 	
	//---------------------------------------------------------
	// Top Panel
	//---------------------------------------------------------
    public Animator giftButtonAnimator;
    public UISprite giftUISprite;
    public BoxCollider giftBoxCollider;

    public UILabel coinAmount;
    public UILabel coinLineAmount;

    public PopupFriendControl popupFriendControl;

	//---------------------------------------------------------
	// Map Panel
	//---------------------------------------------------------
    public GameObject BlurPanel;

    //public GameObject BlurWordExpressionPanel;

	public UISprite mapTopUISprite;
	public UISprite mapBottomUISprite;
    
	//-----------------------------------------------
	// ScrollView Panel
	//-----------------------------------------------
	public GameObject ScrollView, Grid, GridBg;
	public UIGrid gridUIGrid;
	
	public TweenAlpha gridBgTweenAlpha, blurTweenAlpha;
    public GameObject LessonPrefab, LessonPrefab0, LessonPrefab9, LessonInfoPanelPrefab;
    public GameObject WPrefab, LockPrefab;
    public UIScrollView lessonUIScrollView;
    public UIScrollBar lessonUIScrollBar;

    GameObject GridLessonPrefab;

    UILabel lessonNumUILabel;
    UIEventTrigger lessonUIEventTrigger;
    UISprite rankUISprite;
    public UISprite iconUISprite;

    GameObject[] mapItems;

    public Dictionary<int, string> iconSprites = new Dictionary<int, string>();
    int largeCode, middleCode;
    public List<UISprite> lessonSprites = new List<UISprite>();

    string lessonText = "";
	//-----------------------------------------------
	// WordExpression Panel
	//-----------------------------------------------
	public GameObject WordExpressionPanel, WordExpressionBlurBottomBg;
    
    public TweenPosition WordExpressionTopTweenPos, WordExpressionPanelTweenPos;
	
	public UILabel WordExpressionLessonNumUILabel;
	public UILabel WordExpressionExampleTexNameUILabel;
	public UILabel WordExpressionExampleMeaningUILabel;
	public UILabel WordExpressionExample1UILabel;
	public UILabel WordExpressionExample1MeaningUILabel;


	public UISprite WordExpressionLessonRankUISprite, WordExpressionLessonIconUISprite;
    public UISprite WordExpressionScreenBg, WordExpressionCenterUISprite;

    UIEventTrigger WordExpressionUIEventTrigger;

	//-----------------------------------------------
	// WordBookPanel Panel
	//-----------------------------------------------
	public GameObject WordBookPanel, WordBookPage1, WordBookContentGrid1;
    public GameObject WordBookItemPrefab, WordBookItemMeaningPrefab;
    public GameObject WordBookBlurTopBg;

    public UISprite WordBookScreenBg;
	public TweenPosition WordBookBottomTweenPos, WordBookPanelTweenPos;
	public UILabel WordBookLessonNumUILabel;
	public UISprite WordBookLessonRankUISprite, WordBookLessonIconUISprite, WordBookCenterUISprite;
    UIEventTrigger WordBookUIEventTrigger;

	//-----------------------------------------------
	// Bottom Panel
	//-----------------------------------------------
	public UILabel skipAllNumUILabel, skipAllNumLineUILabel;
	public UILabel skipNumUILabel, skipNumLineUILabel;
	public UILabel skipTimeUILabel, skipTimeLineUILabel;
	public UISprite skipTimeUISprite;
	
	public UILabel hpAllNumUILabel, hpAllNumLineUILabel;
	public UILabel hpNumUILabel, hpNumLineUILabel;
	public UILabel hpTimeUILabel, hpTimeLineUILabel;
	public UISprite hpTimeUISprite;


	//-----------------------------------------------
	// Banner Panel
	//-----------------------------------------------
	public GameObject BannerPanel;

    //-----------------------------------------------
    // Shop Panel
    //-----------------------------------------------
    public UISprite noBannerContextUISprite;
    public BoxCollider noBannerBoxCollider;

    //-----------------------------------------------
    // Part Panel
    //-----------------------------------------------
    public UISprite[] PartLocks = new UISprite[9];
    public Animator leftNaviAnimator,rightNaviAnimator;


    //---------------------------------------------------------
    // Common
    //---------------------------------------------------------
    public PopupShopControl popupShopControl;
    public PopupMapControl popupMapControl;

    public DBO dbo; public DBOW dbow; public Navi navi; public EventCommon ev;

    //---------------------------------------------------------
    // Sound
    //---------------------------------------------------------
    public AudioClip menuClip, startClip;
    public AudioSource audioSource;

	//--------------------------
	// sqlite
	//--------------------------
	DataTable _data; DataRow dr, wordExpressionDr;
    string[] flowerAni = new string[2];

	void Awake(){
		flowerAni[0]  = "lodingicon1";
		flowerAni[1]  = "lodingicon2";
	}

	IEnumerator FlowerAni(){
	//void FlowerAni(){
        //Debug.Log("FlowerAni flowerAni.Length="+flowerAni.Length);
        for(int h=0;h<3;h++) {
		    for(int i=0;i<flowerAni.Length;i++) {
                //Debug.Log("flowerAni[i]="+flowerAni[i]);
                loadingUISprite.spriteName = flowerAni[i];
			    yield return new WaitForSeconds(0.2f);
			    //yield return null;
	        }
        }
        LoadingPanel.SetActive(false);
        //StartCoroutine ( "FlowerAni" );
	}

    public GameObject LoadingPanel;
    public UISprite loadingBgUISprite,loadingUISprite;

	void Start () {
        
        //----------------------------------------
        // 업적보드 테스트
        //----------------------------------------
        //string[] achieveKeys = new string[10] {
        //    "CgkIlY-y4cYcEAIQAQ","CgkIlY-y4cYcEAIQAg","CgkIlY-y4cYcEAIQAw","CgkIlY-y4cYcEAIQBA","CgkIlY-y4cYcEAIQBQ"
        //    ,"CgkIlY-y4cYcEAIQBw","CgkIlY-y4cYcEAIQCA","CgkIlY-y4cYcEAIQCQ","CgkIlY-y4cYcEAIQCg","CgkIlY-y4cYcEAIQCw"
        //};
        //Social.ReportProgress(achieveKeys[0], 100.0f, (bool success) => { });

        //Debug.Log("Application.systemLanguage="+Application.systemLanguage.ToString());
        //Debug.Log("Application.systemLanguage=" + Application.systemLanguage);
        //Debug.Log("UnityEngine.SystemLanguage=" + UnityEngine.SystemLanguage.);

		//StartCoroutine ( navi.FadeIn() );
		//StartCoroutine ( FadeIn2() );

		//----------------------
		// for test, temporaray
		//----------------------

        #if UNITY_EDITOR



            dbo.SelUserInfo();

            // 맵의 curStage 정보 클리어 오류발생
            //yield return StartCoroutine(dbow.SelUserInfo());
            g.wwwDate = System.DateTime.Now.ToString("yyyyMMdd");
        
            //---------------------------------------------
            // google achievement test
            //---------------------------------------------
            //int partNum = 2;
            //string[] achieveKeys = new string[10] {
            //    "CgkIlY-y4cYcEAIQAQ","CgkIlY-y4cYcEAIQAg","CgkIlY-y4cYcEAIQAw","CgkIlY-y4cYcEAIQBA","CgkIlY-y4cYcEAIQBQ"
            //    ,"CgkIlY-y4cYcEAIQBw","CgkIlY-y4cYcEAIQCA","CgkIlY-y4cYcEAIQCQ","CgkIlY-y4cYcEAIQCg","CgkIlY-y4cYcEAIQCw"
            //};

            ///Social.ReportProgress("CgkIlY-y4cYcEAIQAQ", 100.0f, (bool success) => { });

            ////Social.ReportProgress(achieveKeys[partNum-1], 100.0f, (bool success) => { });
            //dbo.InsUserGiftHis(10,2,"");StartCoroutine(dbow.UptUserInfoAchievement(2));
            //dbo.InsUserGiftHis(10,3,"");StartCoroutine(dbow.UptUserInfoAchievement(3));
            //dbo.InsUserGiftHis(10,4,"");StartCoroutine(dbow.UptUserInfoAchievement(4));

        #endif

        //--------------------
        // State
        //--------------------
        mapSceneImg.mainTexture = g.userBgImgTextures[g.userBgImg];
        mapBgCol();

        LoadingPanel.SetActive(true);
        StartCoroutine ( "FlowerAni" );

        Debug.Log("g.curStage=" + g.curStage);
        SetGiftYn();

        g.isHome=false; g.isMap=true; g.isGame=false;

        SetHpSkipCoinValue();


		//InitProgressBar();
		
		//curStarNumUILabel.text = g.curStar.ToString();
		//curStarNumLineUILabel.text = g.curStar.ToString();
		//totalStarNumUILabel.text = g.totalStar.ToString();
		//totalStarNumLineUILabel.text = g.totalStar.ToString();


        #if UNITY_ANDROID && !UNITY_EDITOR
            AdMobAndroid.hideBanner(true);
        # endif

        //------------------------------
        // Sound
        //------------------------------
        if(g.isSound == true) audioSource.volume = g.buttonVolume;
        else audioSource.volume = 0f;

        //------------------------------
        // Part panel
        //------------------------------
        SetPartLock();
        leftNaviAnimator.SetBool("isButtonWiggle", true);
        rightNaviAnimator.SetBool("isButtonWiggle", true);

        //------------------------------
        // Shop panel
        //------------------------------
        popupShopControl.SetAllPack();
        popupShopControl.SetFbShareCoin();
        SetBanner();

		//------------------------------
		// Instantiate GridMapItems
		//------------------------------
        SetLessonScrollView();

        //StopCoroutine ( "FlowerAni" );

		//---------------------------------------------
		// Initializing ProgressBar
		//---------------------------------------------
		InitStarProgressBar();

	}

    public GameObject StarPrefab,ProgressBar;
    public UILabel curStarNumUILabel, curStarNumLineUILabel;
    int comboTotalCnt;
    int comboTotalCntRest;

    public void InitStarProgressBar() {
		UISprite StarCloneUISprite;
		GameObject StarClone;

        comboTotalCnt = dbo.SelUserCorrectHisComboCnt();

        comboTotalCntRest = comboTotalCnt % g.totalStar;
        if(comboTotalCntRest == 0) comboTotalCntRest = 10;

        if(comboTotalCnt > 0) {
            for (int i = 1; i <= comboTotalCntRest; i++)  {
			
                g.backgroudColorR=Random.Range(0.0f,1.0f);g.backgroudColorG=Random.Range(0.0f,1.0f);g.backgroudColorB=Random.Range(0.0f,1.0f);
			
                StarClone = Instantiate(StarPrefab) as GameObject;
                StarClone.transform.parent = ProgressBar.transform;
                StarCloneUISprite = StarClone.GetComponent<UISprite>();
                StarCloneUISprite.color = new Color(g.backgroudColorR,g.backgroudColorG,g.backgroudColorB,1f);
			
                StarClone.transform.localPosition = new Vector3( (float) (180/g.totalStar * i) -10f, 2f, 0f );
                StarClone.transform.localScale = new Vector3(1,1,1);
            }

            curStarNumUILabel.text = comboTotalCntRest.ToString();
            curStarNumLineUILabel.text = comboTotalCntRest.ToString();

            SetAchievement();
        }
	}

    void SetAchievement(){
        
        //----------------------
        // Achievement Combo
        //----------------------
        if(dbo.SelUserStarAchievment(comboTotalCnt)) {

            g.giftSkip++;
            dbo.UptSkip("gift");

            skipNumUILabel.text = g.allSkip().ToString();
            skipNumLineUILabel.text = g.allSkip().ToString();

            StartCoroutine ( AchieveTween() );
        }

    }
    public TweenPosition achieveTweenPosition;

    public IEnumerator AchieveTween(){
		achieveTweenPosition.enabled = true;
		yield return new WaitForSeconds( 5f );
		achieveTweenPosition.ResetToBeginning();
		achieveTweenPosition.enabled = false;
    }

    //----------------------------------------------------------------------
    //                               ScrollView
    //----------------------------------------------------------------------
    SpringPanel springPanel; 
    void SetLessonScrollView(){

        //---------------------------------------------
        // Instantiate Lesson ScrollView
        //---------------------------------------------
        SetLessonPrefab();

        //--------------------------------
        // ScrollView Reposition
        //--------------------------------

        GridBg.SetActive(true);

        gridUIGrid.Reposition();
        //gameObject.GetComponent<UIDraggablePanel>().ResetPosition(); - 스크롤 리스트 처음 위치로 재설정 
        Grid.GetComponent<UICenterOnChild>().Recenter();

        springPanel = ScrollView.GetComponent<SpringPanel>();
        //springPanel.target = new Vector3(0, (g.curStage - 1) * gridUIGrid.cellWidth, 0);
        springPanel.target = new Vector3(0, (g.curStage - 1) * gridUIGrid.cellWidth, 0);
        //springPanel.target = new Vector3(0,(19-1)*gridUIGrid.cellWidth,0);
        //mapItems[19].transform.localScale = Vector3.one;
        //ScrollView.transform.y

        //ShowLessonInfo();


    }


    List<int> partStartStageNums = new List<int>();
    void SetLessonPrefab() {

        Debug.Log("SetLessonPrefab");

        //---------------------------------
        // Set iconSprite
        //---------------------------------
        _data = dbo.SelUserCorrectHisCode();
        for (int i = 0; i < _data.Rows.Count; i++)
        {
            dr = _data.Rows[i];

            largeCode = int.Parse(dr["largeCode"].ToString());
            middleCode = int.Parse(dr["middleCode"].ToString());

            iconSprites.Add(int.Parse(dr["stageNum"].ToString()),
                            largeCode.ToString("00") + middleCode.ToString("00")
                            );
        }

        _data = dbo.SelPartStartStageNum(); dr = _data.Rows[0];
        
        bool isPartStartStage = false;
        for (int i = 0; i < _data.Rows.Count; i++)
        {
            dr = _data.Rows[i];
            partStartStageNums.Add( int.Parse(dr["minStageNum"].ToString()) );
        }

        int loopCnt = g.maxStage;
        for (int i = 1; i <= loopCnt; i++)
        //for (int i = 1; i <= 50; i++)
        {

            //---------------------------------------------
            // Instantiate LessonPrefab
            //---------------------------------------------
            if (i == 1) GridLessonPrefab = Instantiate(LessonPrefab0) as GameObject;
            else if (i == loopCnt) GridLessonPrefab = Instantiate(LessonPrefab9) as GameObject;
            else GridLessonPrefab = Instantiate(LessonPrefab) as GameObject;

            GridLessonPrefab.transform.parent = Grid.transform;
            GridLessonPrefab.transform.localPosition = new Vector3(0f, i * 100, 0f);
            GridLessonPrefab.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            GridLessonPrefab.transform.localScale = Vector3.one;
            GridLessonPrefab.name = "Lesson"+i;

            if (i == 1) {
                //---------------------------------------------
                // Event Trigger : Lesson
                //---------------------------------------------
                lessonUIEventTrigger = GridLessonPrefab.transform.FindChild("1_Intro").GetComponent<UIEventTrigger>();
                EventDelegate.Add(lessonUIEventTrigger.onPress, ev.EventOnPress);
                EventDelegate.Add(lessonUIEventTrigger.onRelease, EventIntroHelpOnRelease);
            }

            //---------------------------------------------
            // Set LessonPrefabComponent
            //---------------------------------------------
            SetGridLessonPrefabComponent(GridLessonPrefab);

            //---------------------------------------------
            // Set lessonNumUILabel
            //---------------------------------------------
            lessonNumUILabel.text = (i).ToString();
            g.lessonNumber = i;

            //---------------------------------------------
            // Set lessonNumUILabel
            //---------------------------------------------
            _data = dbo.SelUserStageHis(i); dr = _data.Rows[0];

            if (dr["stageOpenYn"].ToString() == "Y")
            {
                if (int.Parse(dr["stageRank"].ToString()) == 0) rankUISprite.spriteName = "ribbonmap_star0";
                else if (int.Parse(dr["stageRank"].ToString()) == 1) rankUISprite.spriteName = "ribbonmap_star1";
                else if (int.Parse(dr["stageRank"].ToString()) == 2) rankUISprite.spriteName = "ribbonmap_star2";
                else if (int.Parse(dr["stageRank"].ToString()) == 3) rankUISprite.spriteName = "ribbonmap_star3";
                else if (int.Parse(dr["stageRank"].ToString()) == 4) rankUISprite.spriteName = "ribbonmap_star4";
                else if (int.Parse(dr["stageRank"].ToString()) == 5) rankUISprite.spriteName = "ribbonmap_star5";

                iconUISprite.spriteName = iconSprites[i];

                iconUISprite.MakePixelPerfect();
            }
            else
            {
                for(int j = 0; j < partStartStageNums.Count; j++){
                    if(i == partStartStageNums[j]  ) {
                        //Debug.Log(partStartStageNums[j]);
                        isPartStartStage = true;
                    }
                }
                if(isPartStartStage) {
                    iconUISprite.spriteName = "tmplockicon";
                } else {
                    iconUISprite.spriteName = "ribbonmap_lock";
                }

                rankUISprite.spriteName = "ribbonmap_star0";
                isPartStartStage = false;

            }

            lessonSprites.Add(iconUISprite);
        }

        //lessonUIScrollView.verticalScrollBar = lessonUIScrollBar;

    }

    void SetGridLessonPrefabComponent(GameObject GridLessonPrefab)
    {

        //---------------------------------------------
        // lessonNumUILabel
        //---------------------------------------------
        lessonNumUILabel = GridLessonPrefab.transform.FindChild("2_CenterBoard").FindChild("3_LessonNum").GetComponent<UILabel>();

        //---------------------------------------------
        // Event Trigger : Lesson
        //---------------------------------------------
        lessonUIEventTrigger = GridLessonPrefab.transform.FindChild("2_CenterBoard").GetComponent<UIEventTrigger>();
        EventDelegate.Add(lessonUIEventTrigger.onPress, ev.EventOnPress);
        EventDelegate.Add(lessonUIEventTrigger.onRelease, EventLessonOnRelease);

        //---------------------------------------------
        // Event Trigger : Weekly WordExpression
        //---------------------------------------------
        WordExpressionUIEventTrigger = GridLessonPrefab.transform.FindChild("1_TopBoard").GetComponent<UIEventTrigger>();
        EventDelegate.Add(WordExpressionUIEventTrigger.onPress, ev.EventOnPress);
        EventDelegate.Add(WordExpressionUIEventTrigger.onRelease, EventWordExpressionOnRelease);

        //---------------------------------------------
        // Event Trigger : WordBook
        //---------------------------------------------
        WordBookUIEventTrigger = GridLessonPrefab.transform.FindChild("1_BottomBoard").GetComponent<UIEventTrigger>();
        EventDelegate.Add(WordBookUIEventTrigger.onPress, ev.EventOnPress);
        EventDelegate.Add(WordBookUIEventTrigger.onRelease, EventWordBookOnRelease);

        //---------------------------------------------
        // Rank or Icon 
        //---------------------------------------------

        rankUISprite = GridLessonPrefab.transform.FindChild("2_CenterBoard").FindChild("3_Rank").GetComponent<UISprite>();
        iconUISprite = GridLessonPrefab.transform.FindChild("2_CenterBoard").FindChild("3_Icon").GetComponent<UISprite>();
    
    }

	public void ShowLessonInfo(){
		
		BannerPanel.SetActive(false);

		GameObject LessonInfoPanelClone = Instantiate(LessonInfoPanelPrefab) as GameObject;
		LessonInfoPanelClone.transform.parent =  Camera.main.transform;
		LessonInfoPanelClone.transform.localPosition = new Vector3( 0f, -595f, 0f );
		LessonInfoPanelClone.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
		LessonInfoPanelClone.transform.localScale = Vector3.one;

		int j = 0;
		for(int i=1;i<=10;i++){
			if( j < 4) {
				GameObject WClone = Instantiate(WPrefab) as GameObject;
				WClone.transform.parent = LessonInfoPanelClone.transform;
				WClone.transform.localPosition = new Vector3( -356f +(65f * i), -15f, 0f );
				WClone.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
				WClone.transform.localScale = Vector3.one;
			} else {
				GameObject LockClone = Instantiate(LockPrefab) as GameObject;
				LockClone.transform.parent = LessonInfoPanelClone.transform;
				LockClone.transform.localPosition = new Vector3( -356f +(65f * i), -15f, 0f );
				LockClone.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
				LockClone.transform.localScale = Vector3.one;
			}
			j++;
		}
	}
    //----------------------------------------------------------------------
    //                               Lesson
    //----------------------------------------------------------------------
	public void EventLessonOnRelease(){
		Debug.Log("EventLessonOnRelease");
		ev.EventOnRelease();

		if (ev.isCurPrevDist) {
			//if (isLessonClicked) {
            Debug.Log("g.curStage=" + g.curStage);

            if ( g.allHp() > 0 || g.curStage < 1 )
            {
                _data = dbo.SelUserStageHis(g.curStage); dr = _data.Rows[0];
                if (dr["stageOpenYn"].ToString() == "Y")
                {
                    account.SubtractionHp();
                    dbo.UptUserInfo();
                    StartCoroutine(PlayTTS());
                    GridBoxCollider(false); StartCoroutine(navi.GoGameScene());
                    g.isMap = false;
                } else {
                    for(int j = 0; j < partStartStageNums.Count; j++) {
                        if(g.curStage == partStartStageNums[j]  ) {
                            popupMapControl.EventEnterPartPopup();
                        }
                    }
                }
            }
            else {
                popupMapControl.EventHpWarningPopup();
            }
		}
	}

    string lessonTTS;
	IEnumerator PlayTTS(){

        audioSource.clip = startClip; audioSource.Play();
        yield return new WaitForSeconds(0.2f);

        PlayTTS("lesson");
		yield return new WaitForSeconds(1.0f);

        lessonTTS = g.curStage.ToString();

        PlayTTS(lessonTTS);
        yield return new WaitForSeconds(0.2f);

	}


    //----------------------------------------------------------------------
    //                             WordExpression
    //----------------------------------------------------------------------

    string wordExpressionExample,ttsContext; 
    string wordExpressionExampleTTS1,wordExpressionExampleTTS2;
    int wordExpressionTurnNum = 0;
    string wordExpressionTexShowName;
    void WordExpressionExampleUILabelText() {

        dr = _data.Rows[wordExpressionTurnNum];
        wordExpressionTexShowName = dr["texShowName"].ToString();

        //string example1Meaning = "";

        //-------------------------------
        // ShowName
        //-------------------------------
        if (dr["state"].ToString() == "0") 
            wordExpressionExample = "[056b00]" + dr["texShowName"].ToString() + "[-]";
        else if (dr["state"].ToString() == "1") 
            wordExpressionExample = "[000000]" + dr["texShowName"].ToString() + "[-]";
        else if (dr["state"].ToString() == "2") 
            wordExpressionExample = "[ff0000]" + dr["texShowName"].ToString() + "[-]";

        //else        wordExpressionExample = "[ffffff]" + dr["texShowName"].ToString() + "\n";

        WordExpressionExampleTexNameUILabel.text = wordExpressionExample;
        wordExpressionExampleTTS1 = dr["texShowName"].ToString(); 
        
        //-------------------------------
        // Meaning
        //-------------------------------
        WordExpressionExampleMeaningUILabel.text = "(" + dr["meaning"].ToString() + ")";

        //-------------------------------
        // Example
        //-------------------------------
        WordExpressionExample1UILabel.text = dr["example1"].ToString();
        wordExpressionExampleTTS2 = dr["example1"].ToString(); 

        WordExpressionExample1MeaningUILabel.text = "(" + dr["example1Meaning"].ToString() + ")";
    }

	public void EventWordExpressionOnRelease(){
		ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            //StartCoroutine(OnClickWordExpression());

            _data = dbo.SelUserStageHis(g.curStage); dr = _data.Rows[0];
            if (dr["stageOpenYn"].ToString() == "Y")
            {
                OnClickWordExpression();
            }

        }
	}
	void OnClickWordExpression() 
	{
		Debug.Log("OnClickWordExpression");

        //---------------------
        // State
        //---------------------
        BlurPanel.SetActive(true);
  		WordBg2.SetActive(true);

        mapSceneImg.enabled = false;

        WordExpressionPanel.SetActive(true);
        GridBg.SetActive(false);

        //blurMapSceneImg.mainTexture = g.userBgImgTextures[g.userBgImg];
        blurSceneBg.color = mapSceneBg.color;

        WordExpressionScreenBg.color = mapSceneBg.color;

        //---------------------
        // Sel WordExpression
        //---------------------
        wordExpressionTurnNum = 0;
        _data = dbo.SelTextureInfoCorrect(g.curStage);
        WordExpressionExampleUILabelText();

        //yield return new WaitForSeconds(0.1f);


        if (g.curStage < 1)
        {
            WordExpressionCenterUISprite.spriteName = "ribbonmap_basic";
            WordExpressionLessonNumUILabel.text = lessonText;
            WordExpressionLessonRankUISprite.enabled = false;
        }
        else
        {
            WordExpressionCenterUISprite.spriteName = "ribbonmap_stagem";
            WordExpressionLessonNumUILabel.text = g.curStage.ToString();
            WordExpressionLessonRankUISprite.enabled = true;
            WordExpressionLessonRankUISprite.spriteName = g.curRankName;
        }
        WordExpressionLessonIconUISprite.spriteName = g.curIconName;
		
	
		WordExpressionPanelTweenPos.from = new Vector3(0,0,0);
		WordExpressionPanelTweenPos.to   = new Vector3(0,-300,0);
		WordExpressionPanelTweenPos.enabled = true;
		WordExpressionPanelTweenPos.ResetToBeginning();
		
		WordExpressionTopTweenPos.from = new Vector3(0,-540,0);
		WordExpressionTopTweenPos.to   = new Vector3(0,36,0);
		WordExpressionTopTweenPos.enabled = true;
		WordExpressionTopTweenPos.ResetToBeginning();
		
		blurTweenAlpha.from = 1.0f;
		blurTweenAlpha.to   = 1.0f;
		blurTweenAlpha.enabled = true;
		blurTweenAlpha.ResetToBeginning();
		
		//		gridBgTweenAlpha.from = 1.0f;
		//		gridBgTweenAlpha.to   = 0.1f;
		//		gridBgTweenAlpha.enabled = true;
		//		gridBgTweenAlpha.ResetToBeginning();
		
		
	}
	
	public void EventWordExpressionOffRelease(){
		Debug.Log("EventWordExpressionOffRelease");
		ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            StartCoroutine(OffClickWordExpression());
        }
	}
	IEnumerator OffClickWordExpression() 
	{
		Debug.Log("OffClickWordExpression");

		//WordExpressionTopTweenPos = WordExpressionPanel.transform.FindChild("6_TopBoard").gameObject.GetComponent<TweenPosition>();
		//WordExpressionPanelTweenPos = WordExpressionPanel.GetComponent<TweenPosition>();
		
		WordExpressionPanelTweenPos.from = new Vector3(0,-300,0);
		WordExpressionPanelTweenPos.to   = new Vector3(0,0,0);
		WordExpressionPanelTweenPos.enabled = true;
		WordExpressionPanelTweenPos.ResetToBeginning();

		WordExpressionTopTweenPos.from = new Vector3(0,36,0);
		WordExpressionTopTweenPos.to   = new Vector3(0,-540,0);
		WordExpressionTopTweenPos.enabled = true;
		WordExpressionTopTweenPos.ResetToBeginning();

		blurTweenAlpha.from = 1.0f;
		blurTweenAlpha.to   = 0.0f;
		blurTweenAlpha.enabled = true;
		blurTweenAlpha.ResetToBeginning();

        //		gridBgTweenAlpha.from = 0.1f;
		//		gridBgTweenAlpha.to   = 1.0f;;
		//		gridBgTweenAlpha.enabled = true;
		//		gridBgTweenAlpha.ResetToBeginning();
		
		yield return new WaitForSeconds(1f);
		
		BlurPanel.SetActive(false);
        mapSceneImg.enabled = true;

		WordExpressionPanel.SetActive(false);
        GridBg.SetActive(true);
   		WordBg2.SetActive(false);
	}


    public void EventWordExpressionOffTTS()
    {
        Debug.Log("EventWordExpressionOffTTS");
        ev.EventOnRelease();

        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            StartCoroutine ("WordExpressionTTS");
        }
    }

    IEnumerator WordExpressionTTS() {

        //WordExpressionExampleUILabelText();
        //Debug.Log("wordExpressionExampleTTS1=" + wordExpressionExampleTTS1);

        PlayTTS(wordExpressionExampleTTS1);
        yield return new WaitForSeconds(1.5f);
        PlayTTS(wordExpressionExampleTTS2);

    }

    public void EventWordExpressionForward()
    {
        Debug.Log("EventWordExpressionForward");
        //ev.EventOnRelease();
        //if (ev.isCurPrevDist)
        //{
            StopCoroutine("WordExpressionTTS");
            audioSource.clip = menuClip; audioSource.Play();
            if (wordExpressionTurnNum < 4)
            {
                wordExpressionTurnNum++;
                //Debug.Log("ffff wordExpressionTurnNum=" + wordExpressionTurnNum);
                WordExpressionExampleUILabelText();
            }
            else {
                wordExpressionTurnNum = 0;
                //Debug.Log("ffff wordExpressionTurnNum=" + wordExpressionTurnNum);
                WordExpressionExampleUILabelText();
            }

        //}
    }

    public void EventWordExpressionBackward()
    {
        Debug.Log("EventWordExpressionBackward");
        //ev.EventOnRelease();
        //if (ev.isCurPrevDist)
        //{
            StopCoroutine("WordExpressionTTS");
            audioSource.clip = menuClip; audioSource.Play();
            if (wordExpressionTurnNum > 0)
            {
                wordExpressionTurnNum--;
                //Debug.Log("bbbb wordExpressionTurnNum=" + wordExpressionTurnNum);
                WordExpressionExampleUILabelText();
            }
            else
            {
                wordExpressionTurnNum = 4;
                //Debug.Log("bbbb wordExpressionTurnNum=" + wordExpressionTurnNum);
                WordExpressionExampleUILabelText();
            }


        //}
    }

	public void EventWordExpressionDicOnRelease(){
		ev.EventOnRelease();
        audioSource.clip = menuClip; audioSource.Play();
        Application.OpenURL ("http://dic.daum.net/search.do?dic=en&search_first=Y&q="+wordExpressionTexShowName);
	}
    
    //----------------------------------------------------------------------
    //                               WordBook
    //----------------------------------------------------------------------

	public void EventWordBookOnRelease(){
		Debug.Log("EventWordBookOnRelease");
		ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            audioSource.clip = menuClip; audioSource.Play();
            _data = dbo.SelUserStageHis(g.curStage); dr = _data.Rows[0];
            if (dr["stageOpenYn"].ToString() == "Y") {
                StartCoroutine(OnClickWordBook());
            }
        }		
	}

	IEnumerator OnClickWordBook() {
		
		BlurPanel.SetActive(true);
		WordBg2.SetActive(true);
        mapSceneImg.enabled = false;
		WordBookPanel.SetActive(true);
        GridBg.SetActive(false);

        blurSceneBg.color = mapSceneBg.color;
        WordBookScreenBg.color = mapSceneBg.color;

        if (g.curStage < 1) {
            WordBookCenterUISprite.spriteName = "ribbonmap_basic";
            if (g.curStage == -3) { lessonText = "A"; } else if (g.curStage == -2) { lessonText = "B"; } else if (g.curStage == -1) { lessonText = "C"; } else if (g.curStage == 0) { lessonText = "D"; }
            WordBookLessonNumUILabel.text = lessonText;
            WordBookLessonRankUISprite.enabled = false;
        } else {
            WordBookCenterUISprite.spriteName = "ribbonmap_stagem";
            WordBookLessonNumUILabel.text = g.curStage.ToString();
            WordBookLessonRankUISprite.enabled = true;
            WordBookLessonRankUISprite.spriteName = g.curRankName;
        }
        WordBookLessonIconUISprite.spriteName = g.curIconName;
		
		//clickLessonNum.text.ToString();
		
        //-------------------------- 
        // SettingWordBookContent
        //--------------------------
		yield return StartCoroutine ("SettingWordBookContent");
		
		WordBookPanelTweenPos.from = new Vector3(0,0,0);
		WordBookPanelTweenPos.to   = new Vector3(0,300,0);
		WordBookPanelTweenPos.enabled = true;
		WordBookPanelTweenPos.ResetToBeginning();
		
		WordBookBottomTweenPos.from = new Vector3(0,275,0);
		WordBookBottomTweenPos.to   = new Vector3(0,-340,0);
		WordBookBottomTweenPos.enabled = true;
		WordBookBottomTweenPos.ResetToBeginning();
		
		blurTweenAlpha.from = 1.0f;
		blurTweenAlpha.to   = 1.0f;
		blurTweenAlpha.enabled = true;
		blurTweenAlpha.ResetToBeginning();
		
		//		gridBgTweenAlpha.from = 1.0f;
		//		gridBgTweenAlpha.to   = 0.1f;
		//		gridBgTweenAlpha.enabled = true;
		//		gridBgTweenAlpha.ResetToBeginning();
		
		yield return new WaitForSeconds(1.0f);
	}
	
	public void EventWordBookOffRelease() {
		Debug.Log("EventWordBookOffRelease");
		ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            StopCoroutine ( "RepeatTTS" ); isWordBookRepeatTTS = true;
            StartCoroutine(OffClickWordBook());
        }
	}
	IEnumerator OffClickWordBook() {

		GridBoxCollider(true);
		
		WordBookPanelTweenPos.from = new Vector3(0,300,0);
		WordBookPanelTweenPos.to   = new Vector3(0,0,0);
		WordBookPanelTweenPos.enabled = true;
		WordBookPanelTweenPos.ResetToBeginning();
		
		WordBookBottomTweenPos.from = new Vector3(0,-340,0);
		WordBookBottomTweenPos.to   = new Vector3(0,275,0);
		WordBookBottomTweenPos.enabled = true;
		WordBookBottomTweenPos.ResetToBeginning();
		
		blurTweenAlpha.from = 1.0f;
		blurTweenAlpha.to   = 0.0f;
		blurTweenAlpha.enabled = true;
		blurTweenAlpha.ResetToBeginning();
		
		//		gridBgTweenAlpha.from = 0.1f;
		//		gridBgTweenAlpha.to   = 1.0f;;
		//		gridBgTweenAlpha.enabled = true;
		//		gridBgTweenAlpha.ResetToBeginning();
		
		yield return new WaitForSeconds(1.0f);
		
		WordBookContentGrid1.SetActive(true);
		
		Transform[] trans = WordBookContentGrid1.GetComponentsInChildren<Transform>(); 
		foreach (Transform tr in trans) {
			if(tr.gameObject.tag == "WordBookItem") Destroy(tr.gameObject); 
		}
		
		WordBookContentGrid1.GetComponent<UIGrid>().Reposition();
		
		BlurPanel.SetActive(false);
        mapSceneImg.enabled = true;

		WordBookPanel.SetActive(false);
        GridBg.SetActive(true);
  		WordBg2.SetActive(false);

	}
	
	public void EventWordBookItemOnRelease(GameObject gameo){
		ev.EventOnRelease();
        audioSource.clip = menuClip; audioSource.Play();

        int turnTexNum = int.Parse ( gameo.name.Substring(12,1) );
        //Debug.Log("turnTexNum="+turnTexNum);
        _data = dbo.SelTextureInfoTurn(g.curStage,turnTexNum); dr = _data.Rows[0];
        PlayTTS(dr["texShowName"].ToString());
	}

	public void EventWordBookItemDicOnRelease(UILabel clickWordBookWord){
		ev.EventOnRelease();
        audioSource.clip = menuClip; audioSource.Play();
        Application.OpenURL ("http://dic.daum.net/search.do?dic=en&search_first=Y&q="+clickWordBookWord.text);
	}

    bool isWordBookRepeatTTS = true;
	public void EventWordBookRepeatTTSOnRelease(){
		ev.EventOnRelease();
        audioSource.clip = menuClip; audioSource.Play();
        if(isWordBookRepeatTTS) {
            StartCoroutine ( "RepeatTTS" );
            isWordBookRepeatTTS = false;
        } else {
            StopCoroutine ( "RepeatTTS" );
            isWordBookRepeatTTS = true;
        }
	}

    bool isWordBookRepeatMeanTTS = true;
	public void EventWordBookRepeatMeanTTSOnRelease(){
		ev.EventOnRelease();
        audioSource.clip = menuClip; audioSource.Play();
        if(isWordBookRepeatMeanTTS) {
            StartCoroutine ( "RepeatMeanTTS" );
            isWordBookRepeatMeanTTS = false;
        } else {
            StopCoroutine ( "RepeatMeanTTS" );
            isWordBookRepeatMeanTTS = true;
        }
	}



    IEnumerator RepeatTTS(){
        _data = dbo.SelTextureInfo(g.curStage,"");
		int rowCnt = _data.Rows.Count;
        while(true) {
		    for (int i = 0; i < rowCnt; i++) {
			    dr = _data.Rows[i];
                //Debug.Log(dr["texShowName"].ToString() );
                PlayTTS(dr["texShowName"].ToString());
                yield return new WaitForSeconds(1.5f);
            }
        }
    }

    IEnumerator RepeatMeanTTS(){
        _data = dbo.SelTextureInfo(g.curStage,"");
		int rowCnt = _data.Rows.Count;
        while(true) {
		    for (int i = 0; i < rowCnt; i++) {
			    dr = _data.Rows[i];
                PlayTTS(dr["texShowName"].ToString());
                yield return new WaitForSeconds(1.5f);
                PlayTTS(WordMeaning());
                yield return new WaitForSeconds(1.5f);
            }
        }
    }



	IEnumerator SettingWordBookContent() {
		
		Debug.Log("SettingWordBookContent");
		
		yield return null;
		Color WordBookItemColor = Color.grey;

        _data = dbo.SelTextureInfoCorrect(g.curStage);
		
		int rowCnt = _data.Rows.Count;
		
		GameObject WordBookItemClone, WordBookItemMeaningClone;
        UILabel wordBookItemCloneUILabel;

		UIEventTrigger wordBookItemCloneUIEventTrigger, wordBookItemDicCloneUIEventTrigger;
		for (int i = 0; i < rowCnt; i++) {
			dr = _data.Rows[i];
			
			WordBookItemClone = Instantiate(WordBookItemPrefab) as GameObject;
			WordBookItemClone.name = "WordBookItem" + (i+1).ToString();

            WordBookItemMeaningClone = Instantiate(WordBookItemMeaningPrefab) as GameObject;
			WordBookItemMeaningClone.name = "WordBookItemMeaning" + (i+1).ToString();
			
			WordBookItemClone.transform.parent = WordBookContentGrid1.transform;
			WordBookItemClone.transform.localPosition = new Vector3( 0f, -i * 100 , 0f );

			WordBookItemMeaningClone.transform.parent = WordBookContentGrid1.transform;
			WordBookItemMeaningClone.transform.localPosition = new Vector3( 0f, (-i * 100) - 40 , 0f );

			//WordBookContentGrid1.GetComponent<UIGrid>().Reposition();
			
			WordBookItemClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
			WordBookItemClone.transform.localScale = Vector3.one;
			WordBookItemMeaningClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
			WordBookItemMeaningClone.transform.localScale = Vector3.one;

            wordBookItemCloneUILabel = WordBookItemClone.transform.FindChild("WordBookItemPrefab").GetComponent<UILabel>();
            wordBookItemCloneUILabel.text = dr["texShowName"].ToString();
            //Debug.Log("dr[texShowName].ToString()"+dr["texShowName"].ToString() );
            //Debug.Log("dr[example1].ToString()"+dr["example1"].ToString() );

            WordBookItemMeaningClone.GetComponent<UILabel>().text = WordMeaning();
            //WordBookItemMeaningClone.GetComponent<UILabel>().fontSize

			if      (dr["comboYn"].ToString() == "Y") WordBookItemColor = Color.blue;
			else if (dr["state"].ToString() == "0") WordBookItemColor = Color.grey;
			else if (dr["state"].ToString() == "1") WordBookItemColor = new Color(5f / 255f, 107f / 255f, 0f / 255f);
			else if (dr["state"].ToString() == "2") WordBookItemColor = Color.red;
			
			wordBookItemCloneUIEventTrigger = WordBookItemClone.transform.FindChild("WordBookItemPrefab").GetComponent<UIEventTrigger>();
			EventDelegate.Add (wordBookItemCloneUIEventTrigger.onPress,ev.EventOnPress);
			EventDelegate eventClick = new EventDelegate(this, "EventWordBookItemOnRelease");
			EventDelegate.Parameter param = new EventDelegate.Parameter();
			param.obj = WordBookItemClone;
			param.expectedType = typeof(GameObject);
			eventClick.parameters[0] = param;
			EventDelegate.Add (wordBookItemCloneUIEventTrigger.onRelease,eventClick);

			wordBookItemDicCloneUIEventTrigger = WordBookItemClone.transform.FindChild("WordBookItemDic").GetComponent<UIEventTrigger>();
			EventDelegate.Add (wordBookItemDicCloneUIEventTrigger.onPress,ev.EventOnPress);
			eventClick = new EventDelegate(this, "EventWordBookItemDicOnRelease");
			param = new EventDelegate.Parameter();
			param.obj = wordBookItemCloneUILabel;
			param.expectedType = typeof(UILabel);
			eventClick.parameters[0] = param;
			EventDelegate.Add (wordBookItemDicCloneUIEventTrigger.onRelease,eventClick);
			
			wordBookItemCloneUILabel.color = WordBookItemColor;
			wordBookItemCloneUILabel.tag = "WordBookItem";

            WordBookItemMeaningClone.GetComponent<UILabel>().color = WordBookItemColor;
            WordBookItemMeaningClone.GetComponent<UILabel>().tag = "WordBookItem";
		}
		WordBookContentGrid1.SetActive(true);
	}

    string WordMeaning() {
        string meaning = "";
        meaning = "(" + dr["meaning"].ToString() + ")";
        return meaning;
    }

    //----------------------------------------------------------------------
    //                               Achive
    //----------------------------------------------------------------------
    public void EventAchiveOnRelease()
    {
        Debug.Log("EventAchiveOnRelease");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            audioSource.clip = menuClip; audioSource.Play();
            Social.ShowAchievementsUI();
        }
    }


    //----------------------------------------------------------------------
    //                               Part
    //----------------------------------------------------------------------

    int partStageNum;
    public void EventPartOnRelease(GameObject Part)
    {
        Debug.Log("EventAchiveOnRelease");
        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();

            g.partNum = int.Parse(Part.name.Substring(4, 2));

            _data = dbo.SelPartStageNum(g.partNum); dr = _data.Rows[0];
            partStageNum = int.Parse( dr["minStageNum"].ToString() );

            if (!PartLocks[g.partNum-1].enabled)
            {
                gridUIGrid.Reposition();
                Grid.GetComponent<UICenterOnChild>().Recenter();
                springPanel.target = new Vector3(0, (partStageNum - 1) * gridUIGrid.cellWidth, 0);
            }
            else {
                popupMapControl.EventShowPartItemPopup();
                //Debug.Log("showPopup");
            }
        }
    }

    public void SetPartLock()
    {
        //Debug.Log("g.partMove1=" + g.partMove);

        PartLocks[0].enabled = g.partMove.Substring(0, 1) == "0" ? true : false;
        PartLocks[1].enabled = g.partMove.Substring(1, 1) == "0" ? true : false;
        PartLocks[2].enabled = g.partMove.Substring(2, 1) == "0" ? true : false;
        PartLocks[3].enabled = g.partMove.Substring(3, 1) == "0" ? true : false;
        PartLocks[4].enabled = g.partMove.Substring(4, 1) == "0" ? true : false;
        PartLocks[5].enabled = g.partMove.Substring(5, 1) == "0" ? true : false;
        PartLocks[6].enabled = g.partMove.Substring(6, 1) == "0" ? true : false;
        PartLocks[7].enabled = g.partMove.Substring(7, 1) == "0" ? true : false;
        PartLocks[8].enabled = g.partMove.Substring(8, 1) == "0" ? true : false;

        //Debug.Log("g.partMove2=" + g.partMove);

    }

    public void SetPartLocks(int i, bool tf) {
        PartLocks[i].enabled = tf;
    }

    //----------------------------------------------------------------------
    //                                 Util
    //----------------------------------------------------------------------
    public void mapBgCol() {
        g.SetBgColor();
        g.col = g.bgColOptions[g.userBgCol];

        mapSceneBg.color = g.col;
        blurSceneBg.color = g.col;
        WordExpressionScreenBg.color = g.col;
        WordBookScreenBg.color = g.col;

        loadingBgUISprite.color = g.col;

        //mapTopUISprite.color = g.col;
        //mapBottomUISprite.color = g.col;
    }

    public void SetHpSkipCoinValue() {
        coinAmount.text = g.coin.ToString();
        coinLineAmount.text = g.coin.ToString();
        skipAllNumUILabel.text = g.allSkip().ToString();
        skipAllNumLineUILabel.text = g.allSkip().ToString();
        hpAllNumUILabel.text = g.allHp().ToString();
        hpAllNumLineUILabel.text = g.allHp().ToString();
        skipNumUILabel.text = g.allSkip().ToString();
        skipNumLineUILabel.text = g.allSkip().ToString();
        hpNumUILabel.text = g.allHp().ToString();
        hpNumLineUILabel.text = g.allHp().ToString();
    }

    public void SetGiftYn() {

        _data = dbo.SelUserGiftHis();

        if(_data.Rows.Count> 0) {
            giftUISprite.spriteName = "gifticon2";
            giftButtonAnimator.SetBool("isButtonWiggle", true);
            giftBoxCollider.enabled = true;
        } else { 
            giftUISprite.spriteName = "gifticon";
            giftButtonAnimator.SetBool("isButtonWiggle", false);
            giftBoxCollider.enabled = false;
        }
    }

    public void SetBanner()
    {
        //Debug.Log("SetBanner");

        if (g.noBannerYn == "Y")
        {
            noBannerContextUISprite.color = g.inActiveCol;
            noBannerBoxCollider.enabled = false;
        }
    }	

	void GridBoxCollider(bool isEnabled) {
		
		//Debug.Log("GridBoxCollider");
		
		BoxCollider[] boxColliders = Grid.transform.GetComponentsInChildren<BoxCollider>();
		for(int i=0;i<boxColliders.Length;i++){
			boxColliders[i].enabled = isEnabled;
		}
		
	}

    float _pitch = 1f;
    float _speechRate = 0f;
    int blankIdx;
    string context1, context2;
    public void PlayTTS(string context) {
        if(g.isSound) _pitch = 1f; else _pitch = 0f;

        blankIdx = context.IndexOf("_");

        if (blankIdx < 0) {
            #if UNITY_ANDROID && !UNITY_EDITOR
                TTSManager.Speak(context + ".", false, TTSManager.STREAM.Music, _pitch, _speechRate, transform.name, null, null);
            # endif
        } else {
            context1 = context.Substring(0, blankIdx);
            context2 = context.Substring(blankIdx+1, context.Length - (blankIdx+1) );

            StartCoroutine( MultyPlayTTS() );
        }
    }

    public IEnumerator MultyPlayTTS()
    {
        if(g.isSound) _pitch = 1f; else _pitch = 0f;

        #if UNITY_ANDROID && !UNITY_EDITOR
            TTSManager.Speak(context1 + ".", false, TTSManager.STREAM.Music, _pitch, _speechRate, transform.name, null, null);
        # endif
        yield return new WaitForSeconds(1f);
        #if UNITY_ANDROID && !UNITY_EDITOR
            TTSManager.Speak(context2 + ".", false, TTSManager.STREAM.Music, _pitch, _speechRate, transform.name, null, null);
        # endif
    }



    //--------------------------------------------------------------
    //                            Event
    //--------------------------------------------------------------

    //---------------------
    // Home
    //---------------------
    public void EventHomeSceneOnRelease()
    {
        Debug.Log("EventMapOnRelease");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            BlankSceneBg.SetActive(true);
            GridBoxCollider(false); StartCoroutine(navi.GoHomeScene());
        }
    }

    //---------------------
    // Intro Help
    //---------------------
    public void EventIntroHelpOnRelease()
    {
        Debug.Log("EventIntroHelpOnRelease");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            Application.OpenURL ("http://crazyword.org/help.html");
        }
    }


    //---------------------------------------------------------------
    //                            Popup
    //---------------------------------------------------------------

    //-------------------------------------------
    //          InternetWarning Popup
    //-------------------------------------------
    public GameObject InternetWarning;
    public Animator internetWarningAnimator;

    public void EventInternetWarning() {
        Debug.Log("EventInternetWarning");

        audioSource.clip = startClip; audioSource.Play();
        InternetWarning.SetActive(true);
        PopupBg8.SetActive(true);

        internetWarningAnimator.speed = 2;
        internetWarningAnimator.SetBool("isSmallLargeScale2", true);
    }

    public void EventInternetWarningOK() {
        Debug.Log("EventInternetWarningOK");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) EventInternetWarningClose();
    }

    public void EventInternetWarningClose() {
        Debug.Log("EventInternetWarningClose");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            audioSource.clip = startClip; audioSource.Play();
            InternetWarning.SetActive(false);
            PopupBg8.SetActive(false);
            internetWarningAnimator.SetBool("isSmallLargeScale2", false);
        }
    }

    //-------------------------------------------
    //          QuitWarning Popup
    //-------------------------------------------
	void Update_TouchEvent_Zone__() {}
    public GameObject PopupBg8,WordBg2;
    public GameObject QuitWarningPopup;
    public Animator quitWarningPopupAnimator;
    public void EventQuitWarning() {

        if(!g.isPopupPartItemShow && 
           !g.isPopupShopShow && 
           !g.isPopupPurchaseShow &&
           !g.isPopupFriendShow &&
           !g.isPopupGiftShow &&
           !g.isPopupOptionShow
            ) {
        //if(g.isPopupShopMapShow) {
        //    Debug.Log("isPopupShopMapShow");
        //    popupShopControl.EventCloseShopPopup();
        //} else {
            Debug.Log("EventQuitWarning");
            audioSource.clip = startClip; audioSource.Play();
            QuitWarningPopup.SetActive(true);
            PopupBg8.SetActive(true);
            quitWarningPopupAnimator.speed = 2;
            quitWarningPopupAnimator.SetBool("isSmallLargeScale2", true);
        //}
        }
    }
    public void EventQuitWarningOK() {
        Debug.Log("EventQuitWarningOK");
        audioSource.clip = startClip; audioSource.Play();
        Application.Quit();
    }
    public void EventQuitWarningClose() {
        Debug.Log("EventQuitWarningClose");
        audioSource.clip = startClip; audioSource.Play();
        QuitWarningPopup.SetActive(false);
        PopupBg8.SetActive(false);
        quitWarningPopupAnimator.SetBool("isSmallLargeScale2", false);
    }	

    //---------------------------------------------------------------
    //                           Update
    //---------------------------------------------------------------
	void Update () {	
		
		if(Input.GetKeyDown(KeyCode.Escape)) EventQuitWarning();
   		//if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
		
		//--------------------------------
		// Panel
		//--------------------------------
		WordBookPanel.GetComponent<UIPanel>().clipOffset = WordBookPanel.transform.localPosition * -1;
        WordBookBlurTopBg.transform.position = new Vector3(0f, 0.65f, 0f);
		
		WordExpressionPanel.GetComponent<UIPanel>().clipOffset = WordExpressionPanel.transform.localPosition * -1;
        WordExpressionBlurBottomBg.transform.position = new Vector3(0f, -0.35f, 0f);
		
		//Debug.Log("g.hpAddTime="+g.hpAddTime);
		
		//Debug.Log("g.hpAddTime="+g.hpAddTime);
		
		//--------------------------------
		// Skip
		//--------------------------------
        if (g.allSkip() >= g.maxBonusSkipCnt) // maxFreeSkipCnt:3
        { 
			skipTimeUISprite.enabled = true;
			skipTimeUILabel.text = "";
			skipTimeLineUILabel.text = "";
		} else {
			skipTimeUISprite.enabled = false;
            for (int i = 1; i <= g.freeSkipMinuteTime; i++) { //freeSkipMinuteTime:32
                if (g.skipAddTime < minute1)
                    skipTimeUILabel.text = "00:" + g.skipAddTime.ToString("00");
                else if (i < 10 && g.skipAddTime >= (minute1 * i) && g.skipAddTime < (minute1 * (i + 1)))
                    skipTimeUILabel.text = "0" + i + ":" + (g.skipAddTime - (minute1 * i)).ToString("00");
                else if (g.skipAddTime >= (minute1 * i) && g.skipAddTime < (minute1 * (i + 1)))
                    skipTimeUILabel.text = i + ":" + (g.skipAddTime - (minute1 * i)).ToString("00");
			}
		
		}
		skipTimeLineUILabel.text = skipTimeUILabel.text;
		
		//--------------------------------
		// Hp
		//--------------------------------
		if (g.allHp() >= g.maxBonusHpCnt) { //maxFreeHpCnt:10
			hpTimeUISprite.enabled = true;
			hpTimeUILabel.text = "";
			hpTimeLineUILabel.text = "";
		} else {
			hpTimeUISprite.enabled = false;
            for (int i = 1; i <= g.freeHpMinuteTime; i++) { // freeHpMinuteTime:12
				if (g.hpAddTime < minute1)
					hpTimeUILabel.text = "00:" + g.hpAddTime.ToString ("00");
				else if (i < 10 && g.hpAddTime >= (minute1 * i) && g.hpAddTime < (minute1 * (i + 1)))
					hpTimeUILabel.text = "0" + i + ":" + (g.hpAddTime - (minute1 * i)).ToString ("00");
				else if (g.hpAddTime >= (minute1 * i) && g.hpAddTime < (minute1 * (i + 1)))
					hpTimeUILabel.text = i + ":" + (g.hpAddTime - (minute1 * i)).ToString ("00");
			}

            if (g.hpAddTime == 0) SetHpSkipCoinValue();
			hpTimeLineUILabel.text = hpTimeUILabel.text;
		}

        //if (g.hpAddTime == 0 || g.skipAddTime == 0) SetHpSkipCoinValue();
        SetHpSkipCoinValue();
		
	}

}