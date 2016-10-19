using UnityEngine;
using System.Collections;

public class RecurControl : MonoBehaviour {

	public GameObject recur;

	// Use this for initialization
	void Start () {
		//recur.GetComponent<ReceiveCor>().enabled = true;
		//recur.GetComponent<ReceiveCor>().enabled = false;

		recur.SetActive(true);
		recur.SetActive(false);

	}
	
}
