using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;



public class g : MonoBehaviour {
	
	//---------------------------------------------------------
	// CrazyWord Version
	//---------------------------------------------------------
    public static string gameCode = "0101kr"; // english basic1 kr release

	public static string CrazyWordLanguage  = "English";
    public static int curVersion = 14;
    public static int patchVersion = 14;

	public static string sqliteName = "czw0100100400015en1";


	//---------------------------------------------------------
	// State
	//---------------------------------------------------------
    public static bool isFirstHome = true;

	public static bool isHome = false;
	public static bool isMap = false;
	public static bool isGame = false;
	public static bool isGameTexture = false;        //using in ShowTexture(),ShowCorrectLetter()
	public static bool isGameTurning = false;        //using in LimitTime()
	public static bool isCombo = false;
	public static bool isLevelPassing = false;

	// game list
	public static bool isGameMemoryCombo = false; // *2
	public static bool isGameMemory = false;      // *2
	
	public static bool isGameShooting = false;    // *x
	public static bool isGameRain = false;        // *x 
	public static bool isGameMole = false;        // *x
	public static bool isGameBlank = false;       // *x
	public static bool isGameHint = false;        // *x 
	
	public static bool isGameBonus = false;       // +1000
	
	
	public static bool isProcessHintLetter = false;		
	public static bool isShowLetterEffect = true;
	
    public static bool isInternetReachability = true;

    public static bool isPopupShopShow = false;
    public static bool isPopupPurchaseShow = false;

    public static bool isPopupFriendShow = false;
    public static bool isPopupGiftShow = false;
    public static bool isPopupOptionShow = false;

    public static bool isPopupPartItemShow = false;

    public static bool isSound = true;

    public static bool isGiftAcceptAll = false;

    public static bool isQuit = false;

    public static bool isShopFbShare = false;

    public static bool isSelUserInfo = false;

    public static bool isDowninformPopup = false;


    public static string facebookEvent;

	public static string downY = "Y";
	public static string downN = "N";
	public static string texUrlY = "Y";
	public static string texUrlN = "N";
	
	public static int minDownStage = 1;
	public static int maxDownStage = 1;
	
	//--------------------------------------------------------- 
	// Game Control
	//---------------------------------------------------------
	public static int tileWidth = 118;
	public static int tileHeight = 118;
	
	public static int stageSkip=2; // 5
	
	public static int limitBaseTime = 300;	// 300
	public static int limitTime;
	public static float bonusLimitTime = 1f;	// 5
	
	public static int curStageScore;
	public static int curStageRank = 0;

    public static int curStageVitualMaxScore = 0; // ranking
    public static int curStageMaxScore = 0; // ranking
    public static int curStageMaxRank = 0; // ranking
	
	public static int gameScore = 0;	
	public static int comboCnt = 0;
	public static int comboTotalCnt = 0;
	public static int comboLocalTotalCnt = 0;
	public static int bonusCnt = 0;
	public static int bonusScore = 1000;
	
	public static float turnDelayTime = 1.0f;       // 1.0f 
	public static int volumn = 1;      // Light,Toeic,Tofle..... 
	
	public static int maxStage;



	public static int stageTurn = 1;
    public static int turnTotalCnt = 5;

    public static int wordTexCnt = 4;


	public static int texNum = 1;   
	
	public static int turnTexTotalNum;
	

	public static string curRankName = "";   // db
	public static string curIconName = "";   // db
	
	
	public static int hpAddTime = 5 * 60;
	public static int skipAddTime = 5;
	
	public static string skipDate;
	
	public static string skipTime; // after delete
	public static int uptCnt;      // after delete 
	
	public static int totalStar = 10;	
	
	public static string texName;
	public static string texShowName;
	public static string texTTSName;
	public static string texStateName;

    public static int texShowKind;
    public static string texPath;
	public static string texFileName;
	
	public static List<TexInfo>[] stageTextures;
	public static List<int> randomTurnTexNum = new List<int>();
	
	public static float touchProludeTime = 0.2f;
	public static float autoProludeTime = 0.7f;
	
	//public static int homeDownloadCnt = 1;	

    public static int maxBonusSkipCnt = 3; // 3 32
    public static int maxBonusHpCnt = 10; // 10 12

    public static int freeSkipMinuteTime = 32; // 32
    public static int freeHpMinuteTime = 12; // 12

	public static float buttonVolume = 0.6f;

	public static string internetConnectTestUrl = "http://www.google.com";
	//public static string internetConnectTestUrl = "http://crazyword.org/blank.html";

