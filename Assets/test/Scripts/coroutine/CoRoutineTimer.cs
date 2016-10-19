using UnityEngine;
using System.Collections;

public class CoRoutineTimer : MonoBehaviour {
	float waitTime1 = 3.0f;
	float waitTime2 = 5.0f;
	
	IEnumerator Start () {
		Debug.Log ("Action Start");
		yield return new WaitForSeconds(waitTime1);
		Debug.Log ("Action1 End");
		yield return new WaitForSeconds(waitTime2);
		Debug.Log ("Action2 End");
	}    
}
