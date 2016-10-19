using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameTexture : MonoBehaviour {

	public GameControl gameControl; 
	public GameBasic gameBasic;
	public GameState gameState;
	public GameComponent gameComponent;

    public DBO dbo; public Navi navi; public EventCommon ev;

    public UITexture GanaTexture;

	private float showLetterTime = 0.5f;      // 0.8f
	public float showWordTime = 1.3f;        // 1.8f

    //---------------------------------------------------------
    // sqlite
    //---------------------------------------------------------
    DataTable _data; DataRow dr;

	//--------------------------------------------------------- 
	// TTS Param
	//---------------------------------------------------------
	private float _pitch = 1f;
	private float _speechRate = 0f;

    public void SetBanner() {

        //---------------------------------------------
        // Banner
        //---------------------------------------------
        if (g.noBannerYn == "Y") {
            string bannerHint;

            _data = dbo.SelTextureInfo(g.curStage, g.texName); dr = _data.Rows[0];

            bannerHint = g.texStateName;

            //Debug.Log("bannerHint="+bannerHint);
            gameComponent.bannerHintUILabel.text = bannerHint;
        } else {
            gameComponent.bannerUISprite.spriteName = "baner";
            gameComponent.bannerHintUILabel.text = "";
        }
    }

	public IEnumerator ShowTexture(){

		Debug.Log("ShowTexture");

		//---------------------------------------------
		// Set State
		//---------------------------------------------
		g.isGameTexture = true;
		g.isCombo = true;
		g.isShowLetterEffect = true;
		//gameComponent.EnableGameSceneButtonBoxCollider(false);
		gameState.StateShowTexture();
        gameComponent.bannerHintUILabel.text = "- HINT -";

		//---------------------------------------------
		// BoxCollider
		//---------------------------------------------

        gameComponent.comboGameBoxCollider.enabled = true;
		gameComponent.hardGameBoxCollider.enabled = true;
		gameComponent.easyGameBoxCollider.enabled = true;

		gameComponent.mapBottomBoxCollider.enabled = true;

		//---------------------------------------------
		// Initiate Variable
		//---------------------------------------------
		gameComponent.wordCntUILabel.text = g.texShowName.Length.ToString(); 

		gameComponent.gameIconUISprite.spriteName = "imagegameicons";
		//gameComponent.gameIconUISprite.MakePixelPerfect();

		gameComponent.comboGameAnimator.SetBool("isButtonWiggle",true);

		gameComponent.gameTextureUISprite.color = Color.white;
		gameComponent.pictureFrameUISprite.color = Color.white;


		//---------------------------------------------
		// Setting random Backgroud
		//---------------------------------------------
		if(g.stageTurn > 1) gameBasic.SetBackgroudColor();
		//---------------------------------------------
		// Setting random Texture
		//---------------------------------------------
		if(g.stageTurn > 1) { 
			SetTurnTexture();
		}
		//gameComponent.clickWordUILabel.text = g.texShowName;
		gameComponent.clickWordUILabel.text = "";

        //---------------------------------------------
        // Setting Banner
        //---------------------------------------------

        _data = dbo.SelTextureInfo(g.curStage, g.texName); dr = _data.Rows[0];
        g.texTTSName = dr["texShowName"].ToString();
        g.texStateName = dr["meaning"].ToString();

		//---------------------------------------------
		// FadeIn
		//---------------------------------------------
		if(g.stageTurn > 1) yield return StartCoroutine ( navi.FadeIn() );

		//---------------------------------------------
		// Navigation
		//---------------------------------------------
		if(g.isGameTexture) StartCoroutine( ShowCorrectLetter());
		
	}


    float hue; Color rgbCol; HSBColor hsbCol;
	IEnumerator ShowCorrectLetter(){
		
		Debug.Log("ShowCorrectLetter");
		
		//---------------------------------------------
		// State && Init
		//---------------------------------------------
		gameState.ShowWord.SetActive(true);

		gameComponent.comboNumUILabel.text = "";
		gameComponent.comboNumLineUILabel.text = "";

        g.isCombo = true;

		//---------------------------------------------
		// Setting showWord
		//---------------------------------------------


        ////hsbCol = new HSBColor(hue,1.0f,1.0f);

        //hue = Random.Range(0.0f,1.0f);

        //hsbCol = new HSBColor(hue, 0.5f, 0.3f);
        //rgbCol = hsbCol.ToColor();
        ////gameComponent.showWordUILabel.color = rgbCol;
        //Debug.Log("hueCol111111111111=" + hue);
        //Debug.Log("rgbCol111111111111=" + rgbCol);

        //g.letterColorR = rgbCol.r;
        //g.letterColorG = rgbCol.g;
        //g.letterColorB = rgbCol.b;
        //gameComponent.showWordUILabel.color = g.letterColor;

        //g.letterColorR = Random.Range(0.1f, 0.3f);
        //g.letterColorG = Random.Range(0.1f, 0.3f);
        //g.letterColorB = Random.Range(0.1f, 0.3f);
        //gameComponent.showWordUILabel.color = g.letterColor;
        g.SetLetterColor();

        //Debug.Log("g.letterColor1111111111111111=" + g.letterColor);
        gameComponent.showWordUILabel.color = g.letterColor;

        gameComponent.showWordUILabel.height = 566;

		for(int i=0;i<g.texShowName.Length;i++){
			
			if(g.isGameTexture && g.isShowLetterEffect) {
				
				gameComponent.showWordUILabel.text = g.texName[i].ToString();
				gameComponent.clickWordUILabel.text += g.texName[i].ToString();;

				//Debug.Log("gameComponent.showWordUILabel.text="+gameComponent.showWordUILabel.text);
				//---------------------------------------------
				// TTS Speak
				//---------------------------------------------
				gameBasic.PlayTTS( g.texShowName[i].ToString() );

				//---------
				// tween
				//---------
				gameComponent.showWordScale.from = Vector3.one;
				gameComponent.showWordScale.to = Vector3.one * 2.5f;
				gameComponent.showWordScale.duration = showLetterTime;
				gameComponent.showWordScale.enabled = true; 
				yield return new WaitForSeconds(showLetterTime);
				gameComponent.showWordScale.ResetToBeginning();
				gameComponent.showWordScale.enabled = false; 

			}
		}

		if(g.isGameTexture && g.isShowLetterEffect) StartCoroutine( ShowCorrectWord());

	}

	IEnumerator ShowCorrectWord(){


		//---------------------------------------------
		// Easy Setting State
		//---------------------------------------------
		g.isCombo = false;
		gameComponent.comboGameAnimator.SetBool("isButtonWiggle",false);

		gameState.WordCnt.SetActive(false);
		gameState.ComboGame.SetActive(false);
		gameState.HardGame.SetActive(true);
		gameState.EasyGame.SetActive(true);

		gameComponent.hardGameAnimator.SetBool("isButtonWiggle",true);
		gameComponent.easyGameAnimator.SetBool("isButtonWiggle",true);

		gameComponent.gameIconUISprite.spriteName = "memorygameicons";
		//gameComponent.gameIconUISprite.MakePixelPerfect();

		//---------------------------------------------
		// TTS Speak
		//---------------------------------------------

		gameState.ShowWord.transform.localScale = Vector3.one * 0.00001f;
		yield return new WaitForSeconds(0.2f);

		//gameComponent.clickWordUILabel.text = "[☜Hard?][Easy?☞]";
        gameComponent.clickWordUILabel.text = "[←Hard?][Easy?→]";


		for(int i=1;i<=7;i++) {
			if(g.isGameTexture && g.isShowLetterEffect) {
				yield return StartCoroutine ( CorrectWordEffect() );
			}
		}

		if(g.isGameTexture && g.isShowLetterEffect) {
			//---------------------------------------------
			// Navigation
			//---------------------------------------------
			gameControl.GameStart("easyRandomGame");
		}
		
	}

	public IEnumerator CorrectWordEffect(){

        if (g.texShowName.Length <= 3) gameComponent.showWordUILabel.height = 300;
        else if (g.texShowName.Length <= 5) gameComponent.showWordUILabel.height = 200;
        else if (g.texShowName.Length <= 8) gameComponent.showWordUILabel.height = 100;
        else if (g.texShowName.Length <= 14) gameComponent.showWordUILabel.height = 70;
        else gameComponent.showWordUILabel.height = 40;

        gameComponent.showWordUILabel.text = g.texShowName;

        //gameComponent.showWordUILabel.height = 70;
        //gameComponent.showWordUILabel.text = "일이삼사오육칠팔구십일이삼사";


        gameComponent.showWordUILabel.color = g.letterColor;


		//---------------------------------------------
		// TTS Speak
		//---------------------------------------------
		gameBasic.PlayTTS( g.texTTSName.ToString() );
		
		//---------
		// tween
		//---------
		gameComponent.showWordScale.from = Vector3.one * 0f;
		gameComponent.showWordScale.to = Vector3.one * 1f;
		gameComponent.showWordScale.duration = showWordTime ;
		gameComponent.showWordScale.enabled = true; 
		yield return new WaitForSeconds(showWordTime);
		gameComponent.showWordScale.ResetToBeginning();
		gameComponent.showWordScale.enabled = false; 

	}

    public void SetTurnTexture()
    {
        Debug.Log("g.stageTurn=" + g.stageTurn + " g.curStage=" + g.curStage);

        g.texName = g.stageTextures[g.stageTurn - 1][0].texName;
        g.texShowName = g.stageTextures[g.stageTurn - 1][0].texShowName;

        //Debug.Log("g.texShowKind=" + g.texShowKind);

        _data = dbo.SelLanguageAlphabet(); dr = _data.Rows[0];
        g.letterAlphabet = dr["alphabet"].ToString();
        g.letterCanShowing = dr["canShowAlphabet"].ToString();


        //Debug.Log("g.texName=" + g.texName + " g.texShowName=" + g.texShowName);
        //Debug.Log("g.texShowKind=" + g.texShowKind);

        //----------------
        // Navi
        //----------------
        StartCoroutine("SetTurnTextureBasic");
    }

	public IEnumerator SetTurnTextureBasic() {

        GanaTexture.enabled = false;

		string texPath;
		WWW www1;
		WWW www2;
		WWW www3;
		WWW www4;

		Texture2D wwwTexture1;
		Texture2D wwwTexture2;
		Texture2D wwwTexture3;
		Texture2D wwwTexture4;

		//texNum = Random.Range(1,(g.stageTexBasicName[g.stageTurn-1].Count)*100000)/100000;
		//texNum = Random.Range(1,4);

		texPath  = Application.persistentDataPath + "/Textures/v"+g.volumn.ToString("000");
		texPath += "/s"+g.curStage.ToString("000")+"/";

		//Debug.Log(g.stageTextures[g.stageTurn-1][0].fileName+".jpg");

		//Debug.Log("file:///" + texPath + g.stageTextures[g.stageTurn-1][0].fileName+".jpg");


		www1 = new WWW("file:///" + texPath + g.stageTextures[g.stageTurn-1][0].fileName+".jpg");
		www2 = new WWW("file:///" + texPath + g.stageTextures[g.stageTurn-1][1].fileName+".jpg");
		www3 = new WWW("file:///" + texPath + g.stageTextures[g.stageTurn-1][2].fileName+".jpg");
		www4 = new WWW("file:///" + texPath + g.stageTextures[g.stageTurn-1][3].fileName+".jpg");

		yield return www1;
		yield return www2;
		yield return www3;
		yield return www4;

		//wwwTexture1 = new Texture2D(www1.texture.width, www1.texture.height, TextureFormat.ARGB32, false);
		wwwTexture1 = new Texture2D(272, 272, TextureFormat.ARGB32, false);
		wwwTexture2 = new Texture2D(272, 272, TextureFormat.ARGB32, false);
		wwwTexture3 = new Texture2D(272, 272, TextureFormat.ARGB32, false);
		wwwTexture4 = new Texture2D(272, 272, TextureFormat.ARGB32, false);

		www1.LoadImageIntoTexture(wwwTexture1);
		www2.LoadImageIntoTexture(wwwTexture2);
		www3.LoadImageIntoTexture(wwwTexture3);
		www4.LoadImageIntoTexture(wwwTexture4);

		gameComponent.gameUITexture1.mainTexture = wwwTexture1;
		gameComponent.gameUITexture2.mainTexture = wwwTexture2;
		gameComponent.gameUITexture3.mainTexture = wwwTexture3;
		gameComponent.gameUITexture4.mainTexture = wwwTexture4;

        string photoLisence = "";


        for(int i=0;i<g.wordTexCnt;i++){        
            _data = dbo.SelPhotoInfo(g.stageTextures[g.stageTurn-1][i].texNum); dr = _data.Rows[0];
            if(dr["photoLicense"].ToString() == "4") {
                photoLisence = "CCBY";
                gameComponent.photoLicenseSprites[i].width = 40;
                gameComponent.photoLicenseSprites[i].transform.localPosition = new Vector3(-110,-120,0);
                gameComponent.photoOwnerLabels[i].transform.localPosition = new Vector3(124,-2,0);

            } else if(dr["photoLicense"].ToString() == "5") {
                photoLisence = "CCBYSA";        
                gameComponent.photoLicenseSprites[i].width = 60;
                gameComponent.photoLicenseSprites[i].transform.localPosition = new Vector3(-100,-120,0);
                gameComponent.photoOwnerLabels[i].transform.localPosition = new Vector3(134,-2,0);
            } else if(dr["photoLicense"].ToString() == "6") {
                photoLisence = "CCBYND";        
                gameComponent.photoLicenseSprites[i].width = 60;
                gameComponent.photoLicenseSprites[i].transform.localPosition = new Vector3(-100,-120,0);
                gameComponent.photoOwnerLabels[i].transform.localPosition = new Vector3(134,-2,0);
            }
            else if(dr["photoLicense"].ToString() == "pd") {
                photoLisence = "PD";
                gameComponent.photoLicenseSprites[i].width = 20;
                gameComponent.photoLicenseSprites[i].transform.localPosition = new Vector3(-120,-120,0);
                gameComponent.photoOwnerLabels[i].transform.localPosition = new Vector3(114,-2,0);
            }

            //20 40 
            //20 20
            //20 60     
       		gameComponent.photoLicenseSprites[i].spriteName = photoLisence;
       		gameComponent.photoOwnerLabels[i].text = dr["photoOwner"].ToString();
        }


	}

    IEnumerator SetTurnTextureGana() {
        GanaTexture.enabled = true;
        //Debug.Log("g.texName="+g.texName);

        string imgPath = Application.persistentDataPath + "/Textures/gana/";
        WWW www1;
        Texture2D wwwTexture1 = new Texture2D(576, 569, TextureFormat.ARGB32, false);
        www1 = new WWW("file:///" + imgPath + g.texName +".png" ); yield return www1; 
        www1.LoadImageIntoTexture(wwwTexture1); 
        GanaTexture.mainTexture = wwwTexture1;
    }
}
