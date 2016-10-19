using UnityEngine;
using System.Collections;

public class DialogExample : MonoBehaviour {
	
	bool showDialog = false;
	string answer = "";
	
	IEnumerator Start()
	{
		yield return StartCoroutine("ShowDialog");
		yield return StartCoroutine(answer);
	}
	
	IEnumerator ShowDialog()
	{
		showDialog = true;
		do
		{
			yield return null;
		} while(answer == "");
		
		showDialog = false;
	}
	
	IEnumerator ActionA()
	{
		Debug.Log ("Action A");
		yield return new WaitForSeconds(1f);
	}
	
	IEnumerator ActionB()
	{
		Debug.Log ("Action B");
		yield return new WaitForSeconds(2f);
	}
	
	void OnGUI()
	{
		if(showDialog)
		{
			if(GUI.Button(new Rect(10f, 10f, 100f, 20f), "A"))
			{
				answer = "ActionA";  
			} else if(GUI.Button(new Rect(10f, 50f, 100f, 20f), "B")) {
				answer = "ActionB";
			}
		}
	}
	
}