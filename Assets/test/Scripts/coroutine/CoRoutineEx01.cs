using UnityEngine;
using System.Collections;

public class CoRoutineEx01 : MonoBehaviour {

	bool printToggle = false;
	// Use this for initialization
	void Start () {

		StartCoroutine(waitFor());
		print ( "I'm the second one after start"  );
		if( printToggle == true ) 
		{
			print ( " yield statement 5 second wait is completed" );
		}

	}

    IEnumerator waitFor()
	{
		print ( "We are starting to wait" );
		yield return new WaitForSeconds(5.0f);
		print ( "It's been 5 seconds" );
		printToggle = true;
	}

}

