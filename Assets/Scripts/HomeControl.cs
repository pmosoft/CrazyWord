using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using SimpleJSON;

public class HomeControl : MonoBehaviour {

    //---------------------------------------------------------
    // Common
    //---------------------------------------------------------
	public DBO dbo; public DBOW dbow; public Navi navi; public EventCommon ev;
    public AudioClip bgClip, startClip; public AudioSource bgAudioSource, buttonAudioSource;
	DataTable _data; DataRow dr;

    //---------------------------------------------------------
    // Home
    //---------------------------------------------------------
    public GameObject HomePanel,GameStart,Loading;
	float colorChangeTime, offset;
	float hue; Color rgbCol; HSBColor hsbCol;
	public UISprite homeBgUISprite, startGameUISprite;
	public BoxCollider startGameBoxCollider,facebookLoginBoxCollider,shareBoxCollider;

	//---------------------------------------------------------
	// ScatterLetter
	//---------------------------------------------------------
	public GameObject ScatterLetter, ScatterLetterPrefab;
	private UILabel[] scatterLetterUILabels01,scatterLetterUILabels02;
    private TweenPosition[] scatterLetterTweenPoss01,scatterLetterTweenPoss02;
	private Vector3[] VectorPosX1, VectorPosX2; 
	private string[] texNames;
	private int letterCnt;

    //---------------------------------------------------------
    // Facebook
    //---------------------------------------------------------
    public FacebookLogin FbLogin;
    public UISprite fbUserBgUISprite;
    public UITexture FacebookMyImage;
    public GameObject FacebookLoginBtn;

    //---------------------------------------------------------
    // Android Share
    //---------------------------------------------------------
    #if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject curActivity;
    # endif

    //---------------------------------------------------------
    // Popup
    //---------------------------------------------------------
    public GameObject PopupBg8,PopupBg12;
    public GameObject InternetWarningExit, InternetWarning, FacebookWarning, PatchWarning;
    public GameObject GoogleLoginWarning, UpdateUserInfoWarning, QuitWarningPopup, NickNameWarningPopup;

    public GameObject UserFirstEnterPopup,InformPopup;
    
    public Animator internetWarningExitAnimator,internetWarningAnimator, facebookWarningExitAnimator, patchWarningAnimator;
    public Animator googleLoginWarningAnimator, updateUserInfoWarningAnimator, quitWarningPopupAnimator;
    public Animator userFirstEnterAnimator,informPopupAnimator,nickNameWarningPopupAnimator;

    public UISprite userFirstEnterChkUISprite,informChkUISprite;

    public UITexture informUITexture;
    public UITexture[] informUITextures = new UITexture[6];

    public UITexture[] userFirstEnterUITextures = new UITexture[5];

    int informPopupCnt = 0, informPopupIdx = 0;

    public GameObject NickName;
    public UILabel nickNameUILabel,nickNameWarningLabel,startPlayUILabel;


    Texture2D downTexture;
    List<string> popupIds = new List<string>();
    List<string> popupDownUrls = new List<string>();
    List<string> popupTexNames = new List<string>();

    string popupTexPath;
    //---------------------------------------------------------
    // State
    //---------------------------------------------------------
    bool isGoogleLogin = false, isUpdateUserInfoShow = false;
    bool isCrazyWordPopupFirstEnter = false, isCrazyWordPopup = false;
    bool isQuitWarning = false;

    //---------------------------------------------------------
    // Etc
    //---------------------------------------------------------
    public UISprite soundUISprite;

	void Awake() {
		hue = Random.Range(0.0f,1.0f);
        popupTexPath = Application.persistentDataPath + "/Textures/popup/";
	}

    void Start()
    {
        //------------------------
        // FadeIn
        //------------------------
        StartCoroutine( navi.FadeIn() );

        #if     UNITY_EDITOR
            //Debug.Log("Unity Editor");
            g.googleId = "113424592816649022426";
            g.googleName = "crazy word";
            if(g.isFirstHome) StartCoroutine ( "HomeFirstStart" );
            HomeStart();

        #endif
        
        #if UNITY_ANDROID && !UNITY_EDITOR
            if(g.isFirstHome) StartCoroutine ( "HomeFirstStart" );
            HomeStart();
            //if (g.isFirstHome) GooglePlayLogin();
            //else HomeStart();
        # endif
    }

