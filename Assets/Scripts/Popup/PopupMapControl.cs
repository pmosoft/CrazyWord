using UnityEngine;
using System.Collections;

// edit hyun.
using AndroidMediaBrowser;

public class PopupMapControl : MonoBehaviour
{ 

    public PopupShopControl popupShopControl;
    public MapControl mapControl;

    //--------------------------
    // PopupBg
    //--------------------------
    public GameObject PopupBg2, PopupBg4, PopupBg6;

    //--------------------------
    // Option
    //--------------------------
    public Animator optionAnimator,gameOptionAnimator,instructionAnimator,termsServiceAnimator,techPrincipleAnimator;
    public GameObject optionSubBgPanel, optionBackgroudEditor, optionGameOption;
    public Animator backgroudEditorAnimator;
    public UISprite[] backgroudColorChkSprites = new UISprite[10];
    public UISprite[] backgroudImageChkSprites = new UISprite[6];
    public GameObject NoBannerPopup;
    public Animator noBannerPopupAnimator;
    public UILabel noBannerUILabel;
    public UITexture userBgImgUITexture;

    public GameObject loading;

    //--------------------------
    // PartItem
    //--------------------------
    public GameObject PartItemPopup;
    public Animator partItemPopupAnimator;
    public UILabel partItemPopupUILabel;

    //--------------------------
    // HpWarning
    //--------------------------
    public GameObject HpWarningPopup;
    public Animator hpWarningPopupAnimator;

    //--------------------------
    // EnterPart
    //--------------------------
    public GameObject EnterPartPopup;
    public Animator enterPartPopupAnimator;

    public GameObject EnterPartChallengePopup;
    public Animator enterPartChallengePopupAnimator;

    public GameObject EnterPartPurchasePopup;
    public Animator enterPartPurchasePopupAnimator;
    public UILabel enterPartPurchaseUILabel;



    //---------------------------------------------------------
    // Sound
    //---------------------------------------------------------
    public AudioClip optionClip, menuClip; public AudioSource audioSource;

    //---------------------------------------------------------
    // Common
    //---------------------------------------------------------
    public DBO dbo; public DBOW dbow; public EventCommon ev;
	public static DataTable _data;
	public static DataRow dr;

    //----------------------------------------------------------------------
    //                         Show BuyPartItem
    //----------------------------------------------------------------------
    void Start() {
        userBgImgUITexture.mainTexture = g.userBgImgTextures[5];
    }

    public void EventShowPartItemPopup()
    {
        audioSource.clip = menuClip; audioSource.Play();

        PopupBg2.SetActive(true);
        PartItemPopup.SetActive(true);
        g.isPopupPartItemShow = true;

        partItemPopupUILabel.text = "주제별 이동 아이템\n구매하시겠습니까?\n(Coin x35)";

        partItemPopupAnimator.speed = 2;
        partItemPopupAnimator.SetBool("isSmallLargeScale2", true);

    }

