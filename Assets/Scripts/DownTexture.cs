using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DownTexture : MonoBehaviour {

	public DBO dbo;
	public UISlider loadUISlider;

    public GameObject BannerPanel;

	int downStageCnt = 1; 
	int firstDownStageCnt = 1; // 5
	int downTexCnt = 20;

	//---------------------------------------------------------
	// sqlite
	//---------------------------------------------------------
	DataTable _data; DataRow dr;


	public IEnumerator StartDownload(string kind) {
		
		Debug.Log("start StartDownload");
        BannerPanel.SetActive(false);


		yield return StartCoroutine ( "CopyStageTex" );
		//yield return StartCoroutine ( WWWDownload("all",0 ) );

        BannerPanel.SetActive(true);

		Debug.Log("end DownTexture");
		
	}

	public IEnumerator CopyStageTex() {

		Debug.Log("CopyStageTex");


		int stageNum,turnTexNum,texNum;
		string texName;

		string texPath,texFileName,resPath;
		DirectoryInfo di;

		_data = dbo.SelCopyTextureZone();
		
		//Debug.Log("_data.Rows.Count=" + _data.Rows.Count);
		
		//------------------------------
		// start www download
		//------------------------------
		for (int j = 0; j < _data.Rows.Count; j++) {
			loadUISlider.value = (float)(j+1)/_data.Rows.Count;
			dr = _data.Rows[j];
			
			stageNum = int.Parse( dr["stageNum"].ToString() );
			turnTexNum = int.Parse( dr["turnTexNum"].ToString() );
			texName = dr["texName"].ToString();

    		for (int k = 1; k <= g.wordTexCnt; k++) {
                texNum = k;
			    //Debug.Log(j+" stageNum="+stageNum+" texName=" + texName+ " turnTexNum="+turnTexNum+" texNum="+texNum+" downYn="+downYn);
			    //if(downYn == "N" ) {

				//Debug.Log("stageNum="+stageNum+" texName=" + texName+ " texNum="+texNum);
			    texPath  = Application.persistentDataPath + "/Textures/v"+g.volumn.ToString("000");
				texPath += "/s"+stageNum.ToString("000")+"/";
			    resPath  = "Textures/v"+g.volumn.ToString("000")+"/s"+stageNum.ToString("000")+"/";
                texFileName  = texName + "_"+texNum.ToString("00")+".jpg";

				di = new DirectoryInfo(texPath);
				if (di.Exists == false) di.Create();
                Debug.Log(resPath+texFileName);

		        if (!System.IO.File.Exists(texPath + texFileName)) {
                    TextAsset ta = (TextAsset)Resources.Load(resPath+texFileName);
                    System.IO.File.WriteAllBytes(texPath + texFileName, ta.bytes); // 64MB limit on File.WriteAllBytes.
                    ta = null;
		        }
            }    
   			yield return new WaitForSeconds(0.50f);
		}
	}

	public IEnumerator WWWDownload(string kind) {

		WWW www;
		Texture2D downTexture;
		int texWidth, texHeight;

		int stageNum,turnTexNum,texNum;
		string texName,texUrl,downYn;

		string texPath,texFileName;
		DirectoryInfo di;

		_data = dbo.SelDownTextureZone(kind);
		
		//else if(g.isHome) _data = dbo.SelDownTextureStage10();
		//else if(g.isMap) _data = dbo.SelDownTextureStage20();
		//Debug.Log("_data.Rows.Count=" + _data.Rows.Count);
		
		//------------------------------
		// start www download
		//------------------------------
		for (int j = 0; j < _data.Rows.Count; j++) {
			if(kind != "bulk") loadUISlider.value = (float)(j+1)/_data.Rows.Count;
			
			dr = _data.Rows[j];
			
			stageNum = int.Parse( dr["stageNum"].ToString() );
			turnTexNum = int.Parse( dr["turnTexNum"].ToString() );
			texNum = int.Parse( dr["texNum"].ToString() );
			//downYn = dr["downYn"].ToString();
			texName = dr["texName"].ToString();
			texUrl = dr["texUrl"].ToString();
			
			//Debug.Log(j+" stageNum="+stageNum+" texName=" + texName+ " turnTexNum="+turnTexNum+" texNum="+texNum+" downYn="+downYn);
			
			//if(downYn == "N" ) {
			//if(downYn != "" ) {

				//Debug.Log("stageNum="+stageNum+" texName=" + texName+ " turnTexNum="+turnTexNum+" texNum="+texNum);
				//Debug.Log("texUrl="+texUrl);

                string texExtention = "jpg";
                if(texUrl.Contains(".png")) texExtention = "png";

                //texUrl="http://upload.wikimedia.org/wikipedia/commons/a/a7/Business_casual_male_&_female.svg";
				//Debug.Log("texUrl="+texUrl);

				www = new WWW( texUrl );
				yield return www;

				//Debug.Log("www.size="+www.size);
				//Debug.Log("www.texture.width="+www.texture.width);
				//Debug.Log("www.texture.height="+www.texture.height);


				if(www.texture.width <= 8 || www.size <= 100) {
					dbo.UptTextureInfo(stageNum,turnTexNum,texNum,"N","N");
				} else {
					texWidth = www.texture.width  > 256 ? 256 : www.texture.width; 
					texHeight = www.texture.height > 256 ? 256 : www.texture.height; 
					
					downTexture = new Texture2D(texWidth, texHeight, TextureFormat.ARGB32, false);
					www.LoadImageIntoTexture(downTexture);
					
                    byte[] bytes;
                    if(texExtention == "png") bytes = downTexture.EncodeToPNG();
                    else bytes = downTexture.EncodeToJPG();
					
					texPath  = Application.persistentDataPath + "/Textures/v"+g.volumn.ToString("000");
					texPath += "/s"+stageNum.ToString("000")+"/";
					
					di = new DirectoryInfo(texPath);
					if (di.Exists == false) di.Create();
					
					texFileName  = texName + "_"+texNum.ToString("00")+".jpg";
					//texFileName  = texName + "_"+texNum.ToString("00")+".jpg.bytes";
					
					File.WriteAllBytes(texPath+texFileName, bytes);
					
					//dbo.UptTextureInfo(stageNum,turnTexNum,texNum,"Y","Y");
				}
			//}
			
		}

	}

}
