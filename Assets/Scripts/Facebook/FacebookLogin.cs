using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Facebook.MiniJSON;
using SimpleJSON;
using System.IO;

public class FacebookLogin : MonoBehaviour
{
    public FacebookManager facebookManager;
    public HomeControl homeControl;
    public MapControl mapControl;

    public DBOW dbow;
    public DBO dbo; DataTable _data; DataRow dr; 

    //----------------------------
    // FaceBook Init
    //----------------------------
    public void StartInfo() {
        FB.Init(onInitComplete);
    }
    void onInitComplete() {

        Debug.Log("Facebook Initialize Complete.");

        if (FB.IsLoggedIn == false) {
            Debug.Log("Facebook not Login");

            if(g.isHome){ homeControl.FacebookLoginBtn.SetActive(true); homeControl.FacebookMyImage.gameObject.SetActive(false); }
            else StartFacebookLogin();
        } else {
            Debug.Log("Facebook Login");
            Debug.Log("FB.UserId : " + FB.UserId);

            //---------------------------
            // FacebookMyInfo
            //---------------------------
            FacebookGetMyInfo();

        }
    }
    public void FacebookGetMyInfo() {
        FB.API("/me?", Facebook.HttpMethod.GET, MyInfoCallback);
    }

    string fbUserName,fbUserGender,fbUserLink,fbUserLocale;
    void MyInfoCallback(FBResult result)
    {
        Debug.Log("MyInfoCallback");

        if(result.Text == null || result.Text == "" || result.Text.Length < 1) {
            Debug.Log("MyInfoCallback result Error="+result.Text);
            return;
        } else {
            Debug.Log("MyInfoCallback result="+ result.Text);
        }

        var myInfo = JSON.Parse(result.Text);
        //Debug.Log("myInfo="+myInfo);

        g.fbId       = myInfo["id"];
        fbUserName   = myInfo["name"];
        fbUserGender = myInfo["gender"];
        fbUserLink   = myInfo["link"];
        fbUserLocale = myInfo["locale"];

        dbo.InsFbUserInfo(g.fbId,fbUserName,fbUserGender,fbUserLink,fbUserLocale);
        dbo.UptUserFbId(g.fbId);

        //Debug.Log("MyInfoCallback g.facebookEvent=="+g.facebookEvent);

        if     (g.facebookEvent=="AppRequst") facebookManager.FacebookAppRequest();
        else if(g.facebookEvent=="AppRequstRequestGift") facebookManager.FacebookDirectRequest();
        else if(g.facebookEvent=="AppRequstSendGift") facebookManager.FacebookDirectRequest();

        FB.API("/me/picture?g&redirect=false&width=128&height=128",
                Facebook.HttpMethod.GET,
                MyPictureCallback);
    }

    void MyPictureCallback(FBResult result)
    {
        Debug.Log("MyPictureCallback");
        if (result == null)
        {
            Debug.Log("Picture URL Error.");
            return;
        }

        var myInfo = JSON.Parse(result.Text);
        Debug.Log(result.Text);
        g.fbUserPicUrl = myInfo["data"]["url"];
        dbo.UptFbUserInfoPic(g.fbUserPicUrl);
        StartCoroutine("SaveFbMyPicture");

        //---------------------------
        // FacebookFriends
        //---------------------------
        FacebookGetFriends();
    }
    public IEnumerator SaveFbMyPicture(){
        yield return StartCoroutine( SaveFbPicture(g.fbUserPicUrl,g.fbId+".jpg") );
        StartCoroutine ( LoadMyPicture() );
    }

    public IEnumerator SaveFbPicture(string fbPictureUrl,string texFileName)
    {
        Debug.Log("SaveMyPicture");

        // www
        WWW www = new WWW(fbPictureUrl);
        yield return www;
        Texture2D downTexture = new Texture2D(128, 128, TextureFormat.ARGB32, false);
        www.LoadImageIntoTexture(downTexture);
        byte[] bytes = downTexture.EncodeToJPG();

        // directory
        string texPath  = Application.persistentDataPath + "/fb/";
        DirectoryInfo di = new DirectoryInfo(texPath);
        if (di.Exists == false) di.Create();

        // file write
        File.WriteAllBytes(texPath+texFileName, bytes);
    }
    public IEnumerator LoadMyPicture()
    {
        Debug.Log("LoadMyPicture");

        // file load    
        string texPath  = Application.persistentDataPath + "/fb/";
        string texFileName  = g.fbId+".jpg";
        Texture2D downTexture = new Texture2D(128, 128, TextureFormat.ARGB32, false);
		WWW www = new WWW("file:///"+texPath+texFileName);
		yield return www;
		www.LoadImageIntoTexture(downTexture);

        if(g.isHome) {
            homeControl.FacebookMyImage.mainTexture = downTexture;
            homeControl.FacebookLoginBtn.SetActive(false);
            homeControl.FacebookMyImage.gameObject.SetActive(true);
        }
    }

