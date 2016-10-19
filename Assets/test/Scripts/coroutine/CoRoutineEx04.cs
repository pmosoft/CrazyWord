using UnityEngine;
using System.Collections;

public class CoRoutineEx04 : MonoBehaviour {
	// Use this for initialization
	void Start () {
		print ("1");
		StartCoroutine(cor1());
		print ("3");
		StartCoroutine(cor2());
		print ("5");

	}
	
	IEnumerator cor1() {
		print ("2");
		yield  return new WaitForSeconds(0.01f);
		//yield return null;
		print ("7");
		
	}
	
	IEnumerator cor2() {
		print ("4");
		yield return new WaitForSeconds(0.01f);
		//yield return null;
		print ("8");
		
	}


	// Update is called once per frame
	void Update () {
		print ("6");
	}
}
