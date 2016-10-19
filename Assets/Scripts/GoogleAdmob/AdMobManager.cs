using UnityEngine;
using System.Collections;
using Prime31;


public class AdMobManager : MonoBehaviour {

    private string bannerID = "ca-app-pub-4688127500958485/5475373457";
    private string frontID = "ca-app-pub-4688127500958485/8428839856";
        

	// Use this for initialization
	void Start () {

#if UNITY_ANDROID

        // 하단 광고 배너 생성.
        if(g.noBannerYn == "N")
            AdMobAndroid.createBanner(bannerID, AdMobAndroidAd.phone320x50, AdMobAdPlacement.BottomCenter);


        // 하단 광고 감추기 (생성이 먼저 되어 있어야 가능합니다).
        //AdMobAndroid.hideBanner( true );


        // 하단 광고 다시 보여주기(생성이 먼저 되어 있어야 가능합니다).
        //AdMobAndroid.hideBanner( false );

        // 하단 광고 삭제.
        // AdMobAndroid.destroyBanner();


        // 하단 광고 세로 고침.
        //AdMobAndroid.refreshAd();





        // 전면 광고 요청.
        AdMobAndroid.requestInterstital(frontID);


        // 전면 광고 요청에 대한 콜백. (true 일때만 전면 광고 가능합니다).
        //var isReady = AdMobAndroid.isInterstitalReady();
        //Debug.Log("is interstitial ready? " + isReady);


        // 전면 광고 생성 (닫기 버튼은 광고에 붙어 있습니다).
        //AdMobAndroid.displayInterstital();


#endif

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetFrontAdmob()
    {
        // 전면 광고 요청이 들어갔으면 Show.
        #if UNITY_ANDROID && !UNITY_EDITOR

            if (AdMobAndroid.isInterstitalReady())
            {
                AdMobAndroid.displayInterstital();
            }

            // 전면 광고 요청이 없으면 다시 요청.
            else
            {
                AdMobAndroid.requestInterstital(frontID);
            }
        # endif
    }
}
