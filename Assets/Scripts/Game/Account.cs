using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using SimpleJSON;

public class Account : MonoBehaviour {

	public GameControl gameControl;
	public GameTexture gameTexture;
	public FacebookManager facebookManager;


	int totalLetterCnt, avgLetterCnt, turnCnt, avgWordLength, rankCnt;
	int stageScore,bestCombo,bestCombo2;

	public GameState gameState; public GameComponent gameComponent;
	public DBO dbo; public DBOW dbow; public Navi navi; public EventCommon ev;
	DataTable _data; DataRow dr;

	public void ShowPopUpAccount(){
        //StartCoroutine ( ImageSoure() );
        ImageSoure();
		//-------------------- 
		// BoxCollider
		//--------------------
		gameComponent.EnableGameSceneCommonBoxCollider(false);
        gameState.AccountBestScoreUp.SetActive(false);
        gameState.AccountRankingUp.SetActive(false);
        gameComponent.accountBestScoreUpAnimator.SetBool("isBestScoreUp", false);
        gameComponent.accountRankUpAnimator.SetBool("isBestScoreUp", false);

		//-------------------- 
		// Show Popup
		//--------------------
		gameState.StatePopUpAccount();

		//-------------------- 
		// Set Score
		//--------------------
        SetStageScore();

		//-------------------- 
		// Stage Ranking
		//--------------------
		SetStageRank();

        //----------------------
        // Achievement
        //----------------------
        if(g.isInternetReachability) StartCoroutine(SetAchievement());

        //StartCoroutine(dbow.SelUserGiftHis());

        //----------------------
        // UserStageHis
        //----------------------


		//-------------------- 
		// state
		//--------------------
		if(g.curStage <= g.maxStage) g.curStage++;
        g.curStageScore = 0;

		//-------------------- 
		// Image Source
		//--------------------

	}


    //----------------------------------------------------------
    //                           Achievement
    //----------------------------------------------------------
    public IEnumerator SetAchievement(){
        //Debug.Log("g.curStageRank="+g.curStageRank);
        //Debug.Log("g.comboCnt="+g.comboCnt);

        g.partNum = 0; g.comboTotalCnt = 0;
        if(g.curStageRank == 5 || g.comboCnt == 5) {
            yield return StartCoroutine ( dbow.InsSelUserStageInfo(g.curStageRank, g.comboCnt) );
        }

        // this stage Rank = 5 and server achievement partNum(1-10)
        if(g.curStageRank == 5 && g.partNum > 0) {
            if(g.achievement.Substring(g.partNum-1,1) == "0") {
                string[] achieveKeys = new string[10] {
                     "CgkI3MK4oPYcEAIQAA","CgkI3MK4oPYcEAIQAQ","CgkI3MK4oPYcEAIQAg","CgkI3MK4oPYcEAIQAw","CgkI3MK4oPYcEAIQBA"
                    ,"CgkI3MK4oPYcEAIQBQ","CgkI3MK4oPYcEAIQBg","CgkI3MK4oPYcEAIQBw","CgkI3MK4oPYcEAIQCA","CgkI3MK4oPYcEAIQCQ"
	            };

                #if UNITY_ANDROID && !UNITY_EDITOR
                    Social.ReportProgress(achieveKeys[g.partNum-1], 100.0f, (bool success) => { });
                # endif

                dbo.InsUserGiftHis(10, g.partNum, "");
            }
        }
        
        if(g.comboCnt == 5) {

            if(g.curStageRank == 5 && g.partNum > 0) yield return new WaitForSeconds(3f);

            //Debug.Log("g.comboTotalCnt="+g.comboTotalCnt);
            //Debug.Log("g.comboTotalCnt % g.totalStar="+g.comboTotalCnt % g.totalStar);
            //Debug.Log("g.totalStar="+g.totalStar);

            //----------------------
            // Achievement Combo
            //----------------------
            if       (g.comboTotalCnt == 200)  { SetAchieve(11,"CgkI3MK4oPYcEAIQCg"); 
            } else if(g.comboTotalCnt == 400)  { SetAchieve(12,"CgkI3MK4oPYcEAIQCw"); 
            } else if(g.comboTotalCnt == 600)  { SetAchieve(13,"CgkI3MK4oPYcEAIQDA"); 
            } else if(g.comboTotalCnt == 800)  { SetAchieve(14,"CgkI3MK4oPYcEAIQDQ"); 
            } else if(g.comboTotalCnt == 1000) { SetAchieve(15,"CgkI3MK4oPYcEAIQDg"); 
            }
        }
    
    }