    IEnumerator HomeFirstStart() {
    //public void HomeFirstStart() {
        Debug.Log("HomeFirstStart");
        Debug.Log("curVersion=" + g.curVersion);
        //--------------------------------------
        // State
        //--------------------------------------

        g.isFirstHome = false;
        //startGameBoxCollider.enabled = false;
        //facebookLoginBoxCollider.enabled = false;
        //shareBoxCollider.enabled = false;

        GameStart.SetActive(false);
        Loading.SetActive(true);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //--------------------------------------
        // Check Internet
        //--------------------------------------
        WWW www = new WWW(g.internetConnectTestUrl);
        yield return www;

        if (www.isDone && www.error == null) {
            Debug.Log("CheckInternet www.text=" + www.text);
        } else { Debug.Log("CheckInternet www.error=" + www.error);
            EventInternetWarningExit();
            yield return new WaitForSeconds(3600f);
        }

        //--------------------------------------
        // Check Google Login
        //--------------------------------------
        #if UNITY_ANDROID && !UNITY_EDITOR
            GooglePlayGames.PlayGamesPlatform.Activate();

            Social.localUser.Authenticate((bool success) =>
            {
                if (success) {
                    isGoogleLogin = true;
                    g.googleId = Social.localUser.id;
                    g.googleName = Social.localUser.userName;

                    //--------------------------------------
                    // Check userID and googleId
                    //--------------------------------------
                    if (g.userId != g.googleId && g.userId.Length > 5) {  
                        UpdateUserInfoWarning.SetActive(true);
                    }
                } else {
                    Debug.Log("Error Google Play Login ");
                    EventGoogleLoginWarning();
                    // error popup game out
                }
            });

            while(!isGoogleLogin && !isUpdateUserInfoShow) yield return new WaitForSeconds(0.1f);

        # endif


        //--------------------------------------
        // Create User
        //--------------------------------------
        if (!dbo.IsUserExist()) dbo.CreateUser();
        dbo.SelUserInfo();

        //------------------------------------------------------------
        // www SelUserInfo(or InsUserInfo)
        //------------------------------------------------------------
        yield return StartCoroutine(dbow.SelUserInfo());

        //------------------------------------------------------------
        // Checking Sqlite's patch from Server
        //------------------------------------------------------------
        //yield return StartCoroutine ("ModifySqlite");

        //--------------------------------------
        // Facebook 
        //--------------------------------------
        FbLogin.StartInfo();

        //--------------------------------------
        // TTS 
        //--------------------------------------
        #if UNITY_ANDROID && !UNITY_EDITOR
            TTSControl.instance.Init();
        # endif

        //------------------------------------------------------------
        // Init CopyData, only 1
        //------------------------------------------------------------
        StartCoroutine(CopyBgImgData());
        yield return StartCoroutine(CopyStartHelpData());
       
        //StartCoroutine ( CopyStageTex() );
        
        //--------------------------------------
        // Check Patch
        //--------------------------------------
        //StartCoroutine(CopyBgImgData());
        if(g.curVersion < g.patchVersion) {
            EventPatchWarning();
            yield return new WaitForSeconds(3600f);
        }

        //------------------------------------------------------------
        // Inform Popup User's First Enterance
        //------------------------------------------------------------
        if(dbo.SelUserFirstEnterPopup() > 0) {
            StartCoroutine( CrazyWordPopup() );
        } else {
            //startGameBoxCollider.enabled = true;
            //facebookLoginBoxCollider.enabled = true;
            //shareBoxCollider.enabled = true;
            isQuitWarning = true;
            Loading.SetActive(false);
            GameStart.SetActive(true);
        }


        //imsi
        //EventUserFirstEnterPopup();

    }

	//IEnumerator Start() {
    public void HomeStart() {
        Debug.Log("HomeStart");
        //--------------------------------------
        // State
        //--------------------------------------
        g.isHome = true; g.isMap = false; g.isGame = false;
        //Debug.Log("g.fbUserPicUrl"+g.fbUserPicUrl);
        if(g.fbUserPicUrl.Length > 10) {
            StartCoroutine ( FbLogin.LoadMyPicture() );
        }

		// AdMob
        #if UNITY_ANDROID && !UNITY_EDITOR
            AdMobAndroid.hideBanner(true);
        # endif

        Camera.main.transform.GetComponent<AudioSource>().enabled = g.isSound;
		StartCoroutine (HomeBackgroundColor ());

        //--------------------------------------
		// Sound
        //--------------------------------------
        if(g.isSound == true) {
            bgAudioSource.enabled = true;
            bgAudioSource.volume = 0.750f; 
            buttonAudioSource.volume = g.buttonVolume;
            soundUISprite.spriteName = "soundnyes";
            bgAudioSource.Play();
        } else {
            bgAudioSource.volume = 0f; buttonAudioSource.volume = 0f;
            soundUISprite.spriteName = "soundno";
        }

        //--------------------------------------
        // Android Share
        //--------------------------------------
        #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            curActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        # endif

        //--------------------------------------
		// ScatteLetter
        //--------------------------------------
		SetScatteLetter();		
		StartCoroutine( "scatterLetter01" );
		StartCoroutine( "scatterLetter02" );

        bgAudioSource.clip = bgClip; bgAudioSource.Play();

        if (FB.IsLoggedIn == false) {
            FacebookLoginBtn.SetActive(true); 
            FacebookMyImage.gameObject.SetActive(false);
        } else {
            StartCoroutine ( FbLogin.LoadMyPicture() );
        }

        if(!g.isFirstHome) {
            //startGameBoxCollider.enabled = true;
            //facebookLoginBoxCollider.enabled = true;
            //shareBoxCollider.enabled = true;
            //isQuitWarning = true;
        }        
	}

