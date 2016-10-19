using UnityEngine;
using System.Collections;

public class rotateTest01 : MonoBehaviour {

	//Quaternion startingRotation;

	void Start()
	{
		//startingRotation = transform.rotation;
	}

	void Update()
	{
		transform.Rotate(new Vector3(0,0,1), Time.deltaTime * 20, Space.Self);
		//Quaternion change = Quaternion.FromToRotation( startingRotation, transform.rotation );
		//Debug.Log("Angle changed this much: " + change.eulerAngles);
	}
}