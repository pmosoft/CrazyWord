using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System.Text;
public class UnitySendJson : MonoBehaviour
{

    Dictionary<string, string> dic = new Dictionary<string, string>();
    public string url_Path = "http://crazyword.org/InsUserInfo3.asp";//http://myurl

    // Use this for initialization
    void Start()
    {
        dic.Add("name", "oranc");
        dic.Add("id", "oranc");
    }
     
    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 300, 100), "SEND"))
        {
            string temp = Json.Serialize(dic);
            StartCoroutine(SendJSonData(temp));
        }
    }

    IEnumerator SendJSonData(string temp)
    {
        Encoding encoding = new System.Text.UTF8Encoding();
        Hashtable header = new Hashtable();
        header.Add("Content-Type", "text/json");
        header.Add("Content-Length", temp.Length);
        WWW www = new WWW(url_Path, encoding.GetBytes(temp), header);
        yield return www;
        if (www.isDone && www.error == null)
        {
            Debug.Log("www Result: " + www.text);
            dic.Clear();
        }
        else
        {
            Debug.Log("error : " + www.error);
            dic.Clear();
        }
    }
}