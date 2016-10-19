using UnityEngine;
using System.Collections;

public class random01 : MonoBehaviour {

	// Use this for initialization
	int r1;
	
	// Update is called once per frame
	void Update () {

		// same result below 3 cases
		//r1 = Random.Range(1,4);
		//r1 = (int)Random.Range(1.0001f,3.9999f);
		//r1 = (int)Random.Range(1.0001f,4.0f);

		// result 1,2,3
		r1 = Random.Range(1,4);
		print(r1);
	}
}
