using UnityEngine;
using System.Collections;

public class Color01 : MonoBehaviour {

	float duration = 0.3f;	
	Color rgbCol;
	HSBColor hsbCol;
	float hue = 0.0f;

	// Use this for initialization
	void Start () {

		//test01();
		hue = 0.0f;
		StartCoroutine ( test02() );

	}

	IEnumerator test02(){
		//print (rgbCol);

		hsbCol = new HSBColor(hue,1.0f,1.0f);
		rgbCol = hsbCol.ToColor();	
		camera.backgroundColor = rgbCol;	

		hue = hue + 0.01f;
		if(hue > 1.0f) hue = 0.0f;

		print (hsbCol);
		//print (rgbCol);
		yield return new WaitForSeconds(0.05f);

		StartCoroutine ( test02() );
	}


	void test01() {
		hsbCol = new HSBColor(Color.red);
		Debug.Log("red: " + hsbCol);
	}


	// Update is called once per frame
	void Update () {
//		//print(Colorx.Slerp(Color.red, Color.blue, Time.time * 0.0005f));
//		//float t = Mathf.PingPong (Time.deltaTime, duration) / duration;
//		float t = Mathf.PingPong (Time.deltaTime, duration);
//		//print ("Time.time="+Time.time);
//		//print ("t="+t);
//		//rgbCol = Color.Lerp (Color.red, Color.cyan, t);
//		//rgbCol = Colorx.Slerp(Color.red, Color.white, t);
//		//hsbCol = new HSBColor(rgbCol);
//		hsbCol = new HSBColor(t,1.0f,1.0f);
//		rgbCol = hsbCol.ToColor();
//
//		//print (rgbCol);
//		print (hsbCol);
//
//		camera.backgroundColor = rgbCol;

	}
}