    //----------------------------------------------------------
    //                           PhotoInfo
    //----------------------------------------------------------

	public GameObject ImgSrcPrefab,ImgSrcGrid;

    UITexture[] imgSrcUITextures = new UITexture[20];
    string[] imgSrcFileNames = new string[20];
    int imgSrcStage;

	void ImageSoure(){

        GameObject ImgSrcClone;
        UIEventTrigger imgSrcUIEventTrigger;

        UISprite photoLicenseSprite;
        UILabel photoOwnerLabel;
		int stageNum,turnTexNum,texNum;
		string texName;
		string texPath,texFileName;

        _data = dbo.SelPhotoInfo3(g.curStage);

        for(int i=0;i < _data.Rows.Count; i++) {
            dr = _data.Rows[i]; 
            //int totalRankCnt = int.Parse(dr["totalRankCnt"].ToString());

            ImgSrcClone = Instantiate(ImgSrcPrefab) as GameObject;
            ImgSrcClone.transform.parent = ImgSrcGrid.transform;
            ImgSrcClone.transform.localPosition = new Vector3(0f, -i * 45, 0f);
            ImgSrcClone.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            ImgSrcClone.transform.localScale = Vector3.one;
            ImgSrcClone.transform.name = "ImgSrc"+(i+1).ToString("00");

			imgSrcUIEventTrigger = ImgSrcClone.transform.FindChild("2_ImgTexture").GetComponent<UIEventTrigger>();
   			EventDelegate.Add (imgSrcUIEventTrigger.onPress,ev.EventOnPress);

			EventDelegate eventClick = new EventDelegate(this, "EventImgSrcPopup");
			EventDelegate.Parameter param = new EventDelegate.Parameter();
			param.obj = ImgSrcClone;
			param.expectedType = typeof(GameObject);
			eventClick.parameters[0] = param;
   			EventDelegate.Add (imgSrcUIEventTrigger.onRelease,eventClick);

            //-------------------
            // file
            //-------------------
            imgSrcUITextures[i] = ImgSrcClone.transform.FindChild("2_ImgTexture").GetComponent<UITexture>();

            stageNum = int.Parse( dr["stageNum"].ToString() );
            imgSrcStage = stageNum;

            turnTexNum = int.Parse( dr["turnTexNum"].ToString() );
            texName = dr["texName"].ToString();
            texNum = int.Parse( dr["texNum"].ToString() );

            //Debug.Log("stageNum="+stageNum+" texName=" + texName+ " texNum="+texNum);
            texPath  = Application.persistentDataPath + "/Textures/v"+g.volumn.ToString("000");
            texPath += "/s"+stageNum.ToString("000")+"/";
            texFileName  = texName + "_"+texNum.ToString("00")+".jpg";
            imgSrcFileNames[i] = texPath + texFileName;


			photoLicenseSprite = ImgSrcClone.transform.FindChild("4_CCSprite").GetComponent<UISprite>();
			photoOwnerLabel = ImgSrcClone.transform.FindChild("4_Owner").GetComponent<UILabel>();

            string photoLisence = "";
            //_data = dbo.SelPhotoInfo(g.stageTextures[g.stageTurn-1][i].texNum); dr = _data.Rows[0];
            if(dr["photoLicense"].ToString() == "4") {
                photoLisence = "CCBY";
                photoLicenseSprite.width = 60;
                //photoLicenseSprite.transform.localPosition = new Vector3(-110,-120,0);
                //photoOwnerLabel.transform.localPosition = new Vector3(124,-2,0);

            } else if(dr["photoLicense"].ToString() == "5") {
                photoLisence = "CCBYSA";        
                photoLicenseSprite.width = 90;
                //photoLicenseSprite.transform.localPosition = new Vector3(-100,-120,0);
                //photoOwnerLabel.transform.localPosition = new Vector3(134,-2,0);
            } else if(dr["photoLicense"].ToString() == "6") {
                photoLisence = "CCBYND";        
                photoLicenseSprite.width = 90;
                //photoLicenseSprite.transform.localPosition = new Vector3(-100,-120,0);
                //photoOwnerLabel.transform.localPosition = new Vector3(134,-2,0);
            }
            else if(dr["photoLicense"].ToString() == "pd") {
                photoLisence = "PD";
                photoLicenseSprite.width = 30;
                //photoLicenseSprite.transform.localPosition = new Vector3(-120,-120,0);
                //photoOwnerLabel.transform.localPosition = new Vector3(114,-2,0);
            }
             
            photoLicenseSprite.spriteName = photoLisence;
            photoOwnerLabel.text = dr["photoOwner"].ToString();
        }

        StartCoroutine ( imgSrcTexture() );
    }

