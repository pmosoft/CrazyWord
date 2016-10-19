using UnityEngine;
using System.Collections;

public class CoRoutineEx03 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		print ("1");
		StartCoroutine(cor1());
		print ("3");
	}
		
	IEnumerator cor1() {
		print ("2");
		//yield return new WaitForSeconds(0.01f);
		yield return null;
		print ("5");

	}

	// Update is called once per frame
	void Update () {
		print ("4");
	}
}