    public void EventPartItemPopupOk()
    {
        Debug.Log("EventPartItemPopupOk");
        ev.EventOnRelease();

        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();

            if (g.coin < 35) {
                if (partItemPopupUILabel.text != "코인이 부족합니다\n상점에서 구입하시기\n바랍니다.") {
                    partItemPopupUILabel.text = "코인이 부족합니다\n상점에서 구입하시기\n바랍니다.";
                } else {
                    EventPartItemPopupClose();
                    g.isPopupPartItemShow = false;
                }
            } else {
                //StartCoroutine(dbow.InsUserBuyItemHis(1, g.partNum - 1));
                StartCoroutine(dbow.PurchasePartItem(g.partNum - 1));
                g.isPopupPartItemShow = false;
            }
        }
    }

    public void EventPartItemPopupClose() {
        Debug.Log("EventPartItemPopupClose");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            audioSource.clip = menuClip; audioSource.Play();
            PartItemPopup.SetActive(false);
            PopupBg2.SetActive(false);
            g.isPopupPartItemShow = false;
        }
    }
    public void EventPartItemPopupClose2() {
        Debug.Log("EventPartItemPopupClose2");
        audioSource.clip = menuClip; audioSource.Play();
        PartItemPopup.SetActive(false);
        PopupBg2.SetActive(false);
        g.isPopupPartItemShow = false;
    }

    //----------------------------------------------------------------------
    //                         Show HpWarning
    //----------------------------------------------------------------------
    public void EventHpWarningPopup()
    {
        audioSource.clip = menuClip; audioSource.Play();

        PopupBg2.SetActive(true);
        HpWarningPopup.SetActive(true);

        hpWarningPopupAnimator.speed = 2;
        hpWarningPopupAnimator.SetBool("isSmallLargeScale2", true);
    }

    public void EventHpWarningPopupOk()
    {
        Debug.Log("EventHpWarningPopupOk");
        ev.EventOnRelease();

        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();

            EventHpWarningPopupClose();

        }
    }

    public void EventHpWarningPopupClose()
    {
        Debug.Log("EventPartItemPopupClose");
        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();

            HpWarningPopup.SetActive(false);
            PopupBg2.SetActive(false);

        }
    }

    //----------------------------------------------------------------------
    //                         Show EnterPart
    //----------------------------------------------------------------------
    public void EventEnterPartPopup()
    {
        Debug.Log("EventEnterPartPopup");

        audioSource.clip = menuClip; audioSource.Play();

        PopupBg2.SetActive(true);
        EnterPartPopup.SetActive(true);

        enterPartPopupAnimator.speed = 2;
        enterPartPopupAnimator.SetBool("isSmallLargeScale2", true);
    }

    public void EventEnterPartPopupChallengeShow()
    {
        Debug.Log("EventEnterPartPopupChallenge");
        ev.EventOnRelease();

        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            EventEnterPartPopupChallenge();
        }
    }

    public void EventEnterPartPopupPurchaseShow()
    {
        Debug.Log("EventEnterPartPopupPurchaseShow");
        ev.EventOnRelease();

        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            EventEnterPartPopupPurchase();
        }
    }


    public void EventEnterPartPopupClose()
    {
        Debug.Log("EventEnterPartPopupClose");
        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            EnterPartPopup.SetActive(false);
            PopupBg2.SetActive(false);
        }
    }

    //----------------------------------------------------------------------
    //                  Show EnterPartPopup Challenge
    //----------------------------------------------------------------------
    public void EventEnterPartPopupChallenge()
    {
        Debug.Log("EventEnterPartPopupChallenge");

        audioSource.clip = menuClip; audioSource.Play();

        PopupBg4.SetActive(true);
        EnterPartChallengePopup.SetActive(true);

        enterPartChallengePopupAnimator.speed = 2;
        enterPartChallengePopupAnimator.SetBool("isSmallLargeScale2", true);
    }

    public void EventEnterPartPopupChallengeOk()
    {
        Debug.Log("EventEnterPartPopupChallengeOk");
        ev.EventOnRelease();

        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            mapControl.EventAchiveOnRelease();
            EventEnterPartPopupChallengeClose();
        }
    }

    public void EventEnterPartPopupChallengeClose()
    {
        Debug.Log("EventEnterPartPopupChallengeClose");
        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            EnterPartChallengePopup.SetActive(false);
            PopupBg4.SetActive(false);

            EnterPartPopup.SetActive(false);
            PopupBg2.SetActive(false);
        }
    }

    //----------------------------------------------------------------------
    //                  Show EnterPartPopup Purchase
    //----------------------------------------------------------------------
    public void EventEnterPartPopupPurchase()
    {
        Debug.Log("EventEnterPartPopupPurchase");

        audioSource.clip = menuClip; audioSource.Play();

        if (g.coin < g.enterPart) 
            enterPartPurchaseUILabel.text = "코인이 부족합니다.\n먼저 상점에서 코인을 구매하시기 바랍니다.";
        else enterPartPurchaseUILabel.text = "파트 아이템을 구매합니다.\n(Coin x35)";

        PopupBg4.SetActive(true);
        EnterPartPurchasePopup.SetActive(true);

        enterPartPurchasePopupAnimator.speed = 2;
        enterPartPurchasePopupAnimator.SetBool("isSmallLargeScale2", true);
    }

    public void EventEnterPartPopupPurchaseOk()
    {
        Debug.Log("EventEnterPartPopupPurchaseOk");
        ev.EventOnRelease();

        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();

            if (g.coin < g.enterPart) EventEnterPartPopupPurchaseClose();
            else {
                _data = dbo.SelUserStageHis(g.curStage); dr = _data.Rows[0];                
                g.partNum = int.Parse(dr["largeCode"].ToString());
                StartCoroutine(dbow.PurchasePartItem(g.partNum - 1));
                EventEnterPartPopupPurchaseClose();
            }
        }
    }

    public void EventEnterPartPopupPurchaseClose()
    {
        Debug.Log("EventEnterPartPopupPurchaseClose");
        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            EnterPartPurchasePopup.SetActive(false);
            PopupBg4.SetActive(false);

            EnterPartPopup.SetActive(false);
            PopupBg2.SetActive(false);
        }
    }


    //---------------------------------------------------------------
    //                         Option Popup
    //---------------------------------------------------------------
    public void EventShowOptionPopup()
    {
        Debug.Log("EventShowOptionPopup");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            g.isPopupOptionShow = true;
            PopupBg2.SetActive(true);
            optionAnimator.SetBool("isRightMiddleMove", true);
        }

    }
    public void EventCloseOptionPopup()
    {
        Debug.Log("EventCloseOptionPopup");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();

            optionAnimator.SetBool("isRightMiddleMove", false);
            optionAnimator.SetBool("isMiddleRightMove", true);

            StartCoroutine(EventCloseOptionPopupOnFinished());
        }
    }
    public IEnumerator EventCloseOptionPopupOnFinished()
    {
        yield return new WaitForSeconds(1.4f);
        PopupBg2.SetActive(false);
        optionAnimator.SetBool("isMiddleRightMove", false);
        g.isPopupOptionShow = false;
    }

               
    //---------------------------------------------------------------
    //                         Game Option Popup
    //---------------------------------------------------------------
    public void EventShowGameOptionPopup() {
        Debug.Log("EventShowGameOptionPopup");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) { 
            audioSource.clip = menuClip; audioSource.Play();

            PopupBg4.SetActive(true);
            gameOptionAnimator.gameObject.SetActive(true);
            gameOptionAnimator.SetBool("isSmallLargeScale", true);

        }
    }

    public UISprite soundUISprite,vibrationUISprite;
    public void EventSoundOnOffOnRelease() {
        Debug.Log("EventSoundOnOffOnRelease");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            audioSource.clip = menuClip; audioSource.Play();
            if(g.isSound == true) {
                g.isSound = false;
                audioSource.volume = 0f;
                soundUISprite.spriteName = "soundoff";
            } else {
                g.isSound = true;
                audioSource.volume = 0.400f;
                soundUISprite.spriteName = "soundon";
            }
        }
    }    

    public void EventVibrationOnOffOnRelease() {
        Debug.Log("EventVibrationOnOffOnRelease");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            audioSource.clip = menuClip; audioSource.Play();
            if(g.isVibrate == true) {
                g.isVibrate = false;
                vibrationUISprite.spriteName = "viboff";
            } else {
                g.isVibrate = true;
                vibrationUISprite.spriteName = "vibon";
            }
        }
    }    

    //-------------------------
    //  TTS
    //-------------------------
    //public GameObject TTSExplainPopup;
    //public Animator TTSOptionExplainAnimator;
    //public void EventTTSOptionExplainShow() {
    //    Debug.Log("EventShowGameOptionPopup");
    //    ev.EventOnRelease();
    //    if (ev.isCurPrevDist) { 
    //        audioSource.clip = menuClip; audioSource.Play();
    //        TTSExplainPopup.SetActive(true);
    //        PopupBg6.SetActive(true);

    //        TTSOptionExplainAnimator.speed = 2;
    //        TTSOptionExplainAnimator.SetBool("isSmallLargeScale2", true);
    //    }
    //}

    //public void EventTTSOptionExplainOK() {
    //    Debug.Log("EventTTSOptionExplainOK");
    //    ev.EventOnRelease();
    //    if (ev.isCurPrevDist) { 
    //        EventTTSOptionOnRelease(); 
    //        //EventTTSOptionExplainClose(); 
    //    }
    //}

    //public void EventTTSOptionExplainClose() {
    //    Debug.Log("EventTTSOptionExplainClose");
    //    ev.EventOnRelease();
    //    if (ev.isCurPrevDist) {
    //        audioSource.clip = menuClip; audioSource.Play();
    //        TTSExplainPopup.SetActive(false);
    //        PopupBg6.SetActive(false);
    //        TTSOptionExplainAnimator.SetBool("isSmallLargeScale2", false);

    //        TTSControl.instance.OnDestroy();
    //        TTSControl.instance.Init();
    //    }
    //}

    bool isTTSOption = false;
    public void EventTTSOptionOnRelease() {
        Debug.Log("EventTTSOptionOnRelease");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            audioSource.clip = menuClip; audioSource.Play();
            #if UNITY_ANDROID && !UNITY_EDITOR
                isTTSOption = true;
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject curActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
                curActivity.Call("SetupTTS");
            # endif
        }
    }


    public void EventCloseGameOptionPopup() {
        Debug.Log("EventCloseGameOptionPopup");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            audioSource.clip = menuClip; audioSource.Play();

            gameOptionAnimator.SetBool("isSmallLargeScale", false);
            gameOptionAnimator.gameObject.SetActive(false);
            PopupBg4.SetActive(false);

            #if UNITY_ANDROID && !UNITY_EDITOR
            if(isTTSOption) {
                TTSControl.instance.OnDestroy();
                TTSControl.instance.Init();
                isTTSOption = false;
            }
            # endif

        }
    }

    //---------------------------------------------------------------
    //                         Instruction Option Popup
    //---------------------------------------------------------------
    public void EventShowInstructionPopup() {
        Debug.Log("EventShowInstructionPopup");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            audioSource.clip = menuClip; audioSource.Play();
            Application.OpenURL ("http://crazyword.org/help.html");
        }
    }

    //---------------------------------------------------------------
    //                  TermsService Option Popup
    //---------------------------------------------------------------
    public void EventShowTermsServicePopup() {
        Debug.Log("EventShowTermsServicePopup");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            audioSource.clip = menuClip; audioSource.Play();
            Application.OpenURL ("http://crazyword.org/Service.html");

        }
    }

    //---------------------------------------------------------------
    //                   TechPrinciple Option Popup
    //---------------------------------------------------------------
    public void EventShowTechPrinciplePopup() {
        Debug.Log("EventShowInstructionPopup");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            audioSource.clip = menuClip; audioSource.Play();
            Application.OpenURL ("http://crazyword.org/Principles.html");

        }
    }


    //---------------------------------------------------------------
    //             Option Sub Popup - Background Editor
    //---------------------------------------------------------------
    int chkBgColIdx,chkBgImgIdx;
    public void EventShowOptionBackgroudEditorPopup()
    {
        optionSubBgPanel.SetActive(true);
        optionBackgroudEditor.SetActive(true);
        optionGameOption.SetActive(false);

        chkBgColIdx = g.userBgCol;
        chkBgImgIdx = g.userBgImg;

        backgroudEditorAnimator.SetBool("isSmallLargeScale", true);

        for (int i = 0; i < backgroudColorChkSprites.Length; i++)
        {
            if (g.userBgCol == i) backgroudColorChkSprites[i].enabled = true;
            else backgroudColorChkSprites[i].enabled = false;
        }

        for (int i = 0; i < backgroudImageChkSprites.Length; i++)
        {
            if (g.userBgImg == i) backgroudImageChkSprites[i].enabled = true;
            else backgroudImageChkSprites[i].enabled = false;
        }

    }


    public void ChkBackgroudColor(GameObject gameo)
    {
        Debug.Log("gameo.name.Substring(7, 2)=" + gameo.name.Substring(7, 2));
        chkBgColIdx = int.Parse(gameo.name.Substring(7, 2)) - 1;

        //----------------------
        // Init
        //----------------------
        for (int i = 0; i < backgroudColorChkSprites.Length; i++)
        {
            if (chkBgColIdx == i) backgroudColorChkSprites[i].enabled = true;
            else backgroudColorChkSprites[i].enabled = false;
        }
    }

    public void ChkBackgroudImage(GameObject gameo)
    {
        Debug.Log("gameo.name.Substring(7, 2)=" + gameo.name.Substring(7, 2));
        chkBgImgIdx = int.Parse(gameo.name.Substring(7, 2)) - 1;

        for (int i = 0; i < backgroudImageChkSprites.Length; i++)
        {
            if (chkBgImgIdx == i) backgroudImageChkSprites[i].enabled = true;
            else backgroudImageChkSprites[i].enabled = false;
        }

        // Android Image Gellary Open Here..
        if (chkBgImgIdx == 5)
        {

# if UNITY_ANDROID && !UNITY_EDITOR 
            ImageBrowser.OnPicked += (image) =>
            {
                // Set Image.
                StartCoroutine(LoadTexture(image));
            };

            ImageBrowser.Pick();
# endif
        }
    }

    IEnumerator LoadTexture(Image image)
    {
        loading.SetActive(true);
        yield return StartCoroutine(image.LoadTexture());

        userBgImgUITexture.mainTexture = (Texture)image.Texture;
        g.userBgImgTextures[5] = (Texture2D)image.Texture;
        byte[] by = g.userBgImgTextures[5].EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/Textures/bg/55.png", by); // 64MB limit on File.WriteAllBytes.
        loading.SetActive(false);

    }

    public void EventExitBackgroudEditor()
    {
        Debug.Log("EventExitBackgroudEditor");
        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            //if (chkBgColIdx != g.userBgCol)
            //{
            //    Debug.Log("chkBgColIdx");
            //    g.userBgCol = chkBgColIdx;
            //    StartCoroutine(dbow.UptUserInfoUserBgCol());
            //}

            //if (chkBgImgIdx != g.userBgImg)
            //{
            //    Debug.Log("chkBgImgIdx");
            //    g.userBgImg = chkBgImgIdx;
            //    StartCoroutine(dbow.UptUserInfoUserBgImg());
            //}

            g.userBgCol = chkBgColIdx;
            dbo.UptUserInfoUserBgCol();
            mapControl.mapBgCol();

            g.userBgImg = chkBgImgIdx;
            dbo.UptUserInfoUserBgImg();
            mapControl.mapSceneImg.mainTexture = g.userBgImgTextures[g.userBgImg];

            optionSubBgPanel.SetActive(false);
            optionBackgroudEditor.SetActive(false);
            optionGameOption.SetActive(false);

            //backgroudEditorAnimator
            //backgroudEditorAnimator.SetBool("isSmallLargeScale", false);

        }
    }

    //---------------------------------------------------------------
    //             Option Sub Popup - No Banner
    //---------------------------------------------------------------
    public void EventShowNoBannerPopup()
    {
        audioSource.clip = menuClip; audioSource.Play();

        PopupBg6.SetActive(true);
        NoBannerPopup.SetActive(true);

        if (g.coin < g.nobannerCoin) noBannerUILabel.text = "광고를 제거합니다.(Coin x100)\n코인이 부족합니다.\n상점에서 구매하시기 바랍니다.";
        else noBannerUILabel.text = "광고를 제거합니다(Coin x100)";


        noBannerPopupAnimator.speed = 2;
        noBannerPopupAnimator.SetBool("isSmallLargeScale2", true);

    }

    public void EventNoBannerPopupOk()
    {
        Debug.Log("EventNoBannerPopupOk");
        ev.EventOnRelease();

        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();

            if (g.coin < 100)
            {
                EventNoBannerPopupClose();
            }
            else
            {
                StartCoroutine(dbow.PurchaseTran(g.wwwDate, "nobanner"));
            }
        }
    }

    public void EventNoBannerPopupClose()
    {
        Debug.Log("EventBuyItemPopupClose");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            audioSource.clip = menuClip; audioSource.Play();
            NoBannerPopup.SetActive(false);
            PopupBg6.SetActive(false);
        }
    }

	void Update () {	
		if(Input.GetKeyDown(KeyCode.Escape)) {
            //if(g.isPopupPartItemShow) EventPartItemPopupClose2();
        }
	}

}

