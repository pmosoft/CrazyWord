using UnityEngine;
using System.Collections;
using SimpleJSON;

public class PopupShopControl : MonoBehaviour
{

    //--------------------------
    // PopupBg
    //--------------------------
    public int shopPopupDepth = 3;

    //--------------------------
    // Map Component
    //--------------------------
    public UILabel mapNoBannerUILabel;

    //---------------------------------------------------------
    // Sound
    //---------------------------------------------------------
    AudioClip optionClip, menuClip; AudioSource audioSource;

    //---------------------------------------------------------
    // Common
    //---------------------------------------------------------
    public DBO dbo; public DBOW dbow; public EventCommon ev;
    public FacebookLogin facebookLogin;
    public FacebookManager facebookManager;

    //--------------------------
    // Connect Object
    //--------------------------
    GameObject PopupBg2, PopupBg4, PopupBg6;
    GameObject ShopFbSharePopup, ShopAllPackPopup, CoinWarningPopup;

    Transform PopupBg, PopupShop, ShopPopupPanel;
    Transform ShopCoinGrid, ShopItemGrid, ShopSubPopupPanel, CoinWarningPopupPanel;
    Transform coin100, coin300;


    UIPanel shopPopupPanelUIPanel, shopSubPopupPanelUIPanel,coinWarningPopupPanelUIPanel;

    Animator shopPopupAnimator, shopFbSharePopupAnimator, shopAllPackPopupAnimator;
    Animator coinWarningPopupAnimator;

    UIEventTrigger shopBackButtonEventTrigger;
    UIEventTrigger fbShareEventTrigger, fbShareOkEventTrigger, fbShareXEventTrigger;
    UIEventTrigger coin100EventTrigger, coin300EventTrigger;
    UIEventTrigger shopSkipEventTrigger, shopHpEventTrigger;
    UIEventTrigger allpackEventTrigger, allPackOkEventTrigger, allPackXEventTrigger;
    UIEventTrigger coinWarningEventTrigger, coinWarningOkEventTrigger, coinWarningXEventTrigger;

    UILabel myCoinUILabel, coinWarningUILabel, fbShareCoinDate;
    UISprite allPackBg, fbShareBg;
    BoxCollider allPackBoxCollider, fbShareBoxCollider;


