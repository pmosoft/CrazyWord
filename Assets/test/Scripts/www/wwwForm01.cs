using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class wwwForm01 : MonoBehaviour {

	// Use this for initialization
	void Start () {

        StartCoroutine("form01");
	}


    IEnumerator form01()
    {
        //// Create a form object for sending high score data to the server
        //WWWForm form = new WWWForm();
        //// Assuming the perl script manages high scores for different games
        //form.AddField("game", "MyGameName");

        //// Create a download object
        //WWW download = new WWW("http://crazyword.org/InsUserInfo3.asp/", form);

        //print("1");

        //// Wait until the download is done
        //yield return download;

        //print("2");

        //print("download.error=" + download.error);
        //print("download.text=" + download.text);


        // For call Webpage by wwwform //
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("userId", "Korean-546CDB0B-0-2D90A94");

        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> post_arg in dic)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW("http://crazyword1.org/SelUserInfo.asp", form);

        yield return www;


        if (www.isDone && www.error == null)
        {
            Debug.Log("www Result: " + www.text);
            dic.Clear();

            var UserInfo = JSON.Parse(www.text);

            string rowCnt = UserInfo["rowCnt"];

            print("rowCnt=" + rowCnt);

        }
        else
        {
            Debug.Log("error : " + www.error);
            dic.Clear();
        }

        print("Application.internetReachability=" + Application.internetReachability);

        
//이 값이 
//ReachableViaCarrierDataNetwork  3g 
//ReachableViaLocalAreaNetwork  wifi 
//NotReachable 안됨


        //print("www.error=" + www.error);
        //print("www.text=" + www.text);

        

    }    

}