    IEnumerator imgSrcTexture(){
        for(int i=0;i<20;i++) {
            WWW www1; Texture2D wwwTexture1; www1 = new WWW("file:///" + imgSrcFileNames[i]); 
            yield return www1;
            wwwTexture1 = new Texture2D(40, 40, TextureFormat.ARGB32, false);
            www1.LoadImageIntoTexture(wwwTexture1);
            imgSrcUITextures[i].mainTexture = wwwTexture1;
        }
    }
	void EventImgSrcPopup(GameObject gameo){
        Debug.Log("EventImgSrcPopup");
        int photoInfoNum = int.Parse ( gameo.name.Substring(6,2) );
        //Debug.Log("showNum="+photoInfoNum);
        StartCoroutine ( dbow.SelPhotoInfo(imgSrcStage,photoInfoNum) );
    }    




    //----------------------------------------------------------
    //                           Event
    //----------------------------------------------------------

	public void EventMapNaviOnRelease(){
		Debug.Log("EventMapNaviOnRelease");
		ev.EventOnRelease();
		if (ev.isCurPrevDist) {
			gameComponent.accountMapNaviBoxCollider.enabled = false;
			dbo.UptUserInfo();
			StartCoroutine (navi.GoMapScene ());
		}
	}
	
	public void EventReturnNaviOnRelease(){
		Debug.Log("EventReturnNaviOnRelease");
		ev.EventOnRelease();
		if (ev.isCurPrevDist) {
			gameComponent.accountReturnNaviBoxCollider.enabled = false;
			dbo.DelUserStageTurnHis();
			g.curStage--;
			StartCoroutine (navi.GoGameScene());
		}
	}

    //----------------------------------------------------------
    //                           Score
    //----------------------------------------------------------
	public int GetGameKind(){
		int gameKind = 0;
		if      (g.isGameMemory&&g.isCombo)  gameKind = 1;
		else if (g.isGameMemory&&!g.isCombo) gameKind = 2;
		else if (g.isGameShooting)           gameKind = 11;
		else if (g.isGameRain)               gameKind = 12;
		else if (g.isGameMole)               gameKind = 13;
		else if (g.isGameBlank)              gameKind = 14;
		else if (g.isGameHint)               gameKind = 40;
		return gameKind;
	}

	public void ModifyScore(){
        Debug.Log("ModifyScore");
		int gameKind;
		gameKind = GetGameKind();
		//----------------
		// g.gameScore
		//----------------
		g.gameScore = (int)(int.Parse(gameComponent.limitUILabel.text) * GetGameScoreWeighted());
        
		//----------------
		// stageScore
		//----------------
		g.curStageScore += g.gameScore;
		gameComponent.scoreNumUILabel.text = g.curStageScore.ToString();
		gameComponent.scoreNumLineUILabel.text = g.curStageScore.ToString();

		dbo.InsUserStageTurnHis(GetGameKind(),g.gameScore);
	}

