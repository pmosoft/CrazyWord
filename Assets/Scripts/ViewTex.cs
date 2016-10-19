using UnityEngine;
using System.Collections;

public class ViewTex : MonoBehaviour {

	public UITexture gameUITexture1;
	public UITexture gameUITexture2;
	public UITexture gameUITexture3;
	public UITexture gameUITexture4;

	public UISprite[] photoLicenseSprites = new UISprite[4];
	public UILabel[] photoOwnerLabels = new UILabel[4];


    public UILabel stageInfoUILabel, texInfoUILabel;

    public UISprite[] checks = new UISprite[4];

	public DBO dbo; public EventCommon ev;
	DataTable _data; DataRow dr;

    float delayTime = 3.0f;

    int stageNum=1,turnTexNum=1;
    string texName = "";
    string minData,maxData,reYn;

	// Use this for initialization
	void Start () {

        stageNum = int.Parse(stageInfoUILabel.text);
        turnTexNum=1;
        stageNum = 1;
        _data = dbo.SelShowTexture(stageNum,turnTexNum); 
        for (int i = 0; i < _data.Rows.Count; i++) {
            dr = _data.Rows[i];
            stageNum = int.Parse( dr["stageNum"].ToString() );
            texName = dr["texName"].ToString();
            reYn = dr["updateTime"].ToString();
            if (reYn == "Y") checks[i].spriteName = "bgediteoption3";
            else checks[i].spriteName = "bgediteoption1";
        }
        StartCoroutine( SetTurnTextureBasic(stageNum,texName) );

        //StartCoroutine( AutoShow() );
	}

	public void EventCheck1 () {
        if (checks[0].spriteName == "bgediteoption1") {
            dbo.UptTextureInfo2(stageNum,turnTexNum,1,"Y3");
            checks[0].spriteName = "bgediteoption3";
        }else{ 
            checks[0].spriteName = "bgediteoption1";
            dbo.UptTextureInfo2(stageNum,turnTexNum,1,"N");

        }
	}
	public void EventCheck2 () {
        if (checks[1].spriteName == "bgediteoption1") {
            dbo.UptTextureInfo2(stageNum,turnTexNum,2,"Y3");
            checks[1].spriteName = "bgediteoption3";
        }else{ 
            checks[1].spriteName = "bgediteoption1";
            dbo.UptTextureInfo2(stageNum,turnTexNum,2,"N");

        }
	}
	public void EventCheck3 () {
        if (checks[2].spriteName == "bgediteoption1") {
            dbo.UptTextureInfo2(stageNum,turnTexNum,3,"Y3");
            checks[2].spriteName = "bgediteoption3";
        }else{ 
            checks[2].spriteName = "bgediteoption1";
            dbo.UptTextureInfo2(stageNum,turnTexNum,3,"N");
        }
	}
	public void EventCheck4 () {
        if (checks[3].spriteName == "bgediteoption1") {
            dbo.UptTextureInfo2(stageNum,turnTexNum,4,"Y3");
            checks[3].spriteName = "bgediteoption3";
        }else{ 
            checks[3].spriteName = "bgediteoption1";
            dbo.UptTextureInfo2(stageNum,turnTexNum,4,"N");
        }
	}


    void EventFowd(){

        _data = dbo.SelShowTextureFowd(stageNum,turnTexNum);
        for (int i = 0; i < _data.Rows.Count; i++) {
            dr = _data.Rows[i];
            stageNum = int.Parse( dr["stageNum"].ToString() );
            texName = dr["texName"].ToString();
            turnTexNum = int.Parse ( dr["turnTexNum"].ToString() );
            reYn = dr["updateTime"].ToString();
            if (reYn == "Y") checks[i].spriteName = "bgediteoption3";
            else checks[i].spriteName = "bgediteoption1";
        }

        print("stageNum=" + stageNum);
        print("turnTexNum=" + turnTexNum);
        print("texName=" + texName);

        stageInfoUILabel.text = stageNum.ToString();
        texInfoUILabel.text = texName;

        StartCoroutine( SetTurnTextureBasic(stageNum,texName) );
    }


