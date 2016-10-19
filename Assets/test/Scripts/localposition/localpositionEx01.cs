using UnityEngine;
using System.Collections;

public class localpositionEx01 : MonoBehaviour {

	// Use this for initialization
	void Start () {

		print( "g="+transform.position );	
		print( "l="+transform.localPosition );	

		//tr.localPosition = Vector3( 5.0f,0.0f,0.0f);
		//tr.localPosition = Vector3( 5,0,0);
		transform.localPosition = new Vector3( 7f,0f,0f);

	}
	
}
