using UnityEngine;
using System.Collections;

public class gamecontrol : MonoBehaviour {


	public UITexture tex;
	public Texture2D tDynamicTx;
	public WWW www;
	public string images;

	// Use this for initialization
	IEnumerator	 Start () {

		tDynamicTx= new Texture2D(64, 64);

		//renderer.material.mainTexture = www.texture; 

		//images = "file://"+ Application.dataPath +"Test.jpg";
		images = "http://images.earthcam.com/ec_metros/ourcams/fridays.jpg";
		www = new WWW(images);
		yield return www;

		www.LoadImageIntoTexture(tDynamicTx);

		tex.mainTexture = tDynamicTx;
		tex.MakePixelPerfect();
//		UITexture ut = NGUITools.AddWidget<UITexture>(parentGameObject);
//		ut.material = new Material(Shader.Find("Unlit/Transparent Colored"));
//		ut.material.mainTexture = YourTexture;
//		ut.MakePixelPerfect();


	}

}