    void EventBack(){

	    _data = dbo.SelShowTextureBack(stageNum,turnTexNum);
        for (int i = 0; i < _data.Rows.Count; i++) {
            dr = _data.Rows[i];
            stageNum = int.Parse( dr["stageNum"].ToString() );
            texName = dr["texName"].ToString();
            turnTexNum = int.Parse ( dr["turnTexNum"].ToString() );
            reYn = dr["updateTime"].ToString();
            if (reYn == "Y") checks[i].spriteName = "bgediteoption3";
            else checks[i].spriteName = "bgediteoption1";
        }

        print("stageNum=" + stageNum);
        print("turnTexNum=" + turnTexNum);
        print("texName=" + texName);

        stageInfoUILabel.text = stageNum.ToString();
        texInfoUILabel.text = texName;

        StartCoroutine( SetTurnTextureBasic(stageNum,texName) );
    }
    

    IEnumerator AutoShow(){

        
        _data = dbo.SelShowTextureAuto();

        for (int i = 0; i < _data.Rows.Count; i++)
        {
            dr = _data.Rows[i];
            stageNum = int.Parse( dr["stageNum"].ToString() );
            texName = dr["texName"].ToString();

            stageInfoUILabel.text = stageNum.ToString();
            texInfoUILabel.text = texName;


            StartCoroutine( SetTurnTextureBasic(stageNum,texName) );
            yield return new WaitForSeconds(delayTime);
        }
    }

	public IEnumerator SetTurnTextureBasic(int stageNum, string texName) {

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
		texPath += "/s"+stageNum.ToString("000")+"/";

		//print (g.stageTextures[g.stageTurn-1][0].fileName+".jpg");

        print("file:///" + texPath + texName+"_01.jpg");


		www1 = new WWW("file:///" + texPath + texName+"_01.jpg");
		www2 = new WWW("file:///" + texPath + texName+"_02.jpg");
		www3 = new WWW("file:///" + texPath + texName+"_03.jpg");
		www4 = new WWW("file:///" + texPath + texName+"_04.jpg");
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

		gameUITexture1.mainTexture = wwwTexture1;
		gameUITexture2.mainTexture = wwwTexture2;
		gameUITexture3.mainTexture = wwwTexture3;
		gameUITexture4.mainTexture = wwwTexture4;

        string photoLisence = "";


        for(int i=0;i<g.wordTexCnt;i++){        
            _data = dbo.SelPhotoInfo2(stageNum,texName,i+1); dr = _data.Rows[0];
            if(dr["photoLicense"].ToString() == "4") {
                photoLisence = "CCBY";
                photoLicenseSprites[i].width = 40;
                photoLicenseSprites[i].transform.localPosition = new Vector3(-110,-120,0);
                photoOwnerLabels[i].transform.localPosition = new Vector3(124,-2,0);

            } else if(dr["photoLicense"].ToString() == "5") {
                photoLisence = "CCBYSA";        
                photoLicenseSprites[i].width = 60;
                photoLicenseSprites[i].transform.localPosition = new Vector3(-100,-120,0);
                photoOwnerLabels[i].transform.localPosition = new Vector3(134,-2,0);
            } else if(dr["photoLicense"].ToString() == "6") {
                photoLisence = "CCBYND";        
                photoLicenseSprites[i].width = 60;
                photoLicenseSprites[i].transform.localPosition = new Vector3(-100,-120,0);
                photoOwnerLabels[i].transform.localPosition = new Vector3(134,-2,0);
            }
            else if(dr["photoLicense"].ToString() == "pd") {
                photoLisence = "PD";
                photoLicenseSprites[i].width = 20;
                photoLicenseSprites[i].transform.localPosition = new Vector3(-120,-120,0);
                photoOwnerLabels[i].transform.localPosition = new Vector3(114,-2,0);
            }

            print("11111111111111111111111111111="+dr["photoLicense"].ToString() + " " + dr["photoOwner"].ToString());
            //20 40 
            //20 20
            //20 60
       		photoLicenseSprites[i].spriteName = photoLisence;
       		photoOwnerLabels[i].text = dr["photoOwner"].ToString();

        }

        print("22222222222222222222222222222222");

	}


	void Update () {
		if(Input.GetKeyDown(KeyCode.B))	EventBack();
		if(Input.GetKeyDown(KeyCode.F))	EventFowd();
	}

}