    //---------------------------------------------------------------
    //                              WWW
    //---------------------------------------------------------------
    IEnumerator CrazyWordPopup(){

        DirectoryInfo di;

        WWW www1;
        Texture2D downTexture1;
        byte[] bytes1;
        string texExtention = "jpg";
        isCrazyWordPopupFirstEnter = true;

        //-------------------------------
        // Sel CrazyWordPopup
        //-------------------------------
        _data = dbo.SelCrazyWordPopup();
        informPopupCnt = _data.Rows.Count; if(informPopupCnt >= 6) informPopupCnt = 6;

        di = new DirectoryInfo(popupTexPath);
        if (di.Exists == false) di.Create();
        for(int i = 0; i < informPopupCnt; i++) {
            dr = _data.Rows[i];
            
            //Debug.Log( "popupDownUrl="+dr["popupDownUrl"].ToString() );
            //Debug.Log( "popupTexName="+dr["popupTexName"].ToString() );
            popupIds.Add( dr["popupId"].ToString() );
            popupDownUrls.Add( dr["popupDownUrl"].ToString() );
            popupTexNames.Add( dr["popupTexName"].ToString() );


            if (!System.IO.File.Exists(popupTexPath+popupTexNames[i])) {
                //Debug.Log("down:"+popupTexNames[i]);
                www1 = new WWW( popupDownUrls[i] );
                yield return www1;
                downTexture1 = new Texture2D(www1.texture.width, www1.texture.height, TextureFormat.ARGB32, false);
                www1.LoadImageIntoTexture(downTexture1);
                if(popupDownUrls[i].Contains(".png")) texExtention = "png";;
                if(texExtention == "png") bytes1 = downTexture1.EncodeToPNG(); else bytes1 = downTexture1.EncodeToJPG();
                File.WriteAllBytes(popupTexPath+popupTexNames[i], bytes1);
                while(!www1.isDone) yield return new WaitForSeconds(0.1f);
            }

   		    WWW www = new WWW("file:///" + popupTexPath + popupTexNames[informPopupIdx]); 
            yield return www;
		    Texture2D downTexture = new Texture2D(www.texture.width, www.texture.height,TextureFormat.ARGB32, false);
		    www.LoadImageIntoTexture(downTexture); 
            informUITextures[i].mainTexture = downTexture;
        }

        //---------------------------------------------
        // Delay
        //---------------------------------------------
        yield return new WaitForSeconds(2f);

        //---------------------------------------------
        // Execute Popup
        //---------------------------------------------
        if(isCrazyWordPopupFirstEnter) EventUserFirstEnterPopup();
        else if(informPopupCnt > 0) {
            informPopupIdx = 0;
            //StartCoroutine ( EventInformPopup() );
            EventInformPopup();
        }
    }

    IEnumerator CopyStageTex()
    {
		int stageNum,turnTexNum,texNum;
		string texName,downYn;

		string texPath,texFileName,resPath;
		DirectoryInfo di;

		_data = dbo.SelDownTextureZone("bulk");
		
		Debug.Log("_data.Rows.Count=" + _data.Rows.Count);
		
		//------------------------------
		// start www download
		//------------------------------
		for (int j = 0; j < _data.Rows.Count; j++) {
			
			dr = _data.Rows[j];
			
			stageNum = int.Parse( dr["stageNum"].ToString() );
			turnTexNum = int.Parse( dr["turnTexNum"].ToString() );
			texName = dr["texName"].ToString();
			texNum = int.Parse( dr["texNum"].ToString() );
			downYn = dr["downYn"].ToString();
			
			if(downYn == "N" ) {

				Debug.Log("stageNum="+stageNum+" texName=" + texName+ " texNum="+texNum);

			    texPath  = Application.persistentDataPath + "/Textures/v"+g.volumn.ToString("000");
				texPath += "/s"+stageNum.ToString("000")+"/";
			    resPath  = "Textures/v"+g.volumn.ToString("000")+"/s"+stageNum.ToString("000")+"/";
                texFileName  = texName + "_"+texNum.ToString("00")+".jpg";

				di = new DirectoryInfo(texPath);
				if (di.Exists == false) di.Create();
                //Debug.Log(resPath+texFileName);
                TextAsset ta = (TextAsset)Resources.Load(resPath+texFileName);
                System.IO.File.WriteAllBytes(texPath + texFileName, ta.bytes); // 64MB limit on File.WriteAllBytes.
                ta = null;
					
				dbo.UptTextureInfo(stageNum,turnTexNum,texNum,"Y","Y");
			}
		}
        yield return null;
    }

