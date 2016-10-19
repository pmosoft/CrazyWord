using UnityEngine;
using System.Collections;

public class object02 : MonoBehaviour {

	public int i01;
	public int i02;

	// Use this for initialization
	void Start () {
		i01 = 2;
		obj01();
	}
	
	public void obj01(){
		print (i01);
	}

	public void obj02(){
		i02 = 2;
		print (i02);
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
