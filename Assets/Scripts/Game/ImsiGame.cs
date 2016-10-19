using UnityEngine;
using System.Collections;

public class ImsiGame : MonoBehaviour {

	public DownTexture downTexture;

	// Use this for initialization
	void Start () {
         print("ImsiGame111111111111111111111111111111");
         StartCoroutine( DownStart() );
	}
	
    IEnumerator DownStart(){
        print("222222222222222");
        //////////// g.cs에서 g.curStage = 149; 수정요
        yield return StartCoroutine(downTexture.WWWDownload("bulk"));
        //yield return new WaitForSeconds(10f);
        print("333333333333333");
        //Application.LoadLevel("DownClear");
    }
}