    IEnumerator CopyBgImgData() 
    {
        string imgPath, imgFile;
        DirectoryInfo di;

        WWW www1;
        Texture2D wwwTexture1;

        imgPath = Application.persistentDataPath + "/Textures/bg/";
        imgFile = "11.png";

        if (!System.IO.File.Exists(imgPath + imgFile)) {
            //Debug.Log("copy file =" + imgPath + imgFile);

            di = new DirectoryInfo(imgPath);
            if (di.Exists == false) di.Create();

            g.userBgImgTextures[0] = new Texture2D(720, 1280, TextureFormat.ARGB32, false);

            for(int i = 1; i <= 4; i++) {
                TextAsset ta = (TextAsset)Resources.Load("Textures/bg/" + i + i + ".png");
                System.IO.File.WriteAllBytes(imgPath + i + i + ".png", ta.bytes); // 64MB limit on File.WriteAllBytes.
                ta = null;
            }
        }
        for (int i = 1; i <= 4; i++) {
            www1 = new WWW("file:///" + imgPath + i + i + ".png");
            yield return www1;
            wwwTexture1 = new Texture2D(720, 1280, TextureFormat.ARGB32, false);
            www1.LoadImageIntoTexture(wwwTexture1);
            g.userBgImgTextures[i] = wwwTexture1;
        }
        g.userBgImgTextures[5] = new Texture2D(720, 1280, TextureFormat.ARGB32, false);
        if (System.IO.File.Exists(Application.persistentDataPath + "/Textures/bg/55.png")) {
            www1 = new WWW("file:///" + imgPath + "55.png");
            yield return www1;
            wwwTexture1 = new Texture2D(720, 1280, TextureFormat.ARGB32, false);
            www1.LoadImageIntoTexture(wwwTexture1);
            g.userBgImgTextures[5] = wwwTexture1;
        }
    }


    IEnumerator CopyStartHelpData()
    {
        string imgPath, imgFile;
        DirectoryInfo di;
        WWW www1;
        Texture2D wwwTexture1;

        imgPath = Application.persistentDataPath + "/Textures/StartHelp/";
        imgFile = "minihelp1.png";

        if (!System.IO.File.Exists(imgPath + imgFile)) {
            Debug.Log("copy file =" + imgPath + imgFile);

            di = new DirectoryInfo(imgPath);
            if (di.Exists == false) di.Create();

            for (int i = 1; i <= 5; i++)
            {
                TextAsset ta = (TextAsset)Resources.Load("Textures/StartHelp/minihelp" + i + ".png");
                System.IO.File.WriteAllBytes(imgPath + "minihelp" + i + ".png", ta.bytes); // 64MB limit on File.WriteAllBytes.
                ta = null;
            }
        }

        for (int i = 0; i < 5; i++) {
            www1 = new WWW("file:///" + imgPath + "minihelp" + (i+1) + ".png");
            yield return www1;
            wwwTexture1 = new Texture2D(478, 674, TextureFormat.ARGB32, false);
            www1.LoadImageIntoTexture(wwwTexture1);
            userFirstEnterUITextures[i].mainTexture = wwwTexture1;
            //g.startHelps[i] = wwwTexture1;
        }
    }



    IEnumerator ModifySqlite() {
        // www SelUserInfo
        StartCoroutine(dbow.UptSqliteWww());
        yield return null;
    }

    //---------------------------------------------------------------
    //                          Scatter Letter
    //---------------------------------------------------------------
	IEnumerator scatterLetter01() {
        float speed1 = 1f;
		for (int i = 0; i < letterCnt; i++) {
			scatterLetterUILabels01[i].text = texNames[i];
			scatterLetterUILabels01[i].fontSize = Random.Range(10,80);
			scatterLetterUILabels01[i].enabled = true;
			
			scatterLetterTweenPoss01[i].from = VectorPosX1[i];
			scatterLetterTweenPoss01[i].to = new Vector3(VectorPosX1[i].x,-900f,1f);

			speed1 = Random.Range(1f,20f);
			scatterLetterTweenPoss01[i].duration = speed1;
			scatterLetterTweenPoss01[i].enabled = true;
            scatterLetterTweenPoss01[i].ResetToBeginning();
			
			yield return new WaitForSeconds(3f);
		}
		yield return new WaitForSeconds(speed1-3f); 

		StartCoroutine( "scatterLetter01" );
	}

	IEnumerator scatterLetter02() {
        float speed2 = 40f;
		for (int i = 0; i < letterCnt; i++) {
			scatterLetterUILabels02[i].text = texNames[i];
			scatterLetterUILabels02[i].fontSize = Random.Range(150,200);
			scatterLetterUILabels02[i].enabled = true;
			
			scatterLetterTweenPoss02[i].from = VectorPosX2[i];
			scatterLetterTweenPoss02[i].to = new Vector3(VectorPosX2[i].x,-900f,1f);
			
			speed2 = Random.Range(40f,60f);
			scatterLetterTweenPoss02[i].duration = speed2;
			scatterLetterTweenPoss02[i].enabled = true;
            scatterLetterTweenPoss02[i].ResetToBeginning();
			
			yield return new WaitForSeconds(10f);
		}
		yield return new WaitForSeconds(speed2-10f); 
		StartCoroutine( "scatterLetter02" );
	}

