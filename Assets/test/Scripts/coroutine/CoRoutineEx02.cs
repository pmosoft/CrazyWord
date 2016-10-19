using UnityEngine;
using System.Collections;

public class CoRoutineEx02 : MonoBehaviour {

	IEnumerator Start () {
		print ("Starting " + Time.time);
		yield return StartCoroutine(WaitAndPrint(2.0f));
		print ("Starting " + Time.time);

	}

	IEnumerator WaitAndPrint(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		print ("WaitAndPrint " + Time.time);
	}

}
