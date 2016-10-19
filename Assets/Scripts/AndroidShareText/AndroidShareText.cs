using UnityEngine;
using System.Collections;

public class AndroidShareText : MonoBehaviour {

#if UNITY_ANDROID && !UNITY_EDITOR
    private AndroidJavaObject curActivity;
# endif

    private string contentsTitle = "함께 해요 CrazyWord!!";
    private string contents = "http://crazyword.org";
    private string popupTitle = "CrazyWord 공유하기!";


    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        curActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");

# endif

    }


    public void ShareText()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        CallJavaFunc("shareText", contentsTitle, contents, popupTitle);
# endif
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 100, 100, 100), "Share Text") == true)
        {
            CallJavaFunc("shareText", contentsTitle, contents, popupTitle);
        }

        if (GUI.Button(new Rect(0, 300, 100, 100), "TTS Setup") == true)
        {
            SetupTTS();
        }
    }


    void CallJavaFunc(string strFuncName, string subject, string text, string chooserTitle)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (curActivity == null)
            return;

        curActivity.Call(strFuncName, subject, text, chooserTitle);
# endif

    }


    public void SetupTTS()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        curActivity.Call("SetupTTS");
# endif
    } 
}