    int comboBonus = 0;
    string ComboBonusPoint(){
        if(g.comboCnt == 5) {

            int noComboLargeCnt = dbo.SelUserCorrectHisComboLargeCnt();
            float largeWeight = 0.10f;

            if(noComboLargeCnt >= 7 && noComboLargeCnt <= 9) largeWeight = 0.10f;
            else if(noComboLargeCnt == 6) largeWeight = 0.07f;
            else if(noComboLargeCnt == 5) largeWeight = 0.06f;
            else if(noComboLargeCnt == 4) largeWeight = 0.05f;
            else if(noComboLargeCnt == 3) largeWeight = 0.03f;
            else if(noComboLargeCnt == 2) largeWeight = 0.02f;
            else if(noComboLargeCnt == 1) largeWeight = 0.01f;
            else if(noComboLargeCnt == 0) largeWeight = 0.00f;

            //Debug.Log("noComboLargeCnt="+noComboLargeCnt+" largeWeight="+largeWeight+" comboBonus="+comboBonus);

            comboBonus = (int) (g.curStageVitualMaxScore * largeWeight);

            //Debug.Log("noComboLargeCnt="+noComboLargeCnt+" largeWeight="+largeWeight+" g.curStageVitualMaxScore="+g.curStageVitualMaxScore+" comboBonus="+comboBonus);

        } else {
            comboBonus = 0;
        }

        return comboBonus.ToString();
    }
    void SetStageScore(){
    
        //Debug.Log("g.comboCnt="+g.comboCnt);
		gameComponent.accountLessonNum.text = g.curStage.ToString();
		gameComponent.accountScoreNum.text = g.curStageScore.ToString();
		
        gameComponent.accountComboBonus.text = ComboBonusPoint();

        gameComponent.accountBonusNum.text = (g.bonusCnt * g.bonusScore).ToString();

		gameComponent.accountTotalScoreNum.text = (g.curStageScore + comboBonus + (g.bonusCnt * g.bonusScore)).ToString();
		g.curStageScore = int.Parse(gameComponent.accountTotalScoreNum.text);

		//------------------
		// Stage Score
		//------------------
        if (g.curStageScore > g.curStageMaxScore) {
            g.curStageMaxScore = g.curStageScore;
 
            gameComponent.accountBestScoreNum.text = g.curStageMaxScore.ToString();
            gameComponent.bestScoreUILabel.text = g.curStageMaxScore.ToString();
            gameComponent.bestScoreLineUILabel.text = g.curStageMaxScore.ToString();

			gameState.AccountBestScoreUp.SetActive(true);
			gameComponent.accountBestScoreUpAnimator.SetBool("isBestScoreUp",true);

            g.itemHp++; dbo.UptHp("item");
		}
        dbo.UptUserStageHis();

		//------------------
		// Total Score
		//------------------
        int totalScore = dbo.SelUserStageHisTotalScore();
        //facebookManager.FacebookPostMyScore( totalMaxScore.ToString() );
        if(totalScore > g.bestScore) {
            g.bestScore = totalScore;
            dbo.UptUserInfoBestScore();
            dbo.UptFbFriendInfoScore(g.fbId,g.bestScore);
        }
    }


	public float GetGameScoreWeighted(){
		float weightValue = 0;
		if      (g.isGameMemory&&g.isCombo)  weightValue = 2.00f;
		else if (g.isGameMemory&&!g.isCombo) weightValue = 1.70f;
		else if (g.isGameShooting)           weightValue = 0.50f;
		else if (g.isGameRain)               weightValue = 0.50f;
		else if (g.isGameMole)               weightValue = 0.50f;
		else if (g.isGameBlank)              weightValue = 0.50f;
		else if (g.isGameHint)               weightValue = 0.50f;
        //Debug.Log("GetGameScoreWeighted="+weightValue);
		return weightValue;
	}

	public float GetGameLimitAddTime(){

        float gameLimitAddTime = 1.0f;
        _data = dbo.SelUserStageHisRankCnt(); dr = _data.Rows[0];
        int totalRankCnt = int.Parse(dr["totalRankCnt"].ToString());
        int clearStageCnt = int.Parse(dr["clearStageCnt"].ToString());

        if(totalRankCnt < 50)        gameLimitAddTime = 2.00f;
        else if(totalRankCnt < 100)  gameLimitAddTime = 1.80f;
        else if(totalRankCnt < 200)  gameLimitAddTime = 1.60f;
        else if(totalRankCnt < 300)  gameLimitAddTime = 1.40f;
        else if(totalRankCnt < 400)  gameLimitAddTime = 1.30f;
        else if(totalRankCnt < 500)  gameLimitAddTime = 1.20f;
        else if(totalRankCnt < 600)  gameLimitAddTime = 1.10f;
        else if(totalRankCnt < 700)  gameLimitAddTime = 1.00f;
        else if(totalRankCnt < 800)  gameLimitAddTime = 0.95f;
        else if(totalRankCnt < 900)  gameLimitAddTime = 0.90f;
        else if(totalRankCnt < 1000) gameLimitAddTime = 0.80f;
        else gameLimitAddTime = 0.75f;

        //Debug.Log("GetGameLimitAddTime="+gameLimitAddTime);

        return gameLimitAddTime;
    }

    public void SetCurStageVitualMaxScore(){
        _data = dbo.SelTotalLetterCnt(); dr = _data.Rows[0];
        totalLetterCnt = int.Parse(dr["totalLetterCnt"].ToString());

        g.curStageVitualMaxScore = 0;
        g.curStageVitualMaxScore = (int)(totalLetterCnt * g.limitBaseTime * 2 * GetGameLimitAddTime());
    }