	IEnumerator RoutineLetter() {
		StartCoroutine( "scatterLetter01" );
		StartCoroutine( "scatterLetter02" );
		yield return null; 
		//yield return new WaitForSeconds(1f); 
		//StartCoroutine( RoutineLetter() );
	}
	
	void SetScatteLetter() {
		_data = dbo.SelTextureInfo(g.curStage,"");
		
        scatterLetterUILabels01 = new UILabel[_data.Rows.Count];
        scatterLetterUILabels02 = new UILabel[_data.Rows.Count];
        scatterLetterTweenPoss01 = new TweenPosition[_data.Rows.Count];
        scatterLetterTweenPoss02 = new TweenPosition[_data.Rows.Count];
		VectorPosX1 = new Vector3[_data.Rows.Count];
		VectorPosX2 = new Vector3[_data.Rows.Count];

		texNames = new string[_data.Rows.Count];
		letterCnt = _data.Rows.Count;
		
        //letterCnt = 5;
		
		for (int j = 0; j < _data.Rows.Count; j++) {
		//for (int j = 0; j < 5; j++)
			dr = _data.Rows[j];
			texNames[j] = dr["texShowName"].ToString();

			GameObject scatterLetterPrefab01 = Instantiate(ScatterLetterPrefab) as GameObject;
			scatterLetterPrefab01.transform.parent = ScatterLetter.transform;

			float posX1 = Random.Range(-360f,360f);
			VectorPosX1[j] = new Vector3( posX1, 800f, 1f );
			scatterLetterPrefab01.transform.localPosition = VectorPosX1[j];			
			scatterLetterPrefab01.transform.localScale = new Vector3(1,1,1);

			GameObject scatterLetterPrefab02 = Instantiate(ScatterLetterPrefab) as GameObject;
			scatterLetterPrefab02.transform.parent = ScatterLetter.transform;

			float posX2 = Random.Range(-360f,360f);
			VectorPosX2[j] = new Vector3( posX2, 800f, 1f );
			scatterLetterPrefab02.transform.localPosition = VectorPosX2[j];			
			scatterLetterPrefab02.transform.localScale = new Vector3(1,1,1);

            scatterLetterUILabels01[j] = scatterLetterPrefab01.GetComponent<UILabel>();
            scatterLetterUILabels02[j] = scatterLetterPrefab02.GetComponent<UILabel>();

            scatterLetterTweenPoss01[j] = scatterLetterPrefab01.GetComponent<TweenPosition>();
            scatterLetterTweenPoss02[j] = scatterLetterPrefab02.GetComponent<TweenPosition>();
		}
	}

	IEnumerator HomeBackgroundColor(){
		colorChangeTime = 0.03f;
		offset = 0.001f;
		//hsbCol = new HSBColor(hue,1.0f,1.0f);
		hsbCol = new HSBColor(hue,1.0f,1.0f);
		rgbCol = hsbCol.ToColor();
		homeBgUISprite.color = rgbCol;
		fbUserBgUISprite.color = rgbCol;

		hue = hue + offset;
		if(hue > 1.0f) hue = 0.0f;
		
		yield return new WaitForSeconds(colorChangeTime);		
		
		StartCoroutine ( HomeBackgroundColor() );
	}	

    //---------------------------------------------------------------
	//                            Event
    //---------------------------------------------------------------
	public void EventPlayOnRelease() {
		Debug.Log("EventPlayOnRelease");
		ev.EventOnRelease();
		if (ev.isCurPrevDist) {
			startGameBoxCollider.enabled = false;
            buttonAudioSource.clip = startClip; buttonAudioSource.Play();
            bgAudioSource.Stop();

			StartCoroutine (navi.GoMapScene ());
		}
	}

	public void EventFbLoginOnRelease() {
		Debug.Log("EventFbLoginOnRelease");
		ev.EventOnRelease();
		if (ev.isCurPrevDist) {
            buttonAudioSource.clip = startClip; buttonAudioSource.Play();
            FbLogin.StartFacebookLogin();
		}
	}

	public void EventShareOnRelease() {
		Debug.Log("EventShareOnRelease");
		ev.EventOnRelease();
		if (ev.isCurPrevDist) {
            buttonAudioSource.clip = startClip; buttonAudioSource.Play();
            
            #if UNITY_ANDROID && !UNITY_EDITOR
                string contentsTitle = "이미지로 연상하고\n게임으로 중도되는\n국내최초 중독성\n단어암기게임!!!\n★★★★★★★\n★크레이지워드★\n★기초영어 시즌1★\n★★★★★★★\n";
                string contents = "http://crazyword.org";
                string popupTitle = "CrazyWord 공유하기!";
                if (curActivity == null) return;
                curActivity.Call("shareText", contentsTitle, contents, popupTitle);
            # endif
            
		}
	}