    //public static string[] easyGameList = new string[4] {
    //     "shooting"
    //    ,"rain"
    //    ,"blank"
    //    ,"mole"
    //};



	//--------------------------
	// Easy Game
	//--------------------------
	public static int easyGameCnt = 0;
	public static string[] easyRandomGame = new string[3];
	public static string[] easyGameList = new string[4] {
		"shooting","rain","blank","mole"
	};
	
	//--------------------------
	// Map 
	//--------------------------
    public static int lessonNumber;

	
	//--------------------------------------------------------- 
	// User Option
	//---------------------------------------------------------
	public static bool isVibrate = true;
    public static string userLanguage = "Korean";
    public static int languageClass = 1;

    //--------------------------------------------------------- 
    // User Info
    //---------------------------------------------------------
    public static string userId = " ";

    public static string googleId = " ";
    public static string googleName = " ";
    public static string nickName = " ";
   
    public static string deviceId;
    public static int curStage = 1; // db

    public static int bestScore;         // db


    public static int coin;         // db
    public static int freeHp = 0;   // db
    public static int buyHp = 0;    // db
    public static int giftHp = 0;   // db
    public static int itemHp = 0;   // db
    public static int freeSkip = 0; // db
    public static int buySkip = 0;  // db
    public static int giftSkip = 0; // db
    public static int itemSkip = 0; // db

    public static string partMove;   //db
    public static string noBannerYn; //db
    public static string userPicYn;  //db

    //public static string friendYn;  //db
    public static string achievement;  //db

    public static int userBgCol = 0; // db
    public static int userBgImg = 0; // db

    //public static int bestScore = 0;  // db


    public static string userFbUrl;
    public static string fbId;
    public static string fbFriendId;

    public static int partNum = 0;
    public static int itemGroup = 0;

    public static string wwwDate;
    public static string fbShareDate;

    public static bool isPurchaseSucceed = false;
    public static string purchaseOrderId;
    public static string purchaseProductId;

    public static string fbUserPicUrl = " ";


    //--------------------------------------------------------- 
    // Shop
    //---------------------------------------------------------
    public static int skipCoin = 4;
    public static int hpCoin = 2;
    public static int allpackCoin = 300;
    public static int nobannerCoin = 100;

    public static int enterPart = 35;

	public static List<GiftCode> giftCodes = new List<GiftCode>();
    public static int giftIdx = 0;
    public static int friendListIdx = 0;

	public static List<UISprite> requestGiftUISprites = new List<UISprite>();
	public static List<UISprite> sendGiftUISprites = new List<UISprite>();
	public static List<BoxCollider> requestGiftBoxColliders = new List<BoxCollider>();
	public static List<BoxCollider> sendGiftBoxColliders = new List<BoxCollider>();
    
	//--------------------------------------------------------- 
	// Letter Prefab
	//---------------------------------------------------------
	public static Dictionary<string, Transform> letterDictionary = new Dictionary<string, Transform>();
	public static float letterColorR;
	public static float letterColorG;
	public static float letterColorB;
    public static Color letterColor;
    public static Color[] letterColors = new Color[7];


	public static List<LetterCol> letterCols = new List<LetterCol>();
	public static List<Transform> gridLetterTrans = new List<Transform>();
	
	public static string letterAlphabet;
	public static string letterExcluded = "ABCDEFGHIJKLMNOPQRST_UVWXYZ";
    public static string letterCanShowing = "ABCDEFGHIJKLMNOPQRST_UVWXYZ";


	public static Vector3[] letterStartPos = new Vector3[5]; 
	public static GameObject[] LetterClone = new GameObject[5]; 
	public static int correctLetterIdx;
	
	//--------------------------------------------------------- 
	// Commom
	//---------------------------------------------------------
	public static float backgroudColorR;
	public static float backgroudColorG;
	public static float backgroudColorB;

	public static Animator animator;
	public static Color col;
	
	public static float posRatio,scaleRatio;
	public static int tileWidthCnt,tileHeightCnt;
	
	public static Vector2 PrevPoint;
	public static Vector2 CurPoint;
	
	public static string localDate;
	public static string localTime;

    public static Color[] bgColOptions = new Color[10];
    public static string[] bgImgOptions = new string[6];

    public static Texture2D[] userBgImgTextures = new Texture2D[6];

    public static Texture2D[] startHelps = new Texture2D[5];


   	public static Color inActiveCol = new Color(65 / 255f, 65 / 255f, 65 / 255f, 1);

	//--------------------------
	// Game
	//--------------------------