    public void SetStageRank()
    {
        //Debug.Log(g.curStageScore + ":" + g.curStageVitualMaxScore);
        if      (g.curStageScore < g.curStageVitualMaxScore * 0.20f) g.curStageRank = 1;
        else if (g.curStageScore < g.curStageVitualMaxScore * 0.40f) g.curStageRank = 2;
        else if (g.curStageScore < g.curStageVitualMaxScore * 0.60f) g.curStageRank = 3;
        else if (g.curStageScore < g.curStageVitualMaxScore * 0.80f) g.curStageRank = 4;
        else g.curStageRank = 5;
        if(g.curStageRank == 5 && g.comboCnt < 5) g.curStageRank = 4;

        //----------------
        // Set RankStar
        //----------------
        for (int i = 0; i < gameComponent.accountRankStar.Length; i++) gameComponent.accountRankStar[i].spriteName = "resultfacebooknonlock";
        for (int i = 0; i < g.curStageRank; i++) gameComponent.accountRankStar[i].spriteName = "resultfacestar";

        //Debug.Log("g.curStageVitualMaxScore="+g.curStageVitualMaxScore);
        //Debug.Log("g.curStageScore="+g.curStageScore);
        //Debug.Log("g.curStageRank="+g.curStageRank);

        _data = dbo.SelUserStageHis(g.curStage); dr = _data.Rows[0];
        g.curStageMaxRank = int.Parse(dr["stageMaxRank"].ToString());

        //Debug.Log("g.curStageRank=" + g.curStageRank);
        //Debug.Log("g.curStageMaxRank=" + g.curStageMaxRank);

        if (g.curStageRank > g.curStageMaxRank)
        {
            gameState.AccountRankingUp.SetActive(true);
            gameComponent.accountRankUpAnimator.SetBool("isBestScoreUp", true);

            g.itemHp++; dbo.UptHp("item");
            g.curStageMaxRank = g.curStageRank;
        }
        dbo.UptUserStageHis();
    }
   

    void SetAchieve(int partNum, string achieveKey){

        //Debug.Log("SetAchieve");

        if(g.achievement.Substring(partNum-1,1) == "0") {

            #if UNITY_ANDROID && !UNITY_EDITOR
                Social.ReportProgress(achieveKey, 100.0f, (bool success) => { });
            # endif

            dbo.InsUserGiftHis(10, partNum, "");
        } 
    }



	public void SubtractionSkip() {
		g.stageSkip--;
        if (g.curStage > 0) {
            if (g.freeSkip > 0) { g.freeSkip--; dbo.UptSkip("free"); }
            else if (g.buySkip > 0)  { g.buySkip--; dbo.UptSkip("buy"); }
            else if (g.giftSkip > 0) { g.giftSkip--; dbo.UptSkip("gift"); }
            else if (g.itemSkip > 0) { g.itemSkip--; dbo.UptSkip("item"); }
            else Debug.Log("buy skip popup");
        }
	}
	
	public void SubtractionHp() {
		//Debug.Log("SubtractionHp");
		//Debug.Log("g.freeHp="+g.freeHp);
        if (g.curStage > 0) {
            if (g.freeHp > 0) { g.freeHp--; dbo.UptHp("free"); }
            else if (g.buyHp > 0) { g.buyHp--; dbo.UptHp("buy"); }
            else if (g.giftHp > 0) { g.giftHp--; dbo.UptHp("gift"); }
            else if (g.itemHp > 0) { g.itemHp--; dbo.UptHp("item"); }
            else Debug.Log("buy skip popup");
        }		
	}

	public void HpInfo(){
		gameComponent.hpNumUILabel.text = g.allHp().ToString();
		gameComponent.hpNumLineUILabel.text = g.allHp().ToString();
		if(gameComponent.hpNumUILabel.text.Length == 1) {
			gameComponent.hpTexUISprite.transform.localPosition = new Vector3(40,0,0);
			gameComponent.hpNumUILabel.transform.localPosition = new Vector3(-80,-16,0);
			gameComponent.hpNumLineUILabel.transform.localPosition = new Vector3(-80,-16,0);
		} else if(gameComponent.hpNumUILabel.text.Length == 2) {
			gameComponent.hpTexUISprite.transform.localPosition = new Vector3(40,0,0);
			gameComponent.hpNumUILabel.transform.localPosition = new Vector3(-70,-16,0);
			gameComponent.hpNumLineUILabel.transform.localPosition = new Vector3(-70,-16,0);
		} else if(gameComponent.hpNumUILabel.text.Length == 3) {
			gameComponent.hpTexUISprite.transform.localPosition = new Vector3(44,0,0);
			gameComponent.hpNumUILabel.transform.localPosition = new Vector3(-60,-16,0);
			gameComponent.hpNumLineUILabel.transform.localPosition = new Vector3(-60,-16,0);
		}
	}

	public void SkipInfo(){

        //imsi
		gameComponent.skipNumUILabel.text = g.stageSkip.ToString();
		gameComponent.totalSkipNumUILabel.text = g.allSkip().ToString();
	}
}