	public void EventSoundOnRelease() {
		Debug.Log("EventSoundOnRelease");
		ev.EventOnRelease();
		if (ev.isCurPrevDist) {
            buttonAudioSource.clip = startClip; buttonAudioSource.Play();
            if(g.isSound == true) {
                g.isSound = false;
                bgAudioSource.volume = 0f;
                buttonAudioSource.volume = 0f;
                soundUISprite.spriteName = "soundno";
            } else {
                g.isSound = true;
                bgAudioSource.enabled = true;
                bgAudioSource.volume = 0.750f;
                buttonAudioSource.volume = 0.400f;
                soundUISprite.spriteName = "soundnyes";
            }
		}
	}

    //---------------------------------------------------------------
    //                            Popup
    //---------------------------------------------------------------

    //-------------------------------------------
    //           GoogleLoginWarning Popup
    //-------------------------------------------
    bool isGoogleLoginWarning = false;
    public void EventGoogleLoginWarning() {
        Debug.Log("EventGoogleLoginWarning");
        isGoogleLoginWarning = true;

        buttonAudioSource.clip = startClip; buttonAudioSource.Play();
        GoogleLoginWarning.SetActive(true);
        PopupBg8.SetActive(true);

        googleLoginWarningAnimator.speed = 2;
        googleLoginWarningAnimator.SetBool("isSmallLargeScale2", true);
    }

