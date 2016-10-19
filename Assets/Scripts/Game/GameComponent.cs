using UnityEngine;
using System.Collections;

public class GameComponent : MonoBehaviour {

	public GameState gameState;


    public UITexture gameSceneImgUITexture;


	//---------------------------------------------------------
	// PopupPanel
	//---------------------------------------------------------
    public TweenPosition achieveTweenPosition;

	//---------------------------------------------------------
	// BoardPanel
	//---------------------------------------------------------
	public UILabel limitUILabel,limitLineUILabel;
	public UILabel bestScoreUILabel,bestScoreLineUILabel;
	public UILabel scoreNumUILabel,scoreNumLineUILabel;
	public UILabel clickWordUILabel;

	public UILabel wordCntUILabel;
	public UISprite wordCntUISprite;

	public Animator backSpaceAnimator;

	public Animator comboGameAnimator;
	public Animator hardGameAnimator;
	public Animator easyGameAnimator;

	public Animator levelSkipAnimator;

	public UISprite levelSkipUISprite;
	public UILabel levelSkipUILabel;

	public BoxCollider backSpaceBoxCollider;
	public BoxCollider homeBoxCollider;
	public BoxCollider mapBoxCollider;


	public BoxCollider comboGameBoxCollider;
	public BoxCollider hardGameBoxCollider;
	public BoxCollider easyGameBoxCollider;
	public BoxCollider levelSkipBoxCollider;

	//---------------------------------------------------------
	// TexturePanel
	//---------------------------------------------------------
	public UILabel bonusRuleTouchLabel;
	public UILabel bonusRuleCountLabel;
	public UILabel bonusRuleTimeLabel;
	public UILabel bonusRuleReadyLabel;

	public UISprite pictureFrameUISprite;

	// GameTexture
	public UISprite  gameTextureUISprite;
	public UITexture gameUITexture1;
	public UITexture gameUITexture2;
	public UITexture gameUITexture3;
	public UITexture gameUITexture4;

	public UISprite[] photoLicenseSprites = new UISprite[4];
	public UILabel[] photoOwnerLabels = new UILabel[4];

	// GameComboClear
	public UILabel comboClearNumUILabel;
	
	//---------------------------------------------------------
	// TimeSliderPanel
	//---------------------------------------------------------
	public UISprite EndTimeSliderSprite;
	public TweenScale EndTimeSliderTweenScale;
	public UISlider TimeUISlider;
	public UISprite StartTimeSliderSprite;

	//---------------------------------------------------------
	// GamePanel
	//---------------------------------------------------------
	public UILabel comboNumUILabel;
	public UILabel comboNumLineUILabel;

	// ShowWord
	public UILabel showWordUILabel; 
	public TweenScale showWordScale;
	public TweenAlpha showWordAlpha;

	public TweenAlpha gameInterludeTweenAlpha;
	public UISprite gameInterludeUISprite;
	public Animator gameInterludeAnimator;

	//---------------
	// LetterPrefab
	//---------------
	public Transform letterPrefab;
	public Transform letterLabel;
	public Transform letterEffectLabel;

	public UILabel letterUILabel;
	public TweenScale letterScale;
	public Animator letterAnimator;

	public TweenPosition letterPosition;

	public UILabel letterEffectUILabel;
	public TweenScale letterEffectScale;
	public TweenAlpha letterEffectAlpha;
	public UIEventTrigger letterUIEventTrigger;

	//---------------
	// arrowPrefab
	//---------------
	public UIEventTrigger arrowUIEventTrigger;
	public UISprite arrowUISprite;
	public BoxCollider arrowBoxcollider;

	public UIEventTrigger bowUIEventTrigger;
	public UISprite bowUISprite;
	public BoxCollider bowBoxcollider;

	public UIEventTrigger flowerUIEventTrigger;
	public UISprite flowerUISprite;
	public BoxCollider flowerBoxcollider;

	public UISprite flowerSweatUISprite;
	public Animator flowerSweatAnimator;
	public Animator flowerAnimator;

	//---------------------------------------------------------
	// GameMolePanel
	//---------------------------------------------------------
	public TweenScale moleInCorrectScale;

	//---------------------------------------------------------
	// GameBonusRulePanel
	//---------------------------------------------------------

	//---------------------------------------------------------
	// GameBonusPanel
	//---------------------------------------------------------
	public UILabel bonusEndUILabel;
	public TweenScale bonusEndScale;

	//---------------------------------------------------------
	// BottomPanel
	//---------------------------------------------------------
	public UISprite gameIconUISprite;

	public UILabel skipNumUILabel;
	public UILabel totalSkipNumUILabel;
	
	public UISprite hpTexUISprite;
	public UILabel hpNumUILabel;
	public UILabel hpNumLineUILabel;
	
	public UISprite gameSceneBgUITexture;
	public UISprite soundUISprite;
    
	public UILabel curStarNumUILabel;
	public UILabel curStarNumLineUILabel;
	public UILabel totalStarNumUILabel;
	public UILabel totalStarNumLineUILabel;

	public BoxCollider skipBoxCollider;
	public BoxCollider mapBottomBoxCollider;
	public BoxCollider soundBottonBoxCollider;

    public UIScrollBar rankUIScrollBar;
    public UISprite rankBarForeBg0UISprite,rankBarForeBgUISprite,rankBarStar1,rankBarStar2,rankBarStar3,rankBarStar4,rankBarStar5;

	//---------------------------------------------------------
	// BannerPanel
	//---------------------------------------------------------
    public UISprite bannerUISprite;
    public UILabel bannerHintUILabel;
	
