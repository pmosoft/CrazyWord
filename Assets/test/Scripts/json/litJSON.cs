using UnityEngine;
using System.Collections;

public class litJSON : MonoBehaviour {

    public TextAsset jsonData1;
    public TextAsset jsonData2;

	// Use this for initialization
	void Start () {
        test2();

	}

    void test1()
    {
        LitJson.JsonData getData = LitJson.JsonMapper.ToObject(jsonData1.text);

        string name = getData["name"].ToString();
        int score = int.Parse(getData["score"].ToString());

        print("Name=" + name + " score=" + score);

    }

    void test2()
    {
        LitJson.JsonData getData = LitJson.JsonMapper.ToObject(jsonData2.text);

        for (int ix=0; ix < getData["Person"].Count; ++ix){
            print("Name=" + getData["Person"][ix]["Name"].ToString());
            print("Age=" + getData["Person"][ix]["Age"].ToString());
        
        }


    }

}