	//--------------------------
	// Shooting Game
	//--------------------------
	public static int arrowShootCnt = 0;
	public static BoxCollider[] bowBox = new BoxCollider[5];
	public static UISprite[] arrowSprite = new UISprite[5];
	public static Transform[] arrowTransform = new Transform[5];
	public static UISprite[] bowSprite = new UISprite[5];
	public static int arrowDir = 0;
	public static int arrowIdx;
	public static int bowCnt = 3;

	public static int[,,] letterPattern = new int[8,25,2]
	{
		{ 
			// pattern 0
			{ 0, 0},{ 1, 1},{ 2, 2},{ 3, 3},{ 4, 4},
			{ 5, 5},{ 6, 6},{ 7, 7},{ 8, 8},{ 9, 9},
			{10,10},{11,11},{12,12},{13,13},{14,14},
			{15,15},{16,16},{17,17},{18,18},{19,19},
			{20,20},{21,21},{22,22},{23,23},{24,24}
		},
		{ 
			// pattern 1
			{ 0,20},{ 1,15},{ 2,10},{ 3, 5},{ 4, 0},
			{ 5,21},{ 6,16},{ 7,11},{ 8, 6},{ 9, 1},
			{10,22},{11,17},{12,12},{13, 7},{14, 2},
			{15,23},{16,18},{17,13},{18, 8},{19, 3},
			{20,24},{21,19},{22,14},{23, 9},{24, 4}
		},
		{ 
			// pattern 2
			{ 0,24},{ 1,23},{ 2,22},{ 3,21},{ 4,20},
			{ 5,19},{ 6,18},{ 7,17},{ 8,16},{ 9,15},
			{10,14},{11,13},{12,12},{13,11},{14,10},
			{15, 9},{16, 8},{17, 7},{18, 6},{19, 5},
			{20, 4},{21, 3},{22, 2},{23, 1},{24, 0}
		},
		{ 
			// pattern 3
			{ 0, 4},{ 1, 9},{ 2,14},{ 3,19},{ 4,24},
			{ 5, 3},{ 6, 8},{ 7,13},{ 8,18},{ 9,23},
			{10, 2},{11, 7},{12,12},{13,17},{14,22},
			{15, 1},{16, 6},{17,11},{18,16},{19,21},
			{20, 0},{21, 5},{22,10},{23,15},{24,20}
		},
		{ 
			// pattern 4
			{ 0, 4},{ 1, 3},{ 2, 2},{ 3, 1},{ 4, 0},
			{ 5, 9},{ 6, 8},{ 7, 7},{ 8, 6},{ 9, 5},
			{10,14},{11,13},{12,12},{13,11},{14,10},
			{15,19},{16,18},{17,17},{18,16},{19,15},
			{20,24},{21,23},{22,22},{23,21},{24,20}
		},
		{ 
			// pattern 5
			{ 0, 0},{ 1, 5},{ 2,10},{ 3,15},{ 4,20},
			{ 5, 1},{ 6, 6},{ 7,11},{ 8,16},{ 9,21},
			{10, 2},{11, 7},{12,12},{13,17},{14,22},
			{15, 3},{16, 8},{17,13},{18,18},{19,23},
			{20, 4},{21, 9},{22,14},{23,19},{24,24}
		},
		{ 
			// pattern 6
			{ 0,24},{ 1,23},{ 2,22},{ 3,21},{ 4,20},
			{ 5,19},{ 6,18},{ 7,17},{ 8,16},{ 9,15},
			{10,10},{11,11},{12,12},{13,13},{14,14},
			{15, 5},{16, 6},{17, 7},{18, 8},{19, 9},
			{20, 0},{21, 1},{22, 2},{23, 3},{24, 4}
		},
		{ 
			// pattern 7
			{ 0,24},{ 1,19},{ 2,14},{ 3, 9},{ 4, 4},
			{ 5,23},{ 6,18},{ 7,13},{ 8, 8},{ 9, 3},
			{10,22},{11,17},{12,12},{13, 7},{14, 2},
			{15,21},{16,16},{17,11},{18, 6},{19, 1},
			{20,20},{21,15},{22,10},{23, 5},{24, 0}
		}						
		
	}; 
	
	//--------------------------------------------------------- 
	// Static Function
	//---------------------------------------------------------
	public static int allSkip() {
        //Debug.Log("freeSkip="+freeSkip+" buySkip="+buySkip+" giftSkip="+giftSkip+" itemSkip="+itemSkip);
		return freeSkip+buySkip+giftSkip+itemSkip;
	}
	
