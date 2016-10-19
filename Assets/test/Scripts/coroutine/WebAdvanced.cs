using UnityEngine;
using System.Collections;

public class WebAdvanced : MonoBehaviour {
   
	public string url = "www.ipshield.co.kr";
    WWW www;
   
    IEnumerator Start()
    {
        www = new WWW(url);
        StartCoroutine("CheckProgress");
        yield return www;
        Debug.Log ("Download Completed!");
    }
   
    IEnumerator CheckProgress()
    {
        Debug.Log (www.progress);
        while(!www.isDone)
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log (www.progress);
        }
    }
       
}
