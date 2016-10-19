using UnityEngine;
using System.Collections;

public class WebBasic : MonoBehaviour {
 
    public string url = "www.ipshield.co.kr";
    WWW www;
   
    bool isDownloading = false;
   
    IEnumerator Start()
    {
        www = new WWW(url);
        isDownloading = true;
        yield return www;
        isDownloading = false;
        Debug.Log ("Download Completed!");
    }
   
    void Update()
    {
        if(isDownloading)
            Debug.Log (www.progress);
    }
 
}