    public void EventGoogleLoginWarningOK() {
        Debug.Log("EventGoogleLoginWarningOK");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) EventGoogleLoginWarningClose();
    }

    public void EventGoogleLoginWarningClose() {
        Debug.Log("EventGoogleLoginWarningClose");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            buttonAudioSource.clip = startClip; buttonAudioSource.Play();
            Application.Quit();
            //GoogleLoginWarning.SetActive(false);
            //PopupBg8.SetActive(false);
            //googleLoginWarningAnimator.SetBool("isSmallLargeScale2", false);
            isGoogleLoginWarning = false;
        }
    }


    //-------------------------------------------
    //           InternetWarning Exit Popup
    //-------------------------------------------
    bool isInternetWarningExit = false;
    public void EventInternetWarningExit() {
        Debug.Log("EventInternetWarningExit");
        isInternetWarningExit = true;

        buttonAudioSource.clip = startClip; buttonAudioSource.Play();
        InternetWarningExit.SetActive(true);
        PopupBg8.SetActive(true);

        internetWarningExitAnimator.speed = 2;
        internetWarningExitAnimator.SetBool("isSmallLargeScale2", true);
    }

    public void EventInternetWarningExitOK() {
        Debug.Log("EventInternetWarningExitOK");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) EventInternetWarningExitClose();
    }

    public void EventInternetWarningExitClose() {
        Debug.Log("EventInternetWarningCloseExit");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            buttonAudioSource.clip = startClip; buttonAudioSource.Play();
            Application.Quit();
        }
    }

    //-------------------------------------------
    //           InternetWarning Popup
    //-------------------------------------------
    bool isInternetWarning = false; 
    public void EventInternetWarning() {
        Debug.Log("EventInternetWarning");
        buttonAudioSource.clip = startClip; buttonAudioSource.Play();
        InternetWarning.SetActive(true);
        PopupBg8.SetActive(true);
        isInternetWarning = true;

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
            buttonAudioSource.clip = startClip; buttonAudioSource.Play();

            InternetWarning.SetActive(false);
            PopupBg8.SetActive(false);
            internetWarningAnimator.SetBool("isSmallLargeScale2", false);
            isInternetWarning = false;
        }
    }

    //-------------------------------------------
    //           PatchWarning Exit Popup
    //-------------------------------------------
    bool isPatchWarning = false;
    public void EventPatchWarning() {
        Debug.Log("EventpatchWarning");
        isPatchWarning = true;

        buttonAudioSource.clip = startClip; buttonAudioSource.Play();
        PatchWarning.SetActive(true);
        PopupBg8.SetActive(true);

        patchWarningAnimator.speed = 2;
        patchWarningAnimator.SetBool("isSmallLargeScale2", true);
    }

    public void EventPatchWarningOK() {
        Debug.Log("EventPatchWarningOK");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            StartCoroutine("DelayQuit");
        }            
    }

    IEnumerator DelayQuit() {
        yield return new WaitForSeconds(1f);
        Application.OpenURL("market://details?id=com.nextpiasoft.crazyword.english.basic1");
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }

    //-------------------------------------------
    //        UpdateUserInfoWarning Popup
    //-------------------------------------------
    public void EventUpdateUserInfoWarning() {
        Debug.Log("EventUpdateUserInfoWarning");

        buttonAudioSource.clip = startClip; buttonAudioSource.Play();
        UpdateUserInfoWarning.SetActive(true);
        PopupBg8.SetActive(true);
        isUpdateUserInfoShow = true;
        
        updateUserInfoWarningAnimator.speed = 2;
        updateUserInfoWarningAnimator.SetBool("isSmallLargeScale2", true);
    }

    public void EventUpdateUserInfoWarningOK() {
        Debug.Log("EventUpdateUserInfoWarningOK");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            UpdateUserInfoWarning.SetActive(false);
            updateUserInfoWarningAnimator.SetBool("isSmallLargeScale2", false);
            dbo.CreateUser();
            isUpdateUserInfoShow = false;
        }
    }

    public void EventUpdateUserInfoWarningClose() {
        Debug.Log("EventUpdateUserInfoWarningClose");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            buttonAudioSource.clip = startClip; buttonAudioSource.Play();
            Application.Quit();
            isUpdateUserInfoShow = false;
        }
    }

    //-------------------------------------------
    //           QuitWarning Popup
    //-------------------------------------------
    public void EventQuitWarning() {
        Debug.Log("EventQuitWarning");
        buttonAudioSource.clip = startClip; buttonAudioSource.Play();
        QuitWarningPopup.SetActive(true);
        PopupBg8.SetActive(true);
        quitWarningPopupAnimator.speed = 2;
        quitWarningPopupAnimator.SetBool("isSmallLargeScale2", true);
    }
    public void EventQuitWarningOK() {
        Debug.Log("EventQuitWarningOK");
        buttonAudioSource.clip = startClip; buttonAudioSource.Play();
        Application.Quit();
    }
    public void EventQuitWarningClose() {
        Debug.Log("EventQuitWarningClose");
        buttonAudioSource.clip = startClip; buttonAudioSource.Play();
        QuitWarningPopup.SetActive(false);
        PopupBg8.SetActive(false);
        quitWarningPopupAnimator.SetBool("isSmallLargeScale2", false);
    }

    //-------------------------------------------
    //          UserFirstEnterPopup
    //-------------------------------------------
    public void EventUserFirstEnterPopup() {
        Debug.Log("EventUserFirstEnterPopup");
        buttonAudioSource.clip = startClip; buttonAudioSource.Play();
        isCrazyWordPopup = true;

        StartCoroutine ( dbow.SelUserInfoNickName() );

    }
    public void EventUserFirstEnterPopupClose() {
        Debug.Log("EventUserFirstEnterPopupClose");
        buttonAudioSource.clip = startClip; buttonAudioSource.Play();

        if(NickName.activeSelf) {
            if (nickNameUILabel.text.Length < 5) {
                nickNameWarningLabel.text = "닉네임은 5글자 이상입니다.";
                EventNickNameWarning();
            } else {
                g.nickName = nickNameUILabel.text;
                StartCoroutine ( dbow.UptUserInfoNickName() );

                isQuitWarning = true;
                Loading.SetActive(false);
                GameStart.SetActive(true);
            }
        } else {
            isQuitWarning = true;
            Loading.SetActive(false);
            GameStart.SetActive(true);
            FirstEnterPopupClose();
        }
    }

    public void FirstEnterPopupClose() {
        dbo.UptCrazyWordPopupShowN("2015010101");

        UserFirstEnterPopup.SetActive(false);
        PopupBg8.SetActive(false);
        userFirstEnterAnimator.SetBool("isSmallLargeScale2", false);

        if (informPopupCnt > 0)
        {
            informPopupIdx = 0;
            //StartCoroutine ( EventInformPopup() );
            EventInformPopup();
        }
        else
        {
            isCrazyWordPopup = false;
            //startGameBoxCollider.enabled = true;
            //facebookLoginBoxCollider.enabled = true;
            //shareBoxCollider.enabled = true;
            isQuitWarning = true;
        }
        #if UNITY_ANDROID && !UNITY_EDITOR
            if(isTTSOption) {
                TTSControl.instance.OnDestroy();
                TTSControl.instance.Init();
                isTTSOption = false;
            }
        # endif
    }

    //-------------------------------------------
    //           NickNameWarning Popup
    //-------------------------------------------
    public void EventNickNameWarning()
    {
        Debug.Log("EventNickNameWarning");
        buttonAudioSource.clip = startClip; buttonAudioSource.Play();
        NickNameWarningPopup.SetActive(true);
        PopupBg12.SetActive(true);
        nickNameWarningPopupAnimator.speed = 2;
        nickNameWarningPopupAnimator.SetBool("isSmallLargeScale2", true);
    }
    public void EventNickNameWarningOK()
    {
        Debug.Log("EventNickNameWarningOK");
        buttonAudioSource.clip = startClip; buttonAudioSource.Play();
        EventNickNameWarningClose();
    }
    public void EventNickNameWarningClose()
    {
        Debug.Log("EventNickNameWarningClose");
        buttonAudioSource.clip = startClip; buttonAudioSource.Play();
        NickNameWarningPopup.SetActive(false);
        PopupBg12.SetActive(false);
        nickNameWarningPopupAnimator.SetBool("isSmallLargeScale2", false);
    }    
  

    public UIScrollBar firstEnterUIScrollBar;
    public UISprite[] firstEnterScrollUISprites = new UISprite[5];
    public void EventUserFirstEnterScrollOnChange() {
        if(firstEnterUIScrollBar.value == 0.00f && firstEnterUIScrollBar.value <= 0.10f) {
            for(int i=0; i<firstEnterScrollUISprites.Length;i++) firstEnterScrollUISprites[i].color = Color.white;
            firstEnterScrollUISprites[0].color = Color.gray;
        } else if(firstEnterUIScrollBar.value > 0.20f && firstEnterUIScrollBar.value <= 0.30f) {
            for(int i=0; i<firstEnterScrollUISprites.Length;i++) firstEnterScrollUISprites[i].color = Color.white;
            firstEnterScrollUISprites[1].color = Color.gray;
        } else if(firstEnterUIScrollBar.value > 0.45f && firstEnterUIScrollBar.value <= 0.55f) {
            for(int i=0; i<firstEnterScrollUISprites.Length;i++) firstEnterScrollUISprites[i].color = Color.white;
            firstEnterScrollUISprites[2].color = Color.gray;
        } else if(firstEnterUIScrollBar.value > 0.70f && firstEnterUIScrollBar.value <= 0.80f) {
            for(int i=0; i<firstEnterScrollUISprites.Length;i++) firstEnterScrollUISprites[i].color = Color.white;
            firstEnterScrollUISprites[3].color = Color.gray;
        } else if(firstEnterUIScrollBar.value > 0.95f && firstEnterUIScrollBar.value <= 1.00f) {
            for(int i=0; i<firstEnterScrollUISprites.Length;i++) firstEnterScrollUISprites[i].color = Color.white;
            firstEnterScrollUISprites[4].color = Color.gray;
        }
    }   

    bool isTTSOption = false;
    public void EventTTSOptionOnRelease() {
        Debug.Log("EventTTSOptionOnRelease");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            buttonAudioSource.clip = startClip; buttonAudioSource.Play();
            #if UNITY_ANDROID && !UNITY_EDITOR
                isTTSOption = true;
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject curActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
                curActivity.Call("SetupTTS");
            # endif
        }
    }

    public void EventShowInstructionPopup() {
        Debug.Log("EventShowInstructionPopup");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            buttonAudioSource.clip = startClip; buttonAudioSource.Play();
            Application.OpenURL ("http://crazyword.org/help.html");
        }
    }



    //-------------------------------------------
    //           InformPopup
    //-------------------------------------------
    void EventInformPopup() {
        Debug.Log("EventInformPopup");
        buttonAudioSource.clip = startClip; buttonAudioSource.Play();
        
        InformPopup.SetActive(true);
        PopupBg8.SetActive(true);
        isCrazyWordPopup = true;
        informChkUISprite.spriteName = "bgediteoption1";

        for(int i=0;i<informUITextures.Length;i++) informUITextures[i].enabled = false;
        informUITextures[informPopupIdx].enabled = true;

        informPopupAnimator.speed = 2;
        informPopupAnimator.SetBool("isSmallLargeScale2", true);

    }
    public void EventInformPopupClose() {
        Debug.Log("EventInformPopupClose");
        buttonAudioSource.clip = startClip; buttonAudioSource.Play();

        if (informChkUISprite.spriteName == "bgediteoption3")
            dbo.UptCrazyWordPopupShowN(popupIds[informPopupIdx]);

        InformPopup.SetActive(false);
        PopupBg8.SetActive(false);
        informPopupAnimator.SetBool("isSmallLargeScale2", false);

        if(informPopupCnt > informPopupIdx+1) {
            //Debug.Log(informPopupCnt + ":" + (informPopupIdx+1));
            informPopupIdx++;
            EventInformPopup();
        } else {
            isCrazyWordPopup = false;
            //startGameBoxCollider.enabled = true;
            //facebookLoginBoxCollider.enabled = true;
            //shareBoxCollider.enabled = true;
            isQuitWarning = true;
            Loading.SetActive(false);
            GameStart.SetActive(true);
        }
    }    

    public void EventInformPopupChkBox() {
        Debug.Log("EventInformPopupChkBox");
        buttonAudioSource.clip = startClip; buttonAudioSource.Play();
        if (informChkUISprite.spriteName == "bgediteoption1") {
            informChkUISprite.spriteName = "bgediteoption3";
        } else informChkUISprite.spriteName = "bgediteoption1";
    }    

    //-------------------------------------------
    //  Update
    //-------------------------------------------
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if( isQuitWarning && !isCrazyWordPopup && !isPatchWarning) EventQuitWarning();
        }
        //if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }
}