	public static int allHp() {
        //Debug.Log("freeHp="+freeHp+" buyHp="+buyHp+" giftHp="+giftHp+" itemHp="+itemHp);
        return freeHp + buyHp + giftHp + itemHp;
	}
	
	public static void TileRatio() {
		if     (g.tileWidthCnt == 3)  { g.posRatio = 1.2f;  g.scaleRatio = 1.2f; }
		if     (g.tileWidthCnt == 5)  { g.posRatio = 1.0f;  g.scaleRatio = 1.0f; }
		else if(g.tileWidthCnt == 10) { g.posRatio = 0.45f; g.scaleRatio = 0.5f; }
	}
	
	public static void DestroyGridChildrens(Transform gridTransform) {
		Transform[] trans = gridTransform.GetComponentsInChildren<Transform>(); 
		foreach (Transform tr in trans) {
			if(tr.gameObject.tag != "Grid") {
				Destroy(tr.gameObject); 
			}
		}
	}

	
	//--------------------------------------------------------- 
	// Inner Struct
	//---------------------------------------------------------
	
	public struct TexInfo {
        public string texName, texShowName, fileName;
        public int texNum;
        public TexInfo(string texName, string texShowName, int texNum)
		{
			this.texName = texName;
			this.texShowName = texShowName;
            this.texNum = texNum;
			this.fileName = texName+"_"+texNum.ToString("00");
		}
	}
	
	public struct LetterCol {
		public string letter;
		public Color col;
		
		public LetterCol(string letter, Color col)
		{
			this.letter = letter;
			this.col  = col;
		}
	}

	public struct GiftCode {
		public int itemGroup;
		public int itemCode;
		public string recieveDate;
		public string recieveTime;
		
		public GiftCode(int itemGroup, int itemCode, string recieveDate, string recieveTime)
		{
			this.itemGroup = itemGroup;
			this.itemCode  = itemCode;
			this.recieveDate = recieveDate;
			this.recieveTime = recieveTime;
		}
	}


	public static void SetLetterColor() {
        letterColors[0] = new Color(83f / 255f, 19f / 255f, 19f / 255f);
        letterColors[1] = new Color(83f / 255f, 19f / 255f, 63f / 255f);
        letterColors[2] = new Color(36f / 255f, 19f / 255f, 83f / 255f);
        letterColors[3] = new Color(21f / 255f, 61f / 255f, 64f / 255f);
        letterColors[4] = new Color(15f / 255f, 66f / 255f, 21f / 255f);
        letterColors[5] = new Color(56f / 255f, 56f / 255f, 22f / 255f);
        letterColors[6] = new Color(52f / 255f, 26f / 255f, 15f / 255f);

        g.letterColor = g.letterColors[Random.Range(0, 7)];
	}

    public static Color GetRandomColor()
    {
        return new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }

    public static void SetBgColor()
    {
        bgColOptions[0] = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        bgColOptions[1] = new Color(212f / 255f, 214f / 255f, 252f / 255f);
        bgColOptions[2] = new Color(255f / 255f, 166f / 255f, 179f / 255f);
        bgColOptions[3] = new Color(187f / 255f, 177f / 255f, 244f / 255f);
        bgColOptions[4] = new Color(144f / 255f, 185f / 255f, 210f / 255f);
        bgColOptions[5] = new Color(127f / 255f, 220f / 255f, 230f / 255f);
        bgColOptions[6] = new Color(152f / 255f, 212f / 255f, 130f / 255f);
        bgColOptions[7] = new Color(255f / 255f, 217f / 255f, 133f / 255f);
        bgColOptions[8] = new Color(212f / 255f, 142f / 255f, 124f / 255f);
        bgColOptions[9] = new Color( 64f / 255f,  64f / 255f,  64f / 255f);
        
    }

    void Awake() {
        googleId = " ";

        userLanguage = Application.systemLanguage.ToString();

        letterColors[0] = new Color(83f / 255f, 19f / 255f, 19f / 255f);
        letterColors[1] = new Color(83f / 255f, 19f / 255f, 63f / 255f);
        letterColors[2] = new Color(36f / 255f, 19f / 255f, 83f / 255f);
        letterColors[3] = new Color(21f / 255f, 61f / 255f, 64f / 255f);
        letterColors[4] = new Color(15f / 255f, 66f / 255f, 21f / 255f);
        letterColors[5] = new Color(56f / 255f, 56f / 255f, 22f / 255f);
        letterColors[6] = new Color(52f / 255f, 26f / 255f, 15f / 255f);


        //Debug.Log("start:" + System.DateTime.Now);
    }
	
	
}