    void Awake()
    {
        PopupBg = Camera.main.transform.FindChild("PopupBg");
        PopupShop = Camera.main.transform.FindChild("PopupShop");
        ShopPopupPanel = PopupShop.FindChild("3_ShopPopupPanel");
        ShopSubPopupPanel = PopupShop.FindChild("5_ShopSubPopupPanel");
        CoinWarningPopupPanel = PopupShop.FindChild("3_CoinWarningPanel");

        ShopCoinGrid = ShopPopupPanel.FindChild("1_ShopPopup").FindChild("2_ShopCoinGridBg").FindChild("ShopCoinGrid");
        ShopItemGrid = ShopPopupPanel.FindChild("1_ShopPopup").FindChild("2_ShopItemGridBg").FindChild("ShopItemGrid");
        coin100 = ShopCoinGrid.FindChild("coin100");
        coin300 = ShopCoinGrid.FindChild("coin300");

        //----------------
        // PopupBg
        //----------------
        PopupBg2 = PopupBg.FindChild("2_PopupBgPanel").gameObject;
        PopupBg4 = PopupBg.FindChild("4_PopupBgPanel").gameObject;
        PopupBg6 = PopupBg.FindChild("6_PopupBgPanel").gameObject;

        //------------------
        // shopPopup
        //------------------
        shopPopupPanelUIPanel = ShopPopupPanel.GetComponent<UIPanel>();
        shopPopupAnimator = ShopPopupPanel.FindChild("1_ShopPopup").GetComponent<Animator>();
        myCoinUILabel = ShopPopupPanel.FindChild("1_ShopPopup").FindChild("2_MyCoin").FindChild("4_MyCoin").GetComponent<UILabel>();

        //------------------
        // shopPopup Event
        //------------------

        // BackButton
        shopBackButtonEventTrigger = ShopPopupPanel.FindChild("1_ShopPopup").FindChild("2_ShopBackButton").GetComponent<UIEventTrigger>();
        EventDelegate.Add(shopBackButtonEventTrigger.onPress, ev.EventOnPress);
        EventDelegate.Add(shopBackButtonEventTrigger.onRelease, EventCloseShopPopup);

        // FbShare
        fbShareEventTrigger = ShopCoinGrid.FindChild("FbShare").GetComponent<UIEventTrigger>();
        EventDelegate.Add(fbShareEventTrigger.onPress, ev.EventOnPress);
        EventDelegate.Add(fbShareEventTrigger.onRelease, EventShopFbShare);

        fbShareOkEventTrigger = ShopSubPopupPanel.FindChild("1_ShopFbSharePopup").FindChild("2_FbShareOk").GetComponent<UIEventTrigger>();
        EventDelegate.Add(fbShareOkEventTrigger.onPress, ev.EventOnPress);
        EventDelegate.Add(fbShareOkEventTrigger.onRelease, EventShopFbShareOK);

        fbShareXEventTrigger = ShopSubPopupPanel.FindChild("1_ShopFbSharePopup").FindChild("2_FbShareX").GetComponent<UIEventTrigger>();
        EventDelegate.Add(fbShareXEventTrigger.onPress, ev.EventOnPress);
        EventDelegate.Add(fbShareXEventTrigger.onRelease, EventShopFbShareClose);

        fbShareCoinDate = ShopCoinGrid.FindChild("FbShare").FindChild("4_Date").GetComponent<UILabel>();
        fbShareBg = ShopCoinGrid.FindChild("FbShare").FindChild("3_Bg").GetComponent<UISprite>();
        fbShareBoxCollider = ShopCoinGrid.FindChild("FbShare").GetComponent<BoxCollider>();

        //coin
        coin100EventTrigger = coin100.GetComponent<UIEventTrigger>();
        EventDelegate eventClick = new EventDelegate(this, "EventShopCoinItem");
        EventDelegate.Parameter param = new EventDelegate.Parameter();
        param.obj = coin100;
        param.expectedType = typeof(Transform);
        eventClick.parameters[0] = param;
        EventDelegate.Add(coin100EventTrigger.onPress, ev.EventOnPress);
        EventDelegate.Add(coin100EventTrigger.onRelease, eventClick);

        coin300EventTrigger = coin300.GetComponent<UIEventTrigger>();
        eventClick = new EventDelegate(this, "EventShopCoinItem");
        param = new EventDelegate.Parameter();
        param.obj = coin300;
        param.expectedType = typeof(Transform);
        eventClick.parameters[0] = param;
        EventDelegate.Add(coin300EventTrigger.onPress, ev.EventOnPress);
        EventDelegate.Add(coin300EventTrigger.onRelease, eventClick);


        // Skip,Hp
        shopSkipEventTrigger = ShopItemGrid.FindChild("ShopSkip").GetComponent<UIEventTrigger>();
        EventDelegate.Add(shopSkipEventTrigger.onPress, ev.EventOnPress);
        EventDelegate.Add(shopSkipEventTrigger.onRelease, EventPurchaseSkip);

        shopHpEventTrigger = ShopItemGrid.FindChild("ShopHp").GetComponent<UIEventTrigger>();
        EventDelegate.Add(shopHpEventTrigger.onPress, ev.EventOnPress);
        EventDelegate.Add(shopHpEventTrigger.onRelease, EventPurchaseHp);


        // allpack
        allpackEventTrigger = ShopItemGrid.FindChild("Allpack").GetComponent<UIEventTrigger>();
        EventDelegate.Add(allpackEventTrigger.onPress, ev.EventOnPress);
        EventDelegate.Add(allpackEventTrigger.onRelease, EventShopAllPack);

        allPackOkEventTrigger = ShopSubPopupPanel.FindChild("1_ShopAllPackPopup").FindChild("2_AllPackOk").GetComponent<UIEventTrigger>();
        EventDelegate.Add(allPackOkEventTrigger.onPress, ev.EventOnPress);
        EventDelegate.Add(allPackOkEventTrigger.onRelease, EventShopAllPackOK);

        allPackXEventTrigger = ShopSubPopupPanel.FindChild("1_ShopAllPackPopup").FindChild("2_AllPackX").GetComponent<UIEventTrigger>();
        EventDelegate.Add(allPackXEventTrigger.onPress, ev.EventOnPress);
        EventDelegate.Add(allPackXEventTrigger.onRelease, EventShopAllPackClose);

        allPackBg = ShopItemGrid.FindChild("Allpack").FindChild("3_Bg").GetComponent<UISprite>();
        allPackBoxCollider = ShopItemGrid.FindChild("Allpack").GetComponent<BoxCollider>();

        //-------------------
        // shopSub
        //-------------------
        ShopFbSharePopup = ShopSubPopupPanel.FindChild("1_ShopFbSharePopup").gameObject;
        ShopAllPackPopup = ShopSubPopupPanel.FindChild("1_ShopAllPackPopup").gameObject;
        shopFbSharePopupAnimator = ShopFbSharePopup.GetComponent<Animator>();
        shopAllPackPopupAnimator = ShopAllPackPopup.GetComponent<Animator>();

        shopSubPopupPanelUIPanel = ShopSubPopupPanel.GetComponent<UIPanel>();

        //-------------------------
        // shopSub - CoinWarning
        //-------------------------
        CoinWarningPopup = CoinWarningPopupPanel.FindChild("1_CoinWarningPopup").gameObject;
        coinWarningPopupPanelUIPanel = CoinWarningPopupPanel.GetComponent<UIPanel>();

        coinWarningPopupAnimator = CoinWarningPopup.GetComponent<Animator>();
        coinWarningUILabel = CoinWarningPopup.transform.FindChild("2_CoinWarning").GetComponent<UILabel>();

        coinWarningEventTrigger = ShopItemGrid.FindChild("ShopSkip").GetComponent<UIEventTrigger>();
        EventDelegate.Add(coinWarningEventTrigger.onPress, ev.EventOnPress);
        EventDelegate.Add(coinWarningEventTrigger.onRelease, EventPurchaseSkip);

        coinWarningOkEventTrigger = CoinWarningPopup.transform.FindChild("2_CoinWarningOk").GetComponent<UIEventTrigger>();
        EventDelegate.Add(coinWarningOkEventTrigger.onPress, ev.EventOnPress);
        EventDelegate.Add(coinWarningOkEventTrigger.onRelease, EventCoinWarningOK);

        coinWarningXEventTrigger = CoinWarningPopup.transform.FindChild("2_CoinWarningX").GetComponent<UIEventTrigger>();
        EventDelegate.Add(coinWarningXEventTrigger.onPress, ev.EventOnPress);
        EventDelegate.Add(coinWarningXEventTrigger.onRelease, EventCoinWarningClose);

        //-------------------
        // sound
        //-------------------
        optionClip = Resources.Load("Sounds/option") as AudioClip;
        menuClip = Resources.Load("Sounds/menuClip") as AudioClip;
        audioSource = Camera.main.transform.GetComponent<AudioSource>();


    }