    //----------------------------
    // FaceBook Login
    //----------------------------
    public void StartFacebookLogin() { StartCoroutine("StartFacebookLogin2"); }
    public IEnumerator StartFacebookLogin2() {
        WWW www = new WWW(g.internetConnectTestUrl);
        yield return www;
        if (www.isDone && www.error == null) {
            Debug.Log("CheckInternet www.text=" + www.text);
            FB.Login("public_profile,email,user_friends,user_games_activity",LoginCallback);
        } else { 
            Debug.Log("CheckInternet www.error=" + www.error); 
            if(g.isHome) homeControl.EventInternetWarning();
        }
    }

    void LoginCallback(FBResult result) {
        Debug.Log("LoginCallback");

        Debug.Log(result.Text);
        var profile = JSON.Parse(result.Text);
        string is_logged_in = profile["is_logged_in"];
        if (is_logged_in == "false") {
            Debug.Log("Login Error");
            //if(g.isHome) homeControl.EventFacebookWarningExit();
            //if(g.isMap) mapControl.EventFacebookWarningExit();
            return;
        } else {

            g.fbId = profile["user_id"];
            Debug.Log("g.fbId  : " + g.fbId);

            // facebook my information
            FB.API("/me?", Facebook.HttpMethod.GET, MyInfoCallback);

            if(g.isShopFbShare) {

                facebookManager.FacebookFeed(); g.isShopFbShare = false;
            }

        }
    }

    //-------------------------------------------------------------
    //                      FacebookFriends
    //-------------------------------------------------------------
    public void FacebookGetFriends() {
        Debug.Log("FacebookFriends");
        FB.API("/me/friends?fields=id,name,picture", Facebook.HttpMethod.GET, FriendsCallback);
        //FB.API("/me/friends", Facebook.HttpMethod.GET, FriendsCallback);
    }
    string friendId,friendName,friendImageUrl;int gameFriendCnt;
    void FriendsCallback(FBResult result)
    {
        Debug.Log("FriendsCallback");
        
        var friendData = JSON.Parse(result.Text); 
        //Debug.Log(friendData);

        string friendAllCount = friendData["summary"]["total_count"];
        //Debug.Log("friend ALL Count  : " + friendAllCount);

        dbo.DelFbFriendInfo(); gameFriendCnt = 0;
        for (int i = 0; i < int.Parse(friendAllCount); i++) {
            friendId = friendData["data"][i]["id"];
            friendName = friendData["data"][i]["name"];
            friendImageUrl = friendData["data"][i]["picture"]["data"]["url"];
            //Debug.Log("friend Id="+friendId+" Name="+friendName+" URL="+friendImageUrl);

            if(friendName!=null && friendName.Length > 2) {
                gameFriendCnt++;
                //Debug.Log("friend1111 Id="+friendId+" Name="+friendName+" URL="+friendImageUrl);
                dbo.InsFbFriendInfo(friendId, friendName, friendImageUrl);
                StartCoroutine ( SaveFbPicture(friendImageUrl,friendId+".jpg") );
            }
        }
        if(gameFriendCnt > 0) {
            //Debug.Log("friend2222 Id="+g.fbId+" Name="+fbUserName+" URL="+fbUserLink);
            dbo.InsFbFriendInfo(g.fbId, fbUserName, fbUserLink);
        }

        //--------------------
        // Score
        //--------------------
        StartCoroutine ( dbow.SelFbFriendScore() );

        //FacebookPostMyScore("100");
        //FacebookGetScores();

            //var query = new Dictionary<string, string>();
            //query["score"] = "89";
            //FB.API("/me/scores", Facebook.HttpMethod.POST, delegate(FBResult r) { Debug.Log("Result: " + r.Text); }, query);

            //FB.API("/me/scores", Facebook.HttpMethod.POST, delegate(FBResult r) { Debug.Log("Result: " + r.Text); }, query);

            //FB.API("/1483906351878206/scores", Facebook.HttpMethod.POST, OnPost, scoreData);
            //FB.API("/app/scores", Facebook.HttpMethod.GET, ProcessRanking);
            //FB.API("/app/scores/fields=score,user.limit(20)", Facebook.HttpMethod.GET, ProcessRanking);
    }

    //-------------------------------------------------------------
    //                       FacebookScores
    //-------------------------------------------------------------
    // save my score
    public void FacebookPostMyScore(string myScore) 
    {
        var query = new Dictionary<string, string>();
        query["score"] = "100";
        FB.API(
               "/me/scores", 
               Facebook.HttpMethod.POST, 
               delegate(FBResult r) { Debug.Log("Result: " + r.Text); }, 
               query
               );
    }

    // load friend's scores
    public void FacebookGetScores() {
        FB.API("/app/scores", Facebook.HttpMethod.GET, ProcessRanking);        
    }
    void ProcessRanking(FBResult result) {
        Debug.Log("ProcessRanking");
        if ( result.Error != null ) {
            Debug.Log("Error get ranking");
        }

        //랭킹을 가져와서 정렬.
        var rankingData = JSON.Parse(result.Text);
        Debug.Log("fb sroce="+rankingData);

        SortedList<int, string> rankingList = new SortedList<int, string>();

        for (int i = 0; i < rankingData["data"].Count; i++) {
            rankingList[rankingData["data"][i]["score"].AsInt] = rankingData["data"][i]["user"]["id"];
        }
    }



}
