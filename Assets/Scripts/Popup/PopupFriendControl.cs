using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;


public class PopupFriendControl : MonoBehaviour {

    public MapControl mapControl;
    public FacebookLogin facebookLogin;
    public FacebookManager facebookManager;

    //--------------------------
    // PopupBg
    //--------------------------
    public GameObject PopupBg2, PopupBg4;

    //--------------------------
    // Friend
    //--------------------------
    public Animator friendAnimator;

    //---------------------------------------------------------
    // Sound
    //---------------------------------------------------------
    public AudioClip optionClip, menuClip; public AudioSource audioSource;

    //---------------------------------------------------------
    // Common
    //---------------------------------------------------------
    public DBO dbo; public DBOW dbow; public EventCommon ev;
	public static DataTable _data,_data1;
	public static DataRow dr,dr1;

    //---------------------------------------------------------------
    //                         Friend Popup
    //---------------------------------------------------------------
    public void EventShowFriendPopup()
    {
        Debug.Log("ShowFriendListPopup");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            friendListScrollView.SetActive(false);
            g.isPopupFriendShow = true;
            if (FB.IsLoggedIn == true) {
                fbLoginInfo.SetActive(false);
                StartCoroutine ( MakeFriendGrid() );
            } else {
                fbShareButtonUISprite.spriteName = "facebooklogin2";
                fbLoginInfo.SetActive(true);
                PopupBg2.SetActive(true);
                friendAnimator.SetBool("isRightMiddleMove", true);
                EnableGiftGridBoxCollider(true);
                requestExpainUILabel.enabled = false;
            }
        }
    }

    public UIGrid friendUIGrid;
    public GameObject FriendListPrefab,FriendListPrefabMe;
    public UISprite fbShareButtonUISprite;
    public GameObject fbLoginInfo;


    GameObject FriendListClone;
    UILabel friendRankUILabel,friendScoreUILabel,friendNameUILabel;
    UITexture friendPicUITexture;
    UIEventTrigger requestGiftUIEventTrigger,sendGiftUIEventTrigger;
    UISprite requestGiftUISprite,sendGiftUISprite;
    BoxCollider requestGiftBoxCollider,sendGiftBoxCollider;

    public GameObject friendListScrollView;
    public UIScrollView friendListUIScrollView;
    public UIPanel friendListUIPanel;

    public UILabel requestExpainUILabel;

    string fbFriendId,fbFriendName,fbFriendPicUrl;
    int fbFriendScore;
    List<string> fbFriendIds = new List<string>();

    public IEnumerator MakeFriendGrid(){

        Debug.Log("MakeGiftGrid");

        giftUIScrollView.enabled = true;
        fbLoginInfo.SetActive(false);
        friendListScrollView.SetActive(true);
        requestExpainUILabel.enabled = true;

        _data = dbo.SelFbFriendInfo();

   		Transform[] trans = friendUIGrid.gameObject.transform.GetComponentsInChildren<Transform>(); 
		foreach (Transform tr in trans) {
			if(tr.gameObject.tag != "Grid") {
				Destroy(tr.gameObject); 
			}
		}

        fbFriendIds.Clear();
        for (int i = 0; i < _data.Rows.Count; i++)
        {
            dr = _data.Rows[i];

            fbFriendId     = dr["fbFriendId"].ToString();
            fbFriendName   = dr["fbFriendName"].ToString();
            fbFriendPicUrl = dr["fbFriendPicUrl"].ToString();
            fbFriendScore  = int.Parse(dr["score"].ToString());

            fbFriendIds.Add(fbFriendId);
            //---------------------------------------------
            // Instantiate FriendListPrefab
            //---------------------------------------------
            if (g.fbId == fbFriendId) { FriendListClone = Instantiate(FriendListPrefabMe) as GameObject; }
            else FriendListClone = Instantiate(FriendListPrefab) as GameObject;

            FriendListClone.transform.parent = friendUIGrid.gameObject.transform;
            FriendListClone.transform.localPosition = new Vector3(0f, -(i+1) * 91, 0f);
            FriendListClone.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            FriendListClone.transform.localScale = Vector3.one;
            FriendListClone.transform.name = "Friend"+i;

            friendRankUILabel = FriendListClone.transform.FindChild("5_Rank").GetComponent<UILabel>();
            if (g.fbId != fbFriendId) friendScoreUILabel = FriendListClone.transform.FindChild("5_Rank").FindChild("5_Score").GetComponent<UILabel>();
            else friendScoreUILabel = FriendListClone.transform.FindChild("5_Score").GetComponent<UILabel>();
            friendPicUITexture = FriendListClone.transform.FindChild("5_People").FindChild("5_FriendPic").GetComponent<UITexture>();
            friendNameUILabel = FriendListClone.transform.FindChild("5_People").FindChild("5_Name").GetComponent<UILabel>();

            requestGiftUIEventTrigger = FriendListClone.transform.FindChild("4_RequestGift").GetComponent<UIEventTrigger>();
   			EventDelegate.Add (requestGiftUIEventTrigger.onPress,ev.EventOnPress);

			EventDelegate eventClick = new EventDelegate(this, "EventRequestGiftOnRelease");
			EventDelegate.Parameter param = new EventDelegate.Parameter();
			param.obj = FriendListClone.transform;
			param.expectedType = typeof(Transform);
			eventClick.parameters[0] = param;
   			EventDelegate.Add (requestGiftUIEventTrigger.onRelease,eventClick);

			sendGiftUIEventTrigger = FriendListClone.transform.FindChild("4_SendGift").GetComponent<UIEventTrigger>();
   			EventDelegate.Add (sendGiftUIEventTrigger.onPress,ev.EventOnPress);

			eventClick = new EventDelegate(this, "EventSendGiftOnRelease");
			param = new EventDelegate.Parameter();
			param.obj = FriendListClone.transform;
			param.expectedType = typeof(Transform);
			eventClick.parameters[0] = param;
   			EventDelegate.Add (sendGiftUIEventTrigger.onRelease,eventClick);

            friendRankUILabel.text = (i+1).ToString();
            if (g.fbId == fbFriendId) { friendScoreUILabel.text = "총점 : "+fbFriendScore.ToString(); }
            else friendScoreUILabel.text = fbFriendScore.ToString();
            
            if (g.fbId == fbFriendId) { friendNameUILabel.text = g.nickName; }
            else friendNameUILabel.text = fbFriendName;
            
            string texPath = Application.persistentDataPath + "/fb/";
            string texFileName = dr["fbFriendId"].ToString()+".jpg";
            Texture2D downTexture = new Texture2D(128, 128, TextureFormat.ARGB32, false);
		    WWW www = new WWW("file:///"+texPath+texFileName);
		    yield return www;
		    www.LoadImageIntoTexture(downTexture);
            friendPicUITexture.mainTexture = downTexture;

            if(_data.Rows.Count <= 4) friendListUIScrollView.enabled = false;

            requestGiftUISprite = FriendListClone.transform.FindChild("4_RequestGift").GetComponent<UISprite>();
            sendGiftUISprite = FriendListClone.transform.FindChild("4_SendGift").GetComponent<UISprite>();
            requestGiftBoxCollider = FriendListClone.transform.FindChild("4_RequestGift").GetComponent<BoxCollider>();
            sendGiftBoxCollider = FriendListClone.transform.FindChild("4_SendGift").GetComponent<BoxCollider>();

            g.requestGiftUISprites.Add(requestGiftUISprite);
            g.sendGiftUISprites.Add(sendGiftUISprite);
            g.requestGiftBoxColliders.Add(requestGiftBoxCollider);
            g.sendGiftBoxColliders.Add(sendGiftBoxCollider);

            //requestGiftUISprite.spriteName = "hprequst_x";
            //sendGiftUISprite.spriteName = "presentsend_x";
            //requestGiftBoxCollider.enabled = true;
            //sendGiftBoxCollider.enabled = true;

            _data1 = dbo.SelFbGiftHpHis();
            for (int j = 0; j < _data1.Rows.Count; j++)
            {
                dr1 = _data1.Rows[j];
                if( dr1["kind"].ToString() == "1" && dr["fbFriendId"].ToString() == dr1["fbFriendId"].ToString()) {
                    requestGiftUISprite.spriteName = "hprequst_o"; 
                    requestGiftBoxCollider.enabled = false;
                }
                if( dr1["kind"].ToString() == "2" && dr["fbFriendId"].ToString() == dr1["fbFriendId"].ToString()) {
                    sendGiftUISprite.spriteName = "presentsend_o"; 
                    sendGiftBoxCollider.enabled = false;
                }    

                if( dr["fbFriendId"].ToString() == g.fbId ) {
                    requestGiftUISprite.spriteName = "hprequst_o"; 
                    requestGiftBoxCollider.enabled = false;
                }
                if( dr["fbFriendId"].ToString() == g.fbId ) {
                    sendGiftUISprite.spriteName = "presentsend_o"; 
                    sendGiftBoxCollider.enabled = false;
                }  
            }

            if (g.fbId == fbFriendId) { 
                FriendListClone.transform.FindChild("4_RequestGift").gameObject.SetActive(false);
			    FriendListClone.transform.FindChild("4_SendGift").gameObject.SetActive(false);
            }
            
            yield return null;
        }

        //friendListUIScrollView
        //springPanel = friendListScrollView.GetComponent<SpringPanel>();
        //springPanel.target = new Vector3(0, 0, 0);

        PopupBg2.SetActive(true);
        friendAnimator.SetBool("isRightMiddleMove", true);
        EnableGiftGridBoxCollider(true);
    }


    public void EventFbAppRequstOnRelease() {
        Debug.Log("EventFbAppRequstOnRelease");
        g.facebookEvent = "AppRequst";
        StartCoroutine ( EventFbAppRequstOnRelease2() );
    }
    IEnumerator EventFbAppRequstOnRelease2() {
        WWW www = new WWW(g.internetConnectTestUrl);
        yield return www;
        if (www.isDone && www.error == null) {
            Debug.Log("CheckInternet www.text=" + www.text);
            #if     UNITY_EDITOR
                if(FB.IsLoggedIn == false) facebookLogin.StartInfo();
                else facebookManager.FacebookAppRequest();
            #endif

            #if UNITY_ANDROID && !UNITY_EDITOR
                if(FB.IsLoggedIn == false) facebookLogin.StartFacebookLogin();
                else facebookManager.FacebookAppRequest();
            # endif
        } else { 
            Debug.Log("CheckInternet www.error=" + www.error); 
            mapControl.EventInternetWarning();
        }
    }

    public void EventRequestGiftOnRelease(Transform gameo){
        Debug.Log("EventRequestGiftOnRelease");
        g.facebookEvent = "AppRequstRequestGift";
        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
          	g.friendListIdx = int.Parse(gameo.name.Substring(6,1));
            g.fbFriendId = fbFriendIds[g.friendListIdx];
            StartCoroutine ( EventRequestGiftOnRelease2() );
        }
    }
    IEnumerator EventRequestGiftOnRelease2(){
        Debug.Log("EventRequestGiftOnRelease2");
        WWW www = new WWW(g.internetConnectTestUrl);
        yield return www;
        if (www.isDone && www.error == null) {
            Debug.Log("CheckInternet www.text=" + www.text);

            #if     UNITY_EDITOR
                if(FB.IsLoggedIn == false) facebookLogin.StartInfo();
                else facebookManager.FacebookDirectRequest();
            #endif

            #if UNITY_ANDROID && !UNITY_EDITOR
                if(FB.IsLoggedIn == false) facebookLogin.StartFacebookLogin();
                else facebookManager.FacebookDirectRequest();
            # endif
        } else { 
            Debug.Log("CheckInternet www.error=" + www.error); 
            mapControl.EventInternetWarning();
        }
    }

    public void EventSendGiftOnRelease(Transform gameo){
        Debug.Log("EventSendGiftOnRelease");
        g.facebookEvent = "AppRequstSendGift";
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            audioSource.clip = menuClip; audioSource.Play();
         	g.friendListIdx = int.Parse(gameo.name.Substring(6,1));
            g.fbFriendId = fbFriendIds[g.friendListIdx];
            StartCoroutine ( EventSendGiftOnRelease2() );
        }    
    }
    IEnumerator EventSendGiftOnRelease2(){
        Debug.Log("EventSendGiftOnRelease");
        WWW www = new WWW(g.internetConnectTestUrl);
        yield return www;
        if (www.isDone && www.error == null) {
            Debug.Log("CheckInternet www.text=" + www.text);
            #if     UNITY_EDITOR
                if(FB.IsLoggedIn == false) facebookLogin.StartInfo();
                else facebookManager.FacebookDirectRequest();
            #endif
            #if UNITY_ANDROID && !UNITY_EDITOR
                if(FB.IsLoggedIn == false) facebookLogin.StartFacebookLogin();
                else facebookManager.FacebookDirectRequest();
            # endif
        } else { 
            Debug.Log("CheckInternet www.error=" + www.error); 
            mapControl.EventInternetWarning();
        }
    }

    public void EventCloseFriendPopup()
    {
        Debug.Log("ShowFriendListPopup");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            audioSource.clip = menuClip; audioSource.Play();
            friendAnimator.SetBool("isRightMiddleMove", false);
            friendAnimator.SetBool("isMiddleRightMove", true);
            StartCoroutine( "EventCloseFriendPopupOnFinished" );
        }
    }
    public void EventCloseFriendPopup2()
    {
        Debug.Log("ShowFriendListPopup2");
        audioSource.clip = menuClip; audioSource.Play();
        friendAnimator.SetBool("isRightMiddleMove", false);
        friendAnimator.SetBool("isMiddleRightMove", true);
        StartCoroutine( "EventCloseFriendPopupOnFinished" );
    }

    public IEnumerator EventCloseFriendPopupOnFinished()
    {
        yield return new WaitForSeconds(1.4f);
        PopupBg2.SetActive(false);
        friendAnimator.SetBool("isMiddleRightMove", false);
        g.isPopupFriendShow = false;
    }


    //---------------------------------------------------------------
    //                         Gift Popup
    //---------------------------------------------------------------
    public void EventShowGiftPopup()
    {
        Debug.Log("EventShowGiftPopup");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            g.isPopupGiftShow = true;
            MakeGiftGrid();
        }
    }

    public UIGrid giftUIGrid;
    public GameObject GiftAchievePrefab,GiftHpPrefab;
    GameObject GiftClone,GiftAchieveClone,GiftHpClone;
    UISprite giftIconUISprite;
    UILabel giftTextUILabel,giftTimeUILabel;
    string giftText;
    public UIScrollView giftUIScrollView;

    public Animator giftAnimator;
    int itemGroup,itemCode;
    string receiveDate,receiveTime;
    UIEventTrigger giftUIEventTrigger;
    public BoxCollider giftAllAcceptBoxCollider;

    public void MakeGiftGrid(){

        Debug.Log("MakeGiftGrid");

        giftUIScrollView.enabled = true;

        _data = dbo.SelUserGiftHis();

   		Transform[] trans = giftUIGrid.gameObject.transform.GetComponentsInChildren<Transform>(); 
		foreach (Transform tr in trans) {
			if(tr.gameObject.tag != "Grid") {
				Destroy(tr.gameObject); 
			}
		}
        //giftUIGrid.Reposition();
        g.giftCodes.Clear();
        for (int i = 0; i < _data.Rows.Count; i++)
        {
            dr = _data.Rows[i];

            itemGroup   = int.Parse(dr["itemGroup"].ToString());
            itemCode    = int.Parse(dr["itemCode"].ToString());
            receiveDate = dr["receiveDate"].ToString();
            receiveTime = dr["receiveTime"].ToString();

            fbFriendName = dr["fbFriendName"].ToString();
            fbFriendPicUrl = dr["fbFriendPicUrl"].ToString();

            g.giftCodes.Add(new g.GiftCode(itemGroup,itemCode,receiveDate,receiveTime));

            //---------------------------------------------
            // Instantiate LessonPrefab
            //---------------------------------------------
            if (itemGroup == 10) GiftClone = Instantiate(GiftAchievePrefab) as GameObject;
            else if(itemGroup == 6) GiftClone = Instantiate(GiftHpPrefab) as GameObject;

            GiftClone.transform.parent = giftUIGrid.gameObject.transform;
            GiftClone.transform.localPosition = new Vector3(0f, -i * 100, 0f);
            GiftClone.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            GiftClone.transform.localScale = Vector3.one;
            GiftClone.transform.name = "Gift"+i;
            giftIconUISprite = GiftClone.transform.FindChild("5_GiftIcon").GetComponent<UISprite>();
            giftTextUILabel = GiftClone.transform.FindChild("5_Context").GetComponent<UILabel>();
            giftTimeUILabel = GiftClone.transform.FindChild("5_Time").GetComponent<UILabel>();

			giftUIEventTrigger = GiftClone.transform.FindChild("5_Accect").GetComponent<UIEventTrigger>();
   			EventDelegate.Add (giftUIEventTrigger.onPress,ev.EventOnPress);

			EventDelegate eventClick = new EventDelegate(this, "EventAccectGiftPopup");
			EventDelegate.Parameter param = new EventDelegate.Parameter();
			param.obj = GiftClone.transform;
			param.expectedType = typeof(Transform);
			eventClick.parameters[0] = param;
   			EventDelegate.Add (giftUIEventTrigger.onRelease,eventClick);


            if (itemGroup == 10) {
                if     (itemCode == 1) giftText = "숫자 파트 공략\n";
                else if(itemCode == 2) giftText = "입는것 파트 공략\n";
                else if(itemCode == 3) giftText = "먹는것 파트 공략\n";
                else if(itemCode == 4) giftText = "주거 파트 공략\n";
                else if(itemCode == 5) giftText = "인체 파트 공략\n";
                else if(itemCode == 6) giftText = "건강 파트 공략\n";
                else if(itemCode == 7) giftText = "사회 파트 공략\n";
                else if(itemCode == 8) giftText = "정보 파트 공략\n";
                else if(itemCode == 9) giftText = "교통 파트 공략\n";
                else if(itemCode ==10) giftText = "모든 파트 공략\n";

                if(itemCode == 10) {
                    giftText += "장학금 100Coin 지급"; giftTextUILabel.text = giftText;
                    giftIconUISprite.spriteName = "ribbonmap_stageicon010";
                }
                else {
                    giftText += "장학금 30Coin 지급"; giftTextUILabel.text = giftText; 
                    giftIconUISprite.spriteName = "ribbonmap_stageicon00"+(i+1);
                }

            } else if (itemGroup == 6) {
                giftText  = fbFriendName+"님의 \nHP 선물입니다";
                giftTextUILabel.text = giftText;
            }

            giftTimeUILabel.text = receiveDate.Substring(0,4)+"년"+receiveDate.Substring(4,2)+"월"+receiveDate.Substring(6,2)+"일";

            //giftUIGrid.Reposition();

            if(_data.Rows.Count <= 4) giftUIScrollView.enabled = false;

        }

        // make Grid
        PopupBg2.SetActive(true);
        giftAnimator.SetBool("isRightMiddleMove", true);
        EnableGiftGridBoxCollider(true);
    }

    BoxCollider giftBoxCollider;
    public void EventAccectGiftPopup(Transform gameo)
    {
        Debug.Log("EventAccectGiftPopup");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            //EnableGiftGridBoxCollider(false);
          	g.giftIdx = int.Parse(gameo.name.Substring(4,1));
		    //Debug.Log("EventAccectGiftPopup["+g.giftIdx+"]");
		    //Debug.Log("itemGroup="+g.giftCodes[g.giftIdx].itemGroup);
		    //Debug.Log("itemCode="+g.giftCodes[g.giftIdx].itemCode);
            audioSource.clip = menuClip; audioSource.Play();

            giftBoxCollider = gameo.FindChild("5_Accect").GetComponent<BoxCollider>();
            giftBoxCollider.enabled = false;

            if( g.giftCodes[g.giftIdx].itemGroup == 6) {

                dbo.UptUserGiftHis( g.giftCodes[g.giftIdx].itemGroup,g.giftCodes[g.giftIdx].itemCode,g.giftCodes[g.giftIdx].recieveDate,g.giftCodes[g.giftIdx].recieveTime );
                g.giftHp++; dbo.UptHp("gift");
                giftWarningUILabel.text = "Hp 1개가 추가되었습니다";
                EventGiftWarning();
            } else if( g.giftCodes[g.giftIdx].itemGroup == 10 ) {
                giftWarningUILabel.text = "Coin 30이 지급되었습니다";
                StartCoroutine(dbow.PurchaseTran(g.wwwDate, "achieve"+g.giftCodes[g.giftIdx].itemCode));
            }
        }
    }
    public void GiftBoxColliderTrue() { giftBoxCollider.enabled = true;}
    
	public void EnableGiftGridBoxCollider(bool tf) {
		BoxCollider[] boxCols = giftUIGrid.gameObject.GetComponentsInChildren<BoxCollider>(); 
		foreach (BoxCollider bc in boxCols) {
			bc.enabled = tf;
		}
        giftAllAcceptBoxCollider.enabled = tf;
	}

    int giftAllHp,giftAllCoin;
    public void EventAccectAllGiftPopup()
    {
        Debug.Log("EventAccectGiftPopup");

        g.isGiftAcceptAll = true;

        giftAllHp = 0; giftAllCoin = 0;

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            EnableGiftGridBoxCollider(false);

            //Debug.Log("g.giftCodes.Count="+g.giftCodes.Count);

            for(int i=0;i<g.giftCodes.Count;i++){
                g.giftIdx = i;
                //Debug.Log("g.giftCodes[g.giftIdx].itemCode="+g.giftCodes[g.giftIdx].itemCode);

                if( g.giftCodes[g.giftIdx].itemGroup == 6) {
                    dbo.UptUserGiftHis( g.giftCodes[g.giftIdx].itemGroup,g.giftCodes[g.giftIdx].itemCode,g.giftCodes[g.giftIdx].recieveDate,g.giftCodes[g.giftIdx].recieveTime );
                    g.giftHp++; dbo.UptHp("gift");
                    giftAllHp++;
                } else if( g.giftCodes[g.giftIdx].itemGroup == 10 ) {
                    StartCoroutine(dbow.PurchaseTran(g.wwwDate, "achieve"+g.giftCodes[g.giftIdx].itemCode));
                    giftAllCoin += 30;
                }

                dbo.UptUserGiftHis( g.giftCodes[g.giftIdx].itemGroup,g.giftCodes[g.giftIdx].itemCode,g.giftCodes[g.giftIdx].recieveDate,g.giftCodes[g.giftIdx].recieveTime );
            }
            
            MakeGiftGrid();
            mapControl.SetGiftYn();
            if(giftAllCoin > 0 && giftAllHp > 0) giftWarningUILabel.text = "Coin "+giftAllCoin+" 과 Hp "+giftAllHp+"개가 추가되었습니다";
            if(giftAllCoin > 0 && giftAllHp == 0) giftWarningUILabel.text = "Coin "+giftAllCoin+" 이 지급되었습니다.";
            if(giftAllCoin == 0 && giftAllHp > 0) giftWarningUILabel.text = "Hp "+giftAllHp+"개가 추가되었습니다";
            EventGiftWarning();

        }
    }


    public void EventCloseGiftPopup()
    {
        Debug.Log("EventCloseGiftPopup");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();

            giftAnimator.SetBool("isRightMiddleMove", false);
            giftAnimator.SetBool("isMiddleRightMove", true);

            StartCoroutine(EventCloseGiftPopupOnFinished());

            g.isGiftAcceptAll = false;
        }
    }
    public IEnumerator EventCloseGiftPopupOnFinished()
    {
        yield return new WaitForSeconds(1.4f);
        PopupBg2.SetActive(false);
        giftAnimator.SetBool("isMiddleRightMove", false);
        g.isPopupGiftShow = false;
    }

    //---------------------------------------------------------------
    //                      Gift Warning Popup
    //---------------------------------------------------------------
    public GameObject giftWarningPopup;
    public Animator giftWarningPopupAnimator;
    public UILabel giftWarningUILabel;
    public void EventGiftWarning() {
        Debug.Log("EventGiftWarning");

        audioSource.clip = menuClip; audioSource.Play();

        giftWarningPopup.SetActive(true);
        PopupBg4.SetActive(true);

        mapControl.SetGiftYn();

        giftWarningPopupAnimator.speed = 2;
        giftWarningPopupAnimator.SetBool("isSmallLargeScale2", true);
    }
    public void EventGiftWarningOK() {
        Debug.Log("EventGiftWarningOK");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            MakeGiftGrid();
            EventGiftWarningClose();
        }
    }

    public void EventGiftWarningClose() {
        Debug.Log("EventGiftWarningClose");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            audioSource.clip = menuClip; audioSource.Play();
            giftWarningPopup.SetActive(false);
            PopupBg4.SetActive(false);
            giftWarningPopupAnimator.SetBool("isSmallLargeScale2", false);
        }
    }

	void Update () {	
		if(Input.GetKeyDown(KeyCode.Escape)) {
            //if(g.isPopupFriendShow) EventCloseFriendPopup2();
        }
	}

}