    public void SetActivePopupBg4(bool tf) { PopupBg4.SetActive(tf); }
    public void SetActivePopupBg6(bool tf) { PopupBg6.SetActive(tf); }
    public void SetActiveShopFbSharePopup(bool tf) { ShopFbSharePopup.SetActive(tf); }
    public void SetActiveShopAllPackPopup(bool tf) { ShopAllPackPopup.SetActive(tf); }
    public void SetActiveCoinWarningPopup(bool tf) { CoinWarningPopup.SetActive(tf); }
    public void SetMyCoinUILabel(string txt) { myCoinUILabel.text = txt; }

    //---------------------------------------------------------------
    //                          Shop Popup
    //---------------------------------------------------------------



    public void EventShowShopPopup()
    {
        Debug.Log("EventShowShopPopup");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            shopPopupPanelUIPanel.depth = 3;
            g.isPopupShopShow = true;

            myCoinUILabel.text = g.coin.ToString();
            PopupBg2.SetActive(true);

            shopPopupAnimator.SetBool("isLeftMiddleMove", true);
        }
    }
    public void EventCloseShopPopup() {
        Debug.Log("EventCloseShopPopup");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            shopPopupAnimator.SetBool("isLeftMiddleMove", false);
            shopPopupAnimator.SetBool("isMiddleLeftMove", true);
            StartCoroutine(EventCloseShopPopupOnFinished());
        }
    }
    public void EventCloseShopPopup2() {
        Debug.Log("EventCloseShopPopup2");
        audioSource.clip = menuClip; audioSource.Play();
        shopPopupAnimator.SetBool("isLeftMiddleMove", false);
        shopPopupAnimator.SetBool("isMiddleLeftMove", true);
        StartCoroutine(EventCloseShopPopupOnFinished());
    }

    public IEnumerator EventCloseShopPopupOnFinished()
    {
        yield return new WaitForSeconds(1.4f);

        PopupBg2.SetActive(false);
        shopPopupAnimator.SetBool("isMiddleLeftMove", false);
        g.isPopupShopShow = false;
    }

    //----------------------------------------------------------------------
    //                          ShopFbShareCoin
    //----------------------------------------------------------------------
    public void EventShopFbShare()
    {
        Debug.Log("EventShopFbShare");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();

            ShopFbSharePopup.SetActive(true);
            PopupBg4.SetActive(true);

            shopFbSharePopupAnimator.speed = 2;
            shopFbSharePopupAnimator.SetBool("isSmallLargeScale2", true);
        }
    }

    public void EventShopFbShareOK()
    {
        Debug.Log("EventShopFbShareOK");
        ev.EventOnRelease();

        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            g.isShopFbShare = true;
            //-----------------------------
            //here,facebook sharing
            //-----------------------------
            #if     UNITY_EDITOR
                if(FB.IsLoggedIn == false) facebookLogin.StartInfo();
                    else facebookManager.FacebookFeed();
                //StartCoroutine(dbow.PurchaseTran(g.wwwDate, "fbShare"));
            #elif    UNITY_ANDROID
                if(FB.IsLoggedIn == false) facebookLogin.StartFacebookLogin();
                else facebookManager.FacebookFeed();
            #endif
           
        }

    }

    public void EventShopFbShareClose()
    {
        Debug.Log("EventShopFbShareClose");
        //ev.EventOnRelease();
        //if (ev.isCurPrevDist)
        //{
            audioSource.clip = menuClip; audioSource.Play();

            ShopFbSharePopup.SetActive(false);
            PopupBg4.SetActive(false);
            g.isShopFbShare = false;
            shopFbSharePopupAnimator.SetBool("isSmallLargeScale2", false);
        //}
    }


    //----------------------------------------------------------------------
    //                          Shop CoinItem
    //----------------------------------------------------------------------

    public void EventShopCoinItem(Transform Coin)
    {
        Debug.Log("EventShopCoinItem");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = optionClip; audioSource.Play();
            //Debug.Log("Coin.name=" + Coin.name);
            //Debug.Log("Coin.name=" + int.Parse(Coin.name.Substring(4, 1)));

            #if UNITY_EDITOR
                Debug.Log("Unity Editor");
                //StartCoroutine(dbow.InsUserPurchaseHis(dbo.GetUniqueId(), Coin.name));
                StartCoroutine(dbow.PurchaseTran(dbo.GetUniqueId(), Coin.name));

            #elif    UNITY_ANDROID

                //---------------------------------------------------
                // imsi comments before open
                PurchaseManager.Instance.StartPurchase(Coin.name);
                StartCoroutine ( PurchaseOnFinished() );
                //---------------------------------------------------
                //StartCoroutine(dbow.PurchaseTran(dbo.GetUniqueId(), Coin.name));

            #endif

        }

    }

    //----------------------------------------------------------------------
    //                            Shop Skip
    //----------------------------------------------------------------------
    public void EventPurchaseSkip()
    {
        Debug.Log("EventPurchaseSkip");

        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            audioSource.clip = menuClip; audioSource.Play();
            shopPopupDepth = 5;
            g.isPopupPurchaseShow = true;

            if (g.coin < g.skipCoin) {
                isShopSkip = false; isShopHp = false; isShopAllPack = false;
                coinWarningUILabel.text = "코인이 부족합니다.\n먼저 코인을 구매하시기\n바랍니다.";
                EventCoinWarning();
            } else {
                isShopSkip = true; isShopHp = false; isShopAllPack = false;
                coinWarningUILabel.text = "Skip 1개를 구입합니다.\n4코인이 차감됩니다.";
                EventCoinWarning();
            }
        }
    }

    //----------------------------------------------------------------------
    //                            Shop Hp
    //----------------------------------------------------------------------
    public void EventPurchaseHp()
    {
        Debug.Log("EventPurchaseHp");

        ev.EventOnRelease();
        if (ev.isCurPrevDist) {
            audioSource.clip = menuClip; audioSource.Play();
            shopPopupDepth = 5;

            g.isPopupPurchaseShow = true;

            if (g.coin < g.hpCoin) {
                isShopSkip = false; isShopHp = false; isShopAllPack = false;
                coinWarningUILabel.text = "코인이 부족합니다.\n먼저 코인을 구매하시기\n바랍니다.";
                EventCoinWarning();
            } else {
                isShopSkip = false; isShopHp = true; isShopAllPack = false;
                coinWarningUILabel.text = "Hp 1개를 구입합니다.\n2코인이 차감됩니다.";
                EventCoinWarning();
            }
        }
    }


    //----------------------------------------------------------------------
    //                          ShopAllPackItem
    //----------------------------------------------------------------------
    public void EventShopAllPack()
    {
        Debug.Log("EventShopAllPack");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();
            ShopAllPackPopup.SetActive(true);
            g.isPopupPurchaseShow = true;

            PopupBg4.SetActive(true);

            shopAllPackPopupAnimator.speed = 2;
            shopAllPackPopupAnimator.SetBool("isSmallLargeScale2", true);
        }
    }

    public void EventShopAllPackOK()
    {
        Debug.Log("EventShopAllPackOK");
        ev.EventOnRelease();

        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();

            if (g.coin < g.allpackCoin)
            {
                isShopSkip = false; isShopHp = false; isShopAllPack = false;
                coinWarningUILabel.text = "코인이 부족합니다.\n먼저 코인을 구매하시기\n바랍니다.";

                shopPopupDepth = 7;
                EventCoinWarning();
            }
            else
            {
                StartCoroutine(dbow.PurchaseTran(g.wwwDate, "allpack"));
            }
        }
    }

    public void EventShopAllPackClose()
    {
        Debug.Log("EventBuyItemPopupClose");
        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();

            ShopAllPackPopup.SetActive(false);
            PopupBg4.SetActive(false);

            shopAllPackPopupAnimator.SetBool("isSmallLargeScale2", false);
        }
        g.isPopupPurchaseShow = false;
    }


    //----------------------------------------------------------------------
    //                         ShopSub CoinWarning
    //----------------------------------------------------------------------
    bool isShopSkip, isShopHp, isShopAllPack;
    public void EventCoinWarning()
    {
        Debug.Log("EventCoinWarning");

        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();

            CoinWarningPopup.SetActive(true);

            if (shopPopupDepth == 3)
            {
                PopupBg2.SetActive(true);
                coinWarningPopupPanelUIPanel.depth = 3;
            }
            else if (shopPopupDepth == 5)
            {
                PopupBg4.SetActive(true);
                coinWarningPopupPanelUIPanel.depth = 5;
            }
            else if (shopPopupDepth == 7)
            {
                PopupBg6.SetActive(true);
                coinWarningPopupPanelUIPanel.depth = 7;
            }

            coinWarningPopupAnimator.speed = 2;
            coinWarningPopupAnimator.SetBool("isSmallLargeScale2", true);
        }
    }
    public void EventCoinWarningOK()
    {
        Debug.Log("EventCoinWarningOK");
        ev.EventOnRelease();

        if (ev.isCurPrevDist)
        {
            if (isShopSkip) {
                StartCoroutine(dbow.PurchaseTran(g.wwwDate, "skip1"));
            } else if(isShopHp) {
                StartCoroutine(dbow.PurchaseTran(g.wwwDate, "hp1"));
            }
            else if (isShopAllPack)
            {
                StartCoroutine(dbow.PurchaseTran(g.wwwDate, "allpack"));
            }
            else {
                EventCoinWarningClose();
            }
        }
    }

    public void EventCoinWarningClose()
    {
        Debug.Log("EventCoinWarningClose");
        ev.EventOnRelease();
        if (ev.isCurPrevDist)
        {
            audioSource.clip = menuClip; audioSource.Play();

            CoinWarningPopup.SetActive(false);

            if (shopPopupDepth == 3) PopupBg2.SetActive(false);
            else if (shopPopupDepth == 5) PopupBg4.SetActive(false);
            else if (shopPopupDepth == 7) PopupBg6.SetActive(false);

            coinWarningPopupAnimator.SetBool("isSmallLargeScale2", false);
        }
        g.isPopupPurchaseShow = false;
    }

    //---------------------------------------
    // PurchaseOnFinished
    //---------------------------------------
    IEnumerator PurchaseOnFinished()
    {

        Debug.Log("PurchaseOnFinished start");
        while (!g.isPurchaseSucceed) yield return new WaitForSeconds(0.1f);

        StartCoroutine(dbow.PurchaseTran(g.purchaseOrderId, g.purchaseProductId));

        g.isPurchaseSucceed = false;
        Debug.Log("PurchaseOnFinished end");

    }


    //-------------------------------------------
    //           PurchaseWarning Popup
    //-------------------------------------------
    bool isPurchaseWarning = false;
    public GameObject PopupBg8;
    public GameObject PurchaseWarning;
    public Animator purchaseWarningAnimator;
 
    public void EventPurchaseWarning() {
        Debug.Log("EventPurchaseWarning");
        PurchaseWarning.SetActive(true);
        PopupBg8.SetActive(true);
        isPurchaseWarning = true;

        purchaseWarningAnimator.speed = 2;
        purchaseWarningAnimator.SetBool("isSmallLargeScale2", true);
    }
    public void EventPurchaseWarningOK() {
        Debug.Log("EventPurchaseWarningOK");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) EventPurchaseWarningClose();
    }
    public void EventPurchaseWarningClose() {
        Debug.Log("EventPurchaseWarningClose");
        ev.EventOnRelease();
        if (ev.isCurPrevDist) {

            PurchaseWarning.SetActive(false);
            PopupBg8.SetActive(false);
            purchaseWarningAnimator.SetBool("isSmallLargeScale2", false);
            isPurchaseWarning = false;
        }
    }

    //---------------------------------------
    // Util
    //---------------------------------------
    public void SetAllPack()
    {
        //Debug.Log("SetAllPack");
        //Debug.Log("g.noBannerYn="+g.noBannerYn);
        //Debug.Log("g.userPicYn="+g.userPicYn);

        if (g.partMove == "111111111" && g.noBannerYn == "Y" && g.userPicYn == "Y")
        {
            allPackBg.color = g.inActiveCol;
            allPackBoxCollider.enabled = false;
        }
    }

    public void SetFbShareCoin()
    {
        //Debug.Log("SetFbShareCoin");

        //Debug.Log("g.wwwDate=" + g.wwwDate);
        g.wwwDate = System.DateTime.Now.ToString("yyyyMMdd");
        //Debug.Log("g.wwwDate=" + g.wwwDate);
        fbShareCoinDate.text = g.wwwDate.Substring(4, 2) + "/" + g.wwwDate.Substring(6, 2);

        if (g.fbShareDate == g.wwwDate)
        {
            fbShareBg.color = g.inActiveCol;
            fbShareBoxCollider.enabled = false;
        }
    }

	void Update () {	
		if(Input.GetKeyDown(KeyCode.Escape)) {
            //if(g.isPopupShopShow && !g.isPopupPurchaseShow) EventCloseShopPopup2();
        }
	}

}