	//---------------------------------------------------------
	// AccountPanel
	//---------------------------------------------------------
	public UILabel accountBestScoreNum;

	public UILabel accountLessonNum;
	public UILabel accountScoreNum;
	public UILabel accountComboBonus;
	public UILabel accountBonusNum;
	public UILabel accountTotalScoreNum;

	public UISprite accountRankStar1;
	public UISprite accountRankStar2;
	public UISprite accountRankStar3;
	public UISprite accountRankStar4;
	public UISprite accountRankStar5;

	public UISprite[] accountRankStar = new UISprite[5];

	public BoxCollider accountMapNaviBoxCollider;
	public BoxCollider accountReturnNaviBoxCollider;

	public Animator accountBestScoreUpAnimator;
    public Animator accountRankUpAnimator;

	//---------------------------------------------------------
	// DownloadPanel
	//---------------------------------------------------------
	public UILabel tipTextUILael,tipTextNaviUILael;


	void Awake () {
		

		accountRankStar[0] = accountRankStar1;
		accountRankStar[1] = accountRankStar2;
		accountRankStar[2] = accountRankStar3;
		accountRankStar[3] = accountRankStar4;
		accountRankStar[4] = accountRankStar5;

	}

	public void SetLetterPrefab(Transform letterPrefab){

		//Debug.Log("letterPrefab.name="+letterPrefab.name);
		letterLabel = letterPrefab.FindChild("LetterLabel");
		letterEffectLabel = letterPrefab.FindChild("LetterEffectLabel");

		letterUIEventTrigger = letterPrefab.GetComponent<UIEventTrigger>();
		letterPosition = letterPrefab.GetComponent<TweenPosition>();

		letterUILabel = letterLabel.GetComponent<UILabel>();
		letterScale = letterLabel.GetComponent<TweenScale>();
		letterAnimator = letterLabel.GetComponent<Animator> ();

		letterEffectUILabel = letterEffectLabel.GetComponent<UILabel>();
		letterEffectScale = letterEffectLabel.GetComponent<TweenScale>();
		letterEffectAlpha = letterEffectLabel.GetComponent<TweenAlpha>();

	}

	public void SetArrowPrefab(Transform tr){
		
		//Debug.Log("letterPrefab.name="+letterPrefab.name);
		arrowUIEventTrigger = tr.GetComponent<UIEventTrigger>();
		arrowUISprite = tr.GetComponent<UISprite>();
		arrowBoxcollider = tr.GetComponent<BoxCollider>();

	}

	public void SetBowPrefab(Transform tr){

		//Debug.Log("letterPrefab.name="+letterPrefab.name);
		bowUIEventTrigger = tr.GetComponent<UIEventTrigger>();
		bowUISprite = tr.GetComponent<UISprite>();
		bowBoxcollider = tr.GetComponent<BoxCollider>();
	}

	public void SetFlowerPrefab(Transform tr){
		
		//Debug.Log("letterPrefab.name="+letterPrefab.name);
		flowerUIEventTrigger = tr.GetComponent<UIEventTrigger>();
		flowerUISprite = tr.GetComponent<UISprite>();
		flowerAnimator = tr.GetComponent<Animator>();

		flowerBoxcollider = tr.GetComponent<BoxCollider>();
		flowerSweatUISprite = tr.FindChild("EatFailSprite").GetComponent<UISprite>();
		flowerSweatAnimator = tr.FindChild("EatFailSprite").GetComponent<Animator>();

	}

	public void EnableBowBoxCollider(bool tf) {
		for(int i=0;i < g.bowCnt;i++) { 
			g.bowBox[i].enabled = tf;
		}
	}


	public void EnableGridBoxCollider(bool tf) {
		BoxCollider[] boxCols = gameState.Grid.GetComponentsInChildren<BoxCollider>(); 
		foreach (BoxCollider bc in boxCols) {
			bc.enabled = tf;
		}
	}

	public void EnableGameSceneButtonBoxCollider(bool tf) {

		backSpaceBoxCollider.enabled   = tf;
		comboGameBoxCollider.enabled   = tf;
		hardGameBoxCollider.enabled    = tf;
		easyGameBoxCollider.enabled    = tf;
		levelSkipBoxCollider.enabled   = tf;

		homeBoxCollider.enabled   = tf;
		mapBoxCollider.enabled   = tf;

		//arrowBoxcollider.enabled       = tf;
		//bowBoxcollider.enabled         = tf;
		//flowerBoxcollider.enabled      = tf;

		skipBoxCollider.enabled        = tf;
		mapBoxCollider.enabled         = tf;
	}


	public void EnableGameSceneCommonBoxCollider(bool tf) {
		skipBoxCollider.enabled = tf;
		mapBottomBoxCollider.enabled = tf;
	}






	public void DestroyGridChildrens() {
		Transform[] trans = gameState.Grid.GetComponentsInChildren<Transform>(); 
		foreach (Transform tr in trans) {
			if(tr.gameObject.tag != "Grid") {
				Destroy(tr.gameObject); 
			}
		}
	}


	public void DestroyGridChildrens(Transform gridTransform) {
		Transform[] trans = gridTransform.GetComponentsInChildren<Transform>(); 
		foreach (Transform tr in trans) {
			if(tr.gameObject.tag != "Grid") {
				Destroy(tr.gameObject); 
			}
		}
	}

	public string blankLetterReplace(string s1) {
		return s1.Replace(" ","▒");
	} 


}
