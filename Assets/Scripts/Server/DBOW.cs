using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class DBOW : MonoBehaviour
{

    public DBO dbo;

    string query;
    DataTable _data;
    DataRow dr;

    public HomeControl homeControl;
    public MapControl mapControl;
    public PopupMapControl popupMapControl;
    public PopupShopControl popupShopControl;
    public PopupFriendControl popupFriendControl;

    //-----------------------------------------------------
    //                        User
    //-----------------------------------------------------
    public IEnumerator SelUserInfo()
    {
        g.isSelUserInfo = true;
        Debug.Log("www SelUserInfo");
        WWWForm form = new WWWForm();
        form.AddField("userId", g.userId);
        form.AddField("gameCode", g.gameCode);
        form.AddField("bestScore", g.bestScore);
        form.AddField("deviceId", g.deviceId);

        string popupId = dbo.SelCrazyWordPopupMaxId();

        form.AddField("popupId", popupId);

        WWW www = new WWW("http://crazyword.org/SelUptUserInfo.asp", form);

        yield return www;

        // Success
        if (www.isDone && www.error == null)
        {
            Debug.Log("www UserInfo=" + www.text);

            var UserInfo = JSON.Parse(www.text);
            int rowCnt = UserInfo.Count;

            //Debug.Log("UserInfoWww rowCnt=" + rowCnt);

            g.isInternetReachability = true;

            // Case1 : first start game
            // Case2 : login new google (all clear previous game data)
            if (rowCnt == 0) StartCoroutine( InsUserInfo() );
            // Case1 : same googleId but move device
            // Case2 : user delete game, and reinstall
            //         conconnect                       
            //else if (g.deviceId != UserInfo["deviceId"])
            else
            {
                dbo.UptUserInfoWww(
                  UserInfo[0]["userId"]
                , int.Parse(UserInfo[0]["bestScore"])
                , int.Parse(UserInfo[0]["coin"])
                , UserInfo[0]["partMove"]
                , UserInfo[0]["noBannerYn"]
                , UserInfo[0]["userPicYn"]
                , UserInfo[0]["fbShareDate"]
                , UserInfo[0]["friendYn"]
                , UserInfo[0]["giftYn"]
                , UserInfo[0]["achievement"]
                );

                if( g.bestScore < int.Parse(UserInfo[0]["bestScore"]) ) {
                    g.bestScore = int.Parse(UserInfo[0]["bestScore"]);
                }

                g.coin = int.Parse(UserInfo[0]["coin"]);
                g.noBannerYn = UserInfo[0]["noBannerYn"];
                g.userPicYn = UserInfo[0]["userPicYn"];
                g.achievement = UserInfo[0]["achievement"];

                //Debug.Log("partMove="+UserInfo[0]["partMove"]);

                if( g.partMove != UserInfo[0]["partMove"] ) {
                    g.partMove = UserInfo[0]["partMove"];

                    for(int i=1; i < g.partMove.Length; i++) {
                        if(g.partMove.Substring(i,1) == "1") {
                            _data = dbo.SelPartStageNum(i+1); dr = _data.Rows[0];
                            int minStageNum = int.Parse(dr["minStageNum"].ToString());
                            int maxStageNum = int.Parse(dr["maxStageNum"].ToString());
                            //Debug.Log("minStageNum=" + minStageNum);
                            //Debug.Log("maxStageNum=" + maxStageNum);
                            dbo.UptUserStageHisStageClear(minStageNum, maxStageNum);
                        }
                    }
                }

                //Debug.Log("UserInfo[0][fbId]="+UserInfo[0]["fbId"]);
                
                if( UserInfo[0]["fbId"] == null || UserInfo[0]["fbId"] == "") {
                    if(g.fbId != null && g.fbId.Length > 2) {
                        StartCoroutine( UptUserInfoFbId() );
                    }
                }

                string giftYn = UserInfo[0]["giftYn"];

                Debug.Log("giftYn=" + giftYn);

                if( giftYn == "Y" ) StartCoroutine( SelFbGiftHpHis() );

                g.wwwDate = UserInfo["wwwDate"];
                
                if(int.Parse(UserInfo[0]["informPopupCnt"]) > 0) {
                    StartCoroutine ( SelCrazyWordPopup() );
                }

                g.patchVersion = int.Parse(UserInfo[0]["curVersion"]);
            }

            dbo.SelUserInfo();
        }
        // Fail : impossible to play the game
        else
        {
            Debug.Log("www.error=" + www.error);
            g.isInternetReachability = false;
            if(g.isHome) homeControl.EventInternetWarningExit();
        }
        g.isSelUserInfo = false;
    }

    public IEnumerator SelCrazyWordPopup()
    {
        Debug.Log("www SelCrazyWordPopup");
        WWWForm form = new WWWForm();
        form.AddField("gameCode", g.gameCode);
        form.AddField("popupId", dbo.SelCrazyWordPopupMaxId());

        WWW www = new WWW("http://crazyword.org/SelCrazyWordPopup.asp", form);
        yield return www;

        // Success
        if (www.isDone && www.error == null) {
            Debug.Log("www PopupInfo=" + www.text);
            var PopupInfo = JSON.Parse(www.text);
            for(int i = 0; i < PopupInfo.Count; i++) { 
                dbo.InsCrazyWordPopup(
                  PopupInfo[i]["popupId"]
                , PopupInfo[i]["validStartDate"]
                , PopupInfo[i]["validEndDate"]
                , PopupInfo[i]["pupupDownUrl"]
                , PopupInfo[i]["pupupTexName"]
                );
            }
        }
        // Fail : impossible to play the game
        else {
            Debug.Log("www.error=" + www.error);
        }
    }


    public IEnumerator UptUserInfoFbId()
    {
        Debug.Log("www UptUserInfoFbId");

        WWWForm form = new WWWForm();

        form.AddField("userId", g.userId);
        form.AddField("gameCode", g.gameCode);
        form.AddField("fbId"  , g.fbId);

        WWW www = new WWW("http://crazyword.org/UptUserInfoFbId.asp", form);

        yield return www;

        if (www.isDone && www.error == null) {
            var RetJson = JSON.Parse(www.text);
            int errCode = int.Parse(RetJson["errCode"]);
            string errMsg = RetJson["errMsg"];

            if (errCode == 0) Debug.Log("www.text=" + www.text);
            else Debug.Log("errMsg=" + errMsg);
        } else {
            Debug.Log("www.error=" + www.error);
            mapControl.EventInternetWarning();
        }
    }


    public IEnumerator SelUserInfoNickName()
    {
        Debug.Log("www SelUserInfoNickName");

        WWWForm form = new WWWForm();

        form.AddField("userId", g.userId);
        form.AddField("gameCode", g.gameCode);

        WWW www = new WWW("http://crazyword.org/SelUserInfoNickName.asp", form);

        yield return www;

        if (www.isDone && www.error == null)
        {
            Debug.Log(www.text);
            var RetJson = JSON.Parse(www.text);
            int nickCnt = int.Parse(RetJson["nickCnt"]);
            string nickName = RetJson["nickName"];
            int errCode = int.Parse(RetJson["errCode"]);
            string errMsg = RetJson["errMsg"];

            if (errCode == 0) {
                Debug.Log("www.text=" + www.text);
                if (nickCnt > 0) {
                    homeControl.NickName.SetActive(false);
                    homeControl.startPlayUILabel.text = "    게임시작!!!    ";
                    g.nickName = nickName;
                    dbo.UptUserNickName();
                } else {
                    homeControl.NickName.SetActive(true);
                    homeControl.startPlayUILabel.text = "입력후 게임시작!!";
                }
                homeControl.UserFirstEnterPopup.SetActive(true);
                homeControl.PopupBg8.SetActive(true);
                homeControl.userFirstEnterAnimator.speed = 2;
                homeControl.userFirstEnterAnimator.SetBool("isSmallLargeScale2", true);

            } else {
                Debug.Log("errMsg=" + errMsg);
            }            
            
        }
        else
        {
            Debug.Log("www.error=" + www.error);
            homeControl.EventInternetWarning();
        }
    }



    public IEnumerator UptUserInfoNickName()
    {
        Debug.Log("www UptUserInfoFbId");

        WWWForm form = new WWWForm();

        form.AddField("userId", g.userId);
        form.AddField("gameCode", g.gameCode);
        form.AddField("nickName", g.nickName);

        WWW www = new WWW("http://crazyword.org/UptUserInfoNickName2.asp", form);

        yield return www;

        if (www.isDone && www.error == null)
        {
            Debug.Log(www.text);
            var RetJson = JSON.Parse(www.text);
            int nickCnt = int.Parse(RetJson["nickCnt"]);
            int errCode = int.Parse(RetJson["errCode"]);
            string errMsg = RetJson["errMsg"];

            if (errCode == 0) {
                Debug.Log("www.text=" + www.text);
                if (nickCnt == 0) {
                    dbo.UptUserNickName();
                    homeControl.FirstEnterPopupClose();
                } else {
                    homeControl.nickNameWarningLabel.text = "이미 사용되고 있는 닉네임입니다.";
                    homeControl.EventNickNameWarning();
                }    
            } else {
                Debug.Log("errMsg=" + errMsg);
            }            
            
        }
        else
        {
            Debug.Log("www.error=" + www.error);
            homeControl.EventInternetWarning();
        }
    }


    public IEnumerator InsUserInfo()
    {
        Debug.Log("www InsUserInfo");

        _data = dbo.SelUserInfo(); dr = _data.Rows[0];

        WWWForm form = new WWWForm();

        form.AddField("userId", dr["userId"].ToString());
        form.AddField("gameCode", dr["gameCode"].ToString());
        form.AddField("userName", dr["userName"].ToString());
        form.AddField("nickName", dr["nickName"].ToString());
        form.AddField("deviceId", dr["deviceId"].ToString());
        form.AddField("curStage", dr["curStage"].ToString());
        form.AddField("bestScore", dr["bestScore"].ToString());
        form.AddField("coin", dr["coin"].ToString());
        form.AddField("freeHp", dr["freeHp"].ToString());
        form.AddField("buyHp", dr["buyHp"].ToString());
        form.AddField("giftHp", dr["giftHp"].ToString());
        form.AddField("itemHp", dr["itemHp"].ToString());
        form.AddField("freeSkip", dr["freeSkip"].ToString());
        form.AddField("buySkip", dr["buySkip"].ToString());
        form.AddField("giftSkip", dr["giftSkip"].ToString());
        form.AddField("itemSkip", dr["itemSkip"].ToString());
        form.AddField("partMove", dr["partMove"].ToString());
        form.AddField("noBannerYn", dr["noBannerYn"].ToString());
        form.AddField("userPicYn", dr["userPicYn"].ToString());
        form.AddField("fbShareDate", dr["fbShareDate"].ToString());
        form.AddField("friendYn", dr["friendYn"].ToString());
        form.AddField("giftYn", dr["giftYn"].ToString());
        form.AddField("achievement", dr["achievement"].ToString());
        form.AddField("userBgCol", dr["userBgCol"].ToString());
        form.AddField("userBgImg", dr["userBgImg"].ToString());
        form.AddField("fbId", dr["fbId"].ToString());


        WWW www = new WWW("http://crazyword.org/InsUserInfo.asp", form);

        yield return www;

        if (www.isDone && www.error == null)
        {
            Debug.Log("www.text=" + www.text);
            g.wwwDate = System.DateTime.Now.ToString("yyyyMMdd");
        }
        else
        {
            Debug.Log("www.error=" + www.error);
        }
    }

    public IEnumerator UptUserInfoAchievement(int partNum)
    {
        Debug.Log("www UptUserInfoAchievement");

        WWWForm form = new WWWForm();

        form.AddField("userId", g.userId);
        form.AddField("gameCode", g.gameCode);
        form.AddField("partNum", partNum);

        WWW www = new WWW("http://crazyword.org/UptUserInfoAchievement.asp", form);

        yield return www;

        if (www.isDone && www.error == null)
        {
            //-----------------
            // RetJson
            //-----------------
            var RetJson = JSON.Parse(www.text);
            Debug.Log("www achievement=" + www.text);

            string achievement = RetJson["achievement"];
            int errCode = int.Parse(RetJson["errCode"]);
            string errMsg = RetJson["errMsg"];

            if (errCode == 0)
            {
                //Debug.Log("www achievement=" + achievement);
                dbo.UptUserInfoAchievement(achievement);
            }
            else
            {
                Debug.Log("errMsg=" + errMsg);
            }
        }
        else
        {
            Debug.Log("www.error=" + www.error);
        }
    }

    //-----------------------------------------------------
    //                 UserStageHis
    //-----------------------------------------------------
    public IEnumerator InsSelUserStageInfo(int stageMaxRank, int stageMaxCombo)
    {
        Debug.Log("www InsSelUserStageInfo");

        WWWForm form = new WWWForm();
        form.AddField("userId", g.userId);
        form.AddField("gameCode", g.gameCode);
        form.AddField("stageNum", g.curStage);
        form.AddField("stageMaxRank", stageMaxRank);
        form.AddField("stageMaxCombo", stageMaxCombo);

        WWW www = new WWW("http://crazyword.org/InsSelUserStageHis.asp", form);

        yield return www;

        if (www.isDone && www.error == null)
        {
            //-----------------
            // RetJson
            //-----------------
            var RetJson = JSON.Parse(www.text);
            int errCode = int.Parse(RetJson["errCode"]);
            string errMsg = RetJson["errMsg"];

            if (errCode == 0)
            {
                Debug.Log("www.text=" + www.text);
                g.partNum = int.Parse(RetJson["partNum"]);
                g.comboTotalCnt = int.Parse(RetJson["comboTotalCnt"]);

                //Debug.Log("www InsSelUserStageInfo g.partNum="+g.partNum);
                //Debug.Log("www InsSelUserStageInfo g.comboTotalCnt="+g.comboTotalCnt);

            }
            else
            {
                Debug.Log("errMsg=" + errMsg);
            }
        }
        else
        {
            Debug.Log("www.error=" + www.error);
        }

    }


    //-----------------------------------------------------
    //                        Shop
    //-----------------------------------------------------

    //-----------------------
    // PurchaseTran
    //-----------------------
    public IEnumerator PurchaseTran(string orderId, string productId)
    {
        Debug.Log("www PurchaseTran productId=" + productId);

        WWWForm form = new WWWForm();

        //Debug.Log("userId=" + g.userId);
        //Debug.Log("orderId=" + orderId);
        //Debug.Log("productId=" + productId);

        form.AddField("userId", g.userId);
        form.AddField("gameCode", g.gameCode);
        form.AddField("orderId", orderId);
        //form.AddField("orderId", "111111");
        form.AddField("productId", productId);
        form.AddField("deviceId", g.deviceId);

        WWW www = new WWW("http://crazyword.org/PurchaseTran.asp", form);

        yield return www;

        if (www.isDone && www.error == null)
        {
            Debug.Log("www.text=" + www.text);

            //-----------------
            // RetJson
            //-----------------
            var RetJson = JSON.Parse(www.text);
            int errCode = int.Parse(RetJson["errCode"]);
            string errMsg = RetJson["errMsg"];

            if (errCode == 0) {
                g.coin = int.Parse(RetJson["coin"]);
                Debug.Log("g.coin=" + g.coin);
                UptCoinInfo();

                if (productId == "fbShare") {
                    dbo.UptUserInfoFbShareDate();

                    g.fbShareDate = g.wwwDate;
                    popupShopControl.SetFbShareCoin();

                    popupShopControl.SetActiveShopFbSharePopup(false);
                    popupShopControl.SetActivePopupBg4(false);

                } else if (productId == "allpack") {
                //else if (productId.Substring(0, 4) == "coin") {}
                    //local
                    dbo.UptUserInfoAllPack();

                    mapControl.SetPartLock();

                    dbo.UptUserStageHisStageClear(1, mapControl.lessonSprites.Count);
                    
                    for (int i = 1; i <= mapControl.lessonSprites.Count; i++) {
                        mapControl.lessonSprites[i-1].spriteName = mapControl.iconSprites[i];
                    }

                    popupShopControl.SetAllPack();
                    popupShopControl.SetActiveShopAllPackPopup(false);
                    popupShopControl.SetActivePopupBg4(false);

                } else if (productId == "nobanner") {
                    Debug.Log("productId=" + productId);
                    //local
                    dbo.UptUserInfoNoBanner();
                    g.noBannerYn = "Y";
                    mapControl.SetBanner();

                    #if UNITY_ANDROID && !UNITY_EDITOR
                        AdMobAndroid.hideBanner(true);
                    # endif

                    popupMapControl.NoBannerPopup.SetActive(false);
                    popupMapControl.PopupBg6.SetActive(false);
                } else if (productId == "skip1") {
                    //Debug.Log("productId=" + productId);
                    //local
                    dbo.UptUserInfoSkip1();
                    g.buySkip = g.buySkip + 1;

                    mapControl.skipNumUILabel.text = g.allSkip().ToString();
                    mapControl.skipNumLineUILabel.text = g.allSkip().ToString();

                    popupShopControl.SetActiveCoinWarningPopup(false);
                    //popupGameControl.ChkHpSkipOnFinished();

                    popupShopControl.SetActivePopupBg4(false);

                } else if (productId == "hp1") {
                    //Debug.Log("productId=" + productId);
                    //local
                    dbo.UptUserInfoHp1();
                    g.buyHp = g.buyHp + 1;

                    mapControl.hpNumUILabel.text = g.allHp().ToString();
                    mapControl.hpNumLineUILabel.text = g.allHp().ToString();

                    popupShopControl.SetActiveCoinWarningPopup(false);
                    popupShopControl.SetActivePopupBg4(false);
                } else if (productId.Substring(0,7) == "achieve") {
                    //Debug.Log("productId=" + productId);
                    
                    if(!g.isGiftAcceptAll) {
                        popupFriendControl.EventGiftWarning();
                        dbo.UptUserGiftHis( g.giftCodes[g.giftIdx].itemGroup,g.giftCodes[g.giftIdx].itemCode,g.giftCodes[g.giftIdx].recieveDate,g.giftCodes[g.giftIdx].recieveTime );
                    }

                }
            } else {
                Debug.Log("errMsg=" + errMsg);
                // show warning popup
            }

        }
        else
        {
            Debug.Log("www.error=" + www.error);
            mapControl.EventInternetWarning();
            popupFriendControl.GiftBoxColliderTrue();
        }
        g.isPopupPurchaseShow = false;
    }


    //-----------------------
    // PurchasePartItem
    //-----------------------
    public IEnumerator PurchasePartItem(int itemCode)
    {
        Debug.Log("www PurchasePartItem");

        WWWForm form = new WWWForm();

        form.AddField("userId", g.userId);
        form.AddField("gameCode", g.gameCode);
        form.AddField("itemCode", itemCode);
        form.AddField("deviceId", g.deviceId);

        //Debug.Log("userId   =" + g.userId);
        //Debug.Log("itemCode =" + itemCode);

        WWW www = new WWW("http://crazyword.org/PurchasePartItem.asp", form);

        yield return www;

        if (www.isDone && www.error == null)
        {
            Debug.Log("www.text=" + www.text);

            //-----------------
            // RetJson
            //-----------------
            var RetJson = JSON.Parse(www.text);
            int errCode = int.Parse(RetJson["errCode"]);
            string errMsg = RetJson["errMsg"];

            if (errCode == 0)
            {
                //-----------------
                // coin
                //-----------------
                g.coin = int.Parse(RetJson["coin"]);
                //Debug.Log("g.coin=" + g.coin);
                UptCoinInfo();

                //-----------------
                // PartMove
                //-----------------
                // modify PartMove
                g.partMove = RetJson["partMove"];
                //Debug.Log("g.partMove=" + g.partMove);

                mapControl.SetPartLocks(g.partNum - 1, false);
                dbo.UptUserInfoPartMove();

                _data = dbo.SelPartStageNum(g.partNum); dr = _data.Rows[0];
                int minStageNum = int.Parse(dr["minStageNum"].ToString());
                int maxStageNum = int.Parse(dr["maxStageNum"].ToString());

                //Debug.Log("minStageNum=" + minStageNum);
                //Debug.Log("maxStageNum=" + maxStageNum);

                dbo.UptUserStageHisStageClear(minStageNum, maxStageNum);

                for (int i = 0; i <= maxStageNum - minStageNum; i++)
                {
                    mapControl.lessonSprites[minStageNum - 1 + i].spriteName = mapControl.iconSprites[minStageNum + i];
                }

                //-----------------
                // popup
                //-----------------
                popupMapControl.PartItemPopup.SetActive(false);
                popupMapControl.PopupBg2.SetActive(false);
                

            }
            else
            {
                Debug.Log("errMsg=" + errMsg);
                // show warning popup
            }
        }
        else
        {
            Debug.Log("www.error=" + www.error);
            mapControl.EventInternetWarning();
        }
    }

    void UptCoinInfo()
    {
        mapControl.coinAmount.text = g.coin.ToString();
        mapControl.coinLineAmount.text = g.coin.ToString();

        popupShopControl.SetMyCoinUILabel(g.coin.ToString());

        dbo.UptUserInfoCoin();
    }


    //-----------------------------------------------------
    //                 Fb Friend Hp gift 
    //-----------------------------------------------------

    public IEnumerator SelFbGiftHpHis()
    {
        Debug.Log("www SelFbGiftHpHis");

        WWWForm form = new WWWForm();
        form.AddField("receiveUserId", g.userId);
        form.AddField("gameCode", g.gameCode);
        form.AddField("deviceId", g.deviceId);

        WWW www = new WWW("http://crazyword.org/SelFbGiftHpHis.asp", form);

        yield return www;

        if (www.isDone && www.error == null) {
            Debug.Log("www.text=" + www.text);

            //-----------------
            // RetJson
            //-----------------
            var FbGiftHpHis = JSON.Parse(www.text);
            
            int rowCnt = FbGiftHpHis.Count;
            //Debug.Log("rowCnt = "+ rowCnt);

            for (int i = 0; i < rowCnt; i++) {
                dbo.InsUserGiftHis(6,2,FbGiftHpHis[i]["sendFbId"]);
            }
        }
        else
        {
            Debug.Log("www.error=" + www.error);
            mapControl.EventInternetWarning();
        }

    }


    public IEnumerator InsFbGiftHpHis(string sendFbId, string receiveFbId, string kind)
    {
        Debug.Log("www InsFbGiftHpHis");

        WWWForm form = new WWWForm();
        form.AddField("userId", g.userId);
        form.AddField("gameCode", g.gameCode);

        form.AddField("sendFbId", sendFbId);
        form.AddField("receiveFbId", receiveFbId);
        form.AddField("kind", kind);
        form.AddField("deviceId", g.deviceId);

        Debug.Log(g.userId +" "+ sendFbId +" "+ receiveFbId+" "+kind);

        WWW www = new WWW("http://crazyword.org/InsFbGiftHpHis.asp", form);

        yield return www;


        if (www.isDone && www.error == null)
        {
            Debug.Log("www.text=" + www.text);

            //-----------------
            // RetJson
            //-----------------
            var RetJson = JSON.Parse(www.text);
            int errCode = int.Parse(RetJson["errCode"]);
            string errMsg = RetJson["errMsg"];

            if (errCode == 0)
            {
                //Debug.Log("errCode=0");
                dbo.InsFbGiftHpHis(kind);

                if(g.facebookEvent=="AppRequstRequestGift") {
                    g.requestGiftUISprites[g.friendListIdx].spriteName = "hprequst_o"; 
                    g.requestGiftBoxColliders[g.friendListIdx].enabled = false;          
                } else if(g.facebookEvent=="AppRequstSendGift") {
                    g.sendGiftUISprites[g.friendListIdx].spriteName = "presentsend_o"; 
                    g.sendGiftBoxColliders[g.friendListIdx].enabled = false;
                }

            }
            else
            {
                Debug.Log("errMsg=" + errMsg);
            }
        }
        else
        {
            Debug.Log("www.error=" + www.error);
            mapControl.EventInternetWarning();
        }

    }

    //-----------------------------------------------------
    //                       Score
    //-----------------------------------------------------
    public IEnumerator SelFbFriendScore()
    {
        Debug.Log("www SelFbFriendScore");

        string fbFriendIds = "";
		_data = dbo.SelFbFriendInfo2();
	
		for (int i = 0; i < _data.Rows.Count; i++) {
			dr = _data.Rows[i];
            if(i < _data.Rows.Count-1)
                fbFriendIds += "'"+dr["fbFriendId"].ToString() + "',";
            else
                fbFriendIds += "'"+dr["fbFriendId"].ToString() + "'";
        }

        //Debug.Log("fbFriendIds="+fbFriendIds);

        WWWForm form = new WWWForm();
        form.AddField("gameCode", g.gameCode);
        form.AddField("fbFriendIds", fbFriendIds);

        WWW www = new WWW("http://crazyword.org/SelFbFriendScore.asp", form);

        yield return www;

        // Success
        if (www.isDone && www.error == null)
        {
            Debug.Log("www.text=" + www.text);

            var fbFriendScores = JSON.Parse(www.text);
            int rowCnt = fbFriendScores.Count;

            for (int i = 0; i < rowCnt; i++)
            {
                string fbFriendId = fbFriendScores[i]["fbId"];
                int bestScore = int.Parse( fbFriendScores[i]["bestScore"] );
                //Debug.Log(fbFriendId+"="+bestScore);
                dbo.UptFbFriendInfoScore(fbFriendId,bestScore);
            }
            dbo.UptUserInfoBestScore();
            dbo.UptFbFriendInfoScore(g.fbId,g.bestScore);

        }
        // Fail : impossible to play the game
        else
        {
            Debug.Log("www.error=" + www.error);
            mapControl.EventInternetWarning();
        }
    }



    //-----------------------------------------------------
    //                       Score
    //-----------------------------------------------------
    public IEnumerator SelPhotoInfo(int stageNum,int photoInfoNum)
    {
        Debug.Log("www SelPhotoInfo");

        _data = dbo.SelPhotoInfo3(stageNum);

        dr = _data.Rows[photoInfoNum-1]; 	

        WWWForm form = new WWWForm();
        form.AddField("stageNum", dr["stageNum"].ToString());
        form.AddField("turnTexNum", dr["turnTexNum"].ToString());
        form.AddField("texNum", dr["texNum"].ToString());

        WWW www = new WWW("http://crazyword.org/SelPhotoInfo.asp", form);

        yield return www;

        // Success
        if (www.isDone && www.error == null)
        {
            Debug.Log("www.text=" + www.text);

            var PhotoInfo = JSON.Parse(www.text);
            int rowCnt = PhotoInfo.Count;
            string photoLink = PhotoInfo[0]["photoLink"];

            Application.OpenURL(photoLink);
        }
        // Fail : impossible to play the game
        else
        {
            Debug.Log("www.error=" + www.error);
            //mapControl.EventInternetWarning();
        }
    }




    //-----------------------------------------------------
    //                      Modify
    //-----------------------------------------------------
    public IEnumerator UptSqliteWww()
    {

        Debug.Log("UptSqliteWww");

        WWWForm form = new WWWForm();
        form.AddField("userId", "abc");
        WWW www = new WWW("http://crazyword.org/SelCrazyWordModifySql.asp", form);

        yield return www;

        // Success
        if (www.isDone && www.error == null)
        {
            Debug.Log("www.text=" + www.text);

            var wwwInfo = JSON.Parse(www.text);
            int rowCnt = int.Parse(wwwInfo["rowCnt"]);

            //Debug.Log("rowCnt=" + rowCnt);

            if (rowCnt > 0)
            {
                _data = dbo.SelCrazyWordModifySql(); dr = _data.Rows[0];

                if (dr["curVersion"] == null)
                {
                    dbo.InsCrazyWordModifySql(wwwInfo["curVersion"], wwwInfo["query"]);

                }
                else if (dr["curVersion"].ToString() == wwwInfo["curVersion"])
                {
                    Debug.Log("notify sqlite patch info to admin.");
                }


            }
        }
        // Fail : impossible to play the game
        else
        {
            Debug.Log("www.error=" + www.error);
            g.isInternetReachability = false;
        }
    }

    //-----------------------------------------------------
    //                       Etc
    //-----------------------------------------------------

    //-----------------------
    // ItemCode
    //-----------------------
    public IEnumerator SelItemCode(int itemGroup, int itemCode)
    {

        WWWForm form = new WWWForm();
        form.AddField("itemGroup", itemGroup);
        form.AddField("itemCode", itemCode);
        WWW www = new WWW("http://crazyword.org/SelItemCode.asp", form);

        yield return www;

        // Success
        if (www.isDone && www.error == null)
        {
            Debug.Log("www.text=" + www.text);

            var CodeInfo = JSON.Parse(www.text);
            int rowCnt = int.Parse(CodeInfo["rowCnt"]);

            //Debug.Log("rowCnt=" + rowCnt);

        }
        // Fail : impossible to play the game
        else
        {
            Debug.Log("www.error=" + www.error);
        }

    }



    public IEnumerator SelWwwDate()
    {
        WWW www = new WWW("http://crazyword.org/SelWwwDate.asp");

        yield return www;

        // Success
        if (www.isDone && www.error == null)
        {
            Debug.Log("www.text=" + www.text);

            var UserInfo = JSON.Parse(www.text);
            int rowCnt = int.Parse(UserInfo["rowCnt"]);

            //Debug.Log("rowCnt=" + rowCnt);
            if (rowCnt != 0)
            {
                g.wwwDate = UserInfo["wwwDate"];
            }
        }
        // Fail : impossible to play the game
        else
        {
            Debug.Log("www.error=" + www.error);
            g.isInternetReachability = false;
        }
    }

    //-----------------------
    // CheckInternet
    //-----------------------
    public IEnumerator CheckInternet()
    {
        WWW www = new WWW("g.internetConnectTestUrl");
        yield return www;
        if (www.isDone && www.error == null) Debug.Log("CheckInternet www.text=" + www.text);
        else Debug.Log("CheckInternet www.error=" + www.error);
    }

}