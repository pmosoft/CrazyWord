using UnityEngine;
using System.Collections;

public class TimeTest : MonoBehaviour {

	float t1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		test01();

	}

	void test01(){

		print("start:"+System.DateTime.Now); 

		t1 += Time.deltaTime;
		print ("Time.deltaTime="+Time.deltaTime);
		print ("t1="+t1);

		print("end:"+System.DateTime.Now); 

	}

